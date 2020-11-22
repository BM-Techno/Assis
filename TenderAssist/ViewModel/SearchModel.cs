using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TenderAssist.Models
{
    public class SearchModel
    {
        public class SearchIndianTenderModel
        {
            public int Page { get; set; }
            public string Search { get; set; }
            public int SType { get; set; }
            public int IsFirst { get; set; }
            public int SearchProductId { get; set; }
            public string TenderYear { get; set; }
            public AdvanceSearchParameter AdvanceSearch { get; set; }
            public int FId { get; set; }
            public string FName { get; set; }
            public int TenderBy { get; set; }
            public int OrderBy { get; set; }
            public int SearchId { get; set; }
            public string SearchTxt { get; set; }
            public int IsReset { get; set; }

            public string CountryIds { get; set; }
            public string StateIds { get; set; }
            public string CityIds { get; set; }
            public string ProductIds { get; set; }
            public int? TenderStatusFlag { get; set; }

            public string IndustryIds { get; set; }
            public string SubIndustryIds { get; set; }

            public string AgencyIds { get; set; }
            public string SectorIds { get; set; }
            public string OwnershipIds { get; set; }
        }

        public class AdvanceSearchParameter
        {
            public int SearchProductId { get; set; }
            public string SearchProductName { get; set; }
            public int? StateId { get; set; }
            public int? CityId { get; set; }
            public int? IndustryId { get; set; }
            public int? SubIndustryId { get; set; }
            public int? AgencyId { get; set; }
            public int? SectorId { get; set; }
            public int? OwnershipId { get; set; }
            public int? TenderValFlag { get; set; }
            public double? TenderValue { get; set; }
            public int? OurRefNo { get; set; }
            public int? TenderTypeId { get; set; }
            public int? TenderStatusFlag { get; set; }
            public string SubDateFrom { get; set; }
            public string SubDateTo { get; set; }
            public string OpDateFrom { get; set; }
            public string OpDateTo { get; set; }
            //public int? SubDateFlag { get; set; }
            //public int? OpDateFlag { get; set; }
            public int? CountryId { get; set; }
            public int? FilterType { get; set; }
            public int IcbNcb { get; set; }
            public string RefIds { get; set; }

            public string SelectedStates { get; set; }
            public string SelectedCities { get; set; }
            public string SelectedProducts { get; set; }
            public string SelectedLocations { get; set; }
            public string SelectedCountries { get; set; }
            public string SelectedIndustries { get; set; }
            public string SelectedSubIndustries { get; set; }
            public string SelectedIndsubIndustries { get; set; }
            public string SelectedAgencies { get; set; }
            public string SelectedSectors { get; set; }
            public string SelectedOwnerships { get; set; }
            public string OtherKeywordText { get; set; }
            public string WithinSearchedText { get; set; }
            public int TenderBy { get; set; }
            public int NewPage { get; set; }
        }

        public class SearchUserTenderModel
        {
            public int Page { get; set; }
            public string Search { get; set; }
            public int SType { get; set; }
            public int IsFirst { get; set; }
            public int SearchProductId { get; set; }
            public string TenderYear { get; set; }
            public AdvanceSearchParameter AdvanceSearch { get; set; }
            public int FId { get; set; }
            public string FName { get; set; }
            public int TenderBy { get; set; }
            public int OrderBy { get; set; }
            public int SearchId { get; set; }
            public string SearchTxt { get; set; }
            public int IsReset { get; set; }

            public string CountryIds { get; set; }
            public string StateIds { get; set; }
            public string CityIds { get; set; }
            public string ProductIds { get; set; }
            public string Product1Ids { get; set; }
            public string Product2Ids { get; set; }
            public string Product3Ids { get; set; }

            public int? TenderStatusFlag { get; set; }

            public string IndustryIds { get; set; }
            public string SubIndustryIds { get; set; }

            public string AgencyIds { get; set; }
            public string SectorIds { get; set; }
            public string OwnershipIds { get; set; }

            public string OtherKeywordText { get; set; }


            public string Locationds { get; set; }
            public string IndSubIndustryIds { get; set; }

            public string SubDateFrom { get; set; }
            public string SubDateTo { get; set; }
            public string OpDateFrom { get; set; }
            public string OpDateTo { get; set; }


            public bool IsAdvanceSearch { get; set; }
            public bool IsDefaultDataDisplay { get; set; }

            public bool IsIndianGlobal { get; set; }

            public int PermissionId { get; set; }

            public string Organization { get; set; }
            public string Status { get; set; }
            public DateTime? EnterDate { get; set; }
        }

        public class TenderMetaData
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Keyword { get; set; }
            public string Content { get; set; }
        }
        public class TenderMetaReplaceName
        {
            public static string SearchWord = "{SearchWord}";
            public static string StateName = "{StateName}";
            public static string CityName = "{CityName}";
            public static string IndustryName = "{IndustryName}";
            public static string SubIndustryName = "{SubIndustryName}";
            public static string SectorName = "{SectorName}";
            public static string OwnershipName = "{OwnershipName}";
            public static string AgencyName = "{AgencyName}";
            public static string KeywordName = "{KeywordName}";
            public static string InfoSourceName = "{InfoSourceName}";
            public static string IcbName = "{ICBName}";
            public static string NcbName = "{NCBName}";
            public static string AuctionName = "{AuctionName}";
            public static string CorrigendumName = "{CorrigendumName}";
            public static string RfpRfqName = "{RFPRFQName}";
            public static string VendorRegName = "{VenderRegName}";

            public static string TenderTitle = "{TenderTitle}";
            public static string TenderLocation = "{TenderLocation}";
            public static string TenderDescription = "{TenderDescription}";
            public static string TenderDueDate = "{TenderDueDate}";
            public static string TenderValue = "{TenderValue}";
            public static string TenderId = "{TenderID}";
            public static string TenderOurrefNo = "{TenderOurrefNo}";
            public static string TenderNo = "{TenderNo}";

            public static string CountryName = "{CountryName}";
            public static string CompanyName = "{Company}";
            public static string GlobalCountryName = "{GlobalCountryName}";

        }
    }
}
