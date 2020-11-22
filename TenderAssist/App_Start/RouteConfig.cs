
using System.Web.Mvc;
using System.Web.Routing;
using TenderAssist.CommonHelper;
using static TenderAssist.CommonHelper.Utility;

namespace TenderAssist
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            #region INDIAN TENDER LIST

            routes.MapRoute(
                name: "IndianTenders-Tenders",
                url: TenderTypeDisplayText.IndianTender + "/tenders/",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "Tenders"
                });

            #endregion

            #region CATEGORY TENDERS

            #region STATE / CITY

            #region ================ STATE ================

            routes.MapRoute(
               name: "IndianTenders-State-Tenders",
               url: TenderTypeDisplayText.IndianTender + "/State-List/",
               defaults: new
               {
                   controller = "IndianTenders",
                   action = "StateTenders"
               });
            routes.MapRoute(
                name: "IndianTenders-State-Tenders-Word",
                url: TenderTypeDisplayText.IndianTender + "/State-List/" + TenderWordNameList.StateWord + "{stateName}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "StateTenderList",
                    stateName = UrlParameter.Optional,
                });
            routes.MapRoute(
                name: "IndianTenders-State-Tenders-withinSearchText",
                url: TenderTypeDisplayText.IndianTender + "/State-List/" + TenderWordNameList.StateWord + "{stateName}/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "StateTenderList",
                    stateName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });

            #endregion

            #region ================ CITYS ================

            routes.MapRoute(
               name: "IndianTenders-City-Tenders",
               url: TenderTypeDisplayText.IndianTender + "/city-list/",
               defaults: new
               {
                   controller = "IndianTenders",
                   action = "CityTenders"
               });
            routes.MapRoute(
                name: "IndianTenders-City-Tenders-Word",
                url: TenderTypeDisplayText.IndianTender + "/city-list/" + TenderWordNameList.CityWord + "{cityName}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "CityTenderList",
                    cityName = UrlParameter.Optional,
                });
            routes.MapRoute(
                name: "IndianTenders-City-Tenders-withinSearchText",
                url: TenderTypeDisplayText.IndianTender + "/city-list/" + TenderWordNameList.CityWord + "{cityName}/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "CityTenderList",
                    cityName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });

            #endregion

            #endregion

            #region INDUSTRY / SUB INDUSTRY

            #region ================ INDUSTRY ================

            routes.MapRoute(
                name: "IndianTenders-Industry-Tenders",
                url: TenderTypeDisplayText.IndianTender + "/industry-list/",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "IndustryTenders"
                });
         

            routes.MapRoute(
               name: "IndianTenders-Industry-Tenders-Word",
               url: TenderTypeDisplayText.IndianTender + "/industry-list/" + TenderWordNameList.IndustryWord + "{industryName}",
               defaults: new
               {
                   controller = "IndianTenders",
                   action = "IndustryTenderList",
                   industryName = UrlParameter.Optional,
               });
            routes.MapRoute(
                name: "IndianTenders-Industry-Tenders-withinSearchText",
                url: TenderTypeDisplayText.IndianTender + "/industry-list/" + TenderWordNameList.IndustryWord + "{industryName}/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "IndustryTenderList",
                    industryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });

            #endregion

            #region ================ SUB INDUSTRY ================

            routes.MapRoute(
                name: "IndianTenders-SubIndustry-Tenders",
                url: TenderTypeDisplayText.IndianTender + "/subIndustry-list/",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "SubIndustryTenders"
                });
            routes.MapRoute(
                name: "IndianTenders-SubIndustry-Tenders-Word",
                url: TenderTypeDisplayText.IndianTender + "/subIndustry-list/" + TenderWordNameList.SubIndustryWord + "{subindustryName}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "SubIndustryTenderList",
                    subindustryName = UrlParameter.Optional,
                });
            routes.MapRoute(
                name: "IndianTenders-SubIndustry-Tenders-withinSearchText",
                url: TenderTypeDisplayText.IndianTender + "/subIndustry-list/" + TenderWordNameList.SubIndustryWord + "{subindustryName}/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "SubIndustryTenderList",
                    subindustryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });
            #endregion

            #endregion

            #region AGENCY / SECTOR / OWNERSHIP

            #region ================ AGENCY ================

            routes.MapRoute(
                name: "IndianTenders-Agency-Tenders",
                url: TenderTypeDisplayText.IndianTender + "/agency-list/",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "AgencyTenders"
                });
            routes.MapRoute(
                name: "IndianTenders-Agency-Tenders-Word",
                url: TenderTypeDisplayText.IndianTender + "/agency-list/" + TenderWordNameList.AgencyWord + "{agencyName}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "AgencyTenderList",
                    agencyName = UrlParameter.Optional,
                });
            routes.MapRoute(
                name: "IndianTenders-Agency-Tenders-withinSearchText",
                url: TenderTypeDisplayText.IndianTender + "/agency-list/" + TenderWordNameList.AgencyWord + "{agencyName}/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "AgencyTenderList",
                    agencyName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });

            #endregion

            #region ================ SECTOR ================

            routes.MapRoute(
                name: "IndianTenders-Sector-Tenders",
                url: TenderTypeDisplayText.IndianTender + "/sector-list/",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "SectorTenders"
                });
            routes.MapRoute(
                name: "IndianTenders-Sector-Tenders-Word",
                url: TenderTypeDisplayText.IndianTender + "/sector-list/" + TenderWordNameList.SectorWord + "{sectorName}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "SectorTenderList",
                    sectorName = UrlParameter.Optional,
                });
            routes.MapRoute(
                name: "IndianTenders-Sector-Tenders-withinSearchText",
                url: TenderTypeDisplayText.IndianTender + "/sector-list/" + TenderWordNameList.SectorWord + "{sectorName}/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "SectorTenderList",
                    sectorName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });

            #endregion

            #region ================ OWNERSHIP ================

            routes.MapRoute(
                name: "IndianTenders-Ownership-Tenders",
                url: TenderTypeDisplayText.IndianTender + "/ownership-list/",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "OwnershipTenders"
                });
            routes.MapRoute(
                name: "IndianTenders-Ownership-Tenders-Word",
                url: TenderTypeDisplayText.IndianTender + "/ownership-list/" + TenderWordNameList.OwnershipWord + "{ownershipName}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "OwnershipTenderList",
                    ownershipName = UrlParameter.Optional,
                });
            routes.MapRoute(
                name: "IndianTenders-Ownership-Tenders-withinSearchText",
                url: TenderTypeDisplayText.IndianTender + "/ownership-list/" + TenderWordNameList.OwnershipWord + "{ownershipName}/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "OwnershipTenderList",
                    ownershipName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });

            #endregion

            #endregion

            #region ================ KEYWORDS ================

            routes.MapRoute(
                name: "IndianTenders-Keyword-Tenders",
                url: TenderTypeDisplayText.IndianTender + "/products/",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "KeywordTenders"
                });
            routes.MapRoute(
                name: "IndianTenders-Keyword-Tenders-Word",
                url: TenderTypeDisplayText.IndianTender + "/products/" + TenderWordNameList.KeywordWord + "{keyword}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "KeywordTenderList",
                    keyword = UrlParameter.Optional,
                });

            routes.MapRoute(
                name: "IndianTenders-Keyword-Tenders-withinSearchText",
                url: TenderTypeDisplayText.IndianTender + "/products/" + TenderWordNameList.KeywordWord + "{keyword}/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "KeywordTenderList",
                    keyword = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });

            #endregion

            #endregion

            #region ADVANCE SEARCH

            routes.MapRoute(
                name: "IndianTenders-AdvanceSearchTenders-nosearch",
                url: "Tenders/Advance-Search-Tenders",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "AdvanceSearchTenders",
                });
            routes.MapRoute(
                name: "IndianTenders-AdvanceSearchTenders",
                url: "Tenders/Advance-Search-Tenders/{withinSearchText}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "AdvanceSearchTenders",
                    withinSearchText = UrlParameter.Optional,
                });
            #endregion

            #region TENDER DETAIL PAGES

            routes.MapRoute(
                name: "IndianTenders-TenderNoticeDetail",
                url: TenderTypeDisplayText.IndianTender + "/bid-tenders-details/{tYear}/{refno}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "TenderNotice",
                    tYear = UrlParameter.Optional,
                    refno = UrlParameter.Optional,
                });

            routes.MapRoute(
                name: "Form-BidWithTenderAssist",
                url: TenderTypeDisplayText.IndianTender + "/bid-with-tenderAssist/{tYear}/{refno}",
                defaults: new
                {
                    controller = "IndianTenders",
                    action = "BidWithTenderAssist",
                    tYear = UrlParameter.Optional,
                    refno = UrlParameter.Optional,
                });

            #endregion

            #region ALL INQUIRY FORMS

            routes.MapRoute(
                name: "Form-IndianTenders",
                url: TenderTypeDisplayText.IndianTender,
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "IndianTenders",
                });

            routes.MapRoute(
                name: "Form-GlobalTenders",
                url: "global-tenders",
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "GlobalTenders",
                });

            routes.MapRoute(
                name: "Form-TenderResult",
                url: "tender-awarded",
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "TenderResult",
                });

            routes.MapRoute(
                name: "Form-TenderAssist",
                url: "bid-tender-get-assistance",
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "TenderAssist",
                });
            routes.MapRoute(
              name: "Form-result",
              url: "result",
              defaults: new
              {
                  controller = "InquiryForms",
                  action = "result",
              });
            routes.MapRoute(
                name: "Form-project",
                url: "project",
            defaults: new
            {
                controller = "InquiryForms",
                action = "project",
            });
            routes.MapRoute(
                name: "Form-VendorEmpanelment",
                url: "vendor-empanelment",
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "VendorEmpanelment",
                });

            routes.MapRoute(
                name: "Form-Subcontracting",
                url: "subcontracting",
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "Subcontracting",
                });

            routes.MapRoute(
                name: "Form-DigitalSignature",
                url: "get-tender-digital-signature-certificate",
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "DigitalSignature",
                });

            routes.MapRoute(
                name: "Form-ISO-Consultant",
                url: "iso-consultant",
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "ISOConsultant",
                });

            routes.MapRoute(
                name: "Form-NewRegistration",
                url: "get-new-tender-subscription",
                defaults: new
                {
                    controller = "InquiryForms",
                    action = "NewRegistration",
                });

            routes.MapRoute(
                name: "Form-Contact",
                url: "contactUs",
                defaults: new
                {
                    controller = "Home",
                    action = "Contact",
                });
            routes.MapRoute(
                name: "Form-TermsConditions",
                url: "terms-and-conditions",
                defaults: new
                {
                    controller = "Home",
                    action = "TermsConditions",
                });
            routes.MapRoute(
                name: "Form-PrivacyPolicy",
                url: "privacy-policy",
                defaults: new
                {
                    controller = "Home",
                    action = "PrivacyPolicy",
                });
            routes.MapRoute(
                name: "Form-About",
                url: "aboutUs",
                defaults: new
                {
                    controller = "Home",
                    action = "About",
                });

            #endregion

            #region PAY ONLINE

            routes.MapRoute(
               name: "Form-PayOnline",
               url: "payonline",
               defaults: new
               {
                   controller = "Home",
                   action = "PayOnline",
               });
            routes.MapRoute(
                name: "Form-PayOnline-Status",
                url: "payonline-status",
                defaults: new
                {
                    controller = "Home",
                    action = "PaymentStatus",
                });

            #endregion




            #region GLOBAL TENDER LIST

            routes.MapRoute("global-tenders", TenderTypeDisplayText.GlobalTender + "/tenders",
                new
                {
                    controller = "GlobalTenders",
                    action = "Tenders"
                });
            routes.MapRoute("global-tenders-keyword", TenderTypeDisplayText.GlobalTender + "/products/" + TenderWordNameList.KeywordWord + "{keyword}",
                new
                {
                    controller = "GlobalTenders",
                    action = "Index",
                    keyword = UrlParameter.Optional
                });

            routes.MapRoute("global-tenders-withinSearchText", TenderTypeDisplayText.GlobalTender + "/products/" + TenderWordNameList.KeywordWord + "{keyword}/{withinSearchText}",
               new
               {
                   controller = "GlobalTenders",
                   action = "Index",
                   keyword = UrlParameter.Optional,
                   withinSearchText = UrlParameter.Optional
               });

            #endregion

            #region GLOBAL CATEGORY TENDERS

            #region ================ MIDDLE-EAST-COUNTRY ================

            routes.MapRoute("global-tenders-middle-east-country", TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayMiddleEastCountryName.Replace(" ", "-"),
              new
              {
                  controller = "GlobalTenders",
                  action = "TendersByMiddleEastCountry"
              });

            routes.MapRoute("global-tenders-middle-east-country-tenders",
              TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayMiddleEastCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}",
              new
              {
                  controller = "GlobalTenders",
                  action = "MiddleEastCountryTenders",
                  countryName = UrlParameter.Optional
              });
            routes.MapRoute("global-tenders-middle-east-country-tenders-withsearch",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayMiddleEastCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}/{withinSearchText}",
                new
                {
                    controller = "GlobalTenders",
                    action = "MiddleEastCountryTenders",
                    countryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });


            #endregion

            #region ================ EUROPEAN-COUNTRY ================

            routes.MapRoute("global-tenders-european-country", TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayEuropeanCountryName.Replace(" ", "-"),
              new
              {
                  controller = "GlobalTenders",
                  action = "TendersByEuropeanCountry"
              });

            routes.MapRoute("global-tenders-european-country-tenders",
               TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayEuropeanCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}",
               new
               {
                   controller = "GlobalTenders",
                   action = "EuropeanCountryTenders",
                   countryName = UrlParameter.Optional
               });

            routes.MapRoute("global-tenders-european-country-tenders-withsearch",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayEuropeanCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}/{withinSearchText}",
                new
                {
                    controller = "GlobalTenders",
                    action = "EuropeanCountryTenders",
                    countryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional
                });

            #endregion

            #region ================ AFRICAN-COUNTRY ================

            routes.MapRoute("global-tenders-african-country", TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAfricanCountryName.Replace(" ", "-"),
              new
              {
                  controller = "GlobalTenders",
                  action = "TendersByAfricanCountry"
              });

            routes.MapRoute("global-tenders-african-country-tenders",
               TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAfricanCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}",
               new
               {
                   controller = "GlobalTenders",
                   action = "AfricanCountryTenders",
                   countryName = UrlParameter.Optional
               });
            routes.MapRoute("global-tenders-african-country-tenders-withsearch",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAfricanCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}/{withinSearchText}",
                new
                {
                    controller = "GlobalTenders",
                    action = "AfricanCountryTenders",
                    countryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional,
                });

            #endregion

            #region ================ SAAR-COUNTRY ================

            routes.MapRoute("global-tenders-saar-country", TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplaySaarCountryName.Replace(" ", "-"),
               new
               {
                   controller = "GlobalTenders",
                   action = "TendersBySaarCountry"
               });

            routes.MapRoute("global-tenders-saar-country-tenders",
               TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplaySaarCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}",
               new
               {
                   controller = "GlobalTenders",
                   action = "SAARCountryTenders",
                   countryName = UrlParameter.Optional,
               });

            routes.MapRoute("global-tenders-saar-country-tenders-withsearch",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplaySaarCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}/{withinSearchText}",
                new
                {
                    controller = "GlobalTenders",
                    action = "SAARCountryTenders",
                    countryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional,
                });

            #endregion

            #region ================ SOUTH-AMERICA-COUNTRY ================

            routes.MapRoute("global-tenders-South-America-Country", TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplaySouthAmericaCountryName.Replace(" ", "-"),
              new
              {
                  controller = "GlobalTenders",
                  action = "TendersBySouthAmericaCountry"
              });

            routes.MapRoute("global-tenders-South-America-country-tenders",
               TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplaySouthAmericaCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}",
               new
               {
                   controller = "GlobalTenders",
                   action = "SouthAmericaCountryTenders",
                   countryName = UrlParameter.Optional,
               });

            routes.MapRoute("global-tenders-South-America-country-tenders-withsearch",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplaySouthAmericaCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}/{withinSearchText}",
                new
                {
                    controller = "GlobalTenders",
                    action = "SouthAmericaCountryTenders",
                    countryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional,
                });


            #endregion

            #region ================ ASIAN-COUNTRY ================

            routes.MapRoute("global-tenders-asian-country", TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAsianCountryName.Replace(" ", "-"),
                new
                {
                    controller = "GlobalTenders",
                    action = "TendersByAsianCountry"
                });

            routes.MapRoute("global-tenders-asian-country-tenders",
               TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAsianCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}",
               new
               {
                   controller = "GlobalTenders",
                   action = "AsianCountryTenders",
                   countryName = UrlParameter.Optional,
               });

            routes.MapRoute("global-tenders-asian-country-tenders-withsearch",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAsianCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}/{withinSearchText}",
                new
                {
                    controller = "GlobalTenders",
                    action = "AsianCountryTenders",
                    countryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional,
                });
            #endregion

            #region ================ AUSTRALIA-OCEANIA-COUNTRY ================

            routes.MapRoute("global-tenders-Australia-Oceania-country", TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAustraliaOceaniaCountryName.Replace(" ", "-"),
               new
               {
                   controller = "GlobalTenders",
                   action = "TendersByAustraliaOceaniaCountry"
               });

            routes.MapRoute("global-tenders-Australia-Oceania-country-tenders",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAustraliaOceaniaCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}",
                new
                {
                    controller = "GlobalTenders",
                    action = "AustraliaOceaniaCountryTenders",
                    countryName = UrlParameter.Optional,
                });

            routes.MapRoute("global-tenders-Australia-Oceania-country-tenders-withsearch",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayAustraliaOceaniaCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}/{withinSearchText}",
                new
                {
                    controller = "GlobalTenders",
                    action = "AustraliaOceaniaCountryTenders",
                    countryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional,
                });


            #endregion

            #region ================ NORTH-AMERICA-COUNTRY ================

            routes.MapRoute("global-tenders-North-America-Country", TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayNorthAmericaCountryName.Replace(" ", "-"),
                new
                {
                    controller = "GlobalTenders",
                    action = "TendersByNorthAmericaCountry"
                });

            routes.MapRoute("global-tenders-North-America-country-tenders",
               TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayNorthAmericaCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}",
               new
               {
                   controller = "GlobalTenders",
                   action = "NorthAmericaCountryTenders",
                   countryName = UrlParameter.Optional,
               });

            routes.MapRoute("global-tenders-North-America-country-tenders-withsearch",
                TenderTypeDisplayText.GlobalTender + "/" + TenderTypeDisplayText.DisplayNorthAmericaCountryName.Replace(" ", "-") + "/" + TenderWordNameList.GlobalCountryWord + "{countryName}/{withinSearchText}",
                new
                {
                    controller = "GlobalTenders",
                    action = "NorthAmericaCountryTenders",
                    countryName = UrlParameter.Optional,
                    withinSearchText = UrlParameter.Optional,
                });
            #endregion

            #endregion

            #region GLOBAL TENDER DETAIL
            routes.MapRoute(
               name: "global-tenders-Tender-Notice",
               url: TenderTypeDisplayText.GlobalTender + "/global-tenders-details/{refno}",
               defaults: new
               {
                   controller = "GlobalTenders",
                   action = "TenderNotice",
                   refno = UrlParameter.Optional,
               });
            #endregion


            /*USER*/
            #region USERS

            routes.MapRoute(
                 name: "IndianTenders-TendersList",
                 url: "user/indian-tender/{permissionId}",
                 defaults: new
                 {
                     controller = "User",
                     action = "IndianTenderList",
                     permissionId = UrlParameter.Optional
                 });

            routes.MapRoute(
                name: "User-TenderNoticeDetail",
                url: "user/tenderdetail/{tYear}/{refno}",
                defaults: new
                {
                    controller = "User",
                    action = "TenderDetail",
                    tYear = UrlParameter.Optional,
                    refno = UrlParameter.Optional,
                });

            routes.MapRoute(
                name: "GlobalTenders-TendersList",
                url: "user/global-tender/{permissionId}",
                defaults: new
                {
                    controller = "User",
                    action = "GlobalTenderList",
                    permissionId = UrlParameter.Optional
                });
            routes.MapRoute(
               name: "User-GlobalTenderNoticeDetail",
               url: "user/globaltenderdetail/{refno}",
               defaults: new
               {
                   controller = "User",
                   action = "GlobalTenderDetail",
                   refno = UrlParameter.Optional,
               });


            routes.MapRoute(
                name: "LoginUrl",
                url: "log/{un}",
                defaults: new
                {
                    controller = "Log",
                    action = "Index",
                    un = UrlParameter.Optional
                });

            #endregion

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
