using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TenderAssist.Models;
using TenderAssist.ViewModel;
using static TenderAssist.CommonHelper.Utility;
using static TenderAssist.Models.SearchModel;
using TenderAssist.CommonHelper;
using System.Configuration;
using TenderAssist.Models.DBConnection;
using System.Globalization;

namespace TenderAssist.Controllers
{

    public class HomeController : BaseController
    {
       
        private readonly CommonController _common;
        private readonly GetListItems _getListItems;
        private readonly UserMembershipDetail _userMembershipDetail;
        private readonly TenderInformation _tenderInfo;

        public HomeController()
        {
            _common = new CommonController();
            _getListItems = new GetListItems();
            _userMembershipDetail = new UserMembershipDetail();
            _tenderInfo = new TenderInformation();
        }

        
        public ActionResult Index()
        {
            Session.Clear();
            ViewBag.Title = @"Get All Latest Tenders Information & Tenders Assistance Online in India";
            ViewBag.description = @"TenderAssist247 will give all Tenders information Online, Government Tenders, Private Tenders, India tenders, Global Tenders, Auction Tenders, Corporation Tenders, Industry Tenders, Subscribe Today";
            ViewBag.keywords = @"Tender, Tenders ,Tender Bidding, online tender information, latest tenders information, digital signature, equipments tenders, tender assist, tender alerts, indian tenders, industry tenders, state tenders, agency tenders, city tenders, ownership tenders, corporations, Tender Info, Online Tenders India, Government Tenders, E Tenders, Get Tenders Online, Online Tender Portal, Tender Assistance, tender online, online tender website, tender site, online tender asssistant, Private Tenders, Tenders 24*7, Tenders 24/7, Tender 24*7, Tender 24/7, Tender 24x7, TenderAssist247.com";

            return View(new TenderDetail()
            {
                IsHomePage = true,
                TendersBy = TenderTypeList.Keyword
            });
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult TermsConditions()
        {
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        public ActionResult Contact()
        {
            var tenderDetail = new TenderDetail
            {
                //StateList = _getListItems.StateList(),               
                Subscribetype = SubscribType.ContactUs,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.ContactUs,
                FormType = FormType.ContactForm
            };

            return View("~/Views/InquiryForms/ContactForm.cshtml", tenderDetail);
        }


        #region PAY ONLINE METHODS
        public ActionResult PayOnline()
        {
            var tenderDetail = new TenderDetail
            {
                Subscribetype = SubscribType.PayOnline,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.PayOnline,
                FormType = FormType.PayOnline
            };

            return View(tenderDetail);
        }
        [HttpPost]
        public ActionResult PostPaymentDetail(FormCollection coll)
        {
            string str1 = "OK";
            string str2 = "";
            string AtomUrl = ConfigurationManager.AppSettings["AtomUrl"];
            string MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            string WorkingKey = ConfigurationManager.AppSettings["WorkingKey"];
            string PayOnlineSuccessPageUrl = ConfigurationManager.AppSettings["ApplicationUrl"] + ConfigurationManager.AppSettings["PayOnlineSuccessPageUrl"];

            string PayFrom = coll.Get("PayFrom");
            string PayContactName = coll.Get("PayContactName");
            string PayMobNo = coll.Get("PayMobNo");
            string PayEmail = coll.Get("PayEmail");
            string PayCharges = coll.Get("PayCharges");
            string PayPlanId = coll.Get("PayPlanId");

            string msg = "";
            var isvalid = true;

            if (PayPlanId == "" || PayPlanId == "0")
            { msg = "ERROR : Invalid Plan Selection"; isvalid = false; }

            if (PayCharges == "")
            { msg = "ERROR : Invalid Amount"; isvalid = false; }
            else
            {
                if (!CheckValidAmount(PayCharges))
                { msg = "ERROR : Invalid Amount. It should not be less than 500/-Rs."; isvalid = false; }
                //else
                //{
                //    var planamount = "";
                //    switch (Convert.ToInt32(PayPlanId))
                //    {
                //        case 1:
                //            planamount = "7000";
                //            break;
                //        case 2:
                //            planamount = "10000";
                //            break;
                //        case 3:
                //            planamount = "15000";
                //            break;
                //        case 4:
                //            planamount = "45000";
                //            break;
                //    }
                //    if (planamount != PayCharges)
                //    { msg = "ERROR : Amount is different than Selected Plan Amount"; isvalid = false; }
                //}
            }

            if (!isvalid)
            {
                return (ActionResult)Json((object)new { msg = msg, isvalid = isvalid }, JsonRequestBehavior.AllowGet);
            }

            Session["PayFrom"] = PayFrom;
            Session["PayContactName"] = PayContactName;
            Session["PayMobNo"] = PayMobNo;
            Session["PayEmail"] = PayEmail;
            Session["PayCharges"] = PayCharges;


            var MerchantLogin = ConfigurationManager.AppSettings["MerchantLogin"];//"197";
            var MerchantPass = ConfigurationManager.AppSettings["MerchantPass"];//"Test@123";

            var TransactionType = ConfigurationManager.AppSettings["TransactionType"];// TransactionType = "NBFundtransfer";
            var ProductID = ConfigurationManager.AppSettings["ProductID"];// ProductID = "NSE";
            var TransactionID = ConfigurationManager.AppSettings["TransactionID"];// TransactionID = "123";
            var TransactionAmount = PayCharges;// TransactionAmount = "100";
            var TransactionCurrency = ConfigurationManager.AppSettings["TransactionCurrency"];// TransactionCurrency = "INR";
            var BankID = ConfigurationManager.AppSettings["BankID"];// BankID = "2001";

            //string ru = "http://localhost:258252/Pages/FundTransferFailed.aspx";


            var transactionDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss").Replace("-", "/");
            var url = PayOnlineMethods.TransferFund(MerchantLogin, MerchantPass, "", ProductID, PayEmail, "1234567890", TransactionType,
                 TransactionAmount, TransactionCurrency, "0", TransactionID, transactionDate, BankID, PayOnlineSuccessPageUrl);

            msg = "OK"; isvalid = true;

            return (ActionResult)this.Json((object)new { msg = msg, url = url, isvalid = isvalid }, JsonRequestBehavior.AllowGet);
        }
        private bool CheckValidAmount(string amount)
        {
            bool flag = true;
            if (amount == "")
                flag = false;
            else if (Convert.ToDecimal(amount) >= new Decimal(0) && Convert.ToDecimal(amount) < new Decimal(500))
                flag = false;
            return flag;
        }
        public ActionResult PaymentStatus()
        {
            var messageText = "";
            var isvalid = true;

            var PayFrom = Session["PayFrom"];
            var PayContactName = Session["PayContactName"];
            var PayMobNo = Session["PayMobNo"];
            var PayEmail = Session["PayEmail"];
            var PayCharges = Session["PayCharges"];

            if (Session["PayFrom"] == null || Session["PayContactName"] == null || Session["PayMobNo"] == null || Session["PayEmail"] == null || Session["PayCharges"] == null)
            {
                messageText = "Invalid Payment Informations";
                isvalid = false;
            }

            if (isvalid)
            {
                string MerchantId = this.Request.Form["MerchantID"];
                string AMT = this.Request.Form["AMT"];
                string VERIFIED = this.Request.Form["VERIFIED"];
                switch (VERIFIED)
                {
                    /*
                     VERIFIED

                    Initiated – in Case customer is on Payment page and not submitted payment request.
                    SUCCESS – in case of success transaction.
                    FAILED– in case of failure transaction.
                    NODATA– in case of mismatch of value in the request.
                    Pending from Bank – in case atom not received response from bank
                    Invalid date format – in caseof date formatnotreceived as YYYY-MM-DD
                    BID Reference No. received from bank only for success transactions.
                     */

                    case "Initiated":
                        messageText = "You have not submitted payment request.";
                        isvalid = false;
                        break;
                    case "SUCCESS":
                        messageText = "Your Payment is successful.";
                        isvalid = true;
                        break;
                    case "FAILED":
                        messageText = "Your Payment is not successful.";
                        isvalid = false;
                        break;
                }
            }
            TenderDetail tenderDetail = new TenderDetail()
            {
                DisplayText = messageText
            };

            ViewBag.UserEmail = PayEmail;


            return View(tenderDetail);
        }
        #endregion


        public ActionResult ReloadCaptcha()
        {
            string randomText = Utility.GetRandomText();
            Session["CaptchaCode"] = (object)randomText;
            return Json(new { imageDataURL = randomText }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHomeLinkData(int menuBy, int categoryBy = 0)
        {
            var pageLink = "";
            switch (menuBy)
            {
                default:
                case 1:
                    var sampleTenders = new TenderDetail
                    {
                        StateList = _getListItems.StateList(),
                        CityList = _getListItems.CityList(0),
                        Subscribetype = SubscribType.SampleTenders,
                        DownloadTenderRefNo = 0,
                        FormTitle = SubscribTypeDisplsyText.SampleTenders,
                        FormType = FormType.OtherForms
                    };
                    pageLink = "~/Views/Home/Sections/_HomeSampleTenderListPanel.cshtml";
                    return PartialView(Url.Content(pageLink), sampleTenders);
                    break;
                case 2:
                    pageLink = "~/Views/Home/Sections/_HomeResultListPanel.cshtml";
                    break;
                case 3:
                    pageLink = "~/Views/Home/Sections/_HomeProjectListPanel.cshtml";
                    break;
                case 5:
                    var freeTenders = new TenderDetail
                    {
                        StateList = _getListItems.StateList(),
                        CityList = _getListItems.CityList(0),
                        Subscribetype = SubscribType.FreeTenders,
                        DownloadTenderRefNo = 0,
                        FormTitle = SubscribTypeDisplsyText.FreeTenders,
                        FormType = FormType.OtherForms
                    };
                    pageLink = "~/Views/Home/Sections/_HomeFreeTenderListPanel.cshtml";
                    return PartialView(Url.Content(pageLink), freeTenders);
                    break;
                case 6:
                    pageLink = "~/Views/Home/Sections/_HomePrivateTenderListPanel.cshtml";
                    break;
                case 7:
                    AdvanceSearchParameter AdvanceSearch = new AdvanceSearchParameter()
                    {
                        TenderStatusFlag = categoryBy
                    };

                    ClearSession();
                    var tenderDetail = _common.GetSearchTenderResult(0, "", 1, AdvanceSearch, 0, 0, "", TenderTypeList.Keyword, false, 3, null);
                    pageLink = "~/Views/Home/Sections/_HomeTenderListPanel.cshtml";
                    return PartialView(Url.Content(pageLink), tenderDetail);
                    break;
            }
            return PartialView(Url.Content(pageLink));
        }


        public ActionResult GetTendersByType(int type)
        {
            Random random = new Random();
            const int displayItem = 6;
            string resultLinks = "";
            var indiaId = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);

            switch (type)
            {
                case TenderTypeList.State:                    
                    var tenderStateList = _tenderInfo.ListStateByCountry(indiaId).OrderBy(x => random.Next()).Take(displayItem).ToList();
                    foreach (var item in tenderStateList)
                    {
                        var stateurl = ConfigurationManager.AppSettings["TenderByStateUrl"].ToString() + TenderWordNameList.StateWord + item.StateName.Replace(" ", "-").ToLower().Trim().ToString();
                        resultLinks += @"<li>
                                            <span>»</span>
                                                <a href='"+ stateurl + "'  title='" + item.StateName + "'>" + item.StateName + " Tenders</a>" +
                                         "</li>";
                    }
                    break;
                case TenderTypeList.City:
                    var tenderCityList = _tenderInfo.ListCityByCountry(indiaId).OrderBy(x => random.Next()).Take(displayItem).ToList();
                    foreach (var item in tenderCityList)
                    {
                        var cityurl = ConfigurationManager.AppSettings["TenderByCityUrl"].ToString() + TenderWordNameList.CityWord + item.Location.Replace(" ", "-").ToLower().Trim().ToString();
                        resultLinks += @"<li>
                                            <span>»</span>
                                                <a href='" + cityurl + "'  title='" + item.Location + "'>" + item.Location + " Tenders</a>" +
                                         "</li>";
                    }
                    break;
                case TenderTypeList.Industry:
                    var tenderIndList = _tenderInfo.ListIndustry().OrderBy(x => random.Next()).Take(displayItem).ToList();
                    foreach (var item in tenderIndList)
                    {
                        var indurl = ConfigurationManager.AppSettings["TenderByIndustryUrl"].ToString() + TenderWordNameList.IndustryWord + item.IndustryName.Replace(" ", "-").ToLower().Trim().ToString();
                        resultLinks += @"<li>
                                            <span>»</span>
                                                <a href='" + indurl + "'  title='" + item.IndustryName + "'>" + item.IndustryName + " Tenders</a>" +
                                         "</li>";
                    }
                    break;
                case TenderTypeList.SubIndustry:
                    var tenderSubIndList = _tenderInfo.ListAllSubIndustry().OrderBy(x => random.Next()).Take(displayItem).ToList();
                    foreach (var item in tenderSubIndList)
                    {
                        var subindurl = ConfigurationManager.AppSettings["TenderBySubIndustryUrl"].ToString() + TenderWordNameList.SubIndustryWord + item.SubIndustryName.Replace(" ", "-").ToLower().Trim().ToString();
                        resultLinks += @"<li>
                                            <span>»</span>
                                                <a href='" + subindurl + "'  title='" + item.SubIndustryName + "'>" + item.SubIndustryName + " Tenders</a>" +
                                         "</li>";
                    }
                    break;
                case TenderTypeList.Agency:
                    var tenderAgencyList = _tenderInfo.ListAgencyMaster().OrderBy(x => random.Next()).Take(displayItem).ToList();
                    foreach (var item in tenderAgencyList)
                    {
                        var agencyurl = ConfigurationManager.AppSettings["TenderByAgencyUrl"].ToString() + TenderWordNameList.AgencyWord + item.AgencyName.Replace(" ", "-").ToLower().Trim().ToString();
                        resultLinks += @"<li>
                                            <span>»</span>
                                                <a href='" + agencyurl + "'  title='" + item.AgencyName + "'>" + item.AgencyName + " Tenders</a>" +
                                         "</li>";
                    }
                    break;
                case TenderTypeList.Sector:
                    var tenderSectorList = _tenderInfo.ListSector().OrderBy(x => random.Next()).Take(displayItem).ToList();
                    foreach (var item in tenderSectorList)
                    {
                        var sectorurl = ConfigurationManager.AppSettings["TenderBySectorUrl"].ToString() + TenderWordNameList.SectorWord + item.SectorName.Replace(" ", "-").ToLower().Trim().ToString();
                        resultLinks += @"<li>
                                            <span>»</span>
                                                <a href='" + sectorurl + "'  title='" + item.SectorName + "'>" + item.SectorName + " Tenders</a>" +
                                         "</li>";
                    }
                    break;
                case TenderTypeList.Ownership:
                    var tenderOwnershipList = _tenderInfo.ListOwnership().OrderBy(x => random.Next()).Take(displayItem).ToList();
                    foreach (var item in tenderOwnershipList)
                    {
                        var ownershipurl = ConfigurationManager.AppSettings["TenderByOwnershipUrl"].ToString() + TenderWordNameList.OwnershipWord + item.OwnershipName.Replace(" ", "-").ToLower().Trim().ToString();
                        resultLinks += @"<li>
                                            <span>»</span>
                                                <a href='" + ownershipurl + "'  title='" + item.OwnershipName + "'>" + item.OwnershipName + " Tenders</a>" +
                                         "</li>";
                    }
                    break;
                case TenderTypeList.Keyword:
                    var tenderKeywordList = _tenderInfo.ListAllProducts().OrderBy(x => random.Next()).Take(displayItem).ToList();
                    foreach (var item in tenderKeywordList)
                    {
                        var producturl = ConfigurationManager.AppSettings["TenderByKeywordUrl"].ToString() + TenderWordNameList.KeywordWord + item.ProductsName.Replace(" ", "-").ToLower().Trim().ToString();
                        resultLinks += @"<li>
                                            <span>»</span>
                                                <a href='" + producturl + "'    title='" + item.ProductsName + "'>" + item.ProductsName + " Tenders</a>" +
                                         "</li>";
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(resultLinks))
            {
                resultLinks = "<ul>"+ resultLinks + "</ul>";
            }

            return Json(new { ResultLinks = resultLinks }, JsonRequestBehavior.AllowGet);
        }


        #region UserLogin
        public ActionResult UserLogin()
        {
            var userinfo = new tabClientDetail();
            //userinfo.strEmailId1 = "nationaltender@gmail.com";
            //userinfo.strPassword = "77CCD8";


            var tenderDetail = new TenderDetail()
            {
                DisplayText = "",
                LoginUserDetails = userinfo
            };
            ViewBag.Title = @"Tender, Tenders, e tenders , Tender daily Service , India Tender, Tender assist India, Tender Information service , Government Tenders, Global Tenders,  International Tenders, Indian Tenders , eprocure tenders";
            ViewBag.description = @"The most accurate and reliable tender services  in india .  Register free for list of tenders, government tenders& international tenders  and get unique facility for search by product, tenders by location, online tenders, public tenders, international tenders";
            ViewBag.keywords = @"Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice, Indian tender portal,  tenders notification service,  tender notice, government tenders, govt tenders, tenders info, free tenders, India Tender, Tender India, Global Tenders, Public Tenders, Free Government Tenders, Indian Government Tenders, eprocure tenders, tenderassist247.com";

            return View(tenderDetail);

        }
        public void LoginTender(string userRefNo, string id, string refno)
        {
            string url = ConfigurationManager.AppSettings["ApplicationUrl"];
            tabClientDetail usersByUniqueId = _common.GetUsersByUniqueId(new Guid(userRefNo));
            if (usersByUniqueId != null)
            {
                this.Session["ClientId"] = (object)usersByUniqueId.intClientId;
                this.Session["Purpose"] = (object)usersByUniqueId.intClientPurpose;
                this.Session["PageTitle"] = (object)"";
                this.Session["IsActiveUser"] = (object)usersByUniqueId.intActive.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                this.Session["PageTitle"] = (object)"TenderAssist247 :: Indian Tenders";
                this.Session["IndianPermissionId"] = (object)null;
                url = usersByUniqueId.intActive != 2
                    ? url + "User/TenderDetail/" + DateTime.Now.Year.ToString() + "/" + refno.ToString((IFormatProvider)CultureInfo.InvariantCulture)
                    : url + "User/Renewal";
            }
            
            Response.Redirect(url);
        }
        public void ShowUserTenderByEmail(string userRefNo, int perId, int tStatusFlag, string enterDt)
        {
            string url = ConfigurationManager.AppSettings["ApplicationUrl"];
            tabClientDetail usersByUniqueId = this._common.GetUsersByUniqueId(new Guid(userRefNo));
            if (usersByUniqueId != null)
            {
                Session["ClientId"] = (object)usersByUniqueId.intClientId;
                Session["Purpose"] = (object)usersByUniqueId.intClientPurpose;
                Session["PageTitle"] = (object)"";
                Session["IsActiveUser"] = (object)usersByUniqueId.intActive.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                Session["PageTitle"] = (object)"TenderAssist247 :: Indian Tenders";
                Session["IndianPermissionId"] = (object)null;
                UserTenderPermission userTender = new UserTenderPermission();
                tabClientPermission tenderPermissionById = userTender.GetUsersTenderPermissionById(perId);

                if(usersByUniqueId.intActive == 2)
                {
                    url = url + "User/Renewal";
                }
                else
                {
                    if (tenderPermissionById.bitIndianOrGlobal)
                        url = url + "User/Indian-Tender/" + perId;
                    else
                        url = url + "User/Global-Tender/" + perId;
                }
            }
            this.Response.Redirect(url);
        }
        #endregion UserLogin


        #region SAMPLE TENDERS
        private List<string> SelectRefNoList = new List<string>();
        public ActionResult ViewSampleTenders(int pId = 0, string display = "", string TenderYear = "")
        {
            if (display == "")
                return RedirectToAction("Index", "Home");

            string OrderBys = "OurRefNo";
            string AscDesc = "DESC";

            Session["eId"] = null;
            Session["PId"] = pId;
            Session["Display"] = display;
            Session["SelectRefNo"] = null;
            var objUserMembershipDetail = _userMembershipDetail.GetUsersByPermissionId_Local(pId);
            if (objUserMembershipDetail == null)
            {
               
                return RedirectToAction("Index", "Home");
            }

            var purpose = objUserMembershipDetail.intClientPurpose;
            Session["empContactNo1"] = "+91-97247 00247";
            Session["empContactNo2"] = "+91-96870 00247";

            var username = objUserMembershipDetail.strFName + " " + objUserMembershipDetail.strLName;
            var email1 = !string.IsNullOrEmpty(objUserMembershipDetail.strEmailId1) ? objUserMembershipDetail.strEmailId1 : "";
            var email2 = !string.IsNullOrEmpty(objUserMembershipDetail.strEmailId2) ? objUserMembershipDetail.strEmailId2 : "";

            var purposename = "tenderassist247.com";
            
            var allUserEmailAccountDetail = _userMembershipDetail.GetLocalEmpEamilId_Local(objUserMembershipDetail.intClientId, display, purposename);

            var emailList = new List<SelectListItem>();

            for (var i = 0; i < allUserEmailAccountDetail.Count(); i++)
                emailList.Insert(i, new SelectListItem { Text = allUserEmailAccountDetail[i].strEmailId, Value = allUserEmailAccountDetail[i].strEmailId });

            var empdetails = _userMembershipDetail.GetLocalEmpDetails_Local(objUserMembershipDetail.intClientId, display);

            if (empdetails != null)
            {
                Session["eId"] = empdetails.intEmpId;
                Session["empContactNo1"] = empdetails.intContactNo == null ? "" : "+91-" + empdetails.intContactNo.ToString();
                Session["empContactNo2"] = empdetails.intCompContactNo == null ? "" : "+91-" + empdetails.intCompContactNo.ToString();
            }
            else
                Session["eId"] = "0";

            #region VarDeclaration
            Session["TotalSampleRecordCount"] = null;
            const int page = 0;
            const int pageSize = 10;
            Int64 totalLive = 0;
            Int64 totalFresh = 0;
            Int64 totalClosed = 0;

            var selectedAgency = string.Empty;
            var selectedAgencyNotIn = string.Empty;
            var selectedOwnership = string.Empty;
            var selectedOwnershipNotIn = string.Empty;
            var selectedSector = string.Empty;
            var selectedSectorNotIn = string.Empty;
            var selectedCountry = string.Empty;
            var selectedState = string.Empty;
            var selectedCity = string.Empty;
            var selectedLocation = string.Empty;
            var selectedLocationNotIn = string.Empty;
            var selectedIndustrySubIndustry = string.Empty;
            var selectedIndustrySubIndustryNotIn = string.Empty;
            var selectedKeyword1 = string.Empty;
            var selectedKeyword2 = string.Empty;
            var selectedKeyword3 = string.Empty;
            var otherKeywords = string.Empty;
            var notUsedKeywords = string.Empty;
            var documentType = string.Empty;
            var icbncb = string.Empty;
            var searchText = string.Empty;
            var finalSearchText = string.Empty;
            var tenderValue = string.Empty;
            double tenderValueFrom = 0;
            double tenderValueTo = 0;
            #endregion

            var tenaderInfoWithDetail = new List<SearchTenaderInfoWithAllDetail>();
            Int64 totalCount = 0;
            if (pId != 0)
            {
                var objtabClientPermission = _userMembershipDetail.GetUsersTenderPermissionById_Local(pId);
                ViewBag.PermissionName = objtabClientPermission.strPermissionName;
                Boolean indianGlobal = objtabClientPermission.bitIndianOrGlobal;

                var tenaderList = _common.GetAllSearchTenderInfo_Client_SampleTender(pId, page, pageSize, null, true,
                      ref selectedOwnership, ref selectedOwnershipNotIn, ref selectedSector, ref selectedSectorNotIn,
                      ref selectedAgency, ref selectedAgencyNotIn, ref selectedIndustrySubIndustry, ref selectedIndustrySubIndustryNotIn,
                      ref selectedLocation, ref selectedLocationNotIn, ref selectedKeyword1,
                      ref selectedKeyword2, ref selectedKeyword3, ref otherKeywords, ref notUsedKeywords, ref documentType,
                      ref icbncb, ref tenderValue, ref tenderValueFrom, ref tenderValueTo, ref indianGlobal, ref finalSearchText, TenderYear, OrderBys, AscDesc, true);

                tenaderInfoWithDetail = tenaderList.TenaderDetailSearch;
                if (Session["TotalSampleRecordCount"] == null)
                {
                    var listTenderCount = tenaderList.TenaderDetailCount;

                    if (listTenderCount.Any())
                    {
                        totalCount += listTenderCount.Sum(t => t.TotalRecord);
                        var detActive = listTenderCount.FirstOrDefault(x => x.TenderStatusReturn == 1);
                        totalLive = detActive != null ? detActive.TotalRecord : 0;
                        var detNew = listTenderCount.FirstOrDefault(x => x.TenderStatusReturn == 2);
                        totalFresh = detNew != null ? detNew.TotalRecord : 0;
                        var detClosed = listTenderCount.FirstOrDefault(x => x.TenderStatusReturn == 3);
                        totalClosed = detClosed != null ? detClosed.TotalRecord : 0;
                    }
                    Session["TotalSampleRecordCount"] = totalCount;
                    Session["IndianGlobal"] = indianGlobal;
                    Session["Purpose"] = purpose;
                }
            }

            const int shoecurrentpage = (page / 10) + 1;
            ViewBag.DisplayCurrentPage = shoecurrentpage;
            ViewBag.CurrentPage = shoecurrentpage;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPage = 0;

            if (Session["TotalSampleRecordCount"] != null)
            {
                ViewBag.TotalPage = Math.Ceiling((double)((Int64)Session["TotalSampleRecordCount"]) / pageSize);
                ViewBag.IndianGlobal = Session["IndianGlobal"].ToString();
                ViewBag.purpose = Session["Purpose"].ToString();
            }
            var orderByList = new List<SelectListItem>();
            orderByList.Insert(0, new SelectListItem { Text = "Reference No", Value = "OurRefNo", Selected = true });
            orderByList.Insert(1, new SelectListItem { Text = "Due Date ", Value = "SubmDate" });

            var ascdescList = new List<SelectListItem>();
            ascdescList.Insert(0, new SelectListItem { Text = "Ascending", Value = "ASC" });
            ascdescList.Insert(1, new SelectListItem { Text = "Descending ", Value = "DESC", Selected = true });

            var tenderDetail = new TenderDetail
            {
                AllSearchTenaderInfoWithAllDetail = tenaderInfoWithDetail,
                DisplayText = finalSearchText,
                Newpagecnt = page,
                SelectedOwnership = selectedOwnership,
                SelectedOwnershipNotIn = selectedOwnershipNotIn,
                SelectedSector = selectedSector,
                SelectedSectorNotIn = selectedSectorNotIn,
                SelectedAgency = selectedAgency,
                SelectedAgencyNotIn = selectedAgencyNotIn,
                SelectedIndustrySubIndustry = selectedIndustrySubIndustry,
                SelectedIndustrySubIndustryNotIn = selectedIndustrySubIndustryNotIn,
                SelectedLocation = selectedLocation,
                SelectedLocationNotIn = selectedLocationNotIn,
                SelectedKeyword1 = selectedKeyword1,
                SelectedKeyword2 = selectedKeyword2,
                SelectedKeyword3 = selectedKeyword3,
                OtherKeywords = otherKeywords,
                NotUsedKeywords = notUsedKeywords,
                DocumentType = documentType,
                IsIcbNcb = icbncb,
                TenderValue = tenderValue,
                TenderStatusFlag = 0,
                //TenderValueFrom = Convert.ToDouble(tenderValueFrom),
                //TenderValueTo = Convert.ToDouble(tenderValueTo),
                TenderValueFrom = tenderValueFrom,
                TenderValueTo = tenderValueTo,
                PermissionId = pId,
                TotalLive = totalLive,
                TotalFresh = totalFresh,
                TotalClosed = totalClosed,
                UserId = objUserMembershipDetail.intClientId,
                EmailConfigList = emailList,
                Username = username,
                Purpose = purpose,
                Email1 = email1,
                Email2 = email2,
                eId = empdetails.intEmpId,
                TenderYear = TenderYear,
                OrderBys = OrderBys,
                AscDesc = AscDesc,
                orderByList = orderByList,
                ascdescList = ascdescList
            };
            return View(tenderDetail);
        }

        public ActionResult ViewSampleTendersPartial(int pId, int page, int tenderStatusFlag, string selectedOwnership, string selectedOwnershipNotIn,
            string selectedSector, string selectedSectorNotIn, string selectedAgency, string selectedAgencyNotIn,
            string selectedIndustrySubIndustry, string selectedIndustrySubIndustryNotIn, string selectedLocation, string selectedLocationNotIn,
            string selectedKeyword1, string selectedKeyword2, string selectedKeyword3, string otherKeywords, string notUsedKeywords,
            string documentType, string icbncb, string selectedRefNo, string tenderValue, double tenderValueFrom, double tenderValueTo,
            string TenderYear, string OrderBys, string AscDesc, int isFirst = 0)
        {
            var orderByList = new List<SelectListItem>();
            if (OrderBys == "OurRefNo")
            {
                orderByList.Insert(0, new SelectListItem { Text = "Reference No", Value = "OurRefNo", Selected = true });
                orderByList.Insert(1, new SelectListItem { Text = "Due Date ", Value = "SubmDate" });
            }
            else
            {
                orderByList.Insert(0, new SelectListItem { Text = "Reference No", Value = "OurRefNo" });
                orderByList.Insert(1, new SelectListItem { Text = "Due Date ", Value = "SubmDate", Selected = true });
            }

            var ascdescList = new List<SelectListItem>();
            if (AscDesc == "ASC")
            {
                ascdescList.Insert(0, new SelectListItem { Text = "Ascending", Value = "ASC", Selected = true });
                ascdescList.Insert(1, new SelectListItem { Text = "Descending ", Value = "DESC" });
            }
            else
            {
                ascdescList.Insert(0, new SelectListItem { Text = "Ascending", Value = "ASC" });
                ascdescList.Insert(1, new SelectListItem { Text = "Descending ", Value = "DESC", Selected = true });
            }

            const int pageSize = 10;
            Int64 totalCount = 0;
            if (isFirst == 0)
            {
                Session["TotalSampleRecordCount"] = null;
            }
            var indianGlobal = Convert.ToBoolean(Session["IndianGlobal"]);
            int purpose = 2;// Convert.ToInt32(Session["Purpose"].ToString());
            string finalSearchText = "";
            var tenaderList = _common.GetAllSearchTenderInfo_Client_SampleTender(pId, page, pageSize, tenderStatusFlag, true,
                  ref selectedOwnership, ref selectedOwnershipNotIn, ref selectedSector, ref selectedSectorNotIn,
                  ref selectedAgency, ref selectedAgencyNotIn, ref selectedIndustrySubIndustry, ref selectedIndustrySubIndustryNotIn,
                  ref selectedLocation, ref selectedLocationNotIn, ref selectedKeyword1,
                  ref selectedKeyword2, ref selectedKeyword3, ref otherKeywords, ref notUsedKeywords, ref documentType,
                  ref icbncb, ref tenderValue, ref tenderValueFrom, ref tenderValueTo, ref indianGlobal, ref finalSearchText, TenderYear, OrderBys, AscDesc, true);

            var tenaderInfoWithDetail = tenaderList.TenaderDetailSearch;

            if (Session["TotalSampleRecordCount"] == null)
            {
                var listTenderCount = tenaderList.TenaderDetailCount;

                if (listTenderCount.Any())
                {
                    switch (tenderStatusFlag)
                    {
                        case 1:
                            var detNew = listTenderCount.FirstOrDefault(x => x.TenderStatusReturn == 1);
                            totalCount = detNew != null ? detNew.TotalRecord : 0;
                            break;
                        case 2:
                            var detActive = listTenderCount.FirstOrDefault(x => x.TenderStatusReturn == 2);
                            totalCount = detActive != null ? detActive.TotalRecord : 0;
                            break;
                        case 3:
                            var detClosed = listTenderCount.FirstOrDefault(x => x.TenderStatusReturn == 3);
                            totalCount = detClosed != null ? detClosed.TotalRecord : 0;
                            break;
                        default:
                            totalCount += listTenderCount.Sum(t => t.TotalRecord);
                            break;
                    }
                }
                Session["TotalSampleRecordCount"] = totalCount;
            }

            var shoecurrentpage = (page / 10) + 1;
            ViewBag.DisplayCurrentPage = shoecurrentpage;
            ViewBag.CurrentPage = shoecurrentpage;
            ViewBag.PageSize = pageSize;
            double totalPage = 0;
            ViewBag.TotalPage = 0;

            if (Session["TotalSampleRecordCount"] != null)
            {
                totalPage = Math.Ceiling((double)((Int64)Session["TotalSampleRecordCount"]) / pageSize);
            }
            ViewBag.TotalPage = totalPage;

            var tenderDetail = new TenderDetail
            {
                AllSearchTenaderInfoWithAllDetail = tenaderInfoWithDetail,
                DisplayText = finalSearchText,
                DisplayCurrentPage = shoecurrentpage,
                TotalPage = totalPage,
                Newpagecnt = page,
                SelectedOwnership = selectedOwnership,
                SelectedOwnershipNotIn = selectedOwnershipNotIn,

                SelectedSector = selectedSector,
                SelectedSectorNotIn = selectedSectorNotIn,

                SelectedAgency = selectedAgency,
                SelectedAgencyNotIn = selectedAgencyNotIn,

                SelectedIndustrySubIndustry = selectedIndustrySubIndustry,
                SelectedIndustrySubIndustryNotIn = selectedIndustrySubIndustryNotIn,
                Purpose = purpose,
                SelectedLocation = selectedLocation,
                SelectedLocationNotIn = selectedLocationNotIn,
                SelectedKeyword1 = selectedKeyword1,
                SelectedKeyword2 = selectedKeyword2,
                SelectedKeyword3 = selectedKeyword3,
                OtherKeywords = otherKeywords,
                NotUsedKeywords = notUsedKeywords,
                DocumentType = documentType,
                IsIcbNcb = icbncb,
                TenderValue = tenderValue,
                //TenderValueFrom = Convert.ToDouble(tenderValueFrom),
                //TenderValueTo = Convert.ToDouble(tenderValueTo),
                TenderValueFrom = tenderValueFrom,
                TenderValueTo = tenderValueTo,
                TenderStatusFlag = tenderStatusFlag,
                eId = Convert.ToInt32(Session["eId"]),
                TenderYear = TenderYear,
                OrderBys = OrderBys,
                AscDesc = AscDesc,
                orderByList = orderByList,
                ascdescList = ascdescList
            };
            return PartialView(Url.Content("~/Views/Home/partialSearchSampleTenders.cshtml"), tenderDetail);
        }

        public ActionResult SetRefNo(string refNo, bool checkUncheck)
        {
            string str = "";
            if (this.Session["SelectRefNo"] != null)
                this.SelectRefNoList = (List<string>)this.Session["SelectRefNo"];
            if (!string.IsNullOrEmpty(refNo))
            {
                if (checkUncheck)
                {
                    if (!this.SelectRefNoList.Contains(refNo.Trim()))
                        this.SelectRefNoList.Add(refNo);
                }
                else if (this.SelectRefNoList.Contains(refNo.Trim()))
                    this.SelectRefNoList.Remove(refNo.Trim());
            }
            this.Session["SelectRefNo"] = (object)this.SelectRefNoList;
            if (this.SelectRefNoList.Any<string>())
                str = string.Join(",", (IEnumerable<string>)this.SelectRefNoList);
            return (ActionResult)this.Json((object)new
            {
                SelectRefNo = str
            });
        }
        public JsonResult GetUserTenderPermissionAllListDetails_Local(int permissionId)
        {
            string str1 = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = string.Empty;
            string str5 = string.Empty;
            string str6 = string.Empty;
            string str7 = string.Empty;
            string str8 = string.Empty;
            string str9 = string.Empty;
            string str10 = string.Empty;
            string str11 = string.Empty;
            string str12 = string.Empty;
            string str13 = string.Empty;
            string str14 = string.Empty;
            string str15 = string.Empty;
            if (permissionId != 0)
            {
                var permissionByIdLocal = this._userMembershipDetail.GetUsersTenderPermissionById_Local(permissionId);
                bool flag = true;
                if (permissionByIdLocal != null)
                    flag = permissionByIdLocal.bitIndianOrGlobal;
                List<tabClientPermissionWithAgency> permissionWithAgencyLocal1 = this._userMembershipDetail.GetUserPermissionWithAgency_Local(permissionId);
                if (permissionWithAgencyLocal1.Any())
                {
                    foreach (tabClientPermissionWithAgency permissionWithAgencyLocal2 in permissionWithAgencyLocal1)
                    {
                        str1 = str1 + (object)permissionWithAgencyLocal2.intAgencyId + "-";
                        tbAgencyIndian agencyDetailById = _tenderInfo.GetAgencyDetailById(permissionWithAgencyLocal2.intAgencyId);
                        if (permissionWithAgencyLocal2.bitIsUsed)
                        {
                            string str16 = permissionWithAgencyLocal2.intAgencyId.ToString((IFormatProvider)CultureInfo.InvariantCulture) + "#10";
                            str2 = str2 + "<div onclick=\"return tenderFilteration(this);\" class='tenderFilterationText' id='" + str16 + "'><i class=\"fa fa-hand-o-right mar5\"></i>" +
                                agencyDetailById.AgencyName + "</div>";
                        }
                    }
                    str1 = str1.Trim() != "" ? str1.Substring(0, str1.Length - 1) : "";
                }
                if (flag)
                {
                    List<tabClientPermissionWithOwnership> withOwnershipLocal1 = _userMembershipDetail.GetUserPermissionWithOwnership_Local(permissionId);
                    if (withOwnershipLocal1.Any())
                    {
                        foreach (tabClientPermissionWithOwnership withOwnershipLocal2 in withOwnershipLocal1)
                        {
                            str3 = str3 + (object)withOwnershipLocal2.intOwnershipId + "-";
                            tbOwnership ownershipById = this._tenderInfo.GetOwnershipById(withOwnershipLocal2.intOwnershipId);
                            if (withOwnershipLocal2.bitIsUsed)
                            {
                                string str16 = withOwnershipLocal2.intOwnershipId.ToString((IFormatProvider)CultureInfo.InvariantCulture) + "#11";
                                str4 = str4 + "<div onclick=\"return tenderFilteration(this);\" class='tenderFilterationText' id='" + str16 + "'><i class=\"fa fa-hand-o-right mar5\"></i>" +
                                    ownershipById.OwnershipName + "</div>";
                            }
                        }
                        str3 = str3.Trim() != "" ? str3.Substring(0, str3.Length - 1) : "";
                    }
                    List<tabClientPermissionWithSector> permissionWithSectorLocal1 = _userMembershipDetail.GetUserPermissionWithSector_Local(permissionId);
                    if (permissionWithSectorLocal1.Any())
                    {
                        foreach (tabClientPermissionWithSector permissionWithSectorLocal2 in permissionWithSectorLocal1)
                        {
                            str5 = str5 + (object)permissionWithSectorLocal2.intSectorId + "-";
                            tbSector sectorById = this._tenderInfo.GetSectorById(permissionWithSectorLocal2.intSectorId);
                            if (permissionWithSectorLocal2.bitIsUsed)
                            {
                                string str16 = permissionWithSectorLocal2.intSectorId.ToString((IFormatProvider)CultureInfo.InvariantCulture) + "#11";
                                str6 = str6 + "<div onclick=\"return tenderFilteration(this);\" class='tenderFilterationText' id='" + str16 + "'><i class=\"fa fa-hand-o-right mar5\"></i>" + sectorById.SectorName + "</div>";
                            }
                        }
                        str5 = str5.Trim() != "" ? str5.Substring(0, str5.Length - 1) : "";
                    }
                }
                List<tabClientPermissionWithLocation> withLocationLocal1 = _userMembershipDetail.GetUserPermissionWithLocation_Local(permissionId);
                if (withLocationLocal1.Any())
                {
                    foreach (tabClientPermissionWithLocation withLocationLocal2 in withLocationLocal1)
                    {
                        string str16 = "";
                        string str17 = "";
                        string str18 = "";
                        string str19 = "";
                        string str20 = "";
                        string str21 = "";
                        string str22 = "";
                        if (withLocationLocal2.intCityId != 0)
                        {
                            str9 = str9 + (object)withLocationLocal2.intCityId + "-";
                            str20 = withLocationLocal2.intCityId.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                            str19 = this._tenderInfo.GetCity(withLocationLocal2.intCityId).Location;
                        }
                        if (withLocationLocal2.intStateId != 0)
                        {
                            str8 = str8 + (object)withLocationLocal2.intStateId + "-";
                            str21 = withLocationLocal2.intStateId.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                            str18 = this._tenderInfo.GetState(withLocationLocal2.intStateId).StateName;
                        }
                        if (withLocationLocal2.intCountryId != 0)
                        {
                            str7 = str7 + (object)withLocationLocal2.intCountryId + "-";
                            str22 = withLocationLocal2.intCountryId.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                            str17 = this._tenderInfo.GetCountry(withLocationLocal2.intCountryId).CountryName;
                        }
                        if (str19.Trim() != "")
                            str16 = str19 + " &raquo; " + str18 + " &raquo; " + str17;
                        else if (str18.Trim() != "")
                            str16 = str18 + " &raquo; " + str17;
                        else if (str17.Trim() != "")
                            str16 = str17;
                        string str23 = "";
                        if (str20.Trim() != "")
                            str23 = " ( T.CountryID = " + str22 + " ) AND  ( T.StateId = " + str21 + " ) AND ( T.LocId =  " + str20 + " ) ";
                        else if (str21.Trim() != "")
                            str23 = " ( T.CountryID = " + str22 + " ) AND  ( T.StateId = " + str21 + " ) ";
                        else if (str22.Trim() != "")
                            str23 = " ( T.CountryID = " + str22 + " ) ";
                        if (str23 != "")
                            str23 += "#8";
                        if (!(str16 == "") && withLocationLocal2.bitIsUsed)
                            str10 = str10 + "<div onclick=\"return tenderFilteration(this);\" class='tenderFilterationText' id='" + str23 + "'><i class=\"fa fa-hand-o-right mar5\"></i>" +
                                str16 + "</div>";
                    }
                    str9 = str9.Trim() != "" ? str9.Substring(0, str9.Length - 1) : "";
                    str8 = str8.Trim() != "" ? str8.Substring(0, str8.Length - 1) : "";
                    str7 = str7.Trim() != "" ? str7.Substring(0, str7.Length - 1) : "";
                }
                List<tabClientPermissionWithIndSubIndustry> subIndustryLocal1 = _userMembershipDetail.GetUserPermissionWithIndSubIndustry_Local(permissionId);
                if (subIndustryLocal1.Any())
                {
                    foreach (tabClientPermissionWithIndSubIndustry subIndustryLocal2 in subIndustryLocal1)
                    {
                        string str16 = "";
                        string str17 = "";
                        string str18 = "";
                        string str19 = "";
                        string str20 = "";
                        if (subIndustryLocal2.intSubindustryId != 0)
                        {
                            str12 = str12 + (object)subIndustryLocal2.intSubindustryId + "-";
                            str20 = subIndustryLocal2.intSubindustryId.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                            str18 = this._tenderInfo.GetSubIndById(subIndustryLocal2.intSubindustryId).SubIndustryName;
                        }
                        if (subIndustryLocal2.intIndustryId != 0)
                        {
                            str11 = str11 + (object)subIndustryLocal2.intIndustryId + "-";
                            str19 = subIndustryLocal2.intIndustryId.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                            str17 = this._tenderInfo.GetIndustryById(subIndustryLocal2.intIndustryId).IndustryName;
                        }
                        if (str18.Trim() != "")
                            str16 = str18 + " &raquo; " + str17;
                        else if (str17.Trim() != "")
                            str16 = str17;
                        string str21 = "";
                        if (str20.Trim() != "")
                            str21 = " ( IndustryID = " + str19 + " ) AND ( SubIndustryID = " + str20 + " ) ";
                        else if (str11.Trim() != "")
                            str21 = " ( IndustryID = " + str19 + " ) ";
                        if (str21 != "")
                            str21 = "SELECT DISTINCT OURREFNO FROM TenderClassified WHERE " + str21 + "#9";
                        if (!(str16 == "") && subIndustryLocal2.bitIsUsed)
                            str13 = str13 + "<div onclick=\"return tenderFilteration(this);\"  class='tenderFilterationText' id='" + str21 + "'><i class=\"fa fa-hand-o-right mar5\"></i>" + str16 + "</div>";
                    }
                    str12 = str12.Trim() != "" ? str12.Substring(0, str12.Length - 1) : "";
                    str11 = str11.Trim() != "" ? str11.Substring(0, str11.Length - 1) : "";
                }
                List<tabClientPermissionWithProduct> withProductLocal1 = _userMembershipDetail.GetUserPermissionWithProduct_Local(permissionId);
                if (withProductLocal1.Any())
                {
                    foreach (tabClientPermissionWithProduct withProductLocal2 in withProductLocal1)
                    {
                        string str16 = withProductLocal2.bitIsKeywordSearch ? "1" : "0";
                        string str17 = withProductLocal2.bitIsWordSearch ? "1" : "0";
                        string str18 = withProductLocal2.bitIsExactPhrase ? "1" : "0";
                        str14 = str14 + (object)withProductLocal2.intProductId + "~" + str16 + "~" + str17 + "~" + str18 + "-";
                        tbProduct productById = this._tenderInfo.GetProductById(withProductLocal2.intProductId);
                        string str19 = withProductLocal2.intProductId.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                        if (withProductLocal2.bitIsKeywordSearch && !withProductLocal2.bitIsWordSearch && !withProductLocal2.bitIsExactPhrase)
                            str19 += "#1";
                        else if (!withProductLocal2.bitIsKeywordSearch && withProductLocal2.bitIsWordSearch && !withProductLocal2.bitIsExactPhrase)
                            str19 += "#2";
                        else if (!withProductLocal2.bitIsKeywordSearch && !withProductLocal2.bitIsWordSearch && withProductLocal2.bitIsExactPhrase)
                            str19 += "#3";
                        else if (withProductLocal2.bitIsKeywordSearch && withProductLocal2.bitIsWordSearch && !withProductLocal2.bitIsExactPhrase)
                            str19 += "#4";
                        else if (withProductLocal2.bitIsKeywordSearch && !withProductLocal2.bitIsWordSearch && withProductLocal2.bitIsExactPhrase)
                            str19 += "#5";
                        else if (!withProductLocal2.bitIsKeywordSearch && withProductLocal2.bitIsWordSearch && withProductLocal2.bitIsExactPhrase)
                            str19 += "#6";
                        else if (withProductLocal2.bitIsKeywordSearch && withProductLocal2.bitIsWordSearch && withProductLocal2.bitIsExactPhrase)
                            str19 += "#7";
                        str15 = str15 + "<div onclick=\"return tenderFilteration(this);\" class='tenderFilterationText' id='" + str19 + "'><i class=\"fa fa-hand-o-right mar5\"></i>"
                            + productById.ProductsName + "</div>";
                    }
                    str14 = str14.Trim() != "" ? str14.Substring(0, str14.Length - 1) : "";
                }
            }
            return this.Json((object)new
            {
                SelectedAgency = str1.Trim(),
                SelectedOwnership = str3.Trim(),
                SelectedSector = str5.Trim(),
                SelectedCity = str9.Trim(),
                SelectedState = str8.Trim(),
                SelectedCountry = str7.Trim(),
                SelectedIndustry = str11.Trim(),
                SelectedSubIndustry = str12.Trim(),
                SelectedProduct = str14.Trim(),
                SelectedAgencyWithName = (str2 == "" ? "<div class='NoDataClass'></div>" : str2 + "<div class='clr'></div>"),
                SelectedOwnershipWithName = (str4 == "" ? "<div class='NoDataClass'></div>" : str4 + "<div class='clr'></div>"),
                SelectedSectorWithName = (str6 == "" ? "<div class='NoDataClass'></div>" : str6 + "<div class='clr'></div>"),
                SelectedLocationWithName = (str10 == "" ? "<div class='NoDataClass'></div>" : str10 + "<div class='clr'></div>"),
                SelectedIndustrySubIndustryWithName = (str13 == "" ? "<div class='NoDataClass'></div>" : str13 + "<div class='clr'></div>"),
                SelectedProductWithName = (str15 == "" ? "<div class='NoDataClass'></div>" : str15 + "<div class='clr'></div>")
            }, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult PostTenderMail_MS(FormCollection coll)
        {
            string message = "ok";
            string userEmailId = "";
            try
            {
                string empEmailId = "";
                string empFromPwd = "";
                string empSmtpHost = "";
                int empSmtpPort = 0;
                string emailId = coll.Get("hdnFromEmailID");
                string permissionId = coll.Get("hdnValuesPId");
                string strTest = coll.Get("hdnTest");
                int UserId = Convert.ToInt32(coll.Get("hdnUserId"));
                string Name = coll.Get("hdnUsername");
                int Purpose = Convert.ToInt32(coll.Get("hdnPurpose"));
                string strTo = "";
                string strCc = "";
                string eId = coll.Get("hdneId");
                string displayText = coll.Get("hdnDisplayText");
                bool boolean = Convert.ToBoolean(coll.Get("hdnIndianGlobal"));
                switch (strTest)
                {
                    case "1":
                        strTo = coll.Get("hdnEmail1");
                        strCc = coll.Get("hdnEmail2");
                        break;
                    case "2":
                        strTo = emailId;
                        strCc = "";
                        break;
                }
                tabEmpEmailId withEmailIdLocal = _userMembershipDetail.GetLocalEmpEamilIdWithEmailId_Local(emailId);
                if (withEmailIdLocal != null)
                {
                    empEmailId = withEmailIdLocal.strEmailId.Trim();
                    empFromPwd = withEmailIdLocal.strPassword.Trim();
                    empSmtpHost = withEmailIdLocal.strSmtpName.Trim();
                    empSmtpPort = Convert.ToInt32((object)withEmailIdLocal.intSmtpPort);
                }
                string ourrefno = "";
                if (empSmtpPort != 0)
                    ourrefno = coll.Get("hdnTTNo");
                switch (strTest)
                {
                    case "1":
                        strCc = !(strCc == "") ? strCc + "," + emailId : emailId;
                        break;
                }
                string subject = "";
                subject = "TenderAssist247.com";
                string str7 = "Please find the tender information based on your request " + subject;
                string assignedKeywords = "";
                List<tabClientPermissionWithProduct> withProductLocal1 = _userMembershipDetail.GetUserPermissionWithProduct_Local(Convert.ToInt32(permissionId));
                if (withProductLocal1.Any<tabClientPermissionWithProduct>())
                {
                    foreach (tabClientPermissionWithProduct withProductLocal2 in withProductLocal1)
                    {
                        tbProduct productById = _tenderInfo.GetProductById(withProductLocal2.intProductId);
                        if (productById != null)
                            assignedKeywords = assignedKeywords == "" ? productById.ProductsName : assignedKeywords + ", " + productById.ProductsName;
                    }
                }
                string empEmail = "mailto://" + empEmailId;
                string messagebody = _common.SampleTenderMailFormat();

                string sampleTenderFormat = _common.CreateTenderFormat(ourrefno, Purpose, eId, displayText, boolean);
                sampleTenderFormat = sampleTenderFormat.Replace("{{Name}}", Name)
                                                    .Replace("{{AssignedKeywords}}", assignedKeywords)
                                                    .Replace("{{EmpEmail}}", empEmail)
                                                    .Replace("{{EmpEmailName}}", empEmailId);

                messagebody = messagebody.Replace("{{SampleTennderInfo}}", sampleTenderFormat);

                string strSubject = subject + " : Live Tenders ";
                userEmailId = strTo;
                if (!Utility.SendMail(strTo, strCc, "", strSubject, messagebody.ToString(), "", empEmailId, empFromPwd, empSmtpHost, empSmtpPort))
                {
                    message = "error";
                    userEmailId = "";
                }
                if (strTest == "1")
                    _tenderInfo.InsertSendSampleTenders(UserId);
            }
            catch (Exception ex)
            {
                message = "error";
                userEmailId = "";
            }
            return (ActionResult)Json((object)new
            {
                msg = message,
                usermail = userEmailId
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}