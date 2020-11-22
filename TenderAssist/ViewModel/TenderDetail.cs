using System;
using System.Collections.Generic;
using TenderAssist.Controllers;
using TenderAssist.Models;
using System.Web.Mvc;
using static TenderAssist.Models.SearchModel;
using TenderAssist.Models.DBConnection;

namespace TenderAssist.ViewModel
{
    public class TenderDetail
    {
        public List<TenderInfo_Indian> AllTenderInformation { get; set; }
        //public List<TenaderInfoWithDetail> AllTenaderInfoWithDetail { get; set; }
        public List<SearchTenaderInfoWithAllDetail> AllSearchTenaderInfoWithAllDetail { get; set; }
        public List<SearchTenaderInfoWithAllDetail> AllSearchTenaderInfoWithAllDetailPrivateSector { get; set; }
        public List<SearchTenaderInfoWithAllDetail> AllSearchTenaderInfoWithAllDetailAuction { get; set; }
        //public List<SearchAwardedTenaderInfoWithAllDetail> AllSearchAwardedTenaderInfoWithAllDetail { get; set; }
        //public List<CategorySubCategoryDetail> AllCategorySubCategoryDetail { get; set; }
        
        //public List<TenaderInfoWithStatebyUser> AlltenaderInfoWithStatebyUser { get; set; }
        //public List<TenaderInfoWithAgencybyUser> AlltenaderInfoWithAgencybyUser { get; set; }
        //public List<TenaderInfoWithKeywordbyUser> AlltenaderInfoWithKeywordbyUser { get; set; }
        public List<tbProduct> AllProducts { get; set; }
        //public List<TenderClassifiedIn> AllTenderClassifiedIn { get; set; }
        public List<StateCityForIndianTenders> AllStateCityForIndianTenders { get; set; }
        public List<IndustrySubIndustryDetail> AllIndustrySubIndustryDetail { get; set; }
        public List<AgencySectorOwhershipDetail> AllAgencySectorOwnershipDetail { get; set; }
        public List<tbIndustry> AllIndustry { get; set; }
        public List<tbSubIndustry> AllSubIndustry { get; set; }
        public List<tbAgencyIndian> AllAgency { get; set; }
        public List<tbSector> AllCompanySector { get; set; }
        public List<tbOwnership> AllOwnership { get; set; }
        public List<tbReference> AllReference { get; set; }
        public List<tbState> AllStateList { get; set; }
        public List<tbLocation> AllCityList { get; set; }
        public List<tbCountry> AllCountryList { get; set; }
        //public List<tbRegion> AllRegion { get; set; }
        //public List<Advertisment> AdvertismentList { get; set; }
        public List<tabClientPermission> ClientPermissionList { get; set; }
        public tabClientPermission ClientPermissionItem { get; set; }

        public List<SelectListItem> SearchTypeList { get; set; }
        public List<SelectListItem> SearchTypeList2 { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> IndustryList { get; set; }
        public List<SelectListItem> SubIndustryList { get; set; }
        public List<SelectListItem> SectorList { get; set; }
        public List<SelectListItem> OwnershipList { get; set; }
        public List<SelectListItem> AgencyList { get; set; }
        public List<SelectListItem> CriteriaList { get; set; }
        public List<SelectListItem> ClosedTenderYearList { get; set; }

        public TenderDetails TenderDetails { get; set; }

        public TenderInfo_Indian TenderInformationDetail { get; set; }
        //public TenderInfo_Global GlobalTenderInformationDetail { get; set; }
        public tabClientDetail LoginUserDetails { get; set; }

        public string ClientCountryName { get; set; }
        public string ClientStateName { get; set; }
        public string ClientCityName { get; set; }

        //public TenderDetailsWithInfo TenderDetailsWithInfo { get; set; }
        //public tbTenderAwarded AwardedTenderInformationDetail { get; set; }
        public string PaggingUrl { get; set; }
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public string DisplayText { get; set; }
        public string SearchText { get; set; }
        public string DisplayText2 { get; set; }
        public string DisplayText3 { get; set; }
        public string AdvanceSearchText { get; set; }
        public string AdvsText { get; set; }
        public string LoginUserName { get; set; }
        public string SelectedWord { get; set; }
        public string TextVal { get; set; }
        public string DisplaySearchTextDetail { get; set; }
        public string FormTitle { get; set; }
        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }
        public string SelectedOwnership { get; set; }
        public string SelectedSector { get; set; }
        public string SelectedAgency { get; set; }
        public string SelectedIndustry { get; set; }
        public string SelectedSubIndustry { get; set; }
        public string SelectedProduct { get; set; }
        public string SelectedCountry { get; set; }

