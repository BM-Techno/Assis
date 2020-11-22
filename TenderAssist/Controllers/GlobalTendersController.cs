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
using TenderAssist.ViewModel;
using static TenderAssist.CommonHelper.Utility;
using static TenderAssist.Models.SearchModel;

namespace TenderAssist.Controllers
{
    public class GlobalTendersController : BaseController
    {
        public GlobalTendersController()
        {
            _tenderInfo = new TenderInformation();
            _common = new CommonController();
            _getListItems = new GetListItems();
        }


        #region VARIABLES
        private readonly TenderInformation _tenderInfo;
        private readonly CommonController _common;
        private readonly GetListItems _getListItems;
        private static int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DataPageSize"]);
        private List<string> WithinSearchWords = new List<string>();
        #endregion

        #region GLOBAL TENDER LIST PAGE

        public ActionResult Tenders()
        {
            return View(new TenderDetail()
            {
                IsAdvanceSearch = false,
                TendersBy = TenderTypeList.Keyword,
                CountryList = _getListItems.CountryList(),
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                IdVal = 0,
                Subscribetype = SubscribType.DownloadTender,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.DownloadTender,
                FormType = FormType.PopupForms,
                IsHomePage = true,
                IsGlobalTender = true
            });
        }
        public ActionResult Index(string keyword = "", string withinSearchText = "")
        {
            ResetTotalCountSession_Global();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            const int tenderBy = TenderTypeList.SearchTender;
            var isSearchTextChanged = CheckWithinSearch(withinSearchText);

            var productlist = _tenderInfo.GetExactProduct(keyword.Trim());
            if (productlist.Any()) { id = productlist.FirstOrDefault().ProductsId; }

            var fieldId = id;
            var fieldName = keyword;
            //string countryIds = "";
            //int tenderStatusFlag = 0;

            if (Session["SearhGlobalTenderResult"] != null)
            {
                tenderDetail = (TenderDetail)Session["SearhGlobalTenderResult"];
                //countryIds = tenderDetail.SelectedCountry;
                //tenderStatusFlag = tenderDetail.TenderStatusFlag;

                var oldFieldId = tenderDetail.FieldId;
                if (oldFieldId != fieldId)
                {
                    isSearchTextChanged = true;
                }
            }
            var advanceSearchPara = SetDefaultAdvSearchParam();

            tenderDetail = GetTenderResult(tenderBy, page, "SearhGlobalTenderResult", advanceSearchPara, id, true, isSearchTextChanged);

            SetTenderDetails(tenderBy, "", fieldId, fieldName, ref tenderDetail);

            return View("Index", tenderDetail);
        }
        public ActionResult GetDefaultGlobalTender(int tenderStatus = 1)
        {
            List<SearchTenaderInfoWithAllDetail> tenaderInfoWithDetail = new List<SearchTenaderInfoWithAllDetail>();
            AdvanceSearchParameter AdvanceSearch = new AdvanceSearchParameter()
            {
                TenderStatusFlag = tenderStatus
            };

            var tenderDetail = _common.GetSearchGlobalTenderResult(0, 0, "", 1, AdvanceSearch, 0, false);
            tenderDetail.IsGlobalTender = true;
            tenaderInfoWithDetail = tenderDetail.AllSearchTenaderInfoWithAllDetail;

            return PartialView(Url.Content("~/Views/GlobalTenders/_HomeTenderListPanel.cshtml"), tenderDetail);
        }

        #region ----------ALL REGION BASED TENDER LIST----------

        public ActionResult MiddleEastCountryTenders(string countryName = "", string withinSearchText = "")
        {
            return View("Index", CountryRegionTenderList(TenderTypeList.MiddleEastCountryRegion, countryName, withinSearchText));
        }
        public ActionResult EuropeanCountryTenders(string countryName = "", string withinSearchText = "")
        {
            return View("Index", CountryRegionTenderList(TenderTypeList.EuropeanCountryRegion, countryName, withinSearchText));
        }
        public ActionResult AfricanCountryTenders(string countryName = "", string withinSearchText = "")
        {
            return View("Index", CountryRegionTenderList(TenderTypeList.AfricanCountryRegion, countryName, withinSearchText));
        }
        public ActionResult AsianCountryTenders(string countryName = "", string withinSearchText = "")
        {
            return View("Index", CountryRegionTenderList(TenderTypeList.AsianCountryRegion, countryName, withinSearchText));
        }
        public ActionResult SAARCountryTenders(string countryName = "", string withinSearchText = "")
        {
            return View("Index", CountryRegionTenderList(TenderTypeList.SaarCountryRegion, countryName, withinSearchText));
        }
        public ActionResult AustraliaOceaniaCountryTenders(string countryName = "", string withinSearchText = "")
        {
            return View("Index", CountryRegionTenderList(TenderTypeList.AustraliaOceaniaCountryRegion, countryName, withinSearchText));
        }
        public ActionResult SouthAmericaCountryTenders(string countryName = "", string withinSearchText = "")
        {
            return View("Index", CountryRegionTenderList(TenderTypeList.SouthAmericaCountryRegion, countryName, withinSearchText));
        }
        public ActionResult NorthAmericaCountryTenders(string countryName = "", string withinSearchText = "")
        {
            return View("Index", CountryRegionTenderList(TenderTypeList.NorthAmericaCountryRegion, countryName, withinSearchText));
        }

