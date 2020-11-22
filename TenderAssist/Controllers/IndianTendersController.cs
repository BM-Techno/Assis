using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using TenderAssist.CommonHelper;
using TenderAssist.Models;
using TenderAssist.ViewModel;
using static TenderAssist.CommonHelper.Utility;
using static TenderAssist.Models.SearchModel;

namespace TenderAssist.Controllers
{
    public class IndianTendersController : BaseController
    {
        public IndianTendersController()
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

        #region CATEGORY TENDERS

        #region ----------STATE AND CITY----------

        public ActionResult StateTenders()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            var tenaderStateCitylist = _common.GetStateCityForIndianTenders("");

            var tenderMetaData = _common.GetTenderMetaData(TenderTypeList.State, true);
            ViewBag.Title = tenderMetaData.Title;
            ViewBag.Description = tenderMetaData.Description;
            ViewBag.Keywords = tenderMetaData.Keyword;

            var tenderDetail = new TenderDetail
            {
                AllStateCityForIndianTenders = tenaderStateCitylist,
                TendersBy = TenderTypeList.State,
                FormTitle = TenderTypeDisplayText.State
            };

            return View("IndianCategory", tenderDetail);
        }
        public ActionResult CityTenders()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            var tenaderStateCitylist = _common.GetStateCityForIndianTenders("");

            var tenderMetaData = _common.GetTenderMetaData(TenderTypeList.City, true);
            ViewBag.Title = tenderMetaData.Title;
            ViewBag.Description = tenderMetaData.Description;
            ViewBag.Keywords = tenderMetaData.Keyword;

            var tenderDetail = new TenderDetail
            {
                AllStateCityForIndianTenders = tenaderStateCitylist,
                TendersBy = TenderTypeList.City,
                FormTitle = TenderTypeDisplayText.City
            };

