using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TenderAssist.CommonHelper;
using TenderAssist.Models;
using TenderAssist.Models.DBConnection;
using TenderAssist.ViewModel;
using static TenderAssist.CommonHelper.Utility;
using static TenderAssist.Models.SearchModel;

namespace TenderAssist.Controllers
{
    public class UserController : BaseController
    {
        #region VARIABLE DECLARATION

        private readonly UserMembershipDetail _userInfo;
        private readonly LoginStatus _lginStatus;
        private readonly CommonController _common;
        private readonly UserTenderPermission _userTenderPermissionDetail;
        //private readonly CommonInformation _commonInfo;
        private readonly TenderInformation _tenderInfo;
        private readonly GetListItems _getListItems;

        private List<string> WithinSearchWords = new List<string>();
        private static int PageSize = 20;

        #endregion

        public UserController()
        {
            _userInfo = new UserMembershipDetail();
            _lginStatus = new LoginStatus();
            _common = new CommonController();
            _userTenderPermissionDetail = new UserTenderPermission();
            //_commonInfo = new CommonInformation();
            _tenderInfo = new TenderInformation();
            _getListItems = new GetListItems();

        }

        #region LOGIN & LOG OFF
        public ActionResult CheckUserLogin(string uName, string uPswd)
        {
            var username = "";
            var password = "";
            var errormsg = "";
            var msg = "ok";
            if (uName.Trim() == "" || uName.ToLower().Trim() == "username".Trim())
            {
                username = "User Name is required";
                msg = "error";
            }
            if (uPswd.Trim() == "" || uPswd.ToLower().Trim() == "password".Trim())
            {
                password = "Password is required";
                msg = "error";
            }
            else
            {
                if (uPswd.Trim().Length < 6)
                {
                    password = "'Password' must be at least 6 characters long.";
                    msg = "error";
                }
            }

            if (msg == "error") return Json(new { username, password, msg, errormsg }, JsonRequestBehavior.AllowGet);

            try
            {
                var userinfo = _userInfo.GetUserLoginDetail(uName.Trim(), uPswd);
                if (userinfo != null)
                {
                    var userLoginId = userinfo.intClientId;
                    Session["ClientID"] = userLoginId.ToString(CultureInfo.InvariantCulture);
                    Session["ClientInfo"] = userinfo;
                    //Session["PageTitle"] = "";
                    Session["IsActiveUser"] = userinfo.intActive.ToString(CultureInfo.InvariantCulture);
                    Session["PageTitle"] = "TenderAssist :: Indian Tenders";
                    //Session["IndianPermissionId"] = null;
                    //if (userinfo.intActive == 1)
                    //{

                    //}
                    //else
                    //{
                    //    errormsg = "Sorry! unrecognized Username or Password.";
                    //    msg = "error";
                    //}
                }
                else
                {
                    errormsg = "Sorry! unrecognized Username or Password.";
                    msg = "error";
                }

            }
            catch (Exception ex)
            {
                errormsg = "Oops! Something went wrong. <br\\> Please try again later, Sorry for the inconvenience.";
                msg = "error";

            }

            return Json(new { username, password, msg, errormsg }, JsonRequestBehavior.AllowGet);
        }

        public string GetUserName()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                RedirectToAction("UserLogin", "Home");
            }