        private TenderDetail CountryRegionTenderList(int regionId, string countryName, string withinSearchText)
        {
            string sessionKeyName = "";
            switch (regionId)
            {
                default://SEARCH                             
                    sessionKeyName = "SearhGlobalTenderResult";
                    break;
                case TenderTypeList.MiddleEastCountryRegion:
                    sessionKeyName = "SearhMiddleEastCountryTenderResult";
                    break;
                case TenderTypeList.EuropeanCountryRegion:
                    sessionKeyName = "SearhEuropeanCountryTenderResult";
                    break;
                case TenderTypeList.AfricanCountryRegion:
                    sessionKeyName = "SearhAfricanCountryTenderResult";
                    break;
                case TenderTypeList.AsianCountryRegion:
                    sessionKeyName = "SearhAsianCountryTenderResult";
                    break;
                case TenderTypeList.SaarCountryRegion:
                    sessionKeyName = "SearhSAARCountryTenderResult";
                    break;
                case TenderTypeList.AustraliaOceaniaCountryRegion:
                    sessionKeyName = "SearhAustraliaOceaniaCountryTenderResult";
                    break;
                case TenderTypeList.SouthAmericaCountryRegion:
                    sessionKeyName = "SearhSouthAmericaCountryTenderResult";
                    break;
                case TenderTypeList.NorthAmericaCountryRegion:
                    sessionKeyName = "SearhNorthAmericaCountryTenderResult";
                    break;

            }

            ResetTotalCountSession_Global();
            TenderDetail tenderDetail;
            int id = 0; int page = 0;

            //const int regionId = TenderTypeList.MiddleEastCountryRegion;

            countryName = countryName.Replace("-", " ").ToLower().Trim().ToString();
            var countryList = _tenderInfo.ListCountrybyRegion(regionId).Where(x => x.CountryName.ToLower().Trim().ToString().Equals(countryName));
            if (countryList.Any()) { id = countryList.FirstOrDefault().CountryId; }

            var fieldId = id;
            var fieldName = countryName;
            var countries = "";

            var isSearchTextChanged = CheckWithinSearch(withinSearchText);

            if (Session[sessionKeyName] != null)
            {
                tenderDetail = (TenderDetail)Session[sessionKeyName];
                countries = tenderDetail.SelectedCountry;

                var oldFieldId = tenderDetail.FieldId;
                if (oldFieldId != fieldId)
                {
                    isSearchTextChanged = true;
                    countries = countries == "" ? fieldId.ToString() : countries + "," + fieldId.ToString();
                }
            }
            else
            {
                SearchedWordsClear();
                countries = fieldId.ToString();
            }
            var advanceSearchPara = SetDefaultAdvSearchParam();
            advanceSearchPara.SelectedCountries = countries;
            tenderDetail = CountryRegionWiseSearch(fieldId, fieldName, "", page, regionId, advanceSearchPara, isSearchTextChanged);

            return tenderDetail;
        }
        #endregion

        #endregion

        #region GLOBAL REGION LISTS

        public ActionResult TendersByMiddleEastCountry()
        {
            return View("CountryLinkByRegions", CountryRegion(Utility.TenderTypeList.MiddleEastCountryRegion));
        }
        public ActionResult TendersByEuropeanCountry()
        {
            return View("CountryLinkByRegions", CountryRegion(Utility.TenderTypeList.EuropeanCountryRegion));

        }
        public ActionResult TendersByAfricanCountry()
        {
            return View("CountryLinkByRegions", CountryRegion(Utility.TenderTypeList.AfricanCountryRegion));

        }
        public ActionResult TendersByAsianCountry()
        {
            return View("CountryLinkByRegions", CountryRegion(Utility.TenderTypeList.AsianCountryRegion));

        }
        public ActionResult TendersBySaarCountry()
        {
            return View("CountryLinkByRegions", CountryRegion(Utility.TenderTypeList.SaarCountryRegion));

        }
        public ActionResult TendersByAustraliaOceaniaCountry()
        {
            return View("CountryLinkByRegions", CountryRegion(Utility.TenderTypeList.AustraliaOceaniaCountryRegion));
        }
        public ActionResult TendersBySouthAmericaCountry()
        {
            return View("CountryLinkByRegions", CountryRegion(Utility.TenderTypeList.SouthAmericaCountryRegion));
        }
        public ActionResult TendersByNorthAmericaCountry()
        {
            return View("CountryLinkByRegions", CountryRegion(Utility.TenderTypeList.NorthAmericaCountryRegion));
        }