        public string SelectedLocationIds { get; set; }
        public string SelectedProductIds { get; set; }
        public string SelectedIndustrySubIndustryIds { get; set; }

        public int SearchProuctId { get; set; }
        public int SearchCityId { get; set; }
        public int SearchStateId { get; set; }
        public int SearchAgencyId { get; set; }
        public int SearchSubIndustryId { get; set; }

        public int SearchType { get; set; }
        public int DisplayCurrentPage { get; set; }
        public int Newpagecnt { get; set; }
        public int TendersBy { get; set; }
        public int RegionId { get; set; }
        public int IdVal { get; set; }
        public int TenderStatus { get; set; }
        public int GlobalTenderFrom { get; set; }
        public int PageSize { get; set; }

        public int TotalDisplay { get; set; }
        public double TotalPage { get; set; }

        public Int64 TotalLive { get; set; }
        public Int64 TotalFresh { get; set; }
        public Int64 TotalClosed { get; set; }
        public Int64 Total { get; set; }

        public Dictionary<int, Int64> FilterByDueDate { get; set; }
        public Dictionary<int, Int64> FilterByTenderValue { get; set; }
        public Dictionary<int, Int64> FilterByTenderType { get; set; }
        public Dictionary<int, string> FilterByRelatedKeyword { get; set; }
        public Dictionary<int, string> BindLeftDataList { get; set; }

        public List<string> AllAlphabaticWord { get; set; }

        public int PermissionId { get; set; }
        public string PermissionGroupName { get; set; }
        public Int64 PermissionGroupTenders { get; set; }
        public string UserCriteriaIDs { get; set; }

        public string SelectedOwnershipNotIn { get; set; }
        public string SelectedSectorNotIn { get; set; }
        public string SelectedAgencyNotIn { get; set; }
        public string SelectedIndustrySubIndustryNotIn { get; set; }
        public string SelectedLocationNotIn { get; set; }

        public string SelectedIndustrySubIndustry { get; set; }
        public string SelectedLocation { get; set; }
        public string SelectedKeyword1 { get; set; }
        public string SelectedKeyword2 { get; set; }
        public string SelectedKeyword3 { get; set; }
        public string OtherKeywords { get; set; }
        public string NotUsedKeywords { get; set; }
        public string IsIcbNcb { get; set; }
        public string DocumentType { get; set; }

        public int UserId { get; set; }
        //public int TenderValFlag { get; set; }

        public string TenderValue { get; set; }
        public double TenderValueFrom { get; set; }
        public double TenderValueTo { get; set; }
        public int TenderStatusFlag { get; set; }

        public DateTime? EnterDt { get; set; }
        public List<SelectListItem> EmailConfigList { get; set; }
        public string Username { get; set; }
        public int Purpose { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public int eId { get; set; }
        public tbInquiryRegForm TbInquiryRegForm { get; set; }
        public int Subscribetype { get; set; }
        public int FormType { get; set; }
        public int DownloadTenderRefNo { get; set; }
        public string TenderYear { get; set; }
        public string OrderBys { get; set; }
        public int OrderBy { get; set; }
        public string AscDesc { get; set; }
        public List<SelectListItem> orderByList { get; set; }
        public List<SelectListItem> ascdescList { get; set; }
        public string ClosedTenderTitle { get; set; }

        public int AdvanceSearchType { get; set; }
        public string AdvanceSearchValue { get; set; }
        public string SearchFilterText { get; set; }

        public int StateCityOurRefNo { get; set; }
        public string StateCityOurRefNoValue { get; set; }

        public AdvanceSearchParameter AdvanceSearchPara { get; set; }

        public int TenderType { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }

        public List<UserFavouriteTenderList> AllUserFavouriteTenderList { get; set; }

        public bool IsHomePage { get; set; }
        public bool IsAdvanceSearch { get; set; }
        public bool IsGlobalTender { get; set; }

        //public List<RelatedWordsModel> RelatedWordsList { get; set; }

        public int OurRefNo { get; set; }
        public int ICBNCB { get; set; }
        public decimal TenderValFrom { get; set; }
        public decimal TenderValTo { get; set; }
        public string TenderSubDateFrom { get; set; }
        public string TenderSubDateTo { get; set; }
        public string TenderOpDateFrom { get; set; }
        public string TenderOpDateTo { get; set; }
    }
}