            var username = _lginStatus.GetUserName();
            return username;
        }
        public string GetUserLastLogin()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                RedirectToAction("Index", "IndianTenders");
            }

            var lastlogindate = _lginStatus.GetUserLastLogin();

            return lastlogindate;
        }

        [HttpPost]
        public ActionResult UpdateUserLastLogin()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                RedirectToAction("Index", "IndianTenders");
            }

            var useid = Convert.ToInt32(Session["ClientID"]);
            _userInfo.UpdateUserLastLoginDate(useid);

            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOff()
        {
            if (HttpContext.Session != null) HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region DASHBOARD
        public ActionResult MyDashboard()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];


            var IndianTenderServiceAccess = _userTenderPermissionDetail.IndianTenderServiceAccess(userId);
            var GlobalTenderServiceAccess = _userTenderPermissionDetail.GlobalTenderServiceAccess(userId);

            var indianPermissionCount = 0;
            var globalPermissionCount = 0;
            var awardedPermissionCount = 0;
            var projectPermissionCount = 0;

            if (IndianTenderServiceAccess)
            {
                indianPermissionCount = _userTenderPermissionDetail.GetIndianTenderPermissionList(userId).Count();
            }
            if (GlobalTenderServiceAccess)
            {
                globalPermissionCount = _userTenderPermissionDetail.GetGlobalTenderPermissionList(userId).Count();
            }

            ViewData["IndianTenderServiceAccess"] = IndianTenderServiceAccess;
            ViewData["GlobalTenderServiceAccess"] = GlobalTenderServiceAccess;
            ViewData["AwardedTenderServiceAccess"] = false;
            ViewData["ProjectTenderServiceAccess"] = false;

            ViewData["IndianTenderPermissionCount"] = indianPermissionCount;
            ViewData["GlobalTenderPermissionCount"] = globalPermissionCount;
            ViewData["AwardedTenderPermissionCount"] = awardedPermissionCount;
            ViewData["ProjectTenderPermissionCount"] = projectPermissionCount;

            return View();
        }
        #endregion

        #region INDIAN SERVICE
        public ActionResult IndianService()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];


            var IndianTenderServiceAccess = _userTenderPermissionDetail.IndianTenderServiceAccess(userId);
            List<tabClientPermission> IndianTenderPermissionList = new List<tabClientPermission>();

            if (IndianTenderServiceAccess)
            {
                IndianTenderPermissionList = _userTenderPermissionDetail.GetIndianTenderPermissionList(userId);
            }
            Session["PageName"] = "IndianService";
            ViewData["IndianTenderServiceAccess"] = IndianTenderServiceAccess;
            Session["IndianGlobal"] = true;
            var tenderDetail = new TenderDetail
            {
                ClientPermissionList = IndianTenderPermissionList
            };
            //return View("/Indian/IndianService.cshtml", tenderDetail);
            return View("~/Views/User/Indian/IndianService.cshtml", tenderDetail);
        }
        public ActionResult IndianTenderList(int permissionId = 0)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            string tenderStatus = "";
            int page = 0;
            var pagecnt = page != 0 ? page - 1 : 0;
            var pageSize = PageSize;
            var newpagecnt = (pagecnt * pageSize);

            Session["IndianGlobal"] = true;

            TenderDetail tenderDetail = new TenderDetail();
            if (tenderStatus == "") { tenderStatus = SetTenderStatus(0); }
            var tenderStatusFlag = SetTenderStatusFlag(tenderStatus);
            var isIndianGlobal = true;

            if (Session["UserIndianTenders"] != null)
            {
                tenderDetail = (TenderDetail)Session["UserIndianTenders"];
                if (tenderDetail.PermissionId == permissionId)
                {
                    if (tenderDetail.Newpagecnt != newpagecnt || tenderDetail.TenderStatus != tenderStatusFlag)
                    {
                        var isSearchWithCount = true;

                        tenderDetail = SearchIndianGlobalTenders(tenderDetail.PermissionId, string.Empty,
                            tenderDetail.SelectedKeyword1, tenderDetail.SelectedKeyword2, tenderDetail.SelectedKeyword3, tenderDetail.SelectedLocationIds, tenderDetail.SelectedIndustrySubIndustryIds,
                            tenderDetail.SelectedAgency, tenderDetail.SelectedSector, tenderDetail.SelectedOwnership, tenderStatusFlag, tenderDetail.EnterDt, newpagecnt, pageSize,
                            isSearchWithCount, tenderDetail.OurRefNo, tenderDetail.TenderType, tenderDetail.ICBNCB,
                            tenderDetail.TenderValFrom, tenderDetail.TenderValTo, tenderDetail.TenderSubDateFrom, tenderDetail.TenderSubDateTo, tenderDetail.TenderOpDateFrom,
                            tenderDetail.TenderOpDateTo, "", isIndianGlobal, tenderDetail.OtherKeywords, tenderDetail.TendersBy, null);
                    }
                }
                else
                {
                    DateTime? enterDate = null;

                    tenderDetail = SearchIndianGlobalTenders(permissionId, string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, tenderStatusFlag, enterDate, newpagecnt, pageSize,
                        true, null, null, -1, null, null, null, null, null, null, "", isIndianGlobal, string.Empty, 0, null);
                }
            }
            else
            {
                DateTime? enterDate = null;

                ClearSession();
                tenderDetail = SearchIndianGlobalTenders(permissionId, string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, tenderStatusFlag, enterDate, newpagecnt, pageSize,
                        true, null, null, -1, null, null, null, null, null, null, "", isIndianGlobal, string.Empty, 0, null);
            }

            Session["UserIndianTenders"] = tenderDetail;

            var clientPermissionItem = _userTenderPermissionDetail.GetUsersTenderPermissionById(permissionId);
            tenderDetail.ClientPermissionItem = clientPermissionItem;
            OtherValues();
            var shoecurrentpage = (tenderDetail.Newpagecnt / pageSize) + 1;
            ViewBag.DisplayCurrentPage = shoecurrentpage;
            ViewBag.CurrentPage = shoecurrentpage;
            ViewBag.PageSize = tenderDetail.PageSize;
            ViewBag.TotalPage = Math.Ceiling((double)tenderDetail.Total / tenderDetail.PageSize);
            ViewBag.SearchText = "";
            Session["IndianGlobal"] = true;

            Session["PageName"] = "IndianTenderList";

            return View("Indian/IndianTenderList", tenderDetail);
        }
        public ActionResult TenderDetail(string tYear = "", string refno = "")
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            var intActive = Session["IsActiveUser"] != null ? Convert.ToInt32(Session["IsActiveUser"]) : 2;
            if (intActive == 2)
            { return RedirectToAction("Renewal"); }

            Session["IndianGlobal"] = true;

            tYear = string.IsNullOrEmpty(tYear) ? "0" : tYear;
            var tenderInformation = _common.GetTenderInfoById(refno, tYear);

            ViewBag.AgencyName = "";
            ViewBag.Location = "";
            ViewBag.Ownership = "";
            ViewBag.Sector = "";
            ViewBag.DocType = "";

            if (tenderInformation != null)
            {
                var agency = _tenderInfo.GetAgencyDetailById(tenderInformation.AgencyId);
                if (agency != null)
                    ViewBag.AgencyName = agency.AgencyName;

                var ownership = _tenderInfo.GetOwnershipById(tenderInformation.OwnershipId);
                if (ownership != null)
                    ViewBag.Ownership = ownership.OwnershipName;

                var sector = _tenderInfo.GetSectorById(tenderInformation.SectorId);
                if (sector != null)
                    ViewBag.Sector = sector.SectorName;

                var statename = _tenderInfo.GetState(tenderInformation.StateId).StateName;
                var city = _tenderInfo.GetCity(tenderInformation.LocId).Location;

                string location = city + " - " + statename;
                ViewBag.Location = location;
            }

            int refNo = refno == "" ? 0 : Convert.ToInt32(refno);
            //List<TenderClassifiedIn> tenderClassifiedIn = _common.GetTenderClassifiedIn(refNo);
            Session["IndianGlobal"] = true;
            var tenderDetail = new TenderDetail
            {
                TenderDetails = tenderInformation
            };

            return View(tenderDetail);
        }
        public JsonResult DownloadDocument(string refno)
        {
            var downloadDocumentUrl = ConfigurationManager.AppSettings["DownloadDocumentUrl"].ToString(CultureInfo.InvariantCulture);
            var tenderdocfile = _tenderInfo.GetTenderDocFilesById(refno);
            if (tenderdocfile != null)
            {
                string path = tenderdocfile.DocFilesNamePath;
                path = path.Replace("2015", "2015_01");
                string filename = downloadDocumentUrl + path.Replace("\\", "/");

                var items = new List<SelectListItem>();
                items.Insert(0, (new SelectListItem { Text = filename, Value = refno }));
                ViewData["List"] = new SelectList(items, "Value", "Text");
                return Json(new { FileName = ViewData["List"] }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        #endregion

        #region GLOBAL SERVICE
        public ActionResult GlobalService()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];
            Session["IndianGlobal"] = false;

            var GlobalTenderServiceAccess = _userTenderPermissionDetail.GlobalTenderServiceAccess(userId);
            List<tabClientPermission> GlobalTenderPermissionList = new List<tabClientPermission>();

            if (GlobalTenderServiceAccess)
            {
                GlobalTenderPermissionList = _userTenderPermissionDetail.GetGlobalTenderPermissionList(userId);
            }
            Session["PageName"] = "GlobalService";
            ViewData["GlobalTenderServiceAccess"] = GlobalTenderServiceAccess;
            Session["IndianGlobal"] = false;
            var tenderDetail = new TenderDetail
            {
                ClientPermissionList = GlobalTenderPermissionList
            };
            return View("~/Views/User/Global/GlobalService.cshtml", tenderDetail);
        }
        public ActionResult GlobalTenderList(int permissionId = 0)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            string tenderStatus = "";
            int page = 0;

            var pagecnt = page != 0 ? page - 1 : 0;

            var pageSize = PageSize;
            var newpagecnt = (pagecnt * pageSize);
            TenderDetail tenderDetail = new TenderDetail();
            if (tenderStatus == "") { tenderStatus = SetTenderStatus(0); }
            var tenderStatusFlag = SetTenderStatusFlag(tenderStatus);
            var isIndianGlobal = false;
            Session["IndianGlobal"] = isIndianGlobal;

            if (Session["UserGlobalTenders"] != null)
            {
                tenderDetail = (TenderDetail)Session["UserGlobalTenders"];
                if (tenderDetail.PermissionId == permissionId)
                {
                    if (tenderDetail.Newpagecnt != newpagecnt || tenderDetail.TenderStatus != tenderStatusFlag)
                    {
                        var isSearchWithCount = true;

                        tenderDetail = SearchIndianGlobalTenders(tenderDetail.PermissionId, string.Empty,
                            tenderDetail.SelectedKeyword1, tenderDetail.SelectedKeyword2, tenderDetail.SelectedKeyword3, tenderDetail.SelectedLocationIds, tenderDetail.SelectedIndustrySubIndustryIds,
                            tenderDetail.SelectedAgency, tenderDetail.SelectedSector, tenderDetail.SelectedOwnership, tenderStatusFlag, tenderDetail.EnterDt, newpagecnt, pageSize,
                            isSearchWithCount, tenderDetail.OurRefNo, tenderDetail.TenderType, tenderDetail.ICBNCB,
                            tenderDetail.TenderValFrom, tenderDetail.TenderValTo, tenderDetail.TenderSubDateFrom, tenderDetail.TenderSubDateTo, tenderDetail.TenderOpDateFrom,
                            tenderDetail.TenderOpDateTo, "", isIndianGlobal, tenderDetail.OtherKeywords, tenderDetail.TendersBy, tenderDetail.SelectedCountry);
                    }
                }
                else
                {
                    DateTime? enterDate = null;

                    tenderDetail = SearchIndianGlobalTenders(permissionId, string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, tenderStatusFlag, enterDate, newpagecnt, pageSize,
                        true, null, null, -1, null, null, null, null, null, null, "", isIndianGlobal, string.Empty, 0, null);
                }
            }
            else
            {
                DateTime? enterDate = null;

                ClearSession();
                tenderDetail = SearchIndianGlobalTenders(permissionId, string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, tenderStatusFlag, enterDate, newpagecnt, pageSize,
                        true, null, null, -1, null, null, null, null, null, null, "", isIndianGlobal, string.Empty, 0, null);
            }

            Session["UserGlobalTenders"] = tenderDetail;

            var clientPermissionItem = _userTenderPermissionDetail.GetUsersTenderPermissionById(permissionId);
            tenderDetail.ClientPermissionItem = clientPermissionItem;
            OtherValues();
            var shoecurrentpage = (tenderDetail.Newpagecnt / pageSize) + 1;
            ViewBag.DisplayCurrentPage = shoecurrentpage;
            ViewBag.CurrentPage = shoecurrentpage;
            ViewBag.PageSize = tenderDetail.PageSize;
            ViewBag.TotalPage = Math.Ceiling((double)tenderDetail.Total / tenderDetail.PageSize);
            ViewBag.SearchText = "";
            Session["IndianGlobal"] = false;
            Session["PageName"] = "GlobalTenderList";

            return View("Global/GlobalTenderList", tenderDetail);
        }
        public ActionResult GlobalTenderDetail(string refno = "")
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            var intActive = Session["IsActiveUser"] != null ? Convert.ToInt32(Session["IsActiveUser"]) : 2;
            if (intActive == 2)
            { return RedirectToAction("Renewal"); }

            Session["IndianGlobal"] = false;

            var globalTenderInformation = _tenderInfo.GetGlobalTenderInfoById(refno);

            ViewBag.AgencyName = "";
            ViewBag.Location = "";
            ViewBag.Ownership = "";
            ViewBag.Sector = "";
            ViewBag.DocType = "";

            if (globalTenderInformation != null)
            {
                var agency = _tenderInfo.GetAgencyDetailById(globalTenderInformation.AgencyId);
                if (agency != null)
                    ViewBag.AgencyName = agency.AgencyName;

                var ownership = _tenderInfo.GetOwnershipById(globalTenderInformation.OwnershipId);
                if (ownership != null)
                    ViewBag.Ownership = ownership.OwnershipName;

                var sector = _tenderInfo.GetSectorById(globalTenderInformation.SectorId);
                if (sector != null)
                    ViewBag.Sector = sector.SectorName;

                var statename = _tenderInfo.GetState(globalTenderInformation.StateId).StateName;
                var city = _tenderInfo.GetCity(globalTenderInformation.LocId).Location;
                var country = _tenderInfo.GetCountry(globalTenderInformation.CountryId).CountryName;

                //string location = city + " - " + statename;
                ViewBag.Location = country;
            }

            int refNo = refno == "" ? 0 : Convert.ToInt32(refno);
            Session["IndianGlobal"] = false;
            var tenderDetail = new TenderDetail
            {
                TenderDetails = globalTenderInformation
            };

            return View(tenderDetail);
        }
        #endregion

        public ActionResult GetTenderResultOnLoading(int permissionId, int page = 0)
        {
            bool isIndianGlobal = true;
            if (Session["IndianGlobal"] != null)
            { isIndianGlobal = (bool)Session["IndianGlobal"]; }

            Session["IndianGlobal"] = isIndianGlobal;

            ViewBag.IsEndOfRecords = false;
            TenderDetail tenderDetail = null;

            int regionId = 0;
            string sessionKeyName = "";
            string url = "";

            if (isIndianGlobal)
            { sessionKeyName = "UserIndianTenders"; url = "~/Views/User/Indian/partialSearchResultData.cshtml"; }
            else
            { sessionKeyName = "UserIndianTenders"; url = "~/Views/User/Global/partialSearchResultData.cshtml"; }

            var pagecnt = page != 0 ? page - 1 : 0;
            var pageSize = PageSize;
            var newpagecnt = (pagecnt * pageSize);

            if (Request.IsAjaxRequest())
            {
                var advanceSearch = new AdvanceSearchParameter();
                if (Session["ClientAdvanceSearchParams"] != null)
                {
                    advanceSearch = (AdvanceSearchParameter)Session["ClientAdvanceSearchParams"];
                }

                if (Session[sessionKeyName] != null)
                {
                    tenderDetail = (TenderDetail)Session[sessionKeyName];
                    if (tenderDetail != null)
                    { regionId = tenderDetail.RegionId; }
                }
                var tenderStatus = advanceSearch.TenderStatusFlag;
                tenderDetail = SearchIndianGlobalTenders(permissionId, "", advanceSearch.SelectedProducts, "", "",
                            advanceSearch.SelectedLocations, advanceSearch.SelectedIndsubIndustries, advanceSearch.SelectedAgencies, advanceSearch.SelectedSectors, advanceSearch.SelectedOwnerships,
                            advanceSearch.TenderStatusFlag, null, newpagecnt, PageSize, false, advanceSearch.OurRefNo, advanceSearch.TenderTypeId, advanceSearch.IcbNcb, 0, 0,
                            advanceSearch.SubDateFrom, advanceSearch.SubDateTo, advanceSearch.OpDateFrom, advanceSearch.OpDateTo, "", true, "", regionId, advanceSearch.SelectedCountries);

                if (advanceSearch != null)
                {
                    Session["ClientAdvanceSearchParams"] = advanceSearch;
                }
                var total = tenderDetail.Total;
                switch (tenderStatus)
                {
                    case (int)TenderStatusFlags.AllTenders:
                        total = tenderDetail.Total;
                        break;
                    case (int)TenderStatusFlags.LiveTenders:
                        total = tenderDetail.TotalLive;
                        break;
                    case (int)TenderStatusFlags.NewTenders:
                        total = tenderDetail.TotalFresh;
                        break;
                    case (int)TenderStatusFlags.ClosedTenders:
                        total = tenderDetail.TotalClosed;
                        break;
                }
                ViewBag.TotalPage = Math.Ceiling((double)total / tenderDetail.PageSize);
                ViewBag.CurrentPage = (page + 1);
                ViewBag.IsEndOfRecords = (tenderDetail.AllSearchTenaderInfoWithAllDetail.Any());
                return PartialView(Url.Content(url), tenderDetail);
            }
            return null;
        }
        public ActionResult GetTenderFromTenderStatus(int permissionId, int tenderStatus = 0)
        {
            bool isIndianGlobal = true;
            if (Session["IndianGlobal"] != null)
            { isIndianGlobal = (bool)Session["IndianGlobal"]; }

            Session["IndianGlobal"] = isIndianGlobal;

            int page = 1;
            ViewBag.IsEndOfRecords = false;
            TenderDetail tenderDetail = null;

            int regionId = 0;
            string sessionKeyName = "";
            string url = "";

            if (isIndianGlobal)
            { sessionKeyName = "UserIndianTenders"; url = "~/Views/User/Indian/partialSearchResultData.cshtml"; }
            else
            { sessionKeyName = "UserIndianTenders"; url = "~/Views/User/Global/partialSearchResultData.cshtml"; }

            var pagecnt = page != 0 ? page - 1 : 0;
            var pageSize = PageSize;
            var newpagecnt = (pagecnt * pageSize);


            if (Request.IsAjaxRequest())
            {
                var advanceSearch = new AdvanceSearchParameter();
                if (Session["ClientAdvanceSearchParams"] != null)
                {
                    advanceSearch = (AdvanceSearchParameter)Session["ClientAdvanceSearchParams"];
                }
                advanceSearch.TenderStatusFlag = tenderStatus;

                if (Session[sessionKeyName] != null)
                {
                    tenderDetail = (TenderDetail)Session[sessionKeyName];
                    if (tenderDetail != null)
                    { regionId = tenderDetail.RegionId; }
                }

                tenderDetail = SearchIndianGlobalTenders(permissionId, "", advanceSearch.SelectedProducts, "", "",
                           advanceSearch.SelectedLocations, advanceSearch.SelectedIndsubIndustries, advanceSearch.SelectedAgencies, advanceSearch.SelectedSectors, advanceSearch.SelectedOwnerships,
                           advanceSearch.TenderStatusFlag, null, newpagecnt, PageSize, false, advanceSearch.OurRefNo, advanceSearch.TenderTypeId, advanceSearch.IcbNcb, 0, 0,
                           advanceSearch.SubDateFrom, advanceSearch.SubDateTo, advanceSearch.OpDateFrom, advanceSearch.OpDateTo, "", true, "", regionId, advanceSearch.SelectedCountries);


                if (advanceSearch != null)
                {
                    Session["ClientAdvanceSearchParams"] = advanceSearch;
                }

                var total = tenderDetail.Total;
                switch (tenderStatus)
                {
                    case (int)TenderStatusFlags.AllTenders:
                        total = tenderDetail.Total;
                        break;
                    case (int)TenderStatusFlags.LiveTenders:
                        total = tenderDetail.TotalLive;
                        break;
                    case (int)TenderStatusFlags.NewTenders:
                        total = tenderDetail.TotalFresh;
                        break;
                    case (int)TenderStatusFlags.ClosedTenders:
                        total = tenderDetail.TotalClosed;
                        break;
                }
                ViewBag.TotalPage = Math.Ceiling((double)total / tenderDetail.PageSize);
                ViewBag.CurrentPage = (page + 1);
                ViewBag.IsEndOfRecords = (tenderDetail.AllSearchTenaderInfoWithAllDetail.Any());
                return PartialView(Url.Content(url), tenderDetail);
            }
            return null;
        }
        public ActionResult SearchTenders(SearchUserTenderModel searchResult)
        {
            var permissionId = searchResult.PermissionId;

            var tenderBy = searchResult.TenderBy;
            var pageSize = PageSize;

            var otherKeywords = searchResult.OtherKeywordText == null ? "" : searchResult.OtherKeywordText;
            if (!string.IsNullOrEmpty(otherKeywords))
            {
                otherKeywords = otherKeywords.Replace(";", ",");
            }

            var page = searchResult.Page;
            var searchText = string.IsNullOrEmpty(searchResult.Search) ? "" : searchResult.Search;
            var filterMainText = string.IsNullOrEmpty(searchResult.Search) ? "" : searchResult.Search;
            var searchType = searchResult.SType;
            var advanceSearch = searchResult.AdvanceSearch;
            var orderBy = searchResult.OrderBy;

            Session["ClientAdvanceSearchParams"] = advanceSearch;

            var fieldId = searchResult.FId;
            var fieldName = searchResult.FName;
            var tenderYear = searchResult.TenderYear;

            if (!string.IsNullOrEmpty(filterMainText))
            {
                FillWithinSearchWords(filterMainText);
            }

            var isAdvanceSearch = searchResult.IsAdvanceSearch;
            DateTime? enterDt = searchResult.EnterDate;

            bool isIndianGlobal = true;
            if (Session["IndianGlobal"] != null)
            { isIndianGlobal = (bool)Session["IndianGlobal"]; }

            var regionId = 0;
            var url = "";
            var sessionKeyName = "";
            if (isIndianGlobal == false)
            {
                regionId = searchResult.TenderBy;
                url = "/User/Global-Tender/" + permissionId;
                sessionKeyName = "UserGlobalTenders";
            }
            else
            {
                url = "/User/Indian-Tender/" + permissionId;
                sessionKeyName = "UserIndianTenders";
            }

            var Tenders = SearchIndianGlobalTenders(permissionId, "", advanceSearch.SelectedProducts, searchResult.Product2Ids, searchResult.Product3Ids,
                advanceSearch.SelectedLocations, advanceSearch.SelectedIndsubIndustries, advanceSearch.SelectedAgencies, advanceSearch.SelectedSectors, advanceSearch.SelectedOwnerships,
                advanceSearch.TenderStatusFlag, enterDt, page, pageSize, true, advanceSearch.OurRefNo, advanceSearch.TenderTypeId,
                advanceSearch.IcbNcb, 0, 0, advanceSearch.SubDateFrom, advanceSearch.SubDateTo, advanceSearch.OpDateFrom, advanceSearch.OpDateTo, "", isIndianGlobal, otherKeywords,
                regionId, advanceSearch.SelectedCountries);

            var tenderStatusFlag = advanceSearch.TenderStatusFlag == null ? 0 : advanceSearch.TenderStatusFlag.Value;
            string tenderStatus = SetTenderStatus(tenderStatusFlag);

            page = page == 0 ? 1 : page;

            Session[sessionKeyName] = Tenders;

            return Json(new
            {
                newurl = url,
                page,
                searchText = filterMainText.Replace(" ", "-"),
                tenderStatusFlag = tenderStatus,
            });
        }


        #region USER PROFILE

        public ActionResult Index()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            ClearSession();

            //var username = FindUserName(userId);
            var username = userinfo.strFName + " " + userinfo.strLName;
            if (username.Trim() == "")
            { username = userinfo.strEmailId1; }

            var country = _tenderInfo.GetCountryWithCity(userinfo.intLocId);
            var state = _tenderInfo.GetStateWithCity(userinfo.intLocId);
            var city = _tenderInfo.GetCity(userinfo.intLocId);

            var tenderDetail = new TenderDetail
            {
                LoginUserName = username,
                LoginUserDetails = userinfo,
                ClientCountryName = country == null ? "" : country.CountryName,
                ClientStateName = state == null ? "" : state.StateName,
                ClientCityName = city == null ? "" : city.Location,

            };
            return View(tenderDetail);
        }
        public ActionResult EditProfile()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            ClearSession();

            var intCountryId = 0;
            var intStateId = 0;
            var intLocId = 0;

            if (userinfo != null)
            {
                intLocId = userinfo.intLocId;
                intCountryId = _tenderInfo.GetCountryWithCity(intLocId).CountryId;
                intStateId = _tenderInfo.GetStateWithCity(intLocId).StateId;
            }

            #region Country
            var country = _tenderInfo.ListCountry().ToList();
            var countryList = new List<SelectListItem>();
            countryList.Insert(0, (new SelectListItem { Text = "[COUNTRY]", Value = "0" }));
            for (var i = 1; i <= country.Count(); i++)
            {
                countryList.Insert(i, new SelectListItem
                {
                    Selected = country[i - 1].CountryId == intCountryId,
                    Text = country[i - 1].CountryName,
                    Value = country[i - 1].CountryId.ToString(CultureInfo.InvariantCulture)
                });
            }

            #endregion

            #region State
            var states = _tenderInfo.ListState().ToList();
            var stateList = new List<SelectListItem>();
            stateList.Insert(0, (new SelectListItem { Text = "[STATE]", Value = "0" }));
            for (var i = 1; i <= states.Count(); i++)
            {
                stateList.Insert(i, new SelectListItem
                {
                    Selected = states[i - 1].StateId == intStateId,
                    Text = states[i - 1].StateName,
                    Value = states[i - 1].StateId.ToString(CultureInfo.InvariantCulture)
                });
            }
            #endregion

            #region City
            var city = _tenderInfo.ListAllCity().ToList();
            var cityList = new List<SelectListItem>();
            cityList.Insert(0, (new SelectListItem { Text = "[CITY]", Value = "0" }));
            for (var i = 1; i <= city.Count(); i++)
            {
                cityList.Insert(i, new SelectListItem
                {
                    Selected = city[i - 1].LocId == intLocId,
                    Text = city[i - 1].Location,
                    Value = city[i - 1].LocId.ToString(CultureInfo.InvariantCulture)
                });
            }
            #endregion

            var tenderDetail = new TenderDetail
            {
                LoginUserDetails = userinfo,
                CountryList = countryList,
                StateList = stateList,
                CityList = cityList
            };

            return View(tenderDetail);
        }
        [HttpPost]
        public ActionResult PostEditProfile(FormCollection coll)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }

            var msg = "ok";
            var userId = Convert.ToInt32(Session["ClientId"].ToString());
            try
            {
                var userinfo = _userInfo.GetUsersById(userId);
                if (userinfo != null)
                {
                    var loginid = userinfo.strEmailId1;
                    var password = userinfo.strPassword;

                    var strFName = coll.Get("LoginUserDetails.strFName");
                    var strLName = coll.Get("LoginUserDetails.strLName");
                    var strCmpName = coll.Get("LoginUserDetails.strCmpName");
                    var strDesignation = coll.Get("LoginUserDetails.strDesignation");
                    var strAddress = coll.Get("LoginUserDetails.strAddress");

                    var intCountryId = Convert.ToInt32(coll.Get("drpCountry"));
                    var country = _tenderInfo.GetCountry(intCountryId).CountryName;

                    var intStateId = Convert.ToInt32(coll.Get("drpState"));
                    var state = _tenderInfo.GetState(intStateId).StateName;

                    var intLocId = Convert.ToInt32(coll.Get("drpCity"));
                    var city = _tenderInfo.GetCity(intLocId).Location;

                    var intPinCode = Convert.ToInt32(coll.Get("LoginUserDetails.intPinCode"));
                    var intPhoneNo = coll.Get("LoginUserDetails.intPhoneNo");
                    var strEmailId2 = coll.Get("LoginUserDetails.strEmailId2");

                    var userFormFields = strFName + "#-#" + strLName + "#-#" + strCmpName + "#-#" + strDesignation + "#-#" + strAddress + "#-#"
                                            + country + "#-#" + state + "#-#" + city + "#-#" + intPinCode + "#-#" + intPhoneNo + "#-#" + strEmailId2 + "#-#"
                                            + loginid + "#-#" + password;

                    var mailsubject = "TenderAssist247.com : Update User Information";

                    string fromId = ConfigurationManager.AppSettings["SMTP_EmailId"].ToString(CultureInfo.InvariantCulture);
                    string adminId = ConfigurationManager.AppSettings["CareEMailId"].ToString(CultureInfo.InvariantCulture);

                    /*SEND MAIL TO USER*/
                    var usermail = userinfo.strEmailId1.Trim();
                    var messageBody = _common.MailUpdateUserInformation(userFormFields, true);
                    messageBody = _common.CreateMail(userId, strFName + " " + strLName, "", "", messageBody, fromId, "", "live");
                    Utility.SendMail(usermail, "", "", mailsubject, messageBody, "");

                    /*SEND MAIL TO ADMIN*/
                    messageBody = _common.MailUpdateUserInformation(userFormFields, false);
                    messageBody = _common.CreateMail(userId, "Admin", "", "", messageBody, fromId, "", "live");
                    Utility.SendMail(adminId, "", "", mailsubject, messageBody, "");
                }
            }
            catch (Exception ex)
            {
                msg = "error";
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStateByCountry(int CountryID)
        {
            #region State
            var states = _tenderInfo.ListStateByCountry(CountryID).ToList();
            var StateList = new List<SelectListItem>();
            StateList.Insert(0, (new SelectListItem { Text = "[STATE]", Value = "0" }));
            for (var i = 1; i <= states.Count(); i++)
            {
                StateList.Insert(i, new SelectListItem
                {
                    Text = states[i - 1].StateName,
                    Value = states[i - 1].StateId.ToString(CultureInfo.InvariantCulture)
                });
            }
            #endregion
            return Json(StateList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCityByState(int StateID)
        {
            #region City
            var city = _tenderInfo.ListCityByState(StateID).ToList();
            var CityList = new List<SelectListItem>();
            CityList.Insert(0, (new SelectListItem { Text = "[CITY]", Value = "0" }));
            for (var i = 1; i <= city.Count(); i++)
            {
                CityList.Insert(i, new SelectListItem
                {
                    Text = city[i - 1].Location,
                    Value = city[i - 1].LocId.ToString(CultureInfo.InvariantCulture)
                });
            }
            #endregion
            return Json(CityList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region RESTRICTED

        public ActionResult Restricted(string Error)
        {
            ViewBag.ErrorTitle = Error;
            return View();
        }

        #endregion

        #region CHANGE PASSWORD
        public ActionResult ChangePassword()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            ClearSession();

            var tenderDetail = new TenderDetail
            {
                LoginUserDetails = userinfo
            };
            return View(tenderDetail);
        }
        [HttpPost]
        public ActionResult PostChangePassword(FormCollection collection)
        {
            string msg;
            string errormsg;

            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }

            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            try
            {
                if (userinfo == null)
                    return RedirectToAction("ChangePassword", "User", new { errorMsg = "done" });

                var currentPassword = collection.Get("CurrentPassword");
                var newPassword = collection.Get("NewPassword");

                if (currentPassword != userinfo.strPassword)
                {
                    errormsg = "The current password is incorrect";
                    msg = "error";
                    return Json(new { msg, errormsg }, JsonRequestBehavior.AllowGet);
                }
                if (newPassword.Trim().Length < 6)
                {
                    errormsg = "'Password' must be at least 6 characters long.";
                    msg = "error";
                    return Json(new { msg, errormsg }, JsonRequestBehavior.AllowGet);
                }

                userinfo.strPassword = newPassword;
                _userInfo.Update();

                var username = userinfo.strFName + " " + userinfo.strLName;
                var loginid = userinfo.strEmailId1;
                var password = userinfo.strPassword;

                var userFormFields = username + "#-#" + loginid + "#-#" + password;

                /*SEND MAIL TO USER*/

                var mailsubject = "TenderAssist247.com : Update User Credentials";

                string fromId = ConfigurationManager.AppSettings["SMTP_EmailId"].ToString(CultureInfo.InvariantCulture);
                string adminId = ConfigurationManager.AppSettings["CareEMailId"].ToString(CultureInfo.InvariantCulture);

                var usermail = userinfo.strEmailId1.Trim();
                string messageBody = _common.MailUpdateUserPasswrod(userFormFields, true);
                messageBody = _common.CreateMail(userId, username, "", "", messageBody, fromId, "", "live");

                Utility.SendMail(usermail, "", "", mailsubject, messageBody, "");

                /*SEND MAIL TO ADMIN*/
                messageBody = _common.MailUpdateUserPasswrod(userFormFields, false);
                messageBody = _common.CreateMail(userId, "Admin", "", "", messageBody, fromId, "", "live");
                Utility.SendMail(adminId, "", "", mailsubject, messageBody, "");

                errormsg = "Congratulations! Your password changed successfully.";
                msg = "ok";
            }
            catch (Exception ex)
            {
                errormsg = "Sorry! Your password does not changed, please try again.";
                msg = "error";
            }

            return Json(new { msg, errormsg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FEEDBACK | RENEWAL
        public ActionResult Renewal()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            ClearSession();

            var tenderDetail = new TenderDetail
            {
                LoginUserDetails = userinfo,
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.Renewal,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.Renewal,
                FormType = FormType.OtherForms
            };
            return View(tenderDetail);
        }
        public ActionResult Feedback()
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            ClearSession();

            var totalContactArea = Convert.ToInt32(ConfigurationManager.AppSettings["TotalContactAreas"]);
            var lsts = new List<SelectListItem>();
            for (var i = 1; i <= totalContactArea; i++)
            {
                var name = "TotalContactArea_" + i;
                lsts.Insert(i - 1,
                            (new SelectListItem
                            {
                                Text = ConfigurationManager.AppSettings[name],
                                Value = i.ToString(CultureInfo.InvariantCulture)
                            }));
            }
            ViewData["ContactDept"] = new SelectList(lsts, "Value", "Text");

            var tenderDetail = new TenderDetail
            {
                LoginUserDetails = userinfo
            };

            return View(tenderDetail);
        }
        [HttpPost]
        public ActionResult PostFeedback(FormCollection collection)
        {
            string msg;
            string errormsg;

            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }

            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            var username = "";
            var stateid = 0;

            if (userinfo != null)
            {
                username = userinfo.strFName + " " + userinfo.strLName;
                stateid = _tenderInfo.GetStateWithCity(userinfo.intLocId).StateId;
            }
            var IsFeedback = collection.Get("IsFeedback");
            var name = collection.Get("Name");
            var email = collection.Get("Email");
            var subject = collection.Get("Subject");
            var suggestion = collection.Get("Suggestion");
            var contactArea = "";

            if (IsFeedback == "1")
            {
                var contarea = "TotalContactArea_" + collection.Get("ContactArea");
                contactArea = ConfigurationManager.AppSettings[contarea];
            }


            try
            {
                #region INSERT INTO INQUIRY TABLE

                InquiryRegFormFields RegFormParams = new InquiryRegFormFields()
                {
                    InquiryTypeID = SubscribType.ClientFeedback,
                    intClientPurpose = 1,
                    OurRefNo = 0,
                    NewID = 0,
                    //ModuleType = Convert.ToInt32(coll.Get("ModuleType")),

                    Name = name,
                    EmailID = email,
                    InterestedTenders = suggestion,
                    State = stateid,
                    FormTitle = SubscribTypeDisplsyText.ClientFeedback,

                };

                var userEmail = RegFormParams.EmailID;

                var userName = RegFormParams.Name;
                var userContactNo = RegFormParams.MobNo.ToString();
                var userProductInfo = string.IsNullOrEmpty(RegFormParams.InterestedTenders) ? "" : RegFormParams.InterestedTenders;

                var FormTitle = RegFormParams.FormTitle;

                var inqFormUserId = _common.SubmitInquiryRegForms(RegFormParams);

                #endregion

                var userFormFields = name + "#-#" + email + "#-#" + subject + "#-#" + contactArea + "#-#" + suggestion + "#-#" + username;

                var mailsubject = "TenderAssist247.com : Feedback - Suggestion";

                /*SEND MAIL TO ADMIN*/


                string fromId = ConfigurationManager.AppSettings["SMTP_EmailId"].ToString(CultureInfo.InvariantCulture);
                string adminId = ConfigurationManager.AppSettings["CareEMailId"].ToString(CultureInfo.InvariantCulture);

                var messageBody = _common.MailSendFeedback(userFormFields);
                messageBody = _common.CreateMail(userId, "Admin", "", "", messageBody, fromId, "", "live");

                var isMailSend = Utility.SendMail(adminId, "", "", mailsubject, messageBody.ToString(CultureInfo.InvariantCulture), "");

                if (!isMailSend)
                {
                    errormsg = "Your Suggestion about " + contactArea + " could not send, please try again";
                    msg = "error";
                    return Json(new { msg, errormsg }, JsonRequestBehavior.AllowGet);
                }

                errormsg = "Your Suggestion about " + contactArea + " has been sent successfully.";
                msg = "ok";

            }
            catch (Exception ex)
            {
                errormsg = "Your Suggestion about " + contactArea + " could not be sent, please try again";
                msg = "error";
            }

            return Json(new { msg, errormsg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public TenderDetail SearchIndianGlobalTenders(int permissionId, string searchText, string product1Ids = "", string product2Ids = "", string product3Ids = "", string locationIds = "", string indSubindIds = "",
          string agencyIds = "", string sectorIds = "", string ownershipIds = "", int? tenderStatusFlag = 0, DateTime? enterDt = null, int page = 1, int pageSize = 20, bool isSearchWithCount = true,
           int? ourRefNo = 0, int? tenderType = 0, int? icbNcb = 0, decimal? tenderValueFrom = 0, decimal? tenderValueTo = 0, string subDateFrom = "", string subDateTo = "", string opDateFrom = "", string opDateTo = "",
           string searchFilterText = "", Boolean isIndianGlobal = true, string otherKeywords = "", int RegionId = 0, string countries = "")
        {
            var userId = Convert.ToInt32(Session["ClientID"]);
            var userinfo = (tabClientDetail)Session["ClientInfo"];

            #region CRITERIA LIST

            var listCriteria = _userTenderPermissionDetail.GetIndianTenderPermissionList(userId).ToList();

            if (permissionId == 0)
            {
                if (listCriteria.Any())
                {
                    var tabClientPermission = listCriteria.FirstOrDefault();
                    if (tabClientPermission != null)
                        permissionId = tabClientPermission.intPermissionId;
                }
            }

            #endregion

            var withinSearchedTexts = "";
            var displayKeywordlist = "";

            if (Session["WithinSearchText"] != null)
            {
                WithinSearchWords = (List<string>)Session["WithinSearchText"];
            }
            var DisplayText = "";
            withinSearchedTexts = string.Join(", ", WithinSearchWords);
            if (WithinSearchWords.Any())
            { DisplayText = WithinSearchWords.LastOrDefault().ToString(); }


            Int64 totalLive = 0;
            Int64 totalFresh = 0;
            Int64 totalClosed = 0;
            Int64 totalCount = 0;
            long totalAll = 0;

            var notUsedKeywords = string.Empty;
            var documentType = string.Empty;

            var finalSearchText = string.Empty;
            var tenderValue = string.Empty;

            var tenaderInfoWithDetail = new List<SearchTenaderInfoWithAllDetail>();

            string permissionGroupName = "";
            Int64 permissionGroupTenders = 0;

            page = page == 1 ? 0 : page;
            var allSearchText = "";

            DateTime? subDateFromDt = null;
            DateTime? subDateToDt = null;
            DateTime? opDateFromDt = null;
            DateTime? opDateToDt = null;

            if (!string.IsNullOrEmpty(subDateFrom))
            {
                subDateFromDt = Convert.ToDateTime(subDateFrom);
            }
            if (!string.IsNullOrEmpty(subDateTo))
            {
                subDateToDt = Convert.ToDateTime(subDateTo);
            }
            if (!string.IsNullOrEmpty(opDateFrom))
            {
                opDateFromDt = Convert.ToDateTime(opDateFrom);
            }
            if (!string.IsNullOrEmpty(opDateTo))
            {
                opDateToDt = Convert.ToDateTime(opDateTo);
            }

            #region LOCATIONS [STATE & CITY]

            var allstateId = "";
            var allcityid = "";
            var allcountryid = "";

            if (locationIds == null) { locationIds = ""; }
            var locationIdList = locationIds.Split(',');

            var allStatesAvail = new List<int>();
            var allCitiesAvail = new List<int>();
            var allCountryAvail = new List<int>();


            var allglobalCountryIdsForSearch = "";
            if (!isIndianGlobal && RegionId != 0)
            {
                var countrylist = _tenderInfo.ListCountrybyRegion(RegionId).Select(x => x.CountryId).ToList();
                if (countrylist.Any())
                {
                    allglobalCountryIdsForSearch = String.Join(",", countrylist.ToArray());
                }
            }
            if (!string.IsNullOrEmpty(countries) && !isIndianGlobal)
            { allglobalCountryIdsForSearch = allglobalCountryIdsForSearch == "" ? countries : allglobalCountryIdsForSearch + "," + countries; }


            foreach (var lstItem in locationIdList)
            {
                if (lstItem.Trim() == "")
                {
                    continue;
                }

                var iscityfound = false;

                var locations = lstItem.Split('#');
                var cityid = Convert.ToInt32(locations[0]);
                var stateId = Convert.ToInt32(locations[1]);
                var countryId = Convert.ToInt32(locations[2]);

                if (allCitiesAvail.Contains(cityid) == false && cityid != 0)
                {
                    iscityfound = true;
                    allCitiesAvail.Add(cityid);
                    allcityid = allcityid == ""
                        ? cityid.ToString(CultureInfo.InvariantCulture)
                        : allcityid + "," + cityid;
                }

                if (!iscityfound)
                {
                    if (allStatesAvail.Contains(stateId) == false && stateId != 0)
                    {
                        allStatesAvail.Add(stateId);
                        allstateId = allstateId == ""
                            ? stateId.ToString(CultureInfo.InvariantCulture)
                            : allstateId + "," + stateId;

                    }
                }


                if (allCountryAvail.Contains(countryId) == false && countryId != 0)
                {
                    allCountryAvail.Add(countryId);
                    allcountryid = allcountryid == ""
                        ? countryId.ToString(CultureInfo.InvariantCulture)
                        : allcountryid + "," + countryId;
                }
                if (!string.IsNullOrEmpty(allcountryid) && !isIndianGlobal)
                { allglobalCountryIdsForSearch = allglobalCountryIdsForSearch == "" ? allcountryid : allglobalCountryIdsForSearch + "," + allcountryid; }
                
            }

            if (!string.IsNullOrEmpty(allglobalCountryIdsForSearch) && !isIndianGlobal)
            {
                if (allglobalCountryIdsForSearch.LastIndexOf(",") == allglobalCountryIdsForSearch.Length - 1)
                { allglobalCountryIdsForSearch = allglobalCountryIdsForSearch.Remove(allglobalCountryIdsForSearch.Length - 1, 1); }
            }
            #endregion

            #region PRODUCT

            var allproductId = "";
            var allproductId1 = string.Empty;
            var allproductId2 = string.Empty;
            var allproductId3 = string.Empty;

            var productId = 0;
            var isFirstProduct = true;
            var searchFilterProduct = "";
            var setKeywordProducts = "";
            var setKeywordProductName = "";

            if (!string.IsNullOrEmpty(product1Ids))
            {
                var allselectedprods = Regex.Split(product1Ids, ",");
                foreach (var t in allselectedprods)
                {
                    if (t == "") continue;
                    if (productId == Convert.ToInt32(t)) continue;

                    productId = Convert.ToInt32(t);
                    if (productId == 0) continue;

                    allproductId = (allproductId == "")
                        ? productId.ToString(CultureInfo.InvariantCulture)
                        : allproductId + "," + productId;

                    if (productId != 0)
                    {

                        var productName = _tenderInfo.GetProductById(productId).ProductsName + " Tenders";
                        if (isFirstProduct)
                        {
                            isFirstProduct = false;
                            setKeywordProductName = productName;
                        }
                        else
                        {
                            setKeywordProductName = setKeywordProductName + "," + productName;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(product2Ids))
            {
                var allselectedprods = Regex.Split(product2Ids, ",");
                foreach (var t in allselectedprods)
                {
                    if (t == "") continue;
                    if (productId == Convert.ToInt32(t)) continue;

                    productId = Convert.ToInt32(t);
                    if (productId == 0) continue;

                    allproductId2 = (allproductId2 == "")
                        ? productId.ToString(CultureInfo.InvariantCulture)
                        : allproductId2 + "," + productId;

                    if (productId != 0)
                    {

                        var productName = _tenderInfo.GetProductById(productId).ProductsName + " Tenders";
                        if (isFirstProduct)
                        {
                            isFirstProduct = false;
                            setKeywordProductName = productName;
                        }
                        else
                        {
                            setKeywordProductName = setKeywordProductName + "," + productName;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(product3Ids))
            {
                var allselectedprods = Regex.Split(product3Ids, ",");
                foreach (var t in allselectedprods)
                {
                    if (t == "") continue;
                    if (productId == Convert.ToInt32(t)) continue;

                    productId = Convert.ToInt32(t);
                    if (productId == 0) continue;

                    allproductId3 = (allproductId3 == "")
                        ? productId.ToString(CultureInfo.InvariantCulture)
                        : allproductId3 + "," + productId;

                    if (productId != 0)
                    {

                        var productName = _tenderInfo.GetProductById(productId).ProductsName + " Tenders";
                        if (isFirstProduct)
                        {
                            isFirstProduct = false;
                            setKeywordProductName = productName;
                        }
                        else
                        {
                            setKeywordProductName = setKeywordProductName + "," + productName;
                        }
                    }
                }
            }

            if (setKeywordProducts != "")
            {
                displayKeywordlist = setKeywordProductName.Replace(" Tenders", "");
                searchFilterProduct = setKeywordProducts;
            }

            #endregion

            #region CATEGORIES [INDUSTRY & SUBINDUSTRY]

            var allindustryId = "";
            var allsubindustryid = "";

            if (indSubindIds == null) { indSubindIds = ""; }
            var indsubindIdList = indSubindIds.Split(',');
            var searchFilterIndustry = "";
            var searchFilterSubIndustry = "";

            var isFirstIndustry = true;
            var isFirstSubIndustry = true;

            var allIndustryAvail = new List<int>();
            var allSubIndustryAvail = new List<int>();

            var setKeywordIndustry = "";
            var setKeywordSubIndustry = "";

            foreach (var lstItem in indsubindIdList)
            {
                if (lstItem.Trim() == "")
                {
                    continue;
                }

                var categories = lstItem.Split('#');
                var subindId = Convert.ToInt32(categories[0]);
                var indid = Convert.ToInt32(categories[1]);

                if (allIndustryAvail.Contains(indid) == false && indid != 0)
                {
                    allIndustryAvail.Add(indid);
                    allindustryId = allindustryId == ""
                        ? indid.ToString(CultureInfo.InvariantCulture)
                        : allindustryId + "," + indid;

                    if (indid != 0)
                    {
                        //var industryName = _tenderInfo.GetIndustryById(indid).IndustryName + " Tenders";
                        if (isFirstIndustry)
                        {
                            isFirstIndustry = false;
                        }
                    }
                }

                if (allSubIndustryAvail.Contains(subindId) == false && subindId != 0)
                {
                    allSubIndustryAvail.Add(subindId);
                    allsubindustryid = allsubindustryid == ""
                        ? subindId.ToString(CultureInfo.InvariantCulture)
                        : allsubindustryid + "," + subindId;

                    if (subindId != 0)
                    {
                        //var subIndustryName = _tenderInfo.GetSubIndById(subindId).SubIndustryName + " Tenders";
                        if (isFirstSubIndustry)
                        {
                            isFirstSubIndustry = false;
                        }
                    }
                }
            }

            if (setKeywordIndustry != "")
            {
                searchFilterIndustry = setKeywordIndustry;
            }
            if (setKeywordSubIndustry != "")
            {
                searchFilterSubIndustry = setKeywordSubIndustry;
            }

            #endregion

            #region AGENCY

            var allagencyId = "";
            var agencyId = 0;
            var isFirstAgency = true;
            var searchFilterAgency = "";
            var setKeywordSubAgency = "";

            if (!string.IsNullOrEmpty(agencyIds))
            {
                var allselectedagencys = Regex.Split(agencyIds, ",");
                foreach (var t in allselectedagencys)
                {
                    if (t == "") continue;
                    if (agencyId == Convert.ToInt32(t)) continue;

                    agencyId = Convert.ToInt32(t);
                    if (agencyId == 0) continue;

                    allagencyId = (allagencyId == "")
                        ? agencyId.ToString(CultureInfo.InvariantCulture)
                        : allagencyId + "," + agencyId;

                    if (agencyId != 0)
                    {
                        //var agencyName = _tenderInfo.GetAgencyDetailById(agencyId).AgencyName + " Tenders";
                        if (isFirstAgency)
                        {
                            isFirstAgency = false;
                        }
                    }
                }
                if (setKeywordSubAgency != "")
                {
                    searchFilterAgency = setKeywordSubAgency;
                }
            }

            #endregion

            #region SECTOR

            var allsectorId = "";
            var sectorId = 0;
            var isFirstSector = true;
            var searchFilterSector = "";
            var setKeywordSubSector = "";

            if (!string.IsNullOrEmpty(sectorIds))
            {
                var allselectedsectors = Regex.Split(sectorIds, ",");
                foreach (var t in allselectedsectors)
                {
                    if (t == "") continue;
                    if (sectorId == Convert.ToInt32(t)) continue;

                    sectorId = Convert.ToInt32(t);
                    if (sectorId == 0) continue;

                    allsectorId = (allsectorId == "")
                        ? sectorId.ToString(CultureInfo.InvariantCulture)
                        : allsectorId + "," + sectorId;

                    if (sectorId != 0)
                    {
                        //var sectorName = _tenderInfo.GetSectorById(sectorId).SectorName + " Tenders";
                        if (isFirstSector)
                        {
                            isFirstSector = false;
                        }
                    }
                }
                if (setKeywordSubSector != "")
                {
                    searchFilterSector = setKeywordSubSector;
                }
            }

            #endregion

            #region OWNERSHIP

            var allownershipId = "";
            var ownershipId = 0;
            var isFirstOwnership = true;
            var searchFilterOwnership = "";
            var setKeywordSubOwnership = "";

            if (!string.IsNullOrEmpty(ownershipIds))
            {
                var allselectedownerships = Regex.Split(ownershipIds, ",");
                foreach (var t in allselectedownerships)
                {
                    if (t == "") continue;
                    if (ownershipId == Convert.ToInt32(t)) continue;

                    ownershipId = Convert.ToInt32(t);
                    if (ownershipId == 0) continue;

                    allownershipId = (allownershipId == "")
                        ? ownershipId.ToString(CultureInfo.InvariantCulture)
                        : allownershipId + "," + ownershipId;

                    if (ownershipId != 0)
                    {
                        //var ownershipName = _tenderInfo.GetOwnershipById(ownershipId).OwnershipName + " Tenders";
                        if (isFirstOwnership)
                        {
                            isFirstOwnership = false;
                        }
                    }
                }
                if (setKeywordSubOwnership != "")
                {
                    searchFilterOwnership = setKeywordSubOwnership;
                }
            }

            #endregion

            int? tStatusFlag = 0;
            if (permissionId != 0)
            {
                var objtbMemberUserTenderPermission = _userTenderPermissionDetail.GetUsersTenderPermissionById(permissionId);
                permissionGroupName = objtbMemberUserTenderPermission.strPermissionName;

                var tenaderList = _tenderInfo.GetAllSearchTenderInfo_Client(permissionId, page, pageSize,
                    tenderStatusFlag == 0 ? null : tenderStatusFlag,
                    enterDt, searchText, ourRefNo == 0 ? null : ourRefNo, ownershipIds, sectorIds, agencyIds, indSubindIds, locationIds,
                    product1Ids, product2Ids, product3Ids, otherKeywords, notUsedKeywords, documentType, icbNcb == -1 ? null : icbNcb.ToString(),
                    tenderValue, tenderValueFrom == 0 ? null : tenderValueFrom, tenderValueTo == 0 ? null : tenderValueTo, isIndianGlobal, true, withinSearchedTexts,
                    subDateFromDt, subDateToDt, opDateFromDt, opDateToDt, allglobalCountryIdsForSearch);

                tenaderInfoWithDetail = tenaderList.TenaderDetailSearch;

                tStatusFlag = tenderStatusFlag == null ? 0 : tenderStatusFlag;
                if (isSearchWithCount)
                {
                    List<TenderCount> tenderCountList = tenaderList.TenaderDetailCount;
                    if (tenderCountList.Any())
                    {
                        totalCount = 0;
                        totalAll = tenderCountList.Sum(t => t.TotalRecord);
                        if (tStatusFlag == 0)
                        {
                            totalCount += tenderCountList.Sum(t => t.TotalRecord);
                        }
                        else
                        {
                            var det = tenderCountList.FirstOrDefault(x => x.TenderStatusReturn == tStatusFlag);
                            totalCount = det != null ? det.TotalRecord : 0;
                        }

                        //ACTIVE
                        var detActive = tenderCountList.FirstOrDefault(x => x.TenderStatusReturn == 1);
                        totalLive = detActive != null ? detActive.TotalRecord : 0;

                        //NEW
                        var detNew = tenderCountList.FirstOrDefault(x => x.TenderStatusReturn == 2);
                        totalFresh = detNew != null ? detNew.TotalRecord : 0;

                        //CLOSED
                        var detClosed = tenderCountList.FirstOrDefault(x => x.TenderStatusReturn == 3);
                        totalClosed = detClosed != null ? detClosed.TotalRecord : 0;
                    }
                }
                Session["Client_TotalAllTenders"] = totalAll.ToString(CultureInfo.InvariantCulture);
                Session["Client_TotalSearchedTenders"] = totalCount.ToString(CultureInfo.InvariantCulture);
                Session["Client_TotalLiveTenders"] = totalLive.ToString(CultureInfo.InvariantCulture);
                Session["Client_TotalFreshTenders"] = totalFresh.ToString(CultureInfo.InvariantCulture);
                Session["Client_TotalClosedTenders"] = totalClosed.ToString(CultureInfo.InvariantCulture);

                Session["Client_TotalRecordCount"] = totalCount;
                permissionGroupTenders = totalCount;
            }

            Session["IndianGlobal"] = isIndianGlobal;
            var total = Convert.ToInt64(Session["Client_TotalSearchedTenders"]);
            totalAll = Convert.ToInt64(Session["Client_TotalAllTenders"]);
            totalLive = Convert.ToInt64(Session["Client_TotalLiveTenders"]);
            totalFresh = Convert.ToInt64(Session["Client_TotalFreshTenders"]);
            totalClosed = Convert.ToInt64(Session["Client_TotalClosedTenders"]);

            var heighlightText = searchText;
            var searchType = 1;

            var totalData = total;

            displayKeywordlist = displayKeywordlist == "" ? withinSearchedTexts : displayKeywordlist + "," + withinSearchedTexts;

            var tenderDetail = new TenderDetail
            {
                AllSearchTenaderInfoWithAllDetail = tenaderInfoWithDetail,
                DisplayText = searchText,
                Newpagecnt = page,
                SelectedIndustrySubIndustry = indSubindIds,
                SelectedLocation = locationIds,
                AdvanceSearchText = allSearchText,
                SelectedCountry = allglobalCountryIdsForSearch ?? "",
                SelectedState = allstateId ?? "",
                SelectedCity = allcityid ?? "",
                SelectedProduct = allproductId ?? "",
                SelectedIndustry = allindustryId ?? "",
                SelectedSubIndustry = allsubindustryid ?? "",
                SelectedAgency = allagencyId ?? "",
                SelectedSector = allsectorId ?? "",
                SelectedOwnership = allownershipId ?? "",
                SelectedLocationIds = locationIds,
                SelectedIndustrySubIndustryIds = indSubindIds,
                SelectedKeyword1 = allproductId1,
                SelectedKeyword2 = allproductId2,
                SelectedKeyword3 = allproductId3,
                OtherKeywords = otherKeywords,
                NotUsedKeywords = notUsedKeywords,
                DocumentType = documentType,
                TenderValue = tenderValue,
                PermissionId = permissionId,
                TotalLive = totalLive,
                TotalFresh = totalFresh,
                TotalClosed = totalClosed,
                Total = totalData,
                PageSize = pageSize,
                UserId = userId,
                TenderStatus = tStatusFlag.Value,
                TenderStatusFlag = tStatusFlag.Value,
                EnterDt = enterDt,
                PermissionGroupName = permissionGroupName,
                PermissionGroupTenders = permissionGroupTenders,
                DisplayText2 = displayKeywordlist,
                DisplayText3 = finalSearchText,
                SearchType = searchType,
                SearchFilterText = searchFilterText,
                ClientPermissionList = listCriteria,
                RegionId = RegionId

                // OurRefNo = ourRefNo == null ? 0 : ourRefNo.Value,
                //AdvanceSearchType = advanceSearchType.Value,
                //AdvanceSearchValue = advanceSearchValue,
                //TenderValueFrom = tenderValueFrom != null ? tenderValueFrom.ToString() : "",// Convert.ToDouble(tenderValueFrom),
                //TenderValueTo = tenderValueTo != null ? tenderValueTo.ToString() : "",//Convert.ToDouble(tenderValueTo),
                //TenderSubDateFrom = subDateFrom,
                //TenderSubDateTo = subDateTo,
                //TenderOpDateFrom = opDateFrom,
                //TenderOpDateTo = opDateTo,
                //DisplayText = searchText,
                //SelectedOwnership = ownershipIds,                 
                //SelectedSector = sectorIds,                
                //SelectedAgency = agencyIds,
                //IsIcbNcb = icbNcb.ToString(),
                //ICBNCB = icbNcb == null ? -1 : icbNcb.Value,
                //TotalAll = totalAll,
                //AllUserFavouriteTenderList = allUserFavouriteTenderList,
                //IsIndianGlobal = isIndianGlobal,


            };

            return tenderDetail;
        }



        public int SetTenderStatusFlag(string tenderStatus)
        {
            int tenderStatusFlag;
            if (tenderStatus == null) { tenderStatusFlag = (Int16)TenderStatusFlags.AllTenders; }
            else
            {
                if (tenderStatus.ToLower().Trim() == TenderStatusValues.NewTenders.ToLower().Trim())
                { tenderStatusFlag = (Int16)TenderStatusFlags.NewTenders; }
                else if (tenderStatus.ToLower().Trim() == TenderStatusValues.LiveTenders.ToLower().Trim())
                { tenderStatusFlag = (Int16)TenderStatusFlags.LiveTenders; }
                else if (tenderStatus.ToLower().Trim() == TenderStatusValues.ClosedTenders.ToLower().Trim())
                { tenderStatusFlag = (Int16)TenderStatusFlags.ClosedTenders; }
                else
                { tenderStatusFlag = (Int16)TenderStatusFlags.AllTenders; }
            }
            SetTenderStatus(tenderStatusFlag);

            return tenderStatusFlag;
        }
        public string SetTenderStatus(int tenderStatusFlag)
        {
            string tenderStatus = "";
            switch (tenderStatusFlag)
            {
                case (Int16)TenderStatusFlags.NewTenders:
                    tenderStatus = TenderStatusValues.NewTenders;
                    break;
                case (Int16)TenderStatusFlags.LiveTenders:
                    tenderStatus = TenderStatusValues.LiveTenders;
                    break;
                case (Int16)TenderStatusFlags.ClosedTenders:
                    tenderStatus = TenderStatusValues.ClosedTenders;
                    break;
                case (Int16)TenderStatusFlags.AllTenders:
                    tenderStatus = TenderStatusValues.AllTenders;
                    break;
                default:
                    tenderStatus = TenderStatusValues.AllTenders;
                    break;
            }
            Session["TenderStatus"] = tenderStatus;
            return tenderStatus;
        }

        public ActionResult GetPermissionWithProduct(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbProduct> PermissionWithProduct = new List<tbProduct>();
            var sessionKeyName = "PermissionWithProduct_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithProduct = (from assignproduct in _userTenderPermissionDetail.GetUserPermissionWithProduct(permissionId)
                                         join products in _tenderInfo.ListAllProducts() on assignproduct.intProductId equals products.ProductsId
                                         select new tbProduct
                                         {
                                             ProductsId = products.ProductsId,
                                             ProductsName = products.ProductsName
                                         }).ToList();
                Session[sessionKeyName] = PermissionWithProduct;
            }
            else
            {
                PermissionWithProduct = (List<tbProduct>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllProducts = PermissionWithProduct,
                PermissionId = permissionId
            };
            if (PermissionWithProduct.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/ByKeyword.cshtml"), tenderDetail);
            }
            return null;
        }
        public ActionResult GetPermissionWithCity(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbLocation> PermissionWithCity = new List<tbLocation>();
            var sessionKeyName = "PermissionWithCity_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithCity = (from assigncity in _userTenderPermissionDetail.GetUserPermissionWithLocation(permissionId)
                                      join city in _tenderInfo.ListAllCity() on assigncity.intCityId equals city.LocId
                                      select new tbLocation
                                      {
                                          LocId = city.LocId,
                                          Location = city.Location
                                      }).ToList();
                Session[sessionKeyName] = PermissionWithCity;
            }
            else
            {
                PermissionWithCity = (List<tbLocation>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllCityList = PermissionWithCity,
                PermissionId = permissionId
            };
            if (PermissionWithCity.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/ByCity.cshtml"), tenderDetail);
            }
            return null;

        }
        public ActionResult GetPermissionWithState(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbState> PermissionWithState = new List<tbState>();
            var sessionKeyName = "PermissionWithState_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithState = (from assignstate in _userTenderPermissionDetail.GetUserPermissionWithLocation(permissionId)
                                       join state in _tenderInfo.ListState() on assignstate.intStateId equals state.StateId
                                       select new tbState
                                       {
                                           StateId = state.StateId,
                                           StateName = state.StateName
                                       }).ToList();
                Session[sessionKeyName] = PermissionWithState;
            }
            else
            {
                PermissionWithState = (List<tbState>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllStateList = PermissionWithState,
                PermissionId = permissionId
            };
            if (PermissionWithState.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/ByState.cshtml"), tenderDetail);
            }
            return null;

        }
        public ActionResult GetPermissionWithAgency(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbAgencyIndian> PermissionWithAgency = new List<tbAgencyIndian>();
            var sessionKeyName = "PermissionWithAgency_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithAgency = (from assignagency in _userTenderPermissionDetail.GetUserPermissionWithAgency(permissionId)
                                        join agency in _tenderInfo.ListAgencyMaster() on assignagency.intAgencyId equals agency.AgencyId
                                        select new tbAgencyIndian
                                        {
                                            AgencyId = agency.AgencyId,
                                            AgencyName = agency.AgencyName
                                        }).ToList();
                Session[sessionKeyName] = PermissionWithAgency;
            }
            else
            {
                PermissionWithAgency = (List<tbAgencyIndian>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllAgency = PermissionWithAgency,
                PermissionId = permissionId
            };
            if (PermissionWithAgency.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/ByAgency.cshtml"), tenderDetail);
            }
            return null;

        }
        public ActionResult GetPermissionWithSector(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbSector> PermissionWithSector = new List<tbSector>();
            var sessionKeyName = "PermissionWithSector_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithSector = (from assignsector in _userTenderPermissionDetail.GetUserPermissionWithSector(permissionId)
                                        join sector in _tenderInfo.ListSector() on assignsector.intSectorId equals sector.SectorId
                                        select new tbSector
                                        {
                                            SectorId = sector.SectorId,
                                            SectorName = sector.SectorName
                                        }).ToList();
                Session[sessionKeyName] = PermissionWithSector;
            }
            else
            {
                PermissionWithSector = (List<tbSector>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllCompanySector = PermissionWithSector,
                PermissionId = permissionId
            };
            if (PermissionWithSector.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/BySector.cshtml"), tenderDetail);
            }
            return null;

        }
        public ActionResult GetPermissionWithOwnership(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbOwnership> PermissionWithOwnership = new List<tbOwnership>();
            var sessionKeyName = "PermissionWithOwnership_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithOwnership = (from assignownership in _userTenderPermissionDetail.GetUserPermissionWithOwnership(permissionId)
                                           join ownership in _tenderInfo.ListOwnership() on assignownership.intOwnershipId equals ownership.OwnershipId
                                           select new tbOwnership
                                           {
                                               OwnershipId = ownership.OwnershipId,
                                               OwnershipName = ownership.OwnershipName
                                           }).ToList();
                Session[sessionKeyName] = PermissionWithOwnership;
            }
            else
            {
                PermissionWithOwnership = (List<tbOwnership>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllOwnership = PermissionWithOwnership,
                PermissionId = permissionId
            };
            if (PermissionWithOwnership.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/ByOwnership.cshtml"), tenderDetail);
            }
            return null;

        }
        public ActionResult GetPermissionWithIndustry(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbIndustry> PermissionWithIndustry = new List<tbIndustry>();
            var sessionKeyName = "PermissionWithIndustry_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithIndustry = (from assignindustry in _userTenderPermissionDetail.GetUserPermissionWithIndSubIndustry(permissionId)
                                          join industry in _tenderInfo.ListIndustry() on assignindustry.intIndustryId equals industry.IndustryId
                                          select new tbIndustry
                                          {
                                              IndustryId = industry.IndustryId,
                                              IndustryName = industry.IndustryName
                                          }).ToList();
                Session[sessionKeyName] = PermissionWithIndustry;
            }
            else
            {
                PermissionWithIndustry = (List<tbIndustry>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllIndustry = PermissionWithIndustry,
                PermissionId = permissionId
            };
            if (PermissionWithIndustry.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/ByIndustry.cshtml"), tenderDetail);
            }
            return null;

        }
        public ActionResult GetPermissionWithSubIndustry(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbSubIndustry> PermissionWithSubIndustry = new List<tbSubIndustry>();
            var sessionKeyName = "PermissionWithSubIndustry_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithSubIndustry = (from assignsubIndustry in _userTenderPermissionDetail.GetUserPermissionWithIndSubIndustry(permissionId)
                                             join subIndustry in _tenderInfo.ListAllSubIndustry() on assignsubIndustry.intSubindustryId equals subIndustry.SubIndustryId
                                             select new tbSubIndustry
                                             {
                                                 SubIndustryId = subIndustry.SubIndustryId,
                                                 SubIndustryName = subIndustry.SubIndustryName
                                             }).ToList();
                Session[sessionKeyName] = PermissionWithSubIndustry;
            }
            else
            {
                PermissionWithSubIndustry = (List<tbSubIndustry>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllSubIndustry = PermissionWithSubIndustry,
                PermissionId = permissionId
            };
            if (PermissionWithSubIndustry.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/BySubIndustry.cshtml"), tenderDetail);
            }
            return null;

        }
        public ActionResult GetCountryList(int permissionId)
        {
            if (_lginStatus.CheckLoginSession() == false)
            {
                return RedirectToAction("UserLogin", "Home");
            }
            List<tbCountry> PermissionWithCountry = new List<tbCountry>();
            var sessionKeyName = "PermissionWithCountry_" + permissionId;
            if (Session[sessionKeyName] == null)
            {
                PermissionWithCountry = _tenderInfo.ListCountrybyRegion(0).ToList();
                Session[sessionKeyName] = PermissionWithCountry;
            }
            else
            {
                PermissionWithCountry = (List<tbCountry>)Session[sessionKeyName];
            }

            var tenderDetail = new TenderDetail
            {
                AllCountryList = PermissionWithCountry,
                PermissionId = permissionId
            };
            if (PermissionWithCountry.Any())
            {
                return PartialView(Url.Content("~/Views/User/PermissionCategory/ByGlobalCountry.cshtml"), tenderDetail);
            }
            return null;

        }


        public JsonResult LoadStateList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<tbState> PermissionWithState = new List<tbState>();
            List<SelectListItem> StateList = new List<SelectListItem>();
            var indiaId = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);

            var statePermissionSessionKey = "ClientStatePermissionAccess_" + permissionId;
            if (Session[statePermissionSessionKey] == null)
            {
                PermissionWithState = (from assignstate in _userTenderPermissionDetail.GetUserPermissionWithLocation(permissionId)
                                       join state in _tenderInfo.ListState() on assignstate.intStateId equals state.StateId
                                       select new tbState
                                       {
                                           StateId = state.StateId,
                                           StateName = state.StateName
                                       }).ToList();
                Session[statePermissionSessionKey] = PermissionWithState;
            }
            else
            {
                PermissionWithState = (List<tbState>)Session[statePermissionSessionKey];
            }


            var stateSessionKey = "ClientStatePermissionList_" + permissionId;
            if (PermissionWithState.Any())
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        for (var i = 0; i < PermissionWithState.Count(); i++)
                        {
                            StateList.Insert(i, new SelectListItem { Text = PermissionWithState[i].StateName, Value = PermissionWithState[i].StateId.ToString(CultureInfo.InvariantCulture) });
                        }
                        Session[stateSessionKey] = StateList;
                    }
                    else
                    {
                        if (Session[stateSessionKey] != null)
                        { StateList = (List<SelectListItem>)Session[stateSessionKey]; }
                        else
                        {
                            for (var i = 0; i < PermissionWithState.Count(); i++)
                            {
                                StateList.Insert(i, new SelectListItem { Text = PermissionWithState[i].StateName, Value = PermissionWithState[i].StateId.ToString(CultureInfo.InvariantCulture) });
                            }
                            Session[stateSessionKey] = StateList;
                        }
                    }
                }
                else
                {
                    PermissionWithState = PermissionWithState.Where(x => x.StateName.ToLower().Trim().StartsWith(searchName.ToLower().Trim())).ToList();

                    for (var i = 0; i < PermissionWithState.Count(); i++)
                    {
                        StateList.Insert(i, new SelectListItem { Text = PermissionWithState[i].StateName, Value = PermissionWithState[i].StateId.ToString(CultureInfo.InvariantCulture) });
                    }
                    Session[stateSessionKey] = StateList;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        StateList = _getListItems.GetStateList(indiaId, searchName);
                        Session[stateSessionKey] = StateList;
                    }
                    else
                    {
                        if (Session[stateSessionKey] != null)
                        { StateList = (List<SelectListItem>)Session[stateSessionKey]; }
                        else
                        {
                            StateList = _getListItems.GetStateList(indiaId, searchName);
                            Session[stateSessionKey] = StateList;
                        }
                    }
                }
                else
                {
                    StateList = _getListItems.GetStateList(indiaId, searchName);
                    Session[stateSessionKey] = StateList;
                }
            }

            return Json(new { DataList = StateList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadCityList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<tbLocation> PermissionWithCity = new List<tbLocation>();
            List<SelectListItem> CityList = new List<SelectListItem>();
            var indiaId = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);

            var cityPermissionSessionKey = "ClientCityPermissionAccess_" + permissionId;

            if (Session[cityPermissionSessionKey] == null)
            {
                PermissionWithCity = (from assigncity in _userTenderPermissionDetail.GetUserPermissionWithLocation(permissionId)
                                      join city in _tenderInfo.ListAllCity() on assigncity.intCityId equals city.LocId
                                      select new tbLocation
                                      {
                                          LocId = city.LocId,
                                          Location = city.Location
                                      }).ToList();

                Session[cityPermissionSessionKey] = PermissionWithCity;
            }
            else
            {
                PermissionWithCity = (List<tbLocation>)Session[cityPermissionSessionKey];
            }

            var citySessionKey = "ClientCityPermissionList_" + permissionId;
            if (PermissionWithCity.Any())
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        for (var i = 0; i < PermissionWithCity.Count(); i++)
                        {
                            CityList.Insert(i, new SelectListItem
                            {
                                Text = PermissionWithCity[i].Location,
                                Value = PermissionWithCity[i].LocId.ToString(CultureInfo.InvariantCulture)
                            });
                        }
                        Session[citySessionKey] = CityList;
                    }
                    else
                    {
                        if (Session[citySessionKey] != null)
                        { CityList = (List<SelectListItem>)Session[citySessionKey]; }
                        else
                        {
                            for (var i = 0; i < PermissionWithCity.Count(); i++)
                            {
                                CityList.Insert(i, new SelectListItem
                                {
                                    Text = PermissionWithCity[i].Location,
                                    Value = PermissionWithCity[i].LocId.ToString(CultureInfo.InvariantCulture)
                                });
                            }
                            Session[citySessionKey] = CityList;
                        }
                    }
                }
                else
                {
                    PermissionWithCity = PermissionWithCity.Where(x => x.Location.ToLower().Trim().StartsWith(searchName.ToLower().Trim())).ToList();

                    for (var i = 0; i < PermissionWithCity.Count(); i++)
                    {
                        CityList.Insert(i, new SelectListItem
                        {
                            Text = PermissionWithCity[i].Location,
                            Value = PermissionWithCity[i].LocId.ToString(CultureInfo.InvariantCulture)
                        });
                    }
                    Session[citySessionKey] = CityList;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        CityList = _getListItems.GetCityList(indiaId, searchName);
                        Session[citySessionKey] = CityList;
                    }
                    else
                    {
                        if (Session[citySessionKey] != null)
                        {
                            CityList = (List<SelectListItem>)Session[citySessionKey];
                        }
                        else
                        {
                            CityList = _getListItems.GetCityList(indiaId, searchName);
                            Session[citySessionKey] = CityList;
                        }
                    }
                }
                else
                {
                    CityList = _getListItems.GetCityList(indiaId, searchName);
                    Session[citySessionKey] = CityList;
                }
            }

            return Json(new { DataList = CityList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadIndustryList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<tbIndustry> PermissionWithIndustry = new List<tbIndustry>();
            List<SelectListItem> IndustryList = new List<SelectListItem>();

            var indPermissionSessionKey = "ClientIndustryPermissionAccess_" + permissionId;

            if (Session[indPermissionSessionKey] == null)
            {
                PermissionWithIndustry = (from assignindustry in _userTenderPermissionDetail.GetUserPermissionWithIndSubIndustry(permissionId)
                                          join industry in _tenderInfo.ListIndustry() on assignindustry.intIndustryId equals industry.IndustryId
                                          select new tbIndustry
                                          {
                                              IndustryId = industry.IndustryId,
                                              IndustryName = industry.IndustryName
                                          }).ToList();
                Session[indPermissionSessionKey] = PermissionWithIndustry;
            }
            else
            {
                PermissionWithIndustry = (List<tbIndustry>)Session[indPermissionSessionKey];
            }

            var indSessionKey = "ClientIndustryPermissionList_" + permissionId;
            if (PermissionWithIndustry.Any())
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        for (var i = 0; i < PermissionWithIndustry.Count(); i++)
                        {
                            IndustryList.Insert(i, new SelectListItem
                            {
                                Text = PermissionWithIndustry[i].IndustryName,
                                Value = PermissionWithIndustry[i].IndustryId.ToString(CultureInfo.InvariantCulture)
                            });
                        }
                        Session[indSessionKey] = IndustryList;
                    }
                    else
                    {
                        if (Session[indSessionKey] != null)
                        { IndustryList = (List<SelectListItem>)Session[indSessionKey]; }
                        else
                        {
                            for (var i = 0; i < PermissionWithIndustry.Count(); i++)
                            {
                                IndustryList.Insert(i, new SelectListItem
                                {
                                    Text = PermissionWithIndustry[i].IndustryName,
                                    Value = PermissionWithIndustry[i].IndustryId.ToString(CultureInfo.InvariantCulture)
                                });
                            }
                            Session[indSessionKey] = IndustryList;
                        }
                    }
                }
                else
                {
                    PermissionWithIndustry = PermissionWithIndustry.Where(x => x.IndustryName.ToLower().Trim().StartsWith(searchName.ToLower().Trim())).ToList();

                    for (var i = 0; i < PermissionWithIndustry.Count(); i++)
                    {
                        IndustryList.Insert(i, new SelectListItem
                        {
                            Text = PermissionWithIndustry[i].IndustryName,
                            Value = PermissionWithIndustry[i].IndustryId.ToString(CultureInfo.InvariantCulture)
                        });
                    }
                    Session[indSessionKey] = IndustryList;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        IndustryList = _getListItems.GetIndustryList(searchName);
                        Session[indSessionKey] = IndustryList;
                    }
                    else
                    {
                        if (Session[indSessionKey] != null)
                        {
                            IndustryList = (List<SelectListItem>)Session[indSessionKey];
                        }
                        else
                        {
                            IndustryList = _getListItems.GetIndustryList(searchName);
                            Session[indSessionKey] = IndustryList;
                        }
                    }
                }
                else
                {
                    IndustryList = _getListItems.GetIndustryList(searchName);
                    Session[indSessionKey] = IndustryList;
                }
            }


            return Json(new { DataList = IndustryList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSubIndustryList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<tbSubIndustry> PermissionWithSubIndustry = new List<tbSubIndustry>();
            List<SelectListItem> SubIndustryList = new List<SelectListItem>();

            var subindPermissionSessionKey = "ClientSubIndustryPermissionAccess_" + permissionId;

            if (Session[subindPermissionSessionKey] == null)
            {
                PermissionWithSubIndustry = (from assignsubIndustry in _userTenderPermissionDetail.GetUserPermissionWithIndSubIndustry(permissionId)
                                             join subIndustry in _tenderInfo.ListAllSubIndustry() on assignsubIndustry.intSubindustryId equals subIndustry.SubIndustryId
                                             select new tbSubIndustry
                                             {
                                                 SubIndustryId = subIndustry.SubIndustryId,
                                                 SubIndustryName = subIndustry.SubIndustryName
                                             }).ToList();
                Session[subindPermissionSessionKey] = PermissionWithSubIndustry;
            }
            else
            {
                PermissionWithSubIndustry = (List<tbSubIndustry>)Session[subindPermissionSessionKey];
            }

            var subindSessionKey = "ClientSubIndustryPermissionList_" + permissionId;
            if (PermissionWithSubIndustry.Any())
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        for (var i = 0; i < PermissionWithSubIndustry.Count(); i++)
                        {
                            SubIndustryList.Insert(i, new SelectListItem
                            {
                                Text = PermissionWithSubIndustry[i].SubIndustryName,
                                Value = PermissionWithSubIndustry[i].SubIndustryId.ToString(CultureInfo.InvariantCulture)
                            });
                        }
                        Session[subindSessionKey] = SubIndustryList;
                    }
                    else
                    {
                        if (Session[subindSessionKey] != null)
                        { SubIndustryList = (List<SelectListItem>)Session[subindSessionKey]; }
                        else
                        {
                            for (var i = 0; i < PermissionWithSubIndustry.Count(); i++)
                            {
                                SubIndustryList.Insert(i, new SelectListItem
                                {
                                    Text = PermissionWithSubIndustry[i].SubIndustryName,
                                    Value = PermissionWithSubIndustry[i].SubIndustryId.ToString(CultureInfo.InvariantCulture)
                                });
                            }
                            Session[subindSessionKey] = SubIndustryList;
                        }
                    }
                }
                else
                {
                    PermissionWithSubIndustry = PermissionWithSubIndustry.Where(x => x.SubIndustryName.ToLower().Trim().StartsWith(searchName.ToLower().Trim())).ToList();

                    for (var i = 0; i < PermissionWithSubIndustry.Count(); i++)
                    {
                        SubIndustryList.Insert(i, new SelectListItem
                        {
                            Text = PermissionWithSubIndustry[i].SubIndustryName,
                            Value = PermissionWithSubIndustry[i].SubIndustryId.ToString(CultureInfo.InvariantCulture)
                        });
                    }
                    Session[subindSessionKey] = SubIndustryList;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        SubIndustryList = _getListItems.GetSubIndustryList(searchName);
                        Session[subindSessionKey] = SubIndustryList;
                    }
                    else
                    {
                        if (Session[subindSessionKey] != null)
                        {
                            SubIndustryList = (List<SelectListItem>)Session[subindSessionKey];
                        }
                        else
                        {
                            SubIndustryList = _getListItems.GetSubIndustryList(searchName);
                            Session[subindSessionKey] = SubIndustryList;
                        }
                    }
                }
                else
                {
                    SubIndustryList = _getListItems.GetSubIndustryList(searchName);
                    Session[subindSessionKey] = SubIndustryList;
                }
            }

            return Json(new { DataList = SubIndustryList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAgencyList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<tbAgencyIndian> PermissionWithAgency = new List<tbAgencyIndian>();
            List<SelectListItem> AgencyList = new List<SelectListItem>();

            var agencyPermissionSessionKey = "ClientAgencyPermissionAccess_" + permissionId;
            if (Session[agencyPermissionSessionKey] == null)
            {
                PermissionWithAgency = (from assignagency in _userTenderPermissionDetail.GetUserPermissionWithAgency(permissionId)
                                        join agency in _tenderInfo.ListAgencyMaster() on assignagency.intAgencyId equals agency.AgencyId
                                        select new tbAgencyIndian
                                        {
                                            AgencyId = agency.AgencyId,
                                            AgencyName = agency.AgencyName
                                        }).ToList();
                Session[agencyPermissionSessionKey] = PermissionWithAgency;
            }
            else
            {
                PermissionWithAgency = (List<tbAgencyIndian>)Session[agencyPermissionSessionKey];
            }


            var agencySessionKey = "ClientAgencyPermissionList_" + permissionId;
            if (PermissionWithAgency.Any())
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        for (var i = 0; i < PermissionWithAgency.Count(); i++)
                        {
                            AgencyList.Insert(i, new SelectListItem
                            {
                                Text = PermissionWithAgency[i].AgencyName,
                                Value = PermissionWithAgency[i].AgencyId.ToString(CultureInfo.InvariantCulture)
                            });
                        }
                        Session[agencySessionKey] = AgencyList;
                    }
                    else
                    {
                        if (Session[agencySessionKey] != null)
                        { AgencyList = (List<SelectListItem>)Session[agencySessionKey]; }
                        else
                        {
                            for (var i = 0; i < PermissionWithAgency.Count(); i++)
                            {
                                AgencyList.Insert(i, new SelectListItem
                                {
                                    Text = PermissionWithAgency[i].AgencyName,
                                    Value = PermissionWithAgency[i].AgencyId.ToString(CultureInfo.InvariantCulture)
                                });
                            }
                            Session[agencySessionKey] = AgencyList;
                        }
                    }
                }
                else
                {
                    PermissionWithAgency = PermissionWithAgency.Where(x => x.AgencyName.ToLower().Trim().StartsWith(searchName.ToLower().Trim())).ToList();

                    for (var i = 0; i < PermissionWithAgency.Count(); i++)
                    {
                        AgencyList.Insert(i, new SelectListItem
                        {
                            Text = PermissionWithAgency[i].AgencyName,
                            Value = PermissionWithAgency[i].AgencyId.ToString(CultureInfo.InvariantCulture)
                        });
                    }
                    Session[agencySessionKey] = AgencyList;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        AgencyList = _getListItems.GetAgencyList(searchName);
                        Session[agencySessionKey] = AgencyList;
                    }
                    else
                    {
                        if (Session[agencySessionKey] != null)
                        {
                            AgencyList = (List<SelectListItem>)Session[agencySessionKey];
                        }
                        else
                        {
                            AgencyList = _getListItems.GetAgencyList(searchName);
                            Session[agencySessionKey] = AgencyList;
                        }
                    }
                }
                else
                {
                    AgencyList = _getListItems.GetAgencyList(searchName);
                    Session[agencySessionKey] = AgencyList;
                }
            }

            return Json(new { DataList = AgencyList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSectorList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<tbSector> PermissionWithSector = new List<tbSector>();
            List<SelectListItem> SectorList = new List<SelectListItem>();

            var agencyPermissionSessionKey = "ClientSectorPermissionAccess_" + permissionId;
            if (Session[agencyPermissionSessionKey] == null)
            {
                PermissionWithSector = (from assignsector in _userTenderPermissionDetail.GetUserPermissionWithSector(permissionId)
                                        join sector in _tenderInfo.ListSector() on assignsector.intSectorId equals sector.SectorId
                                        select new tbSector
                                        {
                                            SectorId = sector.SectorId,
                                            SectorName = sector.SectorName
                                        }).ToList();
                Session[agencyPermissionSessionKey] = PermissionWithSector;
            }
            else
            {
                PermissionWithSector = (List<tbSector>)Session[agencyPermissionSessionKey];
            }


            var sectorSessionKey = "ClientSectorPermissionList_" + permissionId;
            if (PermissionWithSector.Any())
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        for (var i = 0; i < PermissionWithSector.Count(); i++)
                        {
                            SectorList.Insert(i, new SelectListItem
                            {
                                Text = PermissionWithSector[i].SectorName,
                                Value = PermissionWithSector[i].SectorId.ToString(CultureInfo.InvariantCulture)
                            });
                        }
                        Session[sectorSessionKey] = SectorList;
                    }
                    else
                    {
                        if (Session[sectorSessionKey] != null)
                        { SectorList = (List<SelectListItem>)Session[sectorSessionKey]; }
                        else
                        {
                            for (var i = 0; i < PermissionWithSector.Count(); i++)
                            {
                                SectorList.Insert(i, new SelectListItem
                                {
                                    Text = PermissionWithSector[i].SectorName,
                                    Value = PermissionWithSector[i].SectorId.ToString(CultureInfo.InvariantCulture)
                                });
                            }
                            Session[sectorSessionKey] = SectorList;
                        }
                    }
                }
                else
                {
                    PermissionWithSector = PermissionWithSector.Where(x => x.SectorName.ToLower().Trim().StartsWith(searchName.ToLower().Trim())).ToList();

                    for (var i = 0; i < PermissionWithSector.Count(); i++)
                    {
                        SectorList.Insert(i, new SelectListItem
                        {
                            Text = PermissionWithSector[i].SectorName,
                            Value = PermissionWithSector[i].SectorId.ToString(CultureInfo.InvariantCulture)
                        });
                    }
                    Session[sectorSessionKey] = SectorList;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        SectorList = _getListItems.GetSectorList(searchName);
                        Session[sectorSessionKey] = SectorList;
                    }
                    else
                    {
                        if (Session[sectorSessionKey] != null)
                        {
                            SectorList = (List<SelectListItem>)Session[sectorSessionKey];
                        }
                        else
                        {
                            SectorList = _getListItems.GetSectorList(searchName);
                            Session[sectorSessionKey] = SectorList;
                        }
                    }
                }
                else
                {
                    SectorList = _getListItems.GetSectorList(searchName);
                    Session[sectorSessionKey] = SectorList;
                }

            }

            return Json(new { DataList = SectorList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadOwnershipList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<SelectListItem> OwnershipList = new List<SelectListItem>();
            List<tbOwnership> PermissionWithOwnership = new List<tbOwnership>();

            var ownershipPermissionSessionKey = "ClientOwnershipPermissionAccess_" + permissionId;

            if (Session[ownershipPermissionSessionKey] == null)
            {
                PermissionWithOwnership = (from assignownership in _userTenderPermissionDetail.GetUserPermissionWithOwnership(permissionId)
                                           join ownership in _tenderInfo.ListOwnership() on assignownership.intOwnershipId equals ownership.OwnershipId
                                           select new tbOwnership
                                           {
                                               OwnershipId = ownership.OwnershipId,
                                               OwnershipName = ownership.OwnershipName
                                           }).ToList();
                Session[ownershipPermissionSessionKey] = PermissionWithOwnership;
            }
            else
            {
                PermissionWithOwnership = (List<tbOwnership>)Session[ownershipPermissionSessionKey];
            }


            var ownershipSessionKey = "ClientOwnershipPermissionList_" + permissionId;
            if (PermissionWithOwnership.Any())
            {
                if (Session[ownershipSessionKey] != null)
                { OwnershipList = (List<SelectListItem>)Session[ownershipSessionKey]; }
                else
                {
                    for (var i = 0; i < PermissionWithOwnership.Count(); i++)
                    {
                        OwnershipList.Insert(i, new SelectListItem
                        {
                            Text = PermissionWithOwnership[i].OwnershipName,
                            Value = PermissionWithOwnership[i].OwnershipId.ToString(CultureInfo.InvariantCulture)
                        });
                    }
                    Session[ownershipSessionKey] = OwnershipList;
                }
            }
            else
            {
                if (Session[ownershipSessionKey] != null)
                {
                    OwnershipList = (List<SelectListItem>)Session[ownershipSessionKey];
                }
                else
                {
                    OwnershipList = _getListItems.GetOwnershipList();
                    Session[ownershipSessionKey] = OwnershipList;
                }
            }


            return Json(new { DataList = OwnershipList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadCountryList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<tbCountry> PermissionWithCountry = new List<tbCountry>();
            List<SelectListItem> CountryList = new List<SelectListItem>();

            var countryPermissionSessionKey = "ClientCountryPermissionAccess_" + permissionId;
            if (Session[countryPermissionSessionKey] == null)
            {
                PermissionWithCountry = _tenderInfo.ListCountrybyRegion(0).ToList();
                Session[countryPermissionSessionKey] = PermissionWithCountry;
            }
            else
            {
                PermissionWithCountry = (List<tbCountry>)Session[countryPermissionSessionKey];
            }


            var countrySessionKey = "ClientCountryPermissionList_" + permissionId;
            if (PermissionWithCountry.Any())
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        for (var i = 0; i < PermissionWithCountry.Count(); i++)
                        {
                            CountryList.Insert(i, new SelectListItem
                            {
                                Text = PermissionWithCountry[i].CountryName,
                                Value = PermissionWithCountry[i].CountryId.ToString(CultureInfo.InvariantCulture)
                            });
                        }
                        Session[countrySessionKey] = CountryList;
                    }
                    else
                    {
                        if (Session[countrySessionKey] != null)
                        { CountryList = (List<SelectListItem>)Session[countrySessionKey]; }
                        else
                        {
                            for (var i = 0; i < PermissionWithCountry.Count(); i++)
                            {
                                CountryList.Insert(i, new SelectListItem
                                {
                                    Text = PermissionWithCountry[i].CountryName,
                                    Value = PermissionWithCountry[i].CountryId.ToString(CultureInfo.InvariantCulture)
                                });
                            }
                            Session[countrySessionKey] = CountryList;
                        }
                    }
                }
                else
                {
                    PermissionWithCountry = PermissionWithCountry.Where(x => x.CountryName.ToLower().Trim().StartsWith(searchName.ToLower().Trim())).ToList();

                    for (var i = 0; i < PermissionWithCountry.Count(); i++)
                    {
                        CountryList.Insert(i, new SelectListItem
                        {
                            Text = PermissionWithCountry[i].CountryName,
                            Value = PermissionWithCountry[i].CountryId.ToString(CultureInfo.InvariantCulture)
                        });
                    }
                    Session[countrySessionKey] = CountryList;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        CountryList = _getListItems.GetGlobalCountryList(searchName);
                        Session[countrySessionKey] = CountryList;
                    }
                    else
                    {
                        if (Session[countrySessionKey] != null)
                        {
                            CountryList = (List<SelectListItem>)Session[countrySessionKey];
                        }
                        else
                        {
                            CountryList = _getListItems.GetGlobalCountryList(searchName);
                            Session[countrySessionKey] = CountryList;
                        }
                    }
                }
                else
                {
                    CountryList = _getListItems.GetGlobalCountryList(searchName);
                    Session[countrySessionKey] = CountryList;
                }

            }

            return Json(new { DataList = CountryList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadKeywordList(int permissionId, string searchName = "", bool isReset = false)
        {
            List<tbProduct> PermissionWithtbProduct = new List<tbProduct>();
            List<SelectListItem> ProductList = new List<SelectListItem>();


            var productPermissionSessionKey = "ClientProductPermissionAccess_" + permissionId;
            if (Session[productPermissionSessionKey] == null)
            {
                PermissionWithtbProduct = (from assignproducts in _userTenderPermissionDetail.GetUserPermissionWithProduct(permissionId)
                                           join prod in _tenderInfo.ListAllProducts() on assignproducts.intProductId equals prod.ProductsId
                                           select new tbProduct
                                           {
                                               ProductsId = prod.ProductsId,
                                               ProductsName = prod.ProductsName
                                           }).ToList();
                Session[productPermissionSessionKey] = PermissionWithtbProduct;
            }
            else
            {
                PermissionWithtbProduct = (List<tbProduct>)Session[productPermissionSessionKey];
            }


            var productSessionKey = "ClientProductPermissionList_" + permissionId;
            if (PermissionWithtbProduct.Any())
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        for (var i = 0; i < PermissionWithtbProduct.Count(); i++)
                        {
                            ProductList.Insert(i, new SelectListItem
                            {
                                Text = PermissionWithtbProduct[i].ProductsName,
                                Value = PermissionWithtbProduct[i].ProductsId.ToString(CultureInfo.InvariantCulture)
                            });
                        }
                        Session[productSessionKey] = ProductList;
                    }
                    else
                    {
                        if (Session[productSessionKey] != null)
                        { ProductList = (List<SelectListItem>)Session[productSessionKey]; }
                        else
                        {
                            for (var i = 0; i < PermissionWithtbProduct.Count(); i++)
                            {
                                ProductList.Insert(i, new SelectListItem
                                {
                                    Text = PermissionWithtbProduct[i].ProductsName,
                                    Value = PermissionWithtbProduct[i].ProductsId.ToString(CultureInfo.InvariantCulture)
                                });
                            }
                            Session[productSessionKey] = ProductList;
                        }
                    }
                }
                else
                {
                    PermissionWithtbProduct = PermissionWithtbProduct.Where(x => x.ProductsName.ToLower().Trim().StartsWith(searchName.ToLower().Trim())).ToList();

                    for (var i = 0; i < PermissionWithtbProduct.Count(); i++)
                    {
                        ProductList.Insert(i, new SelectListItem
                        {
                            Text = PermissionWithtbProduct[i].ProductsName,
                            Value = PermissionWithtbProduct[i].ProductsId.ToString(CultureInfo.InvariantCulture)
                        });
                    }
                    Session[productSessionKey] = ProductList;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(searchName))
                {
                    if (isReset)
                    {
                        ProductList = _getListItems.GetProductList(searchName);
                        Session[productSessionKey] = ProductList;
                    }
                    else
                    {
                        if (Session[productSessionKey] != null)
                        { ProductList = (List<SelectListItem>)Session[productSessionKey]; }
                        else
                        {
                            ProductList = _getListItems.GetProductList(searchName);
                            Session[productSessionKey] = ProductList;
                        }
                    }
                }
                else
                {
                    ProductList = _getListItems.GetProductList(searchName);
                    Session[productSessionKey] = ProductList;
                }
            }

            return Json(new { DataList = ProductList }, JsonRequestBehavior.AllowGet);
        }

        public List<UserFavouriteTenderList> GetTenderInFavourite(int tenderType)
        {
            tenderType = 1;
            var clientId = Convert.ToInt32(Session["ClientID"]);
            List<UserFavouriteTenderList> tenderList = null;
            switch (tenderType)
            {
                case 1://INDIAN
                    tenderList = _userInfo.GetAllFavTendersByClient(clientId, 1);
                    break;
                case 2://GLOBAL
                    tenderList = _userInfo.GetAllFavTendersByClient(clientId, 2);
                    break;
            }

            return tenderList;
        }


        private void FillWithinSearchWords(string searchText)
        {
            if (Session["WithinSearchText"] != null)
            {
                WithinSearchWords = (List<string>)Session["WithinSearchText"];
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                if (!WithinSearchWords.Contains(searchText.Trim()) && searchText.Trim() != "*-*")
                {
                    WithinSearchWords.Add(searchText);
                }
                Session["WithinSearchText"] = WithinSearchWords;
            }
        }

        public JsonResult ClearUserSession()
        {
            ClearUserSearchSession();
            return Json(JsonRequestBehavior.AllowGet);
        }

    }
}