        private TenderDetail CountryRegion(int regionId)
        {
            ResetTotalCountSession_Global();
            ClearSession_Global();
            SearchedWordsClear();
            string displayName = "";
            switch (regionId)
            {
                case Utility.TenderTypeList.MiddleEastCountryRegion:
                    displayName = Utility.TenderTypeDisplayText.DisplayMiddleEastCountryName;
                    break;
                case Utility.TenderTypeList.EuropeanCountryRegion:
                    displayName = Utility.TenderTypeDisplayText.DisplayEuropeanCountryName;
                    break;
                case Utility.TenderTypeList.AfricanCountryRegion:
                    displayName = Utility.TenderTypeDisplayText.DisplayAfricanCountryName;
                    break;
                case Utility.TenderTypeList.AsianCountryRegion:
                    displayName = Utility.TenderTypeDisplayText.DisplayAsianCountryName;
                    break;
                case Utility.TenderTypeList.SaarCountryRegion:
                    displayName = Utility.TenderTypeDisplayText.DisplaySaarCountryName;
                    break;
                case Utility.TenderTypeList.AustraliaOceaniaCountryRegion:
                    displayName = Utility.TenderTypeDisplayText.DisplayAustraliaOceaniaCountryName;
                    break;
                case Utility.TenderTypeList.SouthAmericaCountryRegion:
                    displayName = Utility.TenderTypeDisplayText.DisplaySouthAmericaCountryName;
                    break;
                case Utility.TenderTypeList.NorthAmericaCountryRegion:
                    displayName = Utility.TenderTypeDisplayText.DisplayNorthAmericaCountryName;
                    break;
            }

            var rnd = new Random();

            var listAllCountry = _tenderInfo.ListCountrybyRegion(regionId).ToList();
            var listAllCountrytext = string.Join(" Tenders, ", listAllCountry.Select(x => x.CountryName).ToArray());
            listAllCountrytext += " Tenders";


            var tenderMetaData = _common.GetTenderMetaDataGlobal(regionId);
            ViewBag.Title = tenderMetaData.Title.Replace(TenderMetaReplaceName.GlobalCountryName, displayName);
            ViewBag.description = tenderMetaData.Description;
            ViewBag.keywords = tenderMetaData.Keyword.Replace(TenderMetaReplaceName.GlobalCountryName, displayName + "-" + listAllCountrytext);

            var tenderDetail = new TenderDetail
            {
                RegionId = regionId,
                AllCountryList = listAllCountry,
                FormTitle = displayName,
                IsGlobalTender = true
            };

            return tenderDetail;
        }


        public TenderDetail LoadCountryByRegionIdList(int regionId, int totalDisplay)
        {
            var countrylist = _tenderInfo.ListCountrybyRegion(regionId).OrderBy(x => (new Random()).Next()).Take(totalDisplay).ToList();

            var tenderDetail = new TenderDetail
            {
                TotalDisplay = totalDisplay,
                AllCountryList = countrylist,
                RegionId = regionId
            };
            return tenderDetail;
        }

        #endregion


