using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TenderAssist.Models;

namespace TenderAssist.Controllers
{
    public class BaseController : Controller
    {
        

        public void ClearSession()
        {
            Session["SearhStateTenderResult"] = null;
            Session["SearhCityTenderResult"] = null;
            Session["SearhKeywordTenderResult"] = null;
            Session["SearhIndustryTenderResult"] = null;
            Session["SearhSubIndustryTenderResult"] = null;
            Session["SearhAgencyTenderResult"] = null;
            Session["SearhSectorTenderResult"] = null;
            Session["SearhOwnershipTenderResult"] = null;
            Session["SearhIndianTenderResult"] = null;

            Session["WithinSearchText"] = null;
            Session["AdvanceSearchparams"] = null;

        }
        public void ResetTotalCountSession()
        {
            Session["TotalAllTenders"] = null;
            Session["TotalSearchedTenders"] = null;
            Session["TotalLiveTenders"] = null;
            Session["TotalFreshTenders"] = null;
            Session["TotalClosedTenders"] = null;
        }




        public void ClearSession_Global()
        {
            Session["SearhGlobalTenderResult"] = null;
            Session["SearhMiddleEastCountryTenderResult"] = null;
            Session["SearhEuropeanCountryTenderResult"] = null;
            Session["SearhAfricanCountryTenderResult"] = null;
            Session["SearhAsianCountryTenderResult"] = null;
            Session["SearhSAARCountryTenderResult"] = null;
            Session["SearhAustraliaOceaniaCountryTenderResult"] = null;
            Session["SearhSouthAmericaCountryTenderResult"] = null;
            Session["SearhNorthAmericaCountryTenderResult"] = null;

            Session["WithinSearchGlobalText"] = null;
            Session["AdvanceSearchGlobalParams"] = null;
        }
        public void ResetTotalCountSession_Global()
        {
            Session["TotalAllGlobalTenders"] = null;
            Session["TotalSearchedGlobalTenders"] = null;
            Session["TotalGlobalLiveTenders"] = null;
            Session["TotalGlobalFreshTenders"] = null;
            Session["TotalGlobalClosedTenders"] = null;
        }

        public void SearchedWordsClear()
        {
            /*INDIAN TENDER*/
            Session["WithinSearchTextList"] = null;
            
            ///*TENDER AWARDED*/
            //Session["WithinSearchAwardedText"] = null;
            //Session["ViewAwardedTendersByWithinSearch"] = null;


            ///*GLOBAL AWARDED*/
            Session["WithinSearchGlobalTextList"] = null;
            //Session["ViewGlobalTendersByWithinSearch"] = null;


            Session["withinTotalRecordCount"] = null;
            Session["IsReet"] = "1";
        }



        public void ClearUserSearchSession()
        {
            Session["UserIndianTenders"] = null;
            Session["UserGlobalTenders"] = null;
            Session["WithinSearchText"] = null;
            Session["Client_TotalRecordCount"] = null;
            Session["Client_TotalAllTenders"] = null;
            Session["Client_TotalSearchedTenders"] = null;
            Session["Client_TotalLiveTenders"] = null;
            Session["Client_TotalFreshTenders"] = null;
            Session["Client_TotalClosedTenders"] = null;
        }



        public void OtherValues()
        {
            #region Tender Type
            var tenderType = new TenderType();
            var tendertype = tenderType.FindAllTenderType().ToList();
            ViewData["TenderType"] = new SelectList(tendertype, "TenderTypeValue", "TenderTypeName");
            #endregion

            #region Tender Status
            var tderStatus = new TenderStatus();
            var tenderStatus = tderStatus.FindAllTenderStatus().ToList();
            ViewData["TenderStatus"] = new SelectList(tenderStatus, "TenderStatusValue", "TenderStatusName");
            #endregion

            #region Tender Value Type
            var tenderValType = new TenderValType();
            var tvaltype = tenderValType.FindAllTenderValType().ToList();
            ViewData["TenderValType"] = new SelectList(tvaltype, "TenderValTypeValue", "TenderValTypeName");
            #endregion

            var commondata = new List<SelectListItem>();
            commondata.Insert(0, (new SelectListItem { Text = "[None]", Value = "0" }));
            ViewData["List"] = new SelectList(commondata, "Value", "Text");
        }
    }
}