            return View("IndianCategory", tenderDetail);
        }
        public ActionResult SearchLocations(string searchText)
        {
            var tenaderStateCitylist = _common.GetStateCityForIndianTenders(searchText);
            var tenderDetail = new TenderDetail
            {
                AllStateCityForIndianTenders = tenaderStateCitylist,
                SearchText = searchText

            };
            return PartialView(Url.Content("~/Views/IndianTenders/Partial/IndianCategory/_partialLocation.cshtml"), tenderDetail);
        }


        public ActionResult StateTenderList(string stateName = "", string withinSearchText = "")
        {
            ResetTotalCountSession();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            const int tenderBy = TenderTypeList.State;
            int CountryId = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);
            var locations = "";

            stateName = stateName.Replace("-", " ").ToLower().Trim().ToString();
            var stateList = _tenderInfo.ListState().Where(x => x.StateName.ToLower().Trim().ToString().Equals(stateName));
            if (stateList.Any()) { id = stateList.FirstOrDefault().StateId; }

            CheckWithinSearch(withinSearchText);

            var fieldId = id;
            var fieldName = stateName;

            SearchedWordsClear();
            locations = ("0#" + id + "#" + CountryId);

            //if (Session["SearhStateTenderResult"] != null)
            //{
            //    tenderDetail = (TenderDetail)Session["SearhStateTenderResult"];
            //    locations = tenderDetail.SelectedLocationIds;
            //    var oldFieldId = tenderDetail.FieldId;
            //    if (oldFieldId != fieldId)
            //    {
            //        //isSearchTextChanged = true;
            //        SearchedWordsClear();
            //        locations = ("0#" + id + "#" + CountryId);
            //    }
            //}
            //else
            //{
            //    SearchedWordsClear();
            //    locations = ("0#" + id + "#" + CountryId);
            //}
            var advanceSearchPara = SetDefaultAdvSearchParam();
            advanceSearchPara.StateId = id;
            advanceSearchPara.SelectedLocations = locations;

            tenderDetail = GetTenderResult(page, "SearhStateTenderResult", advanceSearchPara, id, tenderBy, true, true);

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            return View("Index", tenderDetail);
        }
        public ActionResult CityTenderList(string cityName = "", string withinSearchText = "")
        {
            ResetTotalCountSession();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            const int tenderBy = TenderTypeList.City;
            int CountryId = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);
            var locations = "";
            var stateId = 0;

            cityName = cityName.Replace("-", " ").ToLower().Trim().ToString();
            var cityList = _tenderInfo.ListAllCity().Where(x => x.Location.ToLower().Trim().ToString().Equals(cityName));
            if (cityList.Any())
            {
                id = cityList.FirstOrDefault().LocId;
                stateId = cityList.FirstOrDefault().StateId;
            }

            CheckWithinSearch(withinSearchText);
            var fieldId = id;
            var fieldName = cityName;

            SearchedWordsClear();
            locations = (id + "#" + stateId + "#" + CountryId);

            //if (Session["SearhCityTenderResult"] != null)
            //{
            //    tenderDetail = (TenderDetail)Session["SearhCityTenderResult"];
            //    locations = tenderDetail.SelectedLocationIds;
            //    var oldFieldId = tenderDetail.FieldId;
            //    if (oldFieldId != fieldId)
            //    {
            //        //isSearchTextChanged = true;
            //        SearchedWordsClear();
            //        locations = (id + "#" + stateId + "#" + CountryId);
            //    }
            //}
            //else
            //{
            //    SearchedWordsClear();
            //    locations = (id + "#" + stateId + "#" + CountryId);
            //}
            var advanceSearchPara = SetDefaultAdvSearchParam();
            advanceSearchPara.CityId = id;
            advanceSearchPara.SelectedLocations = locations;

            tenderDetail = GetTenderResult(page, "SearhCityTenderResult", advanceSearchPara, id, tenderBy, true, true);

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            return View("Index", tenderDetail);
        }

        #endregion

        #region ----------INDUSTRY AND SUBINDUSTRY----------

        public ActionResult IndustryTenders()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            var tenaderCatlist = _common.GetIndustrySubIndustryList("");

            var tenderMetaData = _common.GetTenderMetaData(TenderTypeList.Industry, true);
            ViewBag.Title = tenderMetaData.Title;
            ViewBag.Description = tenderMetaData.Description;
            ViewBag.Keywords = tenderMetaData.Keyword;

            var tenderDetail = new TenderDetail
            {
                AllIndustrySubIndustryDetail = tenaderCatlist,
                TendersBy = TenderTypeList.Industry,
                FormTitle = TenderTypeDisplayText.Industry
            };

            return View("IndianCategory", tenderDetail);
        }
        public ActionResult SubIndustryTenders()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            var tenaderCatlist = _common.GetIndustrySubIndustryList("");

            var tenderMetaData = _common.GetTenderMetaData(TenderTypeList.SubIndustry, true);
            ViewBag.Title = tenderMetaData.Title;
            ViewBag.Description = tenderMetaData.Description;
            ViewBag.Keywords = tenderMetaData.Keyword;

            var tenderDetail = new TenderDetail
            {
                AllIndustrySubIndustryDetail = tenaderCatlist,
                TendersBy = TenderTypeList.SubIndustry,
                FormTitle = TenderTypeDisplayText.SubIndustry
            };

            return View("IndianCategory", tenderDetail);
        }
        public ActionResult SearchIndustrySubIndustry(string searchText)
        {
            var tenaderCatlist = _common.GetIndustrySubIndustryList(searchText);
            var tenderDetail = new TenderDetail
            {
                AllIndustrySubIndustryDetail = tenaderCatlist,
                SearchText = searchText

            };
            return PartialView(Url.Content("~/Views/IndianTenders/Partial/IndianCategory/_partialIndSubInd.cshtml"), tenderDetail);
        }

        public ActionResult IndustryTenderList(string industryName = "", string withinSearchText = "")
        {
            ResetTotalCountSession();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            var indsubind = "";

            const int tenderBy = TenderTypeList.Industry;

            industryName = industryName.Replace("-", " ").ToLower().Trim().ToString();
            var industryList = _tenderInfo.ListIndustry().Where(x => x.IndustryName.ToLower().Trim().ToString().Equals(industryName));
            if (industryList.Any()) { id = industryList.FirstOrDefault().IndustryId; }

            CheckWithinSearch(withinSearchText);
            var fieldId = id;
            var fieldName = industryName;

            SearchedWordsClear();
            indsubind = ("0#" + id);

            //if (Session["SearhIndustryTenderResult"] != null)
            //{
            //    tenderDetail = (TenderDetail)Session["SearhIndustryTenderResult"];
            //    indsubind = tenderDetail.SelectedIndustrySubIndustryIds;
            //    var oldFieldId = tenderDetail.FieldId;
            //    if (oldFieldId != fieldId)
            //    {
            //        //isSearchTextChanged = true;
            //        SearchedWordsClear();
            //        indsubind = ("0#" + id);
            //    }
            //}
            //else
            //{
            //    SearchedWordsClear();
            //    indsubind = ("0#" + id);
            //}
            var advanceSearchPara = SetDefaultAdvSearchParam();
            advanceSearchPara.IndustryId = id;
            advanceSearchPara.SelectedIndsubIndustries = indsubind;

            tenderDetail = GetTenderResult(page, "SearhIndustryTenderResult", advanceSearchPara, id, tenderBy, true, true);

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            return View("Index", tenderDetail);
        }
        public ActionResult SubIndustryTenderList(string subindustryName = "", string withinSearchText = "")
        {
            ResetTotalCountSession();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            var indsubind = "";
            var industryId = 0;

            const int tenderBy = TenderTypeList.SubIndustry;

            subindustryName = subindustryName.Replace("-", " ").ToLower().Trim().ToString();
            var subindustryList = _tenderInfo.ListAllSubIndustry().Where(x => x.SubIndustryName.ToLower().Trim().ToString().Equals(subindustryName));
            if (subindustryList.Any())
            {
                id = subindustryList.FirstOrDefault().SubIndustryId;
                industryId = subindustryList.FirstOrDefault().IndustryId;
            }

            CheckWithinSearch(withinSearchText);
            var fieldId = id;
            var fieldName = subindustryName;

            SearchedWordsClear();
            indsubind = (id + "#" + industryId);

            //if (Session["SearhSubIndustryTenderResult"] != null)
            //{
            //    tenderDetail = (TenderDetail)Session["SearhSubIndustryTenderResult"];
            //    indsubind = tenderDetail.SelectedIndustrySubIndustryIds;
            //    var oldFieldId = tenderDetail.FieldId;
            //    if (oldFieldId != fieldId)
            //    {
            //        //isSearchTextChanged = true;
            //        SearchedWordsClear();
            //        indsubind = (id + "#" + industryId);
            //    }
            //}
            //else
            //{
            //    SearchedWordsClear();
            //    indsubind = (id + "#" + industryId);
            //}
            var advanceSearchPara = SetDefaultAdvSearchParam();
            advanceSearchPara.SubIndustryId = id;
            advanceSearchPara.SelectedIndsubIndustries = indsubind;

            tenderDetail = GetTenderResult(page, "SearhSubIndustryTenderResult", advanceSearchPara, id, tenderBy, true, true);

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            return View("Index", tenderDetail);
        }
        #endregion

        #region ----------AGENCY AND SECTOR AND OWNERSHIP----------

        public ActionResult AgencyTenders()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            List<AgencySectorOwhershipDetail> tenaderAgencySectorOwnershiplist = new List<AgencySectorOwhershipDetail>();
            if (Session["AgencySectorOwnershipList"] == null)
            {
                tenaderAgencySectorOwnershiplist = _common.GetAgencySectorOwnershipForIndianTenders("");
                Session["AgencySectorOwnershipList"] = tenaderAgencySectorOwnershiplist;
            }
            else
            { tenaderAgencySectorOwnershiplist = (List<AgencySectorOwhershipDetail>)Session["AgencySectorOwnershipList"]; }


            var tenderMetaData = _common.GetTenderMetaData(TenderTypeList.Agency, true);
            ViewBag.Title = tenderMetaData.Title;
            ViewBag.Description = tenderMetaData.Description;
            ViewBag.Keywords = tenderMetaData.Keyword;

            var tenderDetail = new TenderDetail
            {
                AllAgencySectorOwnershipDetail = tenaderAgencySectorOwnershiplist,
                TendersBy = TenderTypeList.Agency,
                FormTitle = TenderTypeDisplayText.Agency
            };

            return View("IndianCategory", tenderDetail);
        }
        public ActionResult SectorTenders()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            List<AgencySectorOwhershipDetail> tenaderAgencySectorOwnershiplist = new List<AgencySectorOwhershipDetail>();
            if (Session["AgencySectorOwnershipList"] == null)
            {
                tenaderAgencySectorOwnershiplist = _common.GetAgencySectorOwnershipForIndianTenders("");
                Session["AgencySectorOwnershipList"] = tenaderAgencySectorOwnershiplist;
            }
            else
            { tenaderAgencySectorOwnershiplist = (List<AgencySectorOwhershipDetail>)Session["AgencySectorOwnershipList"]; }

            var tenderMetaData = _common.GetTenderMetaData(TenderTypeList.Sector, true);
            ViewBag.Title = tenderMetaData.Title;
            ViewBag.Description = tenderMetaData.Description;
            ViewBag.Keywords = tenderMetaData.Keyword;

            var tenderDetail = new TenderDetail
            {
                AllAgencySectorOwnershipDetail = tenaderAgencySectorOwnershiplist,
                TendersBy = TenderTypeList.Sector,
                FormTitle = TenderTypeDisplayText.Sector
            };

            return View("IndianCategory", tenderDetail);
        }
        public ActionResult OwnershipTenders()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            List<AgencySectorOwhershipDetail> tenaderAgencySectorOwnershiplist = new List<AgencySectorOwhershipDetail>();
            if (Session["AgencySectorOwnershipList"] == null)
            {
                tenaderAgencySectorOwnershiplist = _common.GetAgencySectorOwnershipForIndianTenders("");
                Session["AgencySectorOwnershipList"] = tenaderAgencySectorOwnershiplist;
            }
            else
            { tenaderAgencySectorOwnershiplist = (List<AgencySectorOwhershipDetail>)Session["AgencySectorOwnershipList"]; }

            var tenderMetaData = _common.GetTenderMetaData(TenderTypeList.Ownership, true);
            ViewBag.Title = tenderMetaData.Title;
            ViewBag.Description = tenderMetaData.Description;
            ViewBag.Keywords = tenderMetaData.Keyword;

            var tenderDetail = new TenderDetail
            {
                AllAgencySectorOwnershipDetail = tenaderAgencySectorOwnershiplist,
                TendersBy = TenderTypeList.Ownership,
                FormTitle = TenderTypeDisplayText.Ownership
            };

            return View("IndianCategory", tenderDetail);
        }
        public ActionResult SearchAgencySectorOwnership(string searchText)
        {
            List<AgencySectorOwhershipDetail> tenaderAgencySectorOwnershiplist = new List<AgencySectorOwhershipDetail>();
            if (searchText == "")
            {
                if (Session["AgencySectorOwnershipList"] == null)
                {
                    tenaderAgencySectorOwnershiplist = _common.GetAgencySectorOwnershipForIndianTenders("");
                    Session["AgencySectorOwnershipList"] = tenaderAgencySectorOwnershiplist;
                }
                else
                { tenaderAgencySectorOwnershiplist = (List<AgencySectorOwhershipDetail>)Session["AgencySectorOwnershipList"]; }
            }
            else
            {
                tenaderAgencySectorOwnershiplist = _common.GetAgencySectorOwnershipForIndianTenders(searchText);
            }

            var tenderDetail = new TenderDetail
            {
                AllAgencySectorOwnershipDetail = tenaderAgencySectorOwnershiplist,
                SearchText = searchText

            };
            return PartialView(Url.Content("~/Views/IndianTenders/Partial/IndianCategory/_partialAgencySectorOwnership.cshtml"), tenderDetail);
        }

        public ActionResult AgencyTenderList(string agencyName = "", string withinSearchText = "")
        {
            ResetTotalCountSession();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            var agencies = "";

            const int tenderBy = TenderTypeList.Agency;

            agencyName = agencyName.Replace("-", " ").ToLower().Trim().ToString();
            var agencyList = _tenderInfo.ListAgencyMaster().Where(x => x.AgencyName.ToLower().Trim().ToString().Equals(agencyName));
            if (agencyList.Any())
            {
                id = agencyList.FirstOrDefault().AgencyId;
            }

            CheckWithinSearch(withinSearchText);
            var fieldId = id;
            var fieldName = agencyName;

            SearchedWordsClear();
            agencies = id.ToString();

            //if (Session["SearhAgencyTenderResult"] != null)
            //{
            //    tenderDetail = (TenderDetail)Session["SearhAgencyTenderResult"];
            //    var oldFieldId = tenderDetail.FieldId;
            //    agencies = tenderDetail.SelectedAgency;
            //    if (oldFieldId != fieldId)
            //    {
            //        // isSearchTextChanged = true;
            //        SearchedWordsClear();
            //        agencies = id.ToString(); //tenderDetail.SelectedAgency;
            //    }
            //}
            //else
            //{
            //    SearchedWordsClear();
            //    agencies = id.ToString();
            //}

            var advanceSearchPara = SetDefaultAdvSearchParam();
            advanceSearchPara.AgencyId = id;
            advanceSearchPara.SelectedAgencies = agencies;

            tenderDetail = GetTenderResult(page, "SearhAgencyTenderResult", advanceSearchPara, id, tenderBy, true, true);

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            return View("Index", tenderDetail);
        }
        public ActionResult SectorTenderList(string sectorName = "", string withinSearchText = "")
        {
            ResetTotalCountSession();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            var sectors = "";

            const int tenderBy = TenderTypeList.Sector;

            sectorName = sectorName.Replace("-", " ").ToLower().Trim().ToString();
            var sectroList = _tenderInfo.ListSector().Where(x => x.SectorName.ToLower().Trim().ToString().Equals(sectorName));
            if (sectroList.Any())
            {
                id = sectroList.FirstOrDefault().SectorId;
            }

            CheckWithinSearch(withinSearchText);
            var fieldId = id;
            var fieldName = sectorName;

            SearchedWordsClear();
            sectors = id.ToString();

            //if (Session["SearhSectorTenderResult"] != null)
            //{
            //    tenderDetail = (TenderDetail)Session["SearhSectorTenderResult"];
            //    var oldFieldId = tenderDetail.FieldId;
            //    sectors = tenderDetail.SelectedSector;
            //    if (oldFieldId != fieldId)
            //    {
            //        //isSearchTextChanged = true;
            //        SearchedWordsClear();
            //        sectors = id.ToString();// tenderDetail.SelectedSector;
            //    }
            //}
            //else
            //{
            //    SearchedWordsClear();
            //    sectors = id.ToString();
            //}
            var advanceSearchPara = SetDefaultAdvSearchParam();
            advanceSearchPara.SectorId = id;
            advanceSearchPara.SelectedSectors = sectors;

            tenderDetail = GetTenderResult(page, "SearhSectorTenderResult", advanceSearchPara, id, tenderBy, true, true);

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            return View("Index", tenderDetail);
        }
        public ActionResult OwnershipTenderList(string ownershipName = "", string withinSearchText = "")
        {
            ResetTotalCountSession();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            var ownerships = "";

            const int tenderBy = TenderTypeList.Ownership;

            ownershipName = ownershipName.Replace("-", " ").ToLower().Trim().ToString();
            var ownershipList = _tenderInfo.ListOwnership().Where(x => x.OwnershipName.ToLower().Trim().ToString().Equals(ownershipName));
            if (ownershipList.Any())
            {
                id = ownershipList.FirstOrDefault().OwnershipId;
            }

            CheckWithinSearch(withinSearchText);
            var fieldId = id;
            var fieldName = ownershipName;

            SearchedWordsClear();
            ownerships = id.ToString();

            //if (Session["SearhOwnershipTenderResult"] != null)
            //{
            //    tenderDetail = (TenderDetail)Session["SearhOwnershipTenderResult"];
            //    var oldFieldId = tenderDetail.FieldId;
            //    ownerships = tenderDetail.SelectedOwnership;
            //    if (oldFieldId != fieldId)
            //    {
            //        //isSearchTextChanged = true;
            //        SearchedWordsClear();
            //        ownerships = id.ToString();// tenderDetail.SelectedOwnership;
            //    }
            //}
            //else
            //{
            //    SearchedWordsClear();
            //    ownerships = id.ToString();
            //}
            var advanceSearchPara = SetDefaultAdvSearchParam();
            advanceSearchPara.OwnershipId = id;
            advanceSearchPara.SelectedOwnerships = ownerships;

            tenderDetail = GetTenderResult(page, "SearhOwnershipTenderResult", advanceSearchPara, id, tenderBy, true, true);

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            return View("Index", tenderDetail);
        }
        #endregion

        #region ----------KEYWORDS----------

        public ActionResult KeywordTenders()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            var tenaderProductlist = _common.GetProductsForIndianTenders("", "A");

            var tenderMetaData = _common.GetTenderMetaData(TenderTypeList.Keyword, true);
            ViewBag.Title = tenderMetaData.Title;
            ViewBag.Description = tenderMetaData.Description;
            ViewBag.Keywords = tenderMetaData.Keyword;



            var tenderDetail = new TenderDetail
            {
                AllProducts = tenaderProductlist,
                TendersBy = TenderTypeList.Keyword,
                FormTitle = TenderTypeDisplayText.Keyword,
                SelectedWord = "A",
                AllAlphabaticWord = Utility.GetAllAlphabaticWord(),
            };

            return View("IndianCategory", tenderDetail);
        }
        public ActionResult SearchKeywordList(string searchText, string wordSearch)
        {
            var tenaderProductlist = _common.GetProductsForIndianTenders(searchText, wordSearch);


            var tenderDetail = new TenderDetail
            {
                AllProducts = tenaderProductlist,
                TendersBy = TenderTypeList.Keyword,
                SearchText = searchText,
                SelectedWord = wordSearch,
                AllAlphabaticWord = Utility.GetAllAlphabaticWord(),

            };
            return PartialView(Url.Content("~/Views/IndianTenders/Partial/IndianCategory/_partialKeywords.cshtml"), tenderDetail);
        }
        public ActionResult KeywordTenderList(string keyword = "", string withinSearchText = "")
        {
            ResetTotalCountSession();
            TenderDetail tenderDetail;

            int id = 0; int page = 0;
            const int tenderBy = TenderTypeList.Keyword;

            CheckWithinSearch(withinSearchText);

            var originalkeyword = keyword.Trim().Replace("-", " ");

            var productlist = _tenderInfo.GetExactProduct(originalkeyword.Trim());
            if (productlist.Any()) { id = productlist.FirstOrDefault().ProductsId; }


            var fieldId = id;
            var fieldName = keyword;
            var products = "";
            var rnd = new Random();

            var advanceSearchPara = SetDefaultAdvSearchParam();


            products = id.ToString();
            //advanceSearchPara.SelectedProducts = products;
            advanceSearchPara.SearchProductName = originalkeyword;
            SearchedWordsClear();

            //if (Session["SearhKeywordTenderResult"] != null)
            //{
            //    tenderDetail = (TenderDetail)Session["SearhKeywordTenderResult"];
            //    var oldFieldId = tenderDetail.FieldId;
            //    tenderDetail.DisplayText = originalkeyword;
            //    advanceSearchPara.SearchProductName = originalkeyword;
            //    if (oldFieldId != fieldId)
            //    {
            //        //isSearchTextChanged = true;
            //        SearchedWordsClear();
            //        //products = fieldId.ToString();
            //    }
            //    else if (id != 0)
            //    {
            //        //products = id.ToString();
            //    }
            //    advanceSearchPara.SelectedProducts = "";
            //}
            //else
            //{
            //    products = id.ToString();
            //    advanceSearchPara.SelectedProducts = products;
            //    advanceSearchPara.SearchProductName = originalkeyword;
            //    SearchedWordsClear();
            //}

            //advanceSearchPara.SelectedProducts = products;

            tenderDetail = GetTenderResult(page, "SearhKeywordTenderResult", advanceSearchPara, id, tenderBy, true, true);

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            if (tenderDetail.DisplayText == "")
            {
                tenderDetail.DisplayText = keyword.Replace("-", " ");
            }

            ///*Load random 10 keywords list*/
            //var prodId = id;
            //var searchFilterText = "";
            //if (prodId == 0)
            //{ searchFilterText = "Other Keywords"; }
            //else
            //{ searchFilterText = "Related Keywords"; }


            return View("Index", tenderDetail);

        }

        #endregion

        #region TENDE LIST PAGE

        public ActionResult Index()
        {
            return View(new TenderDetail()
            {
                IsAdvanceSearch = false,
                TendersBy = TenderTypeList.Keyword,
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                IdVal = 0,
                Subscribetype = SubscribType.DownloadTender,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.DownloadTender,
                FormType = FormType.PopupForms
            });

        }

        #endregion

        #endregion

        #region ADVANCE SEARCH

        public ActionResult AdvanceSearch()
        {
            ClearSession();
            SearchedWordsClear();


            ResetTotalCountSession();

            OtherValues();

            ViewBag.Title = @"Tender, Tenders, e tenders , Tender daily Service , India Tender, Tender advisor India, Tender Information service , Government Tenders, Global Tenders,  International Tenders, Indian Tenders , eprocure tenders";
            ViewBag.Description = @"The most accurate and reliable tender services  in india .  Register free for list of tenders, government tenders& international tenders  and get unique facility for search by product, tenders by location, online tenders, public tenders, international tenders";
            ViewBag.Keywords = @"Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice, Indian tender portal,  tenders notification service,  tender notice, government tenders, govt tenders, tenders info, free tenders, India Tender, Tender India, Global Tenders, Public Tenders, Free Government Tenders, Indian Government Tenders, eprocure tenders, tenderassist247.com";

            return View(new TenderDetail() { IsAdvanceSearch = true, TendersBy = TenderTypeList.SearchTender });
        }
        public ActionResult AdvanceSearchTenders(string withinSearchText = "")
        {
            int id = 0; int page = 0;
            var tenderBy = TenderTypeList.SearchTender;
            TenderDetail tenderDetail;

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

            var advanceSearchPara = Session["AdvanceSearchparams"]==null?  SetDefaultAdvSearchParam() : (AdvanceSearchParameter)Session["AdvanceSearchparams"];
            id = advanceSearchPara.SearchProductId;
            withinSearchText = withinSearchText.Replace("-", " ");

            tenderDetail = GetTenderResult(page, "SearhIndianTenderResult", advanceSearchPara, id, tenderBy, true, isSearchTextChanged);

            if (id != 0)
            {
                tenderDetail.DisplaySearchTextDetail = "Get complete Tenders information related to latest <span style=\"color:red;\"><b>" + withinSearchText + "</b></span> from all over India at <a href=\"www.tenderassist247.com\">www.tenderassist247.com</a>. Search the best Live tenders from Indian government tenders,  Indian Public Sector Tenders, Indian Private Sector tenders, Indian online tenders, tender invitation notice, business tender notices, E tenders and bidding and Procurement Tenders.";
            }
            var fieldId = id;
            var fieldName = string.IsNullOrEmpty(withinSearchText) ? "" : withinSearchText;

            SetTenderDetails(tenderBy, withinSearchText, fieldId, fieldName, ref tenderDetail);

            tenderDetail.IsAdvanceSearch = true;

            return View("Index", tenderDetail);
        }
        public JsonResult LoadDataList(string searchName = "", bool isReset = false, int type = 0)
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            var indiaId = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);

            var sessionKeyName = "";
            switch (type)
            {
                case TenderTypeList.State:
                    sessionKeyName = "LoadStateList";
                    break;
                case TenderTypeList.City:
                    sessionKeyName = "LoadCityList";
                    break;
                case TenderTypeList.Industry:
                    sessionKeyName = "LoadIndustryList";
                    break;
                case TenderTypeList.SubIndustry:
                    sessionKeyName = "LoadSubIndustryList";
                    break;
                case TenderTypeList.Agency:
                    sessionKeyName = "LoadAgencyList";
                    break;
                case TenderTypeList.Sector:
                    sessionKeyName = "LoadSectorList";
                    break;
                case TenderTypeList.Ownership:
                    sessionKeyName = "LoadOwnershipList";
                    break;
                case TenderTypeList.Keyword:
                    sessionKeyName = "LoadKeywordList";
                    break;
                case TenderTypeList.GlobalCountry:
                    sessionKeyName = "LoadGlobalCountryList";
                    break;
            }

            if (string.IsNullOrEmpty(searchName))
            {
                if (isReset)
                {
                    dataList = LoadFreshDataList(type, searchName);
                    Session[sessionKeyName] = dataList;
                }
                else
                {
                    if (Session[sessionKeyName] != null)
                    { dataList = (List<SelectListItem>)Session[sessionKeyName]; }
                    else
                    {
                        dataList = LoadFreshDataList(type, searchName);
                        Session[sessionKeyName] = dataList;
                    }
                }
            }
            else
            {
                dataList = LoadFreshDataList(type, searchName);
                Session[sessionKeyName] = dataList;
            }

            return Json(new { DataList = dataList }, JsonRequestBehavior.AllowGet);
        }
        private List<SelectListItem> LoadFreshDataList(int type, string searchName = "")
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            var indiaId = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);
            switch (type)
            {
                case TenderTypeList.State:
                    dataList = _getListItems.GetStateList(indiaId, searchName);
                    break;
                case TenderTypeList.City:
                    dataList = _getListItems.GetCityList(indiaId, searchName);
                    break;
                case TenderTypeList.Industry:
                    dataList = _getListItems.GetIndustryList(searchName);
                    break;
                case TenderTypeList.SubIndustry:
                    dataList = _getListItems.GetSubIndustryList(searchName);
                    break;
                case TenderTypeList.Agency:
                    dataList = _getListItems.GetAgencyList(searchName);
                    break;
                case TenderTypeList.Sector:
                    dataList = _getListItems.GetSectorList(searchName);
                    break;
                case TenderTypeList.Ownership:
                    dataList = _getListItems.GetOwnershipList();
                    break;
                case TenderTypeList.Keyword:
                    dataList = _getListItems.GetKeywordList(searchName);
                    break;
                case TenderTypeList.GlobalCountry:
                    dataList = _getListItems.GetGlobalCountryList(searchName);
                    break;
            }
            return dataList;
        }

        #endregion

        #region INDIAN TENDER LIST PAGE

        public ActionResult Tenders()
        {
            ViewBag.Title = @"Indian Tenders, Online Tender Information, E Tenders, Government Tenders, International Tenders, Tender Assistance - TenderAssist247.com";
            ViewBag.description = @"Tender Alerts, Online Tender Information, Global Tenders, Indian Tenders, Online Tender Information service Provider, Tender Notifications, TenderAssist247.com";
            ViewBag.keywords = @"Tender, Tenders ,Tender Bidding, online tender information, latest tenders information, digital signature, equipments tenders, tender assist, tender alerts, indian tenders, industry tenders, state tenders, agency tenders, city tenders, ownership tenders, corporations, Tender Info, Online Tenders India, Government Tenders, E Tenders, Get Tenders Online, Online Tender Portal, Tender Assistance, tender online, online tender website, tender site, online tender asssistant, Private Tenders, Tenders 24*7, Tenders 24/7, Tender 24*7, Tender 24/7, Tender 24x7, TenderAssist247.com";
            return View(new TenderDetail()
            {
                IsHomePage = true,
                IsAdvanceSearch = false,
                TendersBy = TenderTypeList.Keyword,
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                IdVal = 0,
                Subscribetype = SubscribType.DownloadTender,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.DownloadTender,
                FormType = FormType.PopupForms
            });
        }

        #endregion

        #region  TENDER DETAIL PAGES AND DOWNLOAD TENDER

        public ActionResult TenderNotice(string tYear = "", string refno = "")
        {
            tYear = string.IsNullOrEmpty(tYear) ? "0" : tYear;

            var tenderInformation = _common.GetTenderInfoById(refno, tYear);

            ViewBag.Ownership = "";
            ViewBag.AgencyName = "";
            ViewBag.Location = "";
            ViewBag.DocType = "";

            var statename = "";
            var city = "";
            var tenderStatus = 0;
            var searchdesiplaytext = "";
            var relatedProducts = new List<string>();

            if (tenderInformation != null)
            {
                var tendertitle = _tenderInfo.GetOwnershipById(tenderInformation.OwnershipId);
                if (tendertitle != null)
                {
                    ViewBag.Ownership = tendertitle.OwnershipName;
                }
                var tenderAgencyName = _tenderInfo.GetAgencyDetailById(tenderInformation.AgencyId);
                if (tenderAgencyName != null)
                {
                    ViewBag.AgencyName = tenderAgencyName.AgencyName;
                }
                statename = _tenderInfo.GetState(tenderInformation.StateId).StateName;
                city = _tenderInfo.GetCity(tenderInformation.LocId).Location;
                ViewBag.Location = city + " - " + statename;

                if (tenderInformation.dt != null)
                {
                    var dt = tenderInformation.dt.Value;
                    var submDate = tenderInformation.SubmDate;

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

            var refNo = refno == "" ? 0 : Convert.ToInt32(refno);
            var ii = 1;

            if (tenderInformation == null) return null;


            string workDesc = tenderInformation.WorkDesc.Length > 60 ? tenderInformation.WorkDesc.Substring(0, 60).Trim() : tenderInformation.WorkDesc.Trim();
            string tenderid = "";
            if (tenderInformation.TenderNo != "" && tenderInformation.TenderNo != null)
                tenderid = @"Tender Id " + tenderInformation.TenderNo;

            ViewBag.Title = "Online Tender for " + workDesc + ".";
            ViewBag.description = "Get Tender bidding support for " + workDesc + "." + tenderid;
            ViewBag.keywords = "Online Tender for " + workDesc + "." + tenderid;

            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                TenderDetails = tenderInformation,
                TenderStatus = tenderStatus,
                DisplayText3 = searchdesiplaytext,
                IdVal = 0,
                TenderYear = tYear,

                Subscribetype = SubscribType.DownloadTender,
                DownloadTenderRefNo = Convert.ToInt32(refno),
                FormTitle = SubscribTypeDisplsyText.DownloadTender,
                FormType = FormType.PopupForms
            };

            return View(tenderDetail);
        }
        public ActionResult BidWithTenderAssist(string tYear = "", string refno = "")
        {
            ViewBag.TenderWorkDesc = "";
            tYear = string.IsNullOrEmpty(tYear) ? "0" : tYear;
            var tenderInformation = _common.GetTenderInfoById(refno, tYear);
            if (tenderInformation != null)
            {
                string workDesc = tenderInformation.WorkDesc;
                ViewBag.TenderWorkDesc = workDesc;
            }
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.BidWithTenderAssist,
                DownloadTenderRefNo = Convert.ToInt32(refno),
                FormTitle = SubscribTypeDisplsyText.BidWithTenderAssist,
                FormType = FormType.OtherForms
            };

            return View(tenderDetail);
        }
        public ActionResult DownloadTender(int refno)//, string description = "", int newId = 0
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                IdVal = 0,
                Subscribetype = SubscribType.DownloadTender,
                DownloadTenderRefNo = refno,
                FormTitle = SubscribTypeDisplsyText.DownloadTender,
                FormType = FormType.PopupForms
            };
            return PartialView(Url.Content("~/Views/Shared/_partialInquiryForm.cshtml"), tenderDetail);
        }

        #endregion

        #region SEARCHING LOGIC              
        private void CheckWithinSearch(string withinSearchText)
        {
            Session["WithinSearchText"] = null;
            if (withinSearchText != "")
            {
                string[] words = Regex.Split(withinSearchText, ",");
                foreach (var item in words)
                {
                    if (!WithinSearchWords.Contains(item.ToLower().Trim()))
                    {
                        WithinSearchWords.Add(item.ToLower().Trim());
                    }
                }
                Session["WithinSearchText"] = string.Join(",", WithinSearchWords);
            }

            //var isSearchTextChanged = false;
            //if (!string.IsNullOrEmpty(withinSearchText))
            //{
            //    withinSearchText = withinSearchText.Replace("-", " ");
            //    if (Session["WithinSearchText"] == null)
            //    { Session["WithinSearchText"] = withinSearchText; isSearchTextChanged = true; }
            //    else
            //    {
            //        if (Session["WithinSearchText"].ToString().ToLower().Trim() != withinSearchText.ToLower().Trim())
            //        { Session["WithinSearchText"] = withinSearchText; isSearchTextChanged = true; }
            //    }
            //}
            //FillWithinSearchWords(withinSearchText);
            //return isSearchTextChanged;
        }
        public ActionResult SearchTender(SearchIndianTenderModel searchResult)
        {
            var tenderBy = searchResult.TenderBy;
            var page = searchResult.Page;
            var searchText = string.IsNullOrEmpty(searchResult.Search) ? "" : searchResult.Search;
            var filterMainText = string.IsNullOrEmpty(searchResult.Search) ? "" : searchResult.Search;
            var searchType = searchResult.SType;
            var advanceSearch = searchResult.AdvanceSearch;
            var orderBy = searchResult.OrderBy;

            Session["AdvanceSearchparams"] = advanceSearch;

            var isFirst = searchResult.IsFirst;
            //var searchProductId = searchResult.SearchProductId;
            var searchProductId = advanceSearch.SearchProductId;

            var tenderYear = searchResult.TenderYear;
            var fieldId = searchResult.FId;
            var fieldName = searchResult.FName;

            var productIds = advanceSearch.SelectedProducts ?? "";

            string sessionKeyName, newUrl, tenderWordName;
            tenderWordName = string.Empty;
            switch (tenderBy)
            {
                default://SEARCH              
                case TenderTypeList.Keyword:
                    sessionKeyName = "SearhKeywordTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByKeywordUrl"];
                    fieldId = searchProductId;
                    fieldName = searchText.Trim();
                    tenderWordName = TenderWordNameList.KeywordWord;
                    break;
                case TenderTypeList.SearchTender://AdvanceSearch:
                    sessionKeyName = "SearhIndianTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByAdvSearchUrl"];
                    //if (!string.IsNullOrEmpty(searchText))
                    //{
                    //    FillWithinSearchWords(searchText);
                    //    filterMainText = "";
                    //}
                    break;
                case TenderTypeList.State:
                    sessionKeyName = "SearhStateTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByStateUrl"];
                    tenderWordName = TenderWordNameList.StateWord;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        FillWithinSearchWords(searchText);
                        filterMainText = "";
                    }
                    break;
                case TenderTypeList.City:
                    sessionKeyName = "SearhCityTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByCityUrl"];
                    tenderWordName = TenderWordNameList.CityWord;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        FillWithinSearchWords(searchText);
                        filterMainText = "";
                    }
                    break;
                case TenderTypeList.Industry:
                    sessionKeyName = "SearhIndustryTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByIndustryUrl"];
                    tenderWordName = TenderWordNameList.IndustryWord;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        FillWithinSearchWords(searchText);
                        filterMainText = "";
                    }
                    break;
                case TenderTypeList.SubIndustry:
                    sessionKeyName = "SearhSubIndustryTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderBySubIndustryUrl"];
                    tenderWordName = TenderWordNameList.SubIndustryWord;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        FillWithinSearchWords(searchText);
                        filterMainText = "";
                    }
                    break;
                case TenderTypeList.Agency:
                    sessionKeyName = "SearhAgencyTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByAgencyUrl"];
                    tenderWordName = TenderWordNameList.AgencyWord;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        FillWithinSearchWords(searchText);
                        filterMainText = "";
                    }
                    break;
                case TenderTypeList.Sector:
                    sessionKeyName = "SearhSectorTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderBySectorUrl"];
                    tenderWordName = TenderWordNameList.SectorWord;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        FillWithinSearchWords(searchText);
                        filterMainText = "";
                    }
                    break;
                case TenderTypeList.Ownership:
                    sessionKeyName = "SearhOwnershipTenderResult";
                    newUrl = ConfigurationManager.AppSettings["TenderByOwnershipUrl"];
                    tenderWordName = TenderWordNameList.OwnershipWord;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        FillWithinSearchWords(searchText);
                        filterMainText = "";
                    }
                    break;

            }
            Session["PaggingUrl"] = newUrl;

            if (Session["WithinSearchTextList"] != null)
            {
                WithinSearchWords = (List<string>)Session["WithinSearchTextList"];
            }

            //var tenderDetail = GetSearchTenderResult(page, filterMainText.Trim(), searchType, advanceSearch, isFirst, searchProductId, tenderYear, tenderBy, true, orderBy, WithinSearchWords);
            //Session["AdvanceSearchparams"] = advanceSearch;

            //tenderDetail.FieldId = fieldId;
            //tenderDetail.FieldName = fieldName;
            //Session[sessionKeyName] = tenderDetail;

            page = page == 0 ? 1 : page;
            searchText = string.IsNullOrEmpty(searchText) || searchText == "*-*" ? "" : searchText.ToString(CultureInfo.InvariantCulture);

            return Json(new
            {
                newurl = newUrl,
                page,
                searchText = searchText.Replace(" ", "-"),
                fid = fieldId,
                fname = tenderBy == 0 ? "" : fieldName.Replace(" ", "-"),
                tenderBy = tenderBy,
                TenderWordName = tenderWordName
            });
        }
        public ActionResult GetTenderResultOnLoading(AdvanceSearchParameter advanceSearchPara = null)
        {
            int page = 0;
            int tenderBy = 0;
            ViewBag.IsEndOfRecords = false;
            TenderDetail tenderDetail = null;
            /*OtherValues();*/
            int searchId = 0;
            string sessionKeyName = "";
            if (Request.IsAjaxRequest())
            {
                //var advanceSearchPara = new AdvanceSearchParameter();
                //if (Session["AdvanceSearchparams"] != null)
                //{
                //    advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchparams"];
                //}

                if (advanceSearchPara == null)
                {
                    advanceSearchPara = new AdvanceSearchParameter();
                    if (Session["AdvanceSearchparams"] != null)
                    {
                        advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchparams"];
                    }
                }
                tenderBy = advanceSearchPara.TenderBy;
                page = advanceSearchPara.NewPage;
                switch (tenderBy)
                {
                    default://SEARCH
                    case TenderTypeList.Keyword:
                        sessionKeyName = "SearhKeywordTenderResult";
                        searchId = advanceSearchPara.SearchProductId;
                        break;
                    case TenderTypeList.SearchTender:
                        sessionKeyName = "SearhIndianTenderResult";
                        searchId = advanceSearchPara.SearchProductId;
                        break;
                    case TenderTypeList.State:
                        sessionKeyName = "SearhStateTenderResult";
                        break;
                    case TenderTypeList.City:
                        sessionKeyName = "SearhCityTenderResult";
                        break;
                    case TenderTypeList.Industry:
                        sessionKeyName = "SearhIndustryTenderResult";
                        break;
                    case TenderTypeList.SubIndustry:
                        sessionKeyName = "SearhSubIndustryTenderResult";
                        break;
                    case TenderTypeList.Agency:
                        sessionKeyName = "SearhAgencyTenderResult";
                        break;
                    case TenderTypeList.Sector:
                        sessionKeyName = "SearhSectorTenderResult";
                        break;
                    case TenderTypeList.Ownership:
                        sessionKeyName = "SearhOwnershipTenderResult";
                        break;
                }
                var tenderStatus = advanceSearchPara.TenderStatusFlag;
                //FillWithinSearchWords(Session["WithinSearchText"] == null ? "" : Session["WithinSearchText"].ToString());
                if (advanceSearchPara.WithinSearchedText != null)
                    WithinSearchWords = Regex.Split(advanceSearchPara.WithinSearchedText, ",").ToList();

                tenderDetail = GetTenderResult(page, sessionKeyName, advanceSearchPara, searchId, tenderBy, false);
                //if (advanceSearchPara != null)
                //{
                //    Session["AdvanceSearchparams"] = advanceSearchPara;
                //}
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
                return PartialView(Url.Content("~/Views/IndianTenders/Partial/partialSearchResultData.cshtml"), tenderDetail);
            }
            return null;
        }
        public ActionResult GetTenderFromTenderStatus(AdvanceSearchParameter advanceSearchPara = null)
        {
            int page = 1;

            int tenderBy = 0;
            ViewBag.IsEndOfRecords = false;
            TenderDetail tenderDetail = null;
            int searchId = 0;
            string sessionKeyName = "";
            if (Request.IsAjaxRequest())
            {
                if (advanceSearchPara == null)
                {
                    advanceSearchPara = new AdvanceSearchParameter();
                    if (Session["AdvanceSearchparams"] != null)
                    {
                        advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchparams"];
                    }
                }
                tenderBy = advanceSearchPara.TenderBy;

                //var advanceSearchPara = new AdvanceSearchParameter();
                //if (Session["AdvanceSearchparams"] != null)
                //{
                //    advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchparams"];
                //}
                // advanceSearchPara.TenderStatusFlag = tenderStatus;

                switch (tenderBy)
                {
                    default://SEARCH
                    case TenderTypeList.Keyword:
                        sessionKeyName = "SearhKeywordTenderResult";
                        searchId = advanceSearchPara.SearchProductId;
                        break;
                    case TenderTypeList.SearchTender:
                        sessionKeyName = "SearhIndianTenderResult";
                        searchId = advanceSearchPara.SearchProductId;
                        break;
                    case TenderTypeList.State:
                        sessionKeyName = "SearhStateTenderResult";
                        break;
                    case TenderTypeList.City:
                        sessionKeyName = "SearhCityTenderResult";
                        break;
                    case TenderTypeList.Industry:
                        sessionKeyName = "SearhIndustryTenderResult";
                        break;
                    case TenderTypeList.SubIndustry:
                        sessionKeyName = "SearhSubIndustryTenderResult";
                        break;
                    case TenderTypeList.Agency:
                        sessionKeyName = "SearhAgencyTenderResult";
                        break;
                    case TenderTypeList.Sector:
                        sessionKeyName = "SearhSectorTenderResult";
                        break;
                    case TenderTypeList.Ownership:
                        sessionKeyName = "SearhOwnershipTenderResult";
                        break;
                }
                // FillWithinSearchWords(Session["WithinSearchText"] == null ? "" : Session["WithinSearchText"].ToString());
                if (advanceSearchPara.WithinSearchedText != null)
                    WithinSearchWords = Regex.Split(advanceSearchPara.WithinSearchedText, ",").ToList();

                tenderDetail = GetTenderResult(page, sessionKeyName, advanceSearchPara, searchId, tenderBy, true, true);
                //if (advanceSearchPara != null)
                //{
                //    Session["AdvanceSearchparams"] = advanceSearchPara;
                //}
                var total = tenderDetail.Total;
                switch (advanceSearchPara.TenderStatusFlag)
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
                return PartialView(Url.Content("~/Views/IndianTenders/Partial/partialSearchResultData.cshtml"), tenderDetail);
            }
            return null;
        }
        public ActionResult GetTenderFromAdvanceSearch(AdvanceSearchParameter advanceSearchPara = null)
        {
            int page = 1;

            int tenderBy = 0;
            ViewBag.IsEndOfRecords = false;
            TenderDetail tenderDetail = null;
            int searchId = 0;
            string sessionKeyName = "";
            if (Request.IsAjaxRequest())
            {
                if (advanceSearchPara == null)
                {
                    advanceSearchPara = new AdvanceSearchParameter();
                    if (Session["AdvanceSearchparams"] != null)
                    {
                        advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchparams"];
                    }
                }
                tenderBy = advanceSearchPara.TenderBy;
                 
                switch (tenderBy)
                {
                    default://SEARCH
                    case TenderTypeList.Keyword:
                        sessionKeyName = "SearhKeywordTenderResult";
                        searchId = advanceSearchPara.SearchProductId;
                        break;
                    case TenderTypeList.SearchTender:
                        searchId = advanceSearchPara.SearchProductId;
                        sessionKeyName = "SearhIndianTenderResult";
                        break;
                    case TenderTypeList.State:
                        sessionKeyName = "SearhStateTenderResult";
                        break;
                    case TenderTypeList.City:
                        sessionKeyName = "SearhCityTenderResult";
                        break;
                    case TenderTypeList.Industry:
                        sessionKeyName = "SearhIndustryTenderResult";
                        break;
                    case TenderTypeList.SubIndustry:
                        sessionKeyName = "SearhSubIndustryTenderResult";
                        break;
                    case TenderTypeList.Agency:
                        sessionKeyName = "SearhAgencyTenderResult";
                        break;
                    case TenderTypeList.Sector:
                        sessionKeyName = "SearhSectorTenderResult";
                        break;
                    case TenderTypeList.Ownership:
                        sessionKeyName = "SearhOwnershipTenderResult";
                        break;
                }
                
                if (advanceSearchPara.WithinSearchedText != null)
                    WithinSearchWords = Regex.Split(advanceSearchPara.WithinSearchedText, ",").ToList();

                tenderDetail = GetTenderResult(page, sessionKeyName, advanceSearchPara, searchId, tenderBy, true, true);
                //if (advanceSearchPara != null)
                //{
                //    Session["AdvanceSearchparams"] = advanceSearchPara;
                //}
                var total = tenderDetail.Total;
                switch (advanceSearchPara.TenderStatusFlag)
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
                return PartialView(Url.Content("~/Views/IndianTenders/Partial/partialSearchResultData.cshtml"), tenderDetail);
            }
            return null;
        }
        private TenderDetail GetTenderResult(int page, string sessionKeyName, AdvanceSearchParameter advanceSearch, int searchId = 0, int TenderBy = 0, bool isSearchWithCount = true, bool isSearchTextChanged = false)
        {
            TenderDetail tenderDetail = null;

            //Session["AdvanceSearchparams"] = advanceSearch;

            var pagecnt = page != 0 ? page - 1 : 0;
            var newpagecnt = (pagecnt * PageSize);


            tenderDetail = searchId == 0
                   ? GetSearchTenderResult(newpagecnt, "", 2, advanceSearch, 0, 0, "", TenderBy, true, 0, WithinSearchWords)
                   :
                     (TenderBy == TenderTypeList.SearchTender || TenderBy == TenderTypeList.Keyword)
                     ? GetSearchTenderResult(newpagecnt, advanceSearch.SearchProductName, 1, advanceSearch, 0, searchId, "", TenderBy, true, 0, WithinSearchWords)
                     : GetSearchTenderResult(newpagecnt, "", 2, advanceSearch, 0, 0, "", TenderBy, true, 0, WithinSearchWords);
            //Session[sessionKeyName] = tenderDetail;

            //if (Session[sessionKeyName] != null)
            //{
            //    tenderDetail = (TenderDetail)Session[sessionKeyName];
            //    var fieldid = tenderDetail.FieldId;
            //    var fieldname = tenderDetail.FieldName;
            //    //if (tenderDetail.Newpagecnt != newpagecnt || isSearchTextChanged)
            //    //{
            //    string searchText = tenderDetail.DisplayText;
            //    if (TenderBy==TenderTypeList.SearchTender || TenderBy==TenderTypeList.Keyword)
            //    {
            //        searchText = advanceSearch.SearchProductName;
            //    }

            //    tenderDetail = GetSearchTenderResult(newpagecnt, searchText, tenderDetail.SearchType, advanceSearch, 0, tenderDetail.SearchProuctId,
            //        tenderDetail.TenderYear, TenderBy, isSearchWithCount, tenderDetail.OrderBy, WithinSearchWords);

            //    tenderDetail.FieldId = fieldid;
            //    tenderDetail.FieldName = fieldname;
            //    Session[sessionKeyName] = tenderDetail;
            //    //}
            //}
            //else
            //{
            //    tenderDetail = searchId == 0
            //        ? GetSearchTenderResult(newpagecnt, "", 2, advanceSearch, 0, 0, "", TenderBy, true, 0, WithinSearchWords)
            //        : GetSearchTenderResult(newpagecnt, "", 1, advanceSearch, 0, searchId, "", TenderBy, true, 0, WithinSearchWords);
            //    Session[sessionKeyName] = tenderDetail;
            //}

            if (isSearchWithCount)
            {
                //Session["TotalAllTenders"] = totalAll.ToString(CultureInfo.InvariantCulture);
                Session["TotalSearchedTenders"] = tenderDetail.Total.ToString(CultureInfo.InvariantCulture);
                Session["TotalLiveTenders"] = tenderDetail.TotalLive.ToString(CultureInfo.InvariantCulture);
                Session["TotalFreshTenders"] = tenderDetail.TotalFresh.ToString(CultureInfo.InvariantCulture);
                Session["TotalClosedTenders"] = tenderDetail.TotalClosed.ToString(CultureInfo.InvariantCulture);
            }

            tenderDetail.Total = Convert.ToInt64(Session["TotalSearchedTenders"]);
            tenderDetail.TotalLive = Convert.ToInt64(Session["TotalLiveTenders"]);
            tenderDetail.TotalFresh = Convert.ToInt64(Session["TotalFreshTenders"]);
            tenderDetail.TotalClosed = Convert.ToInt64(Session["TotalClosedTenders"]);

            //if (advanceSearch != null)
            //{
            //    Session["AdvanceSearchparams"] = advanceSearch;
            //}

            return tenderDetail;
        }
        public TenderDetail GetSearchTenderResult(int page, string searchText, int searchType, AdvanceSearchParameter advanceSearch,
            int isFirst = 0, int searchProductId = 0, string tenderYear = "", int TenderBy = 0, bool isSearchWithCount = true, int? orderByType = 0,
           List<string> withinSearchWords = null)
        {
            ClearSession();
            //Session["AdvanceSearchparams"] = advanceSearch;
            return _common.GetSearchTenderResult(page, searchText, searchType, advanceSearch, isFirst, searchProductId, tenderYear, TenderBy, isSearchWithCount, orderByType, withinSearchWords);

        }

        private void SetTenderDetails(int tenderBy, string withinSearchText, int fieldId, string fieldName, ref TenderDetail tenderDetail)
        {
            string sessionKeyName = "";

            var shoecurrentpage = (tenderDetail.Newpagecnt / 10) + 1;
            ViewBag.DisplayCurrentPage = shoecurrentpage;
            ViewBag.CurrentPage = shoecurrentpage;
            ViewBag.PageSize = tenderDetail.PageSize;
            ViewBag.TotalPage = Math.Ceiling((double)tenderDetail.Total / tenderDetail.PageSize);
            ViewBag.SearchText = withinSearchText;

            tenderDetail.TendersBy = tenderBy;
            var paggingUrl = "";
            var replaceName = "";
            bool linkListPage = false;

            tenderDetail.FieldId = fieldId;
            tenderDetail.FieldName = fieldName.Replace(" ", "-");
            var displayFieldName = fieldName.Replace("-", " ");

            switch (tenderBy)
            {
                case TenderTypeList.SearchTender:
                    sessionKeyName = "SearhIndianTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByAdvSearchUrl"];
                    replaceName = TenderMetaReplaceName.KeywordName;
                    linkListPage = string.IsNullOrEmpty(withinSearchText) ? true : false;
                    displayFieldName = withinSearchText;
                    break;
                case TenderTypeList.State:
                    sessionKeyName = "SearhStateTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByStateUrl"] + TenderWordNameList.StateWord + fieldName.Replace(" ", "-");
                    replaceName = TenderMetaReplaceName.StateName;
                    break;
                case TenderTypeList.City:
                    sessionKeyName = "SearhCityTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByCityUrl"] + TenderWordNameList.CityWord + fieldName.Replace(" ", "-");
                    replaceName = TenderMetaReplaceName.CityName;
                    break;
                case TenderTypeList.Industry:
                    sessionKeyName = "SearhIndustryTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByIndustryUrl"] + TenderWordNameList.IndustryWord + fieldName.Replace(" ", "-");
                    replaceName = TenderMetaReplaceName.IndustryName;
                    break;
                case TenderTypeList.SubIndustry:
                    sessionKeyName = "SearhSubIndustryTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderBySubIndustryUrl"] + TenderWordNameList.SubIndustryWord + fieldName.Replace(" ", "-");
                    replaceName = TenderMetaReplaceName.SubIndustryName;
                    break;
                case TenderTypeList.Agency:
                    sessionKeyName = "SearhAgencyTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByAgencyUrl"] + TenderWordNameList.AgencyWord + fieldName.Replace(" ", "-");
                    replaceName = TenderMetaReplaceName.AgencyName;
                    break;
                case TenderTypeList.Sector:
                    sessionKeyName = "SearhSectorTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderBySectorUrl"] + TenderWordNameList.SectorWord + fieldName.Replace(" ", "-");
                    replaceName = TenderMetaReplaceName.SectorName;
                    break;
                case TenderTypeList.Ownership:
                    sessionKeyName = "SearhOwnershipTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByOwnershipUrl"] + TenderWordNameList.OwnershipWord + fieldName.Replace(" ", "-");
                    replaceName = TenderMetaReplaceName.OwnershipName;
                    break;
                default:
                case TenderTypeList.Keyword:
                    sessionKeyName = "SearhKeywordTenderResult";
                    paggingUrl = ConfigurationManager.AppSettings["TenderByKeywordUrl"] + TenderWordNameList.KeywordWord + fieldName.Replace(" ", "-");
                    replaceName = TenderMetaReplaceName.KeywordName;
                    break;
            }

            tenderDetail.PaggingUrl = paggingUrl;

            var tenderMetaData = _common.GetTenderMetaData(tenderBy, linkListPage);
            ViewBag.Title = tenderMetaData.Title.Replace(replaceName, displayFieldName);
            ViewBag.Description = tenderMetaData.Description.Replace(replaceName, displayFieldName);
            ViewBag.Keywords = tenderMetaData.Keyword.Replace(replaceName, displayFieldName);
            tenderDetail.DisplaySearchTextDetail = tenderMetaData.Content.Replace(replaceName, displayFieldName);

            OtherValues();

            tenderDetail.StateList = _getListItems.StateList();
            tenderDetail.CityList = _getListItems.CityList(0);
            tenderDetail.IdVal = 0;
            tenderDetail.Subscribetype = SubscribType.DownloadTender;
            tenderDetail.DownloadTenderRefNo = 0;
            tenderDetail.FormTitle = SubscribTypeDisplsyText.DownloadTender;
            tenderDetail.FormType = FormType.PopupForms;

            Session[sessionKeyName] = tenderDetail;
        }
        #endregion


        #region ALL SESSIONS METHODS
        public JsonResult ClearAllSession()
        {
            ClearSession();
            SearchedWordsClear();
            ResetTotalCountSession();

            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult ClearTotalCountSession()
        {
            ResetTotalCountSession();
            return Json(JsonRequestBehavior.AllowGet);
        }


        #endregion

        private AdvanceSearchParameter SetDefaultAdvSearchParam()
        {
            AdvanceSearchParameter advanceSearchPara = new AdvanceSearchParameter()
            {
                IcbNcb = -1,
                TenderTypeId = null
            };
            //if (Session["AdvanceSearchparams"] != null)
            //{
            //    advanceSearchPara = (AdvanceSearchParameter)Session["AdvanceSearchparams"];
            //}
            //else
            //{
            //    advanceSearchPara.IcbNcb = -1;
            //    advanceSearchPara.TenderTypeId = null;
            //}

            //Session["AdvanceSearchparams"] = advanceSearchPara;
            return advanceSearchPara;

        }
        private void FillWithinSearchWords(string searchText)
        {
            if (Session["WithinSearchTextList"] != null)
            {
                WithinSearchWords = (List<string>)Session["WithinSearchTextList"];
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                if (!WithinSearchWords.Contains(searchText.Trim()) && searchText.Trim() != "*-*")
                {
                    WithinSearchWords.Add(searchText);
                }
                Session["WithinSearchTextList"] = WithinSearchWords;
            }
        }

        public JsonResult RemoveWithinSearchWords(string removeText)
        {
            string lastWithinSearch = "";
            if (Session["WithinSearchTextList"] != null)
            {
                WithinSearchWords = (List<string>)Session["WithinSearchTextList"];
            }

            if (!string.IsNullOrEmpty(removeText))
            {
                if (WithinSearchWords.Contains(removeText.Trim()))
                {
                    WithinSearchWords.Remove(removeText);
                }
                Session["WithinSearchTextList"] = WithinSearchWords;
                if (WithinSearchWords.Any())
                {
                    lastWithinSearch = WithinSearchWords[(WithinSearchWords.Count - 1)].ToString();
                }
            }

            return Json(lastWithinSearch, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GenerateSingleLocation(string stateIds, string LocIds)
        {
            var indiaId = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);
            var SelectedLocationIds = "";
            if (!string.IsNullOrEmpty(stateIds) && stateIds != "0")
            {
                var stateIdList = stateIds.Split(',');
                for (int i = 0; i < stateIdList.Length; i++)
                {
                    SelectedLocationIds += ("0#" + stateIdList[i] + "#" + indiaId);
                    SelectedLocationIds += ",";
                }
            }

            if (!string.IsNullOrEmpty(LocIds) && LocIds != "0")
            {
                var locationIdList = LocIds.Split(',');
                for (int i = 0; i < locationIdList.Length; i++)
                {
                    var stateInfo = _tenderInfo.GetCity(Convert.ToInt32(locationIdList[i]));
                    SelectedLocationIds += (locationIdList[i] + "#" + stateInfo.StateId + "#" + indiaId);
                    SelectedLocationIds += ",";
                }
            }

            if (!string.IsNullOrEmpty(SelectedLocationIds))
            {
                SelectedLocationIds = SelectedLocationIds.Substring(0, SelectedLocationIds.Length - 1);
            }

            return Json(SelectedLocationIds, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerateSingleIndustry(string indIds, string subIndIds)
        {
            var SelectedIndustrySubIndustryIds = "";
            if (!string.IsNullOrEmpty(indIds) && indIds != "0")
            {
                var industryIdList = indIds.Split(',');
                for (int i = 0; i < industryIdList.Length; i++)
                {
                    SelectedIndustrySubIndustryIds += ("0#" + industryIdList[i]);
                    SelectedIndustrySubIndustryIds += ",";
                }
            }

            if (!string.IsNullOrEmpty(subIndIds) && subIndIds != "0")
            {
                var subIndustryIdList = subIndIds.Split(',');
                for (int i = 0; i < subIndustryIdList.Length; i++)
                {
                    var industryInfo = _tenderInfo.GetSubIndById(Convert.ToInt32(subIndustryIdList[i]));
                    SelectedIndustrySubIndustryIds += (subIndustryIdList[i] + "#" + industryInfo.IndustryId);
                    SelectedIndustrySubIndustryIds += ",";
                }
            }

            if (!string.IsNullOrEmpty(SelectedIndustrySubIndustryIds))
            {
                SelectedIndustrySubIndustryIds = SelectedIndustrySubIndustryIds.Substring(0, SelectedIndustrySubIndustryIds.Length - 1);
            }

            return Json(SelectedIndustrySubIndustryIds, JsonRequestBehavior.AllowGet);
        }
    }
}