        #region SEARCHING LOGIC
        private bool CheckWithinSearch(string withinSearchText)
        {
            var isSearchTextChanged = false;
            if (!string.IsNullOrEmpty(withinSearchText))
            {
                withinSearchText = withinSearchText.Replace("-", " ");
                if (Session["WithinSearchText"] == null)
                { Session["WithinSearchText"] = withinSearchText; isSearchTextChanged = true; }
                else
                {
                    if (Session["WithinSearchText"].ToString().ToLower().Trim() != withinSearchText.ToLower().Trim())
                    { Session["WithinSearchText"] = withinSearchText; isSearchTextChanged = true; }
                }

                FillWithinSearchWords(withinSearchText);
            }
            return isSearchTextChanged;
        }
        public ActionResult SearchTender(SearchIndianTenderModel searchResult)
        {
            var advanceSearch = searchResult.AdvanceSearch;
            var regionId = searchResult.TenderBy;
            var page = searchResult.Page;
            var searchText = string.IsNullOrEmpty(searchResult.Search) ? "" : searchResult.Search;
            var filterMainText = string.IsNullOrEmpty(searchResult.Search) ? "" : searchResult.Search;
            var searchType = searchResult.SType;
            var isFirst = searchResult.IsFirst;
            var fieldId = searchResult.FId;
            var fieldName = searchResult.FName;
            var searchProductId = advanceSearch.SearchProductId;

            Session["AdvanceSearchGlobalParams"] = advanceSearch;

            //var countryIds = searchResult.CountryIds ?? "";
            string sessionKeyName, newUrl, tenderWordName;
            tenderWordName = regionId == 0 ? TenderWordNameList.KeywordWord : TenderWordNameList.GlobalCountryWord;
            switch (regionId)
            {
                default://SEARCH                             
                    sessionKeyName = "SearhGlobalTenderResult";
                    newUrl = ConfigurationManager.AppSettings["GlobalTenderKeywordUrl"];
                    break;
                case TenderTypeList.MiddleEastCountryRegion:
                    sessionKeyName = "SearhMiddleEastCountryTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByMiddleEastCountryUrl"];
                    break;
                case TenderTypeList.EuropeanCountryRegion:
                    sessionKeyName = "SearhEuropeanCountryTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByEuropeanCountryUrl"];
                    break;
                case TenderTypeList.AfricanCountryRegion:
                    sessionKeyName = "SearhAfricanCountryTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByAfricanCountryUrl"];
                    break;
                case TenderTypeList.AsianCountryRegion:
                    sessionKeyName = "SearhAsianCountryTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByAsianCountryUrl"];
                    break;
                case TenderTypeList.SaarCountryRegion:
                    sessionKeyName = "SearhSAARCountryTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderBySAARCountryUrl"];
                    break;
                case TenderTypeList.AustraliaOceaniaCountryRegion:
                    newUrl = ConfigurationManager.AppSettings["TenderByAustraliaOceaniaCountryUrl"];
                    sessionKeyName = "SearhAustraliaOceaniaCountryTenderResult";
                    break;
                case TenderTypeList.SouthAmericaCountryRegion:
                    newUrl = ConfigurationManager.AppSettings["TenderBySouthAmericaCountryUrl"];
                    sessionKeyName = "SearhSouthAmericaCountryTenderResult";
                    break;
                case TenderTypeList.NorthAmericaCountryRegion:
                    newUrl = ConfigurationManager.AppSettings["TenderByNorthAmericaCountryUrl"];
                    sessionKeyName = "SearhNorthAmericaCountryTenderResult";
                    break;

            }
            Session["PaggingUrl"] = newUrl;

            if (Session["WithinSearchGlobalTextList"] != null)
            {
                WithinSearchWords = (List<string>)Session["WithinSearchGlobalTextList"];
            }

            var tenderDetail = GetSearchTenderResult(regionId, page, filterMainText.Trim(), searchType, advanceSearch, searchProductId, true, 0, WithinSearchWords);
            tenderDetail.FieldId = fieldId;
            tenderDetail.FieldName = fieldName;
            Session[sessionKeyName] = tenderDetail;

            page = page == 0 ? 1 : page;
            searchText = string.IsNullOrEmpty(searchText) || searchText == "*-*" ? "" : searchText.ToString(CultureInfo.InvariantCulture);

            return Json(new
            {
                newurl = newUrl,
                page,
                searchText = searchText.Replace(" ", "-"),
                fid = fieldId,
                fname = fieldName.Replace(" ", "-"),
                tenderBy = regionId,
                TenderWordName = tenderWordName
            });
        }
        public ActionResult GetTenderResultOnLoading(int tenderBy, int page = 0)
        {
            ViewBag.IsEndOfRecords = false;
            TenderDetail tenderDetail = null;
            /*OtherValues();*/
            int searchId = 0;
            string sessionKeyName = "";
            if (Request.IsAjaxRequest())
            {
                var advanceSearchPara = new AdvanceSearchParameter();
                if (Session["AdvanceSearchGlobalParams"] != null)
                {
                    advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchGlobalParams"];
                }

                switch (tenderBy)
                {
                    default://SEARCH                             
                        sessionKeyName = "SearhGlobalTenderResult";
                        break;
                    case TenderTypeList.MiddleEastCountryRegion:
                        sessionKeyName = "SearhMiddleEastCountryTenderResult";
                        break;
                    case TenderTypeList.EuropeanCountryRegion:
                        sessionKeyName = "SearhEuropeanCountryTenderResult";
                        break;
                    case TenderTypeList.AfricanCountryRegion:
                        sessionKeyName = "SearhAfricanCountryTenderResult";
                        break;
                    case TenderTypeList.AsianCountryRegion:
                        sessionKeyName = "SearhAsianCountryTenderResult";
                        break;
                    case TenderTypeList.SaarCountryRegion:
                        sessionKeyName = "SearhSAARCountryTenderResult";
                        break;
                    case TenderTypeList.AustraliaOceaniaCountryRegion:
                        sessionKeyName = "SearhAustraliaOceaniaCountryTenderResult";
                        break;
                    case TenderTypeList.SouthAmericaCountryRegion:
                        sessionKeyName = "SearhSouthAmericaCountryTenderResult";
                        break;
                    case TenderTypeList.NorthAmericaCountryRegion:
                        sessionKeyName = "SearhNorthAmericaCountryTenderResult";
                        break;
                }
                var tenderStatus = advanceSearchPara.TenderStatusFlag;
                FillWithinSearchWords(Session["WithinSearchGlobalText"] == null ? "" : Session["WithinSearchGlobalText"].ToString());
                tenderDetail = GetTenderResult(tenderBy, page, sessionKeyName, advanceSearchPara, searchId, false);
                if (advanceSearchPara != null)
                {
                    Session["AdvanceSearchGlobalParams"] = advanceSearchPara;
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
                return PartialView(Url.Content("~/Views/GlobalTenders/Partial/partialSearchResultData.cshtml"), tenderDetail);
            }
            return null;
        }
        public ActionResult GetTenderFromTenderStatus(int tenderBy, int tenderStatus = 0)
        {
            int page = 1;
            ViewBag.IsEndOfRecords = false;
            TenderDetail tenderDetail = null;
            int searchId = 0;
            string sessionKeyName = "";
            if (Request.IsAjaxRequest())
            {
                var advanceSearchPara = new AdvanceSearchParameter();
                if (Session["AdvanceSearchGlobalParams"] != null)
                {
                    advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchGlobalParams"];
                }
                advanceSearchPara.TenderStatusFlag = tenderStatus;

                switch (tenderBy)
                {
                    default://SEARCH                             
                        sessionKeyName = "SearhGlobalTenderResult";
                        break;
                    case TenderTypeList.MiddleEastCountryRegion:
                        sessionKeyName = "SearhMiddleEastCountryTenderResult";
                        break;
                    case TenderTypeList.EuropeanCountryRegion:
                        sessionKeyName = "SearhEuropeanCountryTenderResult";
                        break;
                    case TenderTypeList.AfricanCountryRegion:
                        sessionKeyName = "SearhAfricanCountryTenderResult";
                        break;
                    case TenderTypeList.AsianCountryRegion:
                        sessionKeyName = "SearhAsianCountryTenderResult";
                        break;
                    case TenderTypeList.SaarCountryRegion:
                        sessionKeyName = "SearhSAARCountryTenderResult";
                        break;
                    case TenderTypeList.AustraliaOceaniaCountryRegion:
                        sessionKeyName = "SearhAustraliaOceaniaCountryTenderResult";
                        break;
                    case TenderTypeList.SouthAmericaCountryRegion:
                        sessionKeyName = "SearhSouthAmericaCountryTenderResult";
                        break;
                    case TenderTypeList.NorthAmericaCountryRegion:
                        sessionKeyName = "SearhNorthAmericaCountryTenderResult";
                        break;
                }
                FillWithinSearchWords(Session["WithinSearchGlobalText"] == null ? "" : Session["WithinSearchGlobalText"].ToString());
                tenderDetail = GetTenderResult(tenderBy, page, sessionKeyName, advanceSearchPara, searchId, true, true);
                if (advanceSearchPara != null)
                {
                    Session["AdvanceSearchGlobalParams"] = advanceSearchPara;
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
                return PartialView(Url.Content("~/Views/GlobalTenders/Partial/partialSearchResultData.cshtml"), tenderDetail);
            }
            return null;
        }
        private TenderDetail GetTenderResult(int regionId, int page, string sessionKeyName, AdvanceSearchParameter advanceSearch, int searchId = 0,
            bool isSearchWithCount = true, bool isSearchTextChanged = false)
        {
            TenderDetail tenderDetail = null;
            Session["AdvanceSearchGlobalParams"] = advanceSearch;

            var pagecnt = page != 0 ? page - 1 : 0;
            var newpagecnt = (pagecnt * PageSize);

            if (Session[sessionKeyName] != null)
            {
                tenderDetail = (TenderDetail)Session[sessionKeyName];
                var fieldid = tenderDetail.FieldId;
                var fieldname = tenderDetail.FieldName;
                if (tenderDetail.Newpagecnt != newpagecnt || isSearchTextChanged)
                {
                    tenderDetail = GetSearchTenderResult(regionId, newpagecnt, tenderDetail.DisplayText, tenderDetail.SearchType, advanceSearch, tenderDetail.SearchProuctId,
                       isSearchWithCount, tenderDetail.OrderBy, WithinSearchWords);

                    tenderDetail.FieldId = fieldid;
                    tenderDetail.FieldName = fieldname;
                    Session[sessionKeyName] = tenderDetail;
                }
            }
            else
            {
                tenderDetail = searchId == 0
                    ? GetSearchTenderResult(regionId, newpagecnt, "", 2, advanceSearch, 0, isSearchWithCount, 0, WithinSearchWords)
                    : GetSearchTenderResult(regionId, newpagecnt, "", 1, advanceSearch, 0, isSearchWithCount, 0, WithinSearchWords);
                Session[sessionKeyName] = tenderDetail;
            }

            if (isSearchWithCount)
            {
                Session["TotalSearchedGlobalTenders"] = tenderDetail.Total.ToString(CultureInfo.InvariantCulture);
                Session["TotalGlobalLiveTenders"] = tenderDetail.TotalLive.ToString(CultureInfo.InvariantCulture);
                Session["TotalGlobalFreshTenders"] = tenderDetail.TotalFresh.ToString(CultureInfo.InvariantCulture);
                Session["TotalGlobalClosedTenders"] = tenderDetail.TotalClosed.ToString(CultureInfo.InvariantCulture);
            }

            tenderDetail.Total = Convert.ToInt64(Session["TotalSearchedGlobalTenders"]);
            tenderDetail.TotalLive = Convert.ToInt64(Session["TotalGlobalLiveTenders"]);
            tenderDetail.TotalFresh = Convert.ToInt64(Session["TotalGlobalFreshTenders"]);
            tenderDetail.TotalClosed = Convert.ToInt64(Session["TotalGlobalClosedTenders"]);

            if (advanceSearch != null)
            {
                Session["AdvanceSearchGlobalParams"] = advanceSearch;
            }

            return tenderDetail;
        }

        public TenderDetail GetSearchTenderResult(int regionId, int page, string searchText, int searchType, AdvanceSearchParameter advanceSearch,
            //string countryIds = "", int? tenderStatusFlag = 0,
            int searchProductId = 0, bool isSearchWithCount = true, int? orderByType = 0, List<string> WithinSearchWords = null)
        {
            ClearSession();
            return _common.GetSearchGlobalTenderResult(regionId, page, searchText, searchType, advanceSearch, searchProductId, isSearchWithCount, orderByType, WithinSearchWords);
        }

        public TenderDetail CountryRegionWiseSearch(int fieldId, string fieldName, string searchText, int page, int regionId, AdvanceSearchParameter advanceSearch,
            bool isSearchTextChanged = false)
        {
            var pagecnt = page != 0 ? page - 1 : 0;
            var pageCount = 10;
            var newpagecnt = (pagecnt * pageCount);
            string sessionKeyName = "";
            var countries = advanceSearch.SelectedCountries;

            fieldName = fieldName.Replace("-", " ");
            //searchText = _common.ReplaceSpaceWith(searchText.Trim(), SpaceSeperator);
            switch (regionId)
            {
                case TenderTypeList.MiddleEastCountryRegion:
                    sessionKeyName = "SearhMiddleEastCountryTenderResult";
                    break;
                case TenderTypeList.EuropeanCountryRegion:
                    sessionKeyName = "SearhEuropeanCountryTenderResult";
                    break;
                case TenderTypeList.AfricanCountryRegion:
                    sessionKeyName = "SearhAfricanCountryTenderResult";
                    break;
                case TenderTypeList.AsianCountryRegion:
                    sessionKeyName = "SearhAsianCountryTenderResult";
                    break;
                case TenderTypeList.SaarCountryRegion:
                    sessionKeyName = "SearhSAARCountryTenderResult";
                    break;
                case TenderTypeList.AustraliaOceaniaCountryRegion:
                    sessionKeyName = "SearhAustraliaOceaniaCountryTenderResult";
                    break;
                case TenderTypeList.SouthAmericaCountryRegion:
                    sessionKeyName = "SearhSouthAmericaCountryTenderResult";
                    break;
                case TenderTypeList.NorthAmericaCountryRegion:
                    sessionKeyName = "SearhNorthAmericaCountryTenderResult";
                    break;
            }
            TenderDetail tenderDetail;

            if (Session[sessionKeyName] != null)
            {
                tenderDetail = (TenderDetail)Session[sessionKeyName];
                var oldId = tenderDetail.FieldId;
                countries = tenderDetail.SelectedCountry;
                if (oldId != fieldId)
                {
                    countries = "";
                    SearchedWordsClear();
                    countries = string.IsNullOrEmpty(countries) ? fieldId.ToString(CultureInfo.InvariantCulture) : countries;
                    tenderDetail = _common.GetSearchGlobalTenderResult(regionId, newpagecnt, tenderDetail.DisplayText.Trim(), tenderDetail.SearchType, advanceSearch, fieldId, true, 0, WithinSearchWords);
                    Session[sessionKeyName] = tenderDetail;
                }
                else
                {
                    if (tenderDetail.Newpagecnt != newpagecnt || tenderDetail.TenderStatus != advanceSearch.TenderStatusFlag || isSearchTextChanged)
                    {
                        var isSearchWithCount = (tenderDetail.TenderStatus != advanceSearch.TenderStatusFlag) || (tenderDetail.DisplayText.Trim() != searchText.Trim());
                        tenderDetail = _common.GetSearchGlobalTenderResult(regionId, newpagecnt, tenderDetail.DisplayText.Trim(), tenderDetail.SearchType, advanceSearch, fieldId, isSearchWithCount, 0, WithinSearchWords);
                        Session[sessionKeyName] = tenderDetail;
                    }
                }
            }
            else
            {
                SearchedWordsClear();
                countries = string.IsNullOrEmpty(countries) ? fieldId.ToString(CultureInfo.InvariantCulture) : countries;
                tenderDetail = _common.GetSearchGlobalTenderResult(regionId, newpagecnt, searchText.Trim(), 1, advanceSearch, fieldId, true, 0, WithinSearchWords);
                Session[sessionKeyName] = tenderDetail;
            }

            SetTenderDetails(regionId, advanceSearch.OtherKeywordText, fieldId, fieldName, ref tenderDetail);

            return tenderDetail;
        }

        private void SetTenderDetails(int regionId, string withinSearchText, int fieldId, string fieldName, ref TenderDetail tenderDetail)
        {
            string sessionKeyName = "";

            var shoecurrentpage = (tenderDetail.Newpagecnt / 10) + 1;
            ViewBag.DisplayCurrentPage = shoecurrentpage;
            ViewBag.CurrentPage = shoecurrentpage;
            ViewBag.PageSize = tenderDetail.PageSize;
            ViewBag.TotalPage = Math.Ceiling((double)tenderDetail.Total / tenderDetail.PageSize);
            ViewBag.SearchText = withinSearchText;

            tenderDetail.TendersBy = regionId;
            var paggingUrl = "";
            var replaceName = "";
            bool linkListPage = false;

            tenderDetail.FieldId = fieldId;
            tenderDetail.FieldName = fieldName.Replace(" ", "-");
            var displayFieldName = fieldName.Replace("-", " ");
            replaceName = TenderMetaReplaceName.GlobalCountryName;

            switch (regionId)
            {
                default:
                case TenderTypeList.SearchTender:
                    sessionKeyName = "SearhGlobalTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["GlobalTenderKeywordUrl"] + TenderWordNameList.KeywordWord + fieldName.Replace(" ", "-");
                    break;
                case TenderTypeList.MiddleEastCountryRegion:
                    sessionKeyName = "SearhMiddleEastCountryTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByMiddleEastCountryUrl"] + TenderWordNameList.GlobalCountryWord + fieldName.Replace(" ", "-");
                    break;
                case TenderTypeList.EuropeanCountryRegion:
                    sessionKeyName = "SearhEuropeanCountryTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByEuropeanCountryUrl"] + TenderWordNameList.GlobalCountryWord + fieldName.Replace(" ", "-");
                    break;
                case TenderTypeList.AfricanCountryRegion:
                    sessionKeyName = "SearhAfricanCountryTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByAfricanCountryUrl"] + TenderWordNameList.GlobalCountryWord + fieldName.Replace(" ", "-");
                    break;
                case TenderTypeList.AsianCountryRegion:
                    sessionKeyName = "SearhAsianCountryTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByAsianCountryUrl"] + TenderWordNameList.GlobalCountryWord + fieldName.Replace(" ", "-");
                    break;
                case TenderTypeList.SaarCountryRegion:
                    sessionKeyName = "SearhSAARCountryTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderBySAARCountryUrl"] + TenderWordNameList.GlobalCountryWord + fieldName.Replace(" ", "-");
                    break;
                case TenderTypeList.AustraliaOceaniaCountryRegion:
                    paggingUrl = ConfigurationManager.AppSettings["TenderByAustraliaOceaniaCountryUrl"] + TenderWordNameList.GlobalCountryWord + fieldName.Replace(" ", "-");
                    sessionKeyName = "SearhAustraliaOceaniaCountryTenderResult";
                    break;
                case TenderTypeList.SouthAmericaCountryRegion:
                    paggingUrl = ConfigurationManager.AppSettings["TenderBySouthAmericaCountryUrl"] + TenderWordNameList.GlobalCountryWord + fieldName.Replace(" ", "-");
                    sessionKeyName = "SearhSouthAmericaCountryTenderResult";
                    break;
                case TenderTypeList.NorthAmericaCountryRegion:
                    paggingUrl = ConfigurationManager.AppSettings["TenderByNorthAmericaCountryUrl"] + TenderWordNameList.GlobalCountryWord + fieldName.Replace(" ", "-");
                    sessionKeyName = "SearhNorthAmericaCountryTenderResult";
                    break;
            }
            var countryNm = "";
            if (regionId != 0)
            {
                var countrylist = _tenderInfo.ListCountrybyRegion(regionId).Select(x => x.CountryName).ToList();
                if (countrylist.Any())
                {
                    countryNm = String.Join(" Tenders ,", countrylist.ToArray());
                }
            }

            tenderDetail.PaggingUrl = paggingUrl;

            var tenderMetaData = _common.GetTenderMetaDataGlobal(regionId, linkListPage);
            ViewBag.Title = tenderMetaData.Title.Replace(replaceName, countryNm);
            ViewBag.Description = tenderMetaData.Description.Replace(replaceName, countryNm);
            ViewBag.Keywords = tenderMetaData.Keyword.Replace(replaceName, countryNm);
            //tenderDetail.DisplaySearchTextDetail = tenderMetaData.Content.Replace(replaceName, countryNm);

            OtherValues();

            tenderDetail.StateList = _getListItems.StateList();
            tenderDetail.CityList = _getListItems.CityList(0);
            tenderDetail.IdVal = 0;
            tenderDetail.Subscribetype = SubscribType.DownloadTender;
            tenderDetail.DownloadTenderRefNo = 0;
            tenderDetail.FormTitle = SubscribTypeDisplsyText.DownloadTender;
            tenderDetail.FormType = FormType.PopupForms;

            tenderDetail.IsGlobalTender = true;

            Session[sessionKeyName] = tenderDetail;
        }
        #endregion

        #region ALL SESSIONS METHODS
        public JsonResult ClearAllSession()
        {
            ClearSession_Global();
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult ClearTotalCountSession()
        {
            ResetTotalCountSession_Global();
            return Json(JsonRequestBehavior.AllowGet);
        }


        #endregion


        public ActionResult TenderNotice(string refno)
        {
            var globalTenderInformation = _tenderInfo.GetGlobalTenderInfoById(refno);

            ViewBag.Location = "";
            ViewBag.LocationForLink = "";

            var statename = "";
            var city = "";
            var country = "";
            var location = "";
            int tenderStatus = 0;// 1:ACTIVE, 2:NEW, 3:CLOSE                        
            ViewBag.DocType = "";

            const string searchdesiplaytext = "";
            if (globalTenderInformation != null)
            {
                country = _tenderInfo.GetCountry(globalTenderInformation.CountryId).CountryName;
                statename = _tenderInfo.GetState(globalTenderInformation.StateId).StateName;
                city = _tenderInfo.GetCity(globalTenderInformation.LocId).Location;
                ViewBag.Location = location = country;

                if (globalTenderInformation.dt != null)
                {
                    var dt = globalTenderInformation.dt.Value;
                    var submDate = globalTenderInformation.SubmDate;

                    if (dt.ToString("MM/dd/yyyy") == DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy") && submDate >= DateTime.Today)
                    { tenderStatus = 2; }
                    else if (submDate >= DateTime.Today)
                    { tenderStatus = 1; }
                    else if (submDate < DateTime.Today)
                    { tenderStatus = 3; }
                }
            }

            city = new CultureInfo("en-US", false).TextInfo.ToTitleCase(city.ToLower());
            statename = new CultureInfo("en-US", false).TextInfo.ToTitleCase(statename.ToLower());

            int refNo = refno == "" ? 0 : Convert.ToInt32(refno);

            var searchtype = new SearchType();
            if (globalTenderInformation != null)
            {
                var duedate = globalTenderInformation.SubmDate.ToString("dd MMM, yyyy");
                var detail = globalTenderInformation.WorkDesc;

                var tenderMetaData = _common.GetTenderMetaDataGlobal(TenderTypeList.GlobalDetailPage);
                var Title = tenderMetaData.Title;
                Title = Title.Replace(TenderMetaReplaceName.TenderDescription, detail.Replace("-", " "));
                Title = Title.Replace(TenderMetaReplaceName.CityName, location.Replace("-", " "));

                var Description = tenderMetaData.Description;
                Description = Description.Replace(TenderMetaReplaceName.TenderDescription, detail);
                Description = Description.Replace(TenderMetaReplaceName.TenderDueDate, duedate);
                Description = Description.Replace(TenderMetaReplaceName.TenderLocation, location.Replace("-", " "));
                Description = Description.Replace(TenderMetaReplaceName.TenderOurrefNo, globalTenderInformation.OurRefNo.ToString());

                string displayKeyword = "";
                string[] keywordList = null;
                if (!string.IsNullOrEmpty(detail))
                {
                    keywordList = Regex.Split(detail, " ");
                }

                if (keywordList != null)
                {
                    displayKeyword =
                        keywordList.Where(
                            item =>
                                item.Length > 2 && !item.ToLower().Trim().Contains("and") &&
                                !item.ToLower().Trim().Contains("but") && !item.ToLower().Trim().Contains("before") &&
                                !item.ToLower().Trim().Contains("after") && !item.ToLower().Trim().Contains("above") &&
                                !item.ToLower().Trim().Contains("via") && !item.ToLower().Trim().Contains("from") &&
                                !item.ToLower().Trim().Contains("length") && !item.ToLower().Trim().Contains("height") &&
                                !item.ToLower().Trim().Contains("weight"))
                            .Aggregate(displayKeyword,
                                (current, item) => current == "" ? item + " Tenders" : current + ", " + item + " Tenders");
                }

                ViewBag.Title = Title;
                ViewBag.description = Description;
                ViewBag.keywords = tenderMetaData.Keyword.Replace(TenderMetaReplaceName.KeywordName, displayKeyword);

            }
            var tenderDetail = new TenderDetail
            {
                TenderDetails = globalTenderInformation,
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),

                OurRefNo = globalTenderInformation.OurRefNo,
                TenderStatus = tenderStatus,
                DisplayText3 = searchdesiplaytext,
                IdVal = 0,
                Subscribetype = SubscribType.DownloadTender,
                DownloadTenderRefNo = Convert.ToInt32(refno),
                FormTitle = SubscribTypeDisplsyText.DownloadTender,
                FormType = FormType.PopupForms,
                IsGlobalTender = true
            };

            return View("TenderNotice", tenderDetail);
        }

        private void FillWithinSearchWords(string searchText)
        {
            if (Session["WithinSearchGlobalTextList"] != null)
            {
                WithinSearchWords = (List<string>)Session["WithinSearchGlobalTextList"];
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                if (!WithinSearchWords.Contains(searchText.Trim()) && searchText.Trim() != "*-*")
                {
                    WithinSearchWords.Add(searchText);
                }
                Session["WithinSearchGlobalTextList"] = WithinSearchWords;
            }
        }
        private AdvanceSearchParameter SetDefaultAdvSearchParam()
        {
            AdvanceSearchParameter advanceSearchPara = new AdvanceSearchParameter();
            if (Session["AdvanceSearchGlobalParams"] != null)
            {
                advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchGlobalParams"];
            }
            else
            {
                advanceSearchPara.TenderTypeId = null;
                advanceSearchPara.TenderStatusFlag = 0;
            }

            Session["AdvanceSearchGlobalParams"] = advanceSearchPara;
            return advanceSearchPara;

        }
        public JsonResult RemoveWithinSearchWords(string removeText)
        {
            string lastWithinSearch = "";
            if (Session["WithinSearchGlobalTextList"] != null)
            {
                WithinSearchWords = (List<string>)Session["WithinSearchGlobalTextList"];
            }

            if (!string.IsNullOrEmpty(removeText))
            {
                if (WithinSearchWords.Contains(removeText.Trim()))
                {
                    WithinSearchWords.Remove(removeText);
                }
                Session["WithinSearchGlobalTextList"] = WithinSearchWords;
                if (WithinSearchWords.Any())
                {
                    lastWithinSearch = WithinSearchWords[(WithinSearchWords.Count - 1)].ToString();
                }
            }

            return Json(lastWithinSearch, JsonRequestBehavior.AllowGet);
        }
    }
}