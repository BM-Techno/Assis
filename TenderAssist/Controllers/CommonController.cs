using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TenderAssist.Models;
using TenderAssist.Models.DBConnection;
using TenderAssist.ViewModel;
using static TenderAssist.CommonHelper.Utility;
using static TenderAssist.Models.SearchModel;

namespace TenderAssist.Controllers
{
    public class CommonController : BaseController
    {
        private readonly TenderInformation _tenderInfo;
        private static int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DataPageSize"]);


        public CommonController()
        {
            _tenderInfo = new TenderInformation();

        }

        public TenderMetaData GetTenderMetaData(int tendersBy, bool linkListPage = false)
        {
            #region SET TITLE DESCRIPTION KEYWORD AND DISPLAY NAME

            var ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
            var SiteName = ConfigurationManager.AppSettings["SiteName"].ToString();

            string sitename = "<b><a href='" + appName + "'>" + SiteName + "</a></b>";
            string contenthighlightstart = "<span style=\"color:red;\"><b>";
            string contenthighlightend = "</b></span>";

            string setMetaState = TenderMetaReplaceName.StateName;
            string setMetaCity = TenderMetaReplaceName.CityName;
            string setMetaProducts = TenderMetaReplaceName.KeywordName;
            string setSubIndustry = TenderMetaReplaceName.SubIndustryName;
            string setIndustry = TenderMetaReplaceName.IndustryName;
            string setAgency = TenderMetaReplaceName.AgencyName;
            string setSector = TenderMetaReplaceName.SectorName;
            string setOwnership = TenderMetaReplaceName.OwnershipName;
            string setTenderDescription = TenderMetaReplaceName.TenderDescription;
            string setTenderOurrefNo = TenderMetaReplaceName.TenderOurrefNo;
            string setTenderNo = TenderMetaReplaceName.TenderNo;
            string setMetaKeyword = TenderMetaReplaceName.KeywordName;

            string title = "", description = "", keywords = "", Content = "";
            switch (tendersBy)
            {
                case TenderTypeList.HomePage:
                    title = "Get All Latest Tenders Information & Tenders Assistance Online in India";
                    description = "TenderAssist247 will give all Tenders information Online, Government Tenders, Private Tenders, India tenders, Global Tenders, Auction Tenders, Corporation Tenders, Industry Tenders, Subscribe Today";
                    keywords = "Tender, Tenders ,Tender Bidding, online tender information, latest tenders information, digital signature, equipments tenders, tender assist, tender alerts, indian tenders, industry tenders, state tenders, agency tenders, city tenders, ownership tenders, corporations, Tender Info, Online Tenders India, Government Tenders, E Tenders, Get Tenders Online, Online Tender Portal, Tender Assistance, tender online, online tender website, tender site, online tender asssistant, Private Tenders, Tenders 24*7, Tenders 24/7, Tender 24*7, Tender 24/7, Tender 24x7, TenderAssist247.com";
                    break;

                case TenderTypeList.IndianTenderListing:
                    title = "Indian Tenders, Indian Online Tenders Information, Indian E Tenders, Indian Government Tenders, Indian International Tenders, Indian Global Tenders, Indian Tender Assistance - TenderAssist247.com";
                    description = "Indian Tender Alerts, Indian Online Tender Information, Indian Tenders, Indian Online Tender Information service Provider, Indian Tender Notifications, Tender Information By Indian Tenders, TenderAssist247.com";
                    keywords = "Indian Tender, Indian Tenders, Indian Tender Bidding, Indian online tender information, Indian latest tenders information, Indian digital signature, Indian equipments tenders, Indian tender assist, Indian tender alerts, Indian industry tenders, Indian state tenders, Indian ownership tenders, Indian corporations, Indian Tender Info, Indian Online Tenders India, Indian Government Tenders, Indian E Tenders, Indian Get Tenders Online, Indian Online Tender Portal, Indian Tender Assistance, Indian tender online, Indian online tender website, Indian tender site, Indian online tender asssistant, Indian Private Tenders, Indian Tenders 24*7, Indian Tenders 24/7, Indian Tender 24*7, Indian Tender 24/7, Indian Tender 24x7 - TenderAssist247";
                    break;

                case TenderTypeList.DetailPage:
                    title = setTenderDescription + ". Tenders in " + setMetaCity + " - " + setMetaState;
                    description = "Ref No : " + setTenderOurrefNo + ". Tender No : " + setTenderNo + " Tender from " + setAgency + ", Invited Tenders for " + setTenderDescription + ". " + setMetaCity + " - " + setMetaState + ".";
                    keywords = setMetaProducts != "" ? "Tenders for " + setMetaProducts : "Tenders for " + setTenderDescription;
                    break;

                case TenderTypeList.SearchTender:
                    if (linkListPage)
                    {
                        title = @"Tender, Tenders, e tenders , Tender daily Service , India Tender, Tender advisor India, Tender Information service , Government Tenders, Global Tenders,  International Tenders, Indian Tenders , eprocure tenders";
                        description = @"The most accurate and reliable tender services  in india .  Register free for list of tenders, government tenders& international tenders  and get unique facility for search by product, tenders by location, online tenders, public tenders, international tenders";
                        keywords = @"Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice, Indian tender portal,  tenders notification service,  tender notice, government tenders, govt tenders, tenders info, free tenders, India Tender, Tender India, Global Tenders, Public Tenders, Free Government Tenders, Indian Government Tenders, eprocure tenders, tenderassist247.com";
                    }
                    else
                    {
                        title = @"" + setMetaKeyword + " tenders, " + setMetaKeyword + " Tenders in india , online " + setMetaKeyword + " tenders  , " + setMetaKeyword + " Tenders Information , " + setMetaKeyword + " Tender Notice, " + setMetaKeyword + " e tenders From India";
                        description = @"" + setMetaKeyword + " Tenders - Get access of " + setMetaKeyword + " Tenders information and notifications ,  Get  free email alert for " + setMetaKeyword + " Tenders from India , tender advisor for " + setMetaKeyword + ", " + setMetaKeyword + " Tender,  " + setMetaKeyword + " Tenders From India, Download " + setMetaKeyword + " Tender Documents. Search Tenders for " + setMetaKeyword + ", Register Now for " + setMetaKeyword + " Tenders in India, Government " + setMetaKeyword + " Tenders," + setMetaKeyword + " tenderassist247.com .";
                        keywords = @"private " + setMetaKeyword + " Tenders, " + setMetaKeyword + " Tender, " + setMetaKeyword + " Tenders Information, " + setMetaKeyword + " Tenders Notification, " + setMetaKeyword + " Tenders in  India, Tender for " + setMetaKeyword + "," + setMetaKeyword + " Tenders," + setMetaKeyword + " Tender Documents, Tenders in " + setMetaKeyword + ", Tender Notice For " + setMetaKeyword + " , " + setMetaKeyword + " Tenders Details, " + setMetaKeyword + " Tender Information Leads, " + setMetaKeyword + " Tenders Online News," + setMetaKeyword + " online tenders info ,all tenders of " + setMetaKeyword + " , eprocurement  tenders for " + setMetaKeyword + " , etendering of " + setMetaKeyword + " Tenders";
                    }
                    break;

                case TenderTypeList.State:
                    if (linkListPage)
                    {
                        title = "State Online Tender Information, State E Tenders, State Government Tenders, State International Tenders, State Indian Tenders, State Tender Assistance- Online State Tenders - Bid Online Tenders by State - TenderAssist247";
                        description = "State Tender Alerts, State Online Tender Information, State Global Tenders, State Indian Tenders, State Online Tender Information service Provider, State Tender Notifications, TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all cities. Bid Tenders State wise";
                        keywords = "State Tender, State Tenders, State Tender Bidding, State online tender information, State latest tenders information, State digital signature, State equipments tenders, State tender assist, State tender alerts, State indian tenders, State ownership tenders, State corporations, State Tender Info, State Online Tenders India, State Government Tenders, State E Tenders, State Get Tenders Online, State Online Tender Portal, State Tender Assistance, State tender online, State online tender website, State tender site, State online tender asssistant, State Private Tenders, State Tenders 24*7, State Tenders 24/7, State Tender 24*7, State Tender 24/7, State Tender 24x7 - TenderAssist247";
                    }
                    else
                    {
                        title = "Online " + setMetaState + " Online Tender Information, " + setMetaState + " E Tenders, " + setMetaState + " Government Tenders, " + setMetaState + " International Tenders, " + setMetaState + " Indian Tenders, " + setMetaState + " Tender Assistance - " + " Online " + setMetaState + " Tenders - " + " Bid Online Tenders by "  + setMetaState + " TenderAssist247";
                        description = "Online " + setMetaState + " Tender Alerts, " + setMetaState + " Online Tender Information, " + setMetaState + " Global Tenders, " + setMetaState + " Indian Tenders, " + setMetaState + " Online Tender Information service Provider, " + setMetaState + " Tender Notifications, " + " TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all cities. Bid Tenders State wise";
                        keywords = "tenders in " + setMetaState + " Tender, " + setMetaState + " Tenders, " + setMetaState + " Tender Bidding, " + setMetaState + " online tender information, " + setMetaState + " latest tenders information, " + setMetaState + " digital signature, " + setMetaState + " equipments tenders, " + setMetaState + " tender assist, " + setMetaState + " tender alerts, " + setMetaState + " indian tenders, " + setMetaState + " ownership tenders, " + setMetaState + " corporations, " + setMetaState + " Tender Info, " + setMetaState + " Online Tenders India, " + setMetaState + " Government Tenders, " + setMetaState + " E Tenders, " + setMetaState + " Get Tenders Online, " + setMetaState + " Online Tender Portal, " + setMetaState + " Tender Assistance, " + setMetaState + " tender online, " + setMetaState + " online tender website, " + setMetaState + " tender site, " + setMetaState + " online tender asssistant, " + setMetaState + " Private Tenders, " + setMetaState + " Tenders 24*7, " + setMetaState + " Tenders 24/7, " + setMetaState + " Tender 24*7, " + setMetaState + " Tender 24/7, " + setMetaState + " Tender 24x7 - " + " TenderAssist247";

                        setMetaState = contenthighlightstart + setMetaState + contenthighlightend;
                        Content = "Get  Daily Online Tender Alerts for " + setMetaState + " Government tenders on your Email. Access daily available  e tender from " + setMetaState + ", through Login id and password provided by www.TenderAssist247.com. Online Work Tenders for Every Tender Notices published in " + setMetaState + ", Get Free Sample Tenders which are available in " + setMetaState + ". for your various products services and Works on your E-Mail. You can participate  or upload your Bids via E procure or E procurement portals of " + setMetaState + " tenders";
                    }
                    break;

                case TenderTypeList.City:

                    if (linkListPage)
                    {
                        title = "Online City Tenders - Bid Online Tenders by City - TenderAssist247";
                        description = "TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all cities. Bid Tenders City wise.";
                        keywords = "Bid Online City wise Tenders, Bid online Tenders of Delhi,Bid Online Tenders of Mumbai, Get Bidding Support for any City Tenders";
                    }
                    else
                    {
                        title = "Tenders of " + setMetaCity + " Online Tender Information, " + setMetaCity + " E Tenders, " + setMetaCity + " Government Tenders, " + setMetaCity + " International Tenders, " + setMetaCity + " Indian Tenders, " + " Tender Assistance-Online " + setMetaCity + " Tenders " + " Bid Online Tenders by "   + setMetaCity + " TenderAssist247 ";
                        description = "City wise Tenders, online Tenders of " + setMetaCity + " Tender Alerts, " + setMetaCity + " Online Tender Information, " + setMetaCity + " Global Tenders, " + setMetaCity + " Indian Tenders, " + setMetaCity + " Online Tender Information service Provider, " + setMetaCity + " Tender Notifications, " + " TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all cities.Bid Tenders + setMetaCity + wise.";
                       keywords = "online Tenders of " + setMetaCity + " Tender, " + setMetaCity + " Tenders, " + setMetaCity + " Tender Bidding," + setMetaCity + " online tender information, " + setMetaCity + " latest tenders information, " + setMetaCity + " digital signature, " + setMetaCity + " equipments tenders, " + setMetaCity + " tender assist, " + setMetaCity + " tender alerts, " + setMetaCity + " indian tenders, " + setMetaCity + " industry tenders, " + setMetaCity + " state tenders, " + setMetaCity + " ownership tenders, " + setMetaCity + " corporations, " + setMetaCity + " Tender Info, " + setMetaCity + " Online Tenders India, " + setMetaCity + " Government Tenders, " + setMetaCity + " E Tenders, " + setMetaCity + " Get Tenders Online, " + setMetaCity + " Online Tender Portal, " + setMetaCity + " Tender Assistance, " + setMetaCity + " tender online, " + setMetaCity + " online tender website, " + setMetaCity + " tender site, " + setMetaCity + " online tender asssistant, " + setMetaCity + " Private Tenders, " + setMetaCity + " Tenders 24*7, " + setMetaCity + " Tenders 24/7, " + setMetaCity + " Tender 24*7, " + setMetaCity + " Tender 24/7, " + setMetaCity + " Tender 24x7 " +  " - TenderAssist247";

                        setMetaCity = contenthighlightstart + setMetaCity + contenthighlightend;
                        Content = "Find Online local tenders in " + setMetaCity + ". View all Government tenders, e tender, Online Tender Notices published from " + setMetaCity + ". Till date more than 100000+ tenders are published by various Government Department, Corporations, State, PSU’s & Private Companies from " + setMetaCity + ". Get live tenders which are available in " + setMetaCity + " for your various products, services and Works on your E-Mail and Get Bidding Support";
                    }
                    break;

                case TenderTypeList.Keyword:
                    if (linkListPage)
                    {
                        title = "Keyword Online Work Tenders, Keyword Bid Online Tenders by Work, Keyword Online Tender Information, Keyword E Tenders, Keyword Government Tenders, Keyword International Tenders, Keyword Indian Tenders, Keyword Tender Assistance - TenderAssist247";
                        description = "Keyword Tender Alerts, Keyword Online Tender Information, Keyword Global Tenders, Keyword Indian Tenders, Keyword Online Tender Information service Provider, Keyword Tender Notifications, TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all Keyword. Bid Tenders Keyword wise.";
                        keywords = "Keyword Tender, Keyword Tenders, Keyword Tender Bidding, Keyword online tender information, Keyword latest tenders information, Keyword digital signature, Keyword equipments tenders, Keyword tender assist, Keyword tender alerts, Keyword Keyword tenders, Keyword Keyword tenders, Keyword state tenders, Keyword tenders, Keyword Keyword tenders, Keyword ownership tenders, Keyword corporations, Keyword Tender Info, Keyword Online Tenders India, Keyword Government Tenders, Keyword E Tenders, Keyword Get Tenders Online, Keyword Online Tender Portal, Keyword Tender Assistance, Keyword tender online, Keyword online tender website, Keyword tender site, Keyword online tender asssistant, Keyword Private Tenders, Keyword Tenders 24*7, Keyword Tenders 24/7, Keyword Tender 24*7, Keyword Tender 24/7, Keyword Tender 24x7 - TenderAssist247";
                    }

                    else
                    {
                        title = "Online Tender Information,"  + setMetaProducts +  " E Tenders, "  + setMetaProducts +  " Government Tenders, "  + setMetaProducts +  " International Tenders,"  + setMetaProducts +  " Indian Tenders,"  + setMetaProducts +  " Tender Assistance " + "Online " + setMetaProducts + " Tenders - "+ " Bid Online Tenders by " + setMetaProducts + " TenderAssist247";
                       description = "online Tenders for " + setMetaProducts + " Tender Alerts, " + setMetaProducts + "Online Tender Information," + setMetaProducts +  " Global Tenders, " + setMetaProducts +  " Indian Tenders," + setMetaProducts + " Online Tender Information service Provider," + setMetaProducts + " Tender Notifications," + " TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all" +setMetaProducts +" Bid Tenders Keyword wise.";
                        keywords = "online Tenders for " + setMetaProducts + " Tender, " + setMetaProducts + " Tender Bidding," + setMetaProducts +  " online tender information," + setMetaProducts + " latest tenders information, " + setMetaProducts + " digital signature," + setMetaProducts + " equipments tenders," + setMetaProducts + " tender assist," + setMetaProducts + " tender alerts," + setMetaProducts + " Government Tenders," + setMetaProducts + " E Tenders," + setMetaProducts + " Get Tenders Online," + setMetaProducts + " Online Tender Portal," + setMetaProducts + " Tender Assistance, " + setMetaProducts + " tender online," + setMetaProducts + " online tender website," + setMetaProducts + " tender site," + setMetaProducts + " online tender asssistant, " + setMetaProducts + " Private Tenders, " + setMetaProducts + " Tenders 24*7, " + setMetaProducts + " Tenders 24/7," + setMetaProducts + " Tender 24*7, " + setMetaProducts + " Tender 24/7," + setMetaProducts + " Tender 24x7 " + "TenderAssist247";

                        setMetaProducts = contenthighlightstart + setMetaProducts + contenthighlightend;
                        Content = "Find Online local tenders for " + setMetaProducts + ". View all Government tenders, e tender, Online Tender Notices published for " + setMetaProducts + ". Till date more than 100000+ tenders are published by various Government Department, Corporations, State, PSU’s & Private Companies for " + setMetaProducts + ". Get live tenders which are available for " + setMetaProducts + " for your various products, services and Works on your E-Mail and Get Bidding Support";
                    }
                    break;

                case TenderTypeList.Industry:
                    if (linkListPage)
                    {
                        title = "Industry Online Tender Information, Industry E Tenders, Industry Government Tenders, Industry International Tenders, Industry Indian Tenders, Industry Tender Assistance- Online Industry Tenders - Bid Online Tenders by Industry - TenderAssist247";
                        description = "Industry Tender Alerts, Industry Online Tender Information, Industry Global Tenders, Industry Indian Tenders, Industry Online Tender Information service Provider, Industry Tender Notifications, TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all Industry. Bid Tenders Industry wise.";
                        keywords = "Industry Tender, Industry Tenders, Industry Tender Bidding, Industry online tender information, Industry latest tenders information, Industry digital signature, Industry equipments tenders, Industry tender assist, Industry tender alerts, Industry indian tenders, Industry state tenders, Industry ownership tenders, Industry corporations, Industry Tender Info, Industry Online Tenders India, Industry Government Tenders, Industry E Tenders, Industry Get Tenders Online, Industry Online Tender Portal, Industry Tender Assistance, Industry tender online, Industry online tender website, Industry tender site, Industry online tender asssistant, Industry Private Tenders, Industry Tenders 24*7, Industry Tenders 24/7, Industry Tender 24*7, Industry Tender 24/7, Industry Tender 24x7 - TenderAssist247";
                    }
                    else
                    {
                        title = "Online Tenders of " + setIndustry + " - Get Tender bidding support - TenderAssist247";
                        description = "Bid Online Work wise Tenders, Bid online Tenders of " + setIndustry + ",Bid Online Tenders of " + setIndustry + ", Get Bidding Support for any Industry Tenders - " + setIndustry + "";
                        keywords = "Bid Online Work wise Tenders, Bid online Tenders of " + setIndustry + ",Bid Online Tenders of " + setIndustry + ", Get Bidding Support for any Industry Tenders - " + setIndustry + "";

                        setIndustry = contenthighlightstart + setIndustry + contenthighlightend;
                        Content = "Find Online local tenders of " + setIndustry + ". View all Government tenders, e tender, Online Tender Notices published of " + setIndustry + ". Till date more than 100000+ tenders are published by various Government Department, Corporations, State, PSU’s & Private Companies of " + setIndustry + ". Get live tenders which are available of " + setIndustry + " for your various products, services and Works on your E-Mail and Get Bidding Support";
                    }

                    break;
                case TenderTypeList.SubIndustry:
                    if (linkListPage)
                    {
                        title = "SubIndustry Online Tender Information, SubIndustry E Tenders, SubIndustry Government Tenders, SubIndustry International Tenders, SubIndustry Indian Tenders, SubIndustry Tender Assistance- Online SubIndustry Tenders - Bid Online Tenders by SubIndustry - TenderAssist247";
                        description = "SubIndustry Tender Alerts, SubIndustry Online Tender Information, SubIndustry Global Tenders, SubIndustry Indian Tenders, SubIndustry Online Tender Information service Provider, SubIndustry Tender Notifications, TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all SubIndustry. Bid Tenders SubIndustry wise.";
                        keywords = "SubIndustry Tender, SubIndustry Tenders, SubIndustry Tender Bidding, SubIndustry online tender information, SubIndustry latest tenders information, SubIndustry digital signature, SubIndustry equipments tenders, SubIndustry tender assist, SubIndustry tender alerts, SubIndustry indian tenders, SubIndustry ownership tenders, SubIndustry corporations, SubIndustry Tender Info, SubIndustry Online Tenders India, SubIndustry Government Tenders, SubIndustry E Tenders, SubIndustry Get Tenders Online, SubIndustry Online Tender Portal, SubIndustry Tender Assistance, SubIndustry tender online, SubIndustry online tender website, SubIndustry tender site, SubIndustry online tender asssistant, SubIndustry Private Tenders, SubIndustry Tenders 24*7, SubIndustry Tenders 24/7, SubIndustry Tender 24*7, SubIndustry Tender 24/7, SubIndustry Tender 24x7 - TenderAssist247";
                    }
                    else
                    {
                        title = "Online Tenders of " + setSubIndustry + " - Get Tender bidding support - TenderAssist247";
                        description = "Bid Online Work wise Tenders, Bid online Tenders of " + setSubIndustry + ",Bid Online Tenders of " + setSubIndustry + ", Get Bidding Support for any SubIndustry Tenders - " + setSubIndustry + "";
                        keywords = "Bid Online Work wise Tenders, Bid online Tenders of " + setSubIndustry + ",Bid Online Tenders of " + setSubIndustry + ", Get Bidding Support for any SubIndustry Tenders - " + setSubIndustry + "";

                        setSubIndustry = contenthighlightstart + setSubIndustry + contenthighlightend;
                        Content = "Find Online local tenders of " + setSubIndustry + ". View all Government tenders, e tender, Online Tender Notices published of " + setSubIndustry + ". Till date more than 100000+ tenders are published by various Government Department, Corporations, State, PSU’s & Private Companies of " + setSubIndustry + ". Get live tenders which are available of " + setSubIndustry + " for your various products, services and Works on your E-Mail and Get Bidding Support";
                    }
                    break;

                case TenderTypeList.Agency:
                    if (linkListPage)
                    {
                        title = "Agency Online Tender Information, Agency E Tenders, Agency Government Tenders, Agency International Tenders, Agency Indian Tenders, Agency Tender Assistance- Online Agency Tenders - Bid Online Tenders by Agency - TenderAssist247";
                        description = "Agency Tender Alerts, Agency Online Tender Information, Agency Global Tenders, Agency Indian Tenders, Agency Online Tender Information service Provider, Agency Tender Notifications, TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all Agency. Bid Tenders Agency wise.";
                        keywords = "Agency Tender, Agency Tenders , Agency Tender Bidding, Agencyonline tender information, Agencylatest tenders information, Agency digital signature, Agency equipments tenders, Agency tender assist, Agency tender alerts, Agency indian tenders, Agency industry tenders, Agency state tenders,  Agency city tenders, Agency ownership tenders, Agency corporations, Agency Tender Info, Agency Online Tenders India, Agency Government Tenders, Agency E Tenders, Agency Get Tenders Online, Agency Online Tender Portal, Agency Tender Assistance, Agency tender online, Agency online tender website, Agency tender site, Agency online tender asssistant, Agency Private Tenders, Agency Tenders 24*7, Agency Tenders 24/7, Agency Tender 24*7, Agency Tender 24/7, Agency Tender 24x7 - TenderAssist247";
                    }
                    else
                    {
                        title = "Online Tenders of " + setAgency + " Online Tender Information, " + setAgency + " E Tenders, " + setAgency + " Government Tenders, " + setAgency + " International Tenders, " + setAgency + " Indian Tenders, " + setAgency + " Tender Assistance- " + " Online " + setAgency + " Tenders - " + " Bid Online Tenders by " + setAgency + " - TenderAssist247";
                        description = "Bid Online "  + setAgency + " Tender Alerts, " + setAgency + " Online Tender Information, " + setAgency + " Global Tenders, " + setAgency + " Indian Tenders, " + setAgency + " Online Tender Information service Provider, " + setAgency + " Tender Notifications, " + " TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all Agency. "  +  " Bid Tenders Agency wise.";
                        keywords = "Bid online Tenders of " + setAgency + " Tender, " + setAgency + " Tenders, " + setAgency + " Tender Bidding, " + setAgency + " online tender information, " + setAgency + " latest tenders information, " + setAgency + " digital signature, " + setAgency + " equipments tenders, " + setAgency + " tender assist, " + setAgency + " tender alerts, " + setAgency + " indian tenders, " + setAgency + " industry tenders, " + setAgency + " state tenders, " + setAgency + " city tenders, " + setAgency + " ownership tenders, " + setAgency + " corporations, " + setAgency + " Tender Info, " + setAgency + " Online Tenders India, " + setAgency + " Government Tenders, " + setAgency + " E Tenders, " + setAgency + " Get Tenders Online, " + setAgency + " Online Tender Portal, " + setAgency + " Tender Assistance, " + setAgency + " tender online, " + setAgency + " online tender website," + setAgency + " tender site, " + setAgency + " online tender asssistant, " + setAgency + " Private Tenders, " + setAgency + " Tenders 24*7, " + setAgency + " Tenders 24/7, " + setAgency + " Tender 24*7, " + setAgency + " Tender, "  + setAgency + " Tenders, " + setAgency + " Tender Bidding, " + setAgency + " online tender information, " + setAgency + " latest tenders information, " + setAgency + " digital signature, " + setAgency + " equipments tenders, " + setAgency + " tender assist, " + setAgency + " tender alerts, " + setAgency + " indian tenders, " + setAgency + " industry tenders, " + setAgency + " state tenders, "  + setAgency + " city tenders, " + setAgency + " ownership tenders, "  + setAgency + " corporations, " + setAgency + " Tender Info, " + setAgency + " Online Tenders India, " + setAgency + " Government Tenders, " + setAgency + " E Tenders, " + setAgency + " Get Tenders Online, " + setAgency + " Online Tender Portal, " + setAgency + " Tender Assistance, " + setAgency + " tender online, " + setAgency + " online tender website,"  + setAgency + " tender site, " + setAgency + " online tender asssistant, " + setAgency +  " Private Tenders, " + setAgency + " Tenders 24*7, " + setAgency + " Tenders 24/7, " + setAgency + " Tender 24*7, " + setAgency + " Tender 24/7, " + setAgency + " Tender 24x7 " + " TenderAssist247 " + " Tender 24/7, " + setAgency + " Tender 24x7 - TenderAssist247";

                        setAgency = contenthighlightstart + setAgency + contenthighlightend;
                        Content = "Find Online local tenders of " + setAgency + ". View all Government tenders, e tender, Online Tender Notices published of " + setAgency + ". Till date more than 100000+ tenders are published by various Government Department, Corporations, State, PSU’s & Private Companies of " + setAgency + ". Get live tenders which are available of " + setAgency + " for your various products, services and Works on your E-Mail and Get Bidding Support";
                    }
                    break;

                case TenderTypeList.Sector:
                    if (linkListPage)
                    {
                        title = "Sector Online Tender Information, Sector E Tenders, Sector Government Tenders, Sector International Tenders, Sector Global Tenedrs, Sector Indian Tenders, Sector Tender Assistance- Online Company Sector Tenders - Bid Online Tenders by Company Sector - TenderAssist247";
                        description = "Sector Tender Alerts, Sector Online Tender Information, Sector Global Tenders, Sector Indian Tenders, Sector Online Tender Information service Provider, Sector Tender Notifications, TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all Company Sector. Bid Tenders Company Sector wise.";
                        keywords = "Sector Tender, Sector Tenders, Sector Tender Bidding, Sector online tender information, Sector latest tenders information, Sector digital signature, Sector equipments tenders, Sector tender assist, Sector tender alerts, Sector Sector tenders, Sector Sector tenders, Sector state tenders, Sector tenders, Sector Sector tenders, Sector ownership tenders, Sector corporations, Sector Tender Info, Sector Online Tenders India, Sector Government Tenders, Sector E Tenders, Sector Get Tenders Online, Sector Online Tender Portal, Sector Tender Assistance, Sector tender online, Sector online tender website, Sector tender site, Sector online tender asssistant, Sector Private Tenders, Sector Tenders 24*7, Sector Tenders 24/7, Sector Tender 24*7, Sector Tender 24/7, Sector Tender 24x7 - TenderAssist247";
                    }
                    else
                    {
                        title = "Online Tenders by " + setSector + " Online Tender Information, " + setSector + " E Tenders, " + setSector + " Government Tenders, " + setSector + " International Tenders, " + setSector + " Global Tenedrs, " + setSector + " Indian Tenders, " + setSector + " Tender Assistance - " + " Online Company " + setSector + " Tenders - Bid Online Tenders by Company "  + setSector +  " - TenderAssist247";
                        description = "Tenders by " + setSector + " Tender Alerts, " + setSector + " Online Tender Information," + setSector + " Global Tenders, " + setSector + " Indian Tenders, " + setSector + " Online Tender Information service Provider, " + setSector + " Tender Notifications, " + " TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all Company " + setSector + " Bid Tenders Company " + setSector + " wise.";
                        keywords = "Tenders by " + setSector + " Tender, " + setSector + " Tenders, " + setSector + " Tender Bidding, "  + setSector + " online tender information, " + setSector + " latest tenders information, " + setSector + " digital signature, " + setSector + " equipments tenders, " + setSector + " tender assist, " + setSector + " tender alerts, " + setSector + " + setSector + tenders," + setSector + " state tenders, " + setSector + " ownership tenders, " + setSector + " corporations, " + setSector + " Tender Info, " + setSector + " Online Tenders India, " + setSector + " Government Tenders, " + setSector + " E Tenders, " + setSector + " Get Tenders Online, " + setSector + " Online Tender Portal, " + setSector + " Tender Assistance, " + setSector + " tender online, " + setSector + " online tender website, " + setSector + " tender site, " + setSector + " online tender asssistant, " + setSector + " Private Tenders, " + setSector + " Tenders 24*7, " + setSector + " Tenders 24/7, " + setSector + " Tender 24*7, " + setSector + " Tender 24/7, " + setSector + " Tender 24x7 " +  " - TenderAssist247";

                        setSector = contenthighlightstart + setSector + contenthighlightend;
                        Content = "Find Online local tenders by " + setSector + ". View all Government tenders, e tender, Online Tender Notices published by " + setSector + ". Till date more than 100000+ tenders are published by various Government Department, Corporations, State, PSU’s & Private Companies by " + setSector + ". Get live tenders which are available by " + setSector + " for your various products, services and Works on your E-Mail and Get Bidding Support";
                    }

                    break;

                case TenderTypeList.Ownership:
                    var ownershiplist = _tenderInfo.ListOwnership().ToList();
                    var allOwnership = ownershiplist.Aggregate("", (current, item) => current + (item.OwnershipName + " Tenders, "));
                    if (allOwnership != "")
                    { allOwnership = allOwnership.Substring(0, allOwnership.Length - 2); }

                    if (linkListPage)
                    {
                        title = "Online Ownership Tenders - Bid Online Tenders by Ownership - TenderAssist247";
                        description = "TenderAssist247 is the Only Portal Of India who is providing Tender Bidding support published from all Ownership. Bid Tenders Ownership wise.";
                        keywords = "Bid Online Ownership wise Tenders, Bid online Tenders of Delhi,Bid Online Tenders of Mumbai, Get Bidding Support for any Ownership Tenders";
                    }
                    else
                    {
                        title = "Online Tenders by " + setOwnership + " - Get Tender bidding support - TenderAssist247";
                        description = "Bid Online Work wise Tenders, Bid online Tenders by " + setOwnership + ",Bid Online Tenders by " + setOwnership + ", Get Bidding Support for any Ownership Tenders - " + setOwnership + "";
                        keywords = "Get Daily Online Tenders From " + setOwnership + ",eTendering by " + setOwnership + ",Proc Tenders by " + setOwnership + ", Get Bidding Support for any " + setOwnership + " Tenders" + " eTendering processes on various" + setOwnership + "";

                        setOwnership = contenthighlightstart + setOwnership + contenthighlightend;
                        Content = "Find Online local tenders by " + setOwnership + ". View all Government tenders, e tender, Online Tender Notices published by " + setOwnership + ". Till date more than 100000+ tenders are published by various Government Department, Corporations, State, PSU’s & Private Companies by " + setOwnership + ". Get live tenders which are available by " + setOwnership + " for your various products, services and Works on your E-Mail and Get Bidding Support";
                    }
                    break;


                case TenderTypeList.FreeSampleTender:
                    title = "Free Sample Tender - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.TenderRegistration:
                    title = "Tender Registration - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.GetGlobaltender:
                    title = "Get Global Tender - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.GetTenderResults:
                    title = "Get Tender Results - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.VendorManagementsupport:
                    title = "Vendor Management Support - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.GetDigitalcertificate:
                    title = "Get Digital Certificate - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.Contactus:
                    title = "Contact US - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.QuickcontactFromGoogle:
                    title = "Quick Contact - Google - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.DownloadTender:
                    title = "Download Tender - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.TenderBidding:
                    title = "Tender Bidding - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
                case TenderTypeList.SubscriptionSuccess:
                    title = "Subscription Success - Online Tender Bidding Support, Bid Tenders with - TenderAssist247";
                    description = "Tender Bidding Support , Online Bid Portal ,Government Tenders, Free Tenders, Tender document , auction tenders , private tenders ,Online Tenders service, Global Tenders, Tender India Information, Notice Invitation Tender, , Procurement notice, public tenders , e tenders, corporate tenders, tender notification, procurement notice.";
                    keywords = "Get All Public Goverment private sector tenders online on TenderAssist247 and bid tenders with our help and support . Online Tender Bidding Support Portal";
                    break;
            }

            const RegexOptions options = RegexOptions.None;
            var regex = new Regex(@"[ ]{2,}", options);

            title = regex.Replace(title, @" ");
            description = regex.Replace(description, @" ");
            keywords = regex.Replace(keywords, @" ");
            Content = regex.Replace(Content, @" ");

            TenderMetaData objTenderMetaData = new TenderMetaData();
            objTenderMetaData.Title = title;
            objTenderMetaData.Description = description;
            objTenderMetaData.Keyword = keywords;
            objTenderMetaData.Content = Content;

            #endregion

            return objTenderMetaData;
        }
        public TenderMetaData GetTenderMetaDataGlobal(int tendersBy, bool linkListPage = true)
        {
            string globalCountryName = TenderMetaReplaceName.GlobalCountryName;
            string stateName = TenderMetaReplaceName.StateName;
            string cityName = TenderMetaReplaceName.CityName;
            string keywordName = TenderMetaReplaceName.KeywordName;
            string tenderLocation = TenderMetaReplaceName.TenderLocation;
            string tenderDescription = TenderMetaReplaceName.TenderDescription;
            string tenderOurrefNo = TenderMetaReplaceName.TenderOurrefNo;
            string tenderDueDate = TenderMetaReplaceName.TenderDueDate;
            string tenderValue = TenderMetaReplaceName.TenderValue;
            string Title = "";
            string Description = "";
            string Keywords = "";
            if (!linkListPage)
            {
                Title = globalCountryName + " Tenders, Open and Live Tenders from " + globalCountryName;
                Description = "All types of Tenders Details from " + globalCountryName + " Country  ";
                Keywords = "Tenders From " + globalCountryName + ", " + globalCountryName + " Tenders, Tenders of " + globalCountryName + " Government, World Wide " + globalCountryName + " Tenders, ICB Tenders in " + globalCountryName + ", International Competitive Bidding " + globalCountryName + ", global tenders " + globalCountryName + ", world tenders " + globalCountryName + " Tenders,  list of tenders in " + globalCountryName + ", " + globalCountryName + " Tender Notices, " + globalCountryName + " Public Tenders, " + globalCountryName + " Bid News, " + globalCountryName + " and worldwide Tenders, Private Tenders in " + globalCountryName + ".";
            }
            else
            {
                switch (tendersBy)
                {
                    case -3:
                        Title = "Tender for " + tenderDescription + " | Tenders in " + cityName;
                        Description = tenderDescription + " | Due date: " + tenderDueDate + " | Tender Location:  " + tenderLocation + " | TRN: " + tenderOurrefNo;
                        Keywords = "Tenders for " + keywordName;
                        break;

                    case 0:
                        Title = keywordName + " Tenders, Global Tenders of " + keywordName + ", " + keywordName + " online tender portal";
                        Description = "Search " + keywordName + " Tenders, Tenders By " + keywordName + ", Tenders For " + keywordName + ", Private Tenders in " + keywordName + ", Find Local Tenders in " + keywordName + ", " + keywordName + " Tenders in Global Level";
                        Keywords = keywordName + " Tenders, Tenders By " + keywordName + ", Tenders For " + keywordName + ", Private Tenders in " + keywordName + ", Download Tender Documents " + keywordName + ", Tenders in " + keywordName + ", Tender Notice For " + keywordName + ", " + keywordName + " Tenders in Global, " + keywordName + " Tender Process, E tender " + keywordName + ", Public " + keywordName + " Notice, Press Notice ";
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        Title = globalCountryName + " tenders, tenders in " + globalCountryName; ;
                        Description = "Latest Live and Fresh tender's information from " + globalCountryName + ". Download Tender documents of " + globalCountryName;
                        Keywords = globalCountryName + " Global Tenders, International Tender, Tenders From Woldwide, All Countries Tenders.";
                        break;
                }
            }
            Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
            string str1 = regex.Replace(Title, " ");
            string str2 = regex.Replace(Description, " ");
            string str3 = regex.Replace(Keywords, " ");
            return new TenderMetaData()
            {
                Title = str1,
                Description = str2,
                Keyword = str3
            };
        }
        public List<SelectListItem> ClosedYearTenderList(string selectedVal)
        {
            int totalClosedTenderYear = Convert.ToInt32(ConfigurationManager.AppSettings["TotalClosedTenderYear"]);
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            for (int index = 0; index <= totalClosedTenderYear; ++index)
            {
                string yearName;
                if (index == 0)
                {
                    yearName = DateTime.Today.Year.ToString();
                }
                else
                {
                    yearName = DateTime.Today.AddYears(-index).Year.ToString();
                }
                bool flag = yearName == selectedVal.ToString();
                selectListItemList.Insert(index, new SelectListItem()
                {
                    Text = yearName,
                    Value = yearName,
                    Selected = flag
                });
            }
            return selectListItemList;
        }


        public TenderDetail GetSearchTenderResult(int page, string searchText, int searchType, AdvanceSearchParameter advanceSearch,
            int isFirst = 0, int searchProductId = 0, string tenderYear = "", int TenderBy = 0, bool isSearchWithCount = true, int? orderByType = 0,
           List<string> WithinSearchWords = null)
        {
            var withinSearchedTexts = "";
            var displayLastWithinSearchKeyword = "";

            var DisplayText = "";
            DisplayText = searchText == null ? "" : searchText.ToString();
            if (WithinSearchWords != null)
            {

                if (WithinSearchWords.Any())
                {
                    withinSearchedTexts = string.Join(",", WithinSearchWords);
                    displayLastWithinSearchKeyword = WithinSearchWords.LastOrDefault().ToString();
                }
            }


            string DisplaySearchTextDetail = "";
            page = page == 1 ? 0 : page;

            //Session["AdvanceSearchparams"] = advanceSearch;

            var mainSearchText = "";
            //var advanceSearchText = "";
            int pageSize = PageSize;

            DateTime? subDateFromDt = null;
            DateTime? subDateToDt = null;
            DateTime? opDateFromDt = null;
            DateTime? opDateToDt = null;

            long totalLive = 0;
            long totalFresh = 0;
            long totalClosed = 0;
            long totalCount = 0;
            long totalAll = 0;

            if (!string.IsNullOrEmpty(DisplayText))
            {
                var sType = "";
                switch (searchType)
                {
                    case 1:
                        sType = "Search By Keyword";
                        break;
                    case 2:
                        sType = "Search By Word";
                        break;
                    case 3:
                        sType = "Search By Exact Phrase";
                        break;
                }
                mainSearchText = sType + " : ";
                mainSearchText += "<b><span class='title'>" + DisplayText + " </span></b>";

                if (searchProductId != 0)
                {
                    DisplaySearchTextDetail = "Get complete Tenders information related to latest <span style=\"color:red;\"><b>" + DisplayText + "</b></span> from all over India at <a href=\"www.tenderassist247.com\">www.tenderassist247.com</a>. Search the best Live tenders from Indian government tenders,  Indian Public Sector Tenders, Indian Private Sector tenders, Indian online tenders, tender invitation notice, business tender notices, E tenders and bidding and Procurement Tenders.";
                }
            }


            var tsf = 0;
            string closedTenderTitle;
            //Session["TenderYear"] = null;
            string advSearchTxt = "";


            var allstateId = "";
            var allcityid = "";
            var allproductId = "";
            var allindustryId = "";
            var allsubindustryid = "";
            var allagencyId = "";
            var allsectorId = "";
            var allownershipId = "";
            string locationIds = "";
            string indSubindIds = "";


            #region SET ADVANCE SEARCH PARAMETERS

            var isAnyDataFound = false;

            #region LOCATIONS [STATE & CITY]

            locationIds = advanceSearch.SelectedLocations;


            var setKeywordState = "";
            var setKeywordCity = "";

            if (locationIds == null) { locationIds = ""; }
            var locationIdList = locationIds.Split(',');
            //var searchFilterState = "";
            //var searchFilterCity = "";

            //var isFirstState = true;
            //var isFirstCity = true;

            var allStatesAvail = new List<int>();
            var allCitiesAvail = new List<int>();

            foreach (var lstItem in locationIdList)
            {
                var isCitySelected = false;
                if (lstItem.Trim() == "")
                {
                    continue;
                }

                var locations = lstItem.Split('#');
                var cityid = Convert.ToInt32(locations[0]);
                var stateId = Convert.ToInt32(locations[1]);

                if (allCitiesAvail.Contains(cityid) == false && cityid != 0)
                {
                    isCitySelected = true;
                    allCitiesAvail.Add(cityid);
                    allcityid = allcityid == ""
                        ? cityid.ToString(CultureInfo.InvariantCulture)
                        : allcityid + "," + cityid;

                }

                if (!isCitySelected)
                {
                    if (allStatesAvail.Contains(stateId) == false && stateId != 0)
                    {
                        allStatesAvail.Add(stateId);
                        allstateId = allstateId == ""
                            ? stateId.ToString(CultureInfo.InvariantCulture)
                            : allstateId + "," + stateId;

                    }
                }

            }
            #endregion

            #region CATEGORIES [INDUSTRY & SUBINDUSTRY]

            indSubindIds = advanceSearch.SelectedIndsubIndustries;

            if (indSubindIds == null) { indSubindIds = ""; }
            var indsubindIdList = indSubindIds.Split(',');

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

                }

                if (allSubIndustryAvail.Contains(subindId) == false && subindId != 0)
                {
                    allSubIndustryAvail.Add(subindId);
                    allsubindustryid = allsubindustryid == ""
                        ? subindId.ToString(CultureInfo.InvariantCulture)
                        : allsubindustryid + "," + subindId;

                }
            }

            #endregion

            #region PRODUCT
            string productIds = advanceSearch.SelectedProducts;


            var productId = 0;
            //var searchFilterProduct = "";
            //var isFirstProduct = true;
            //var setKeywordProducts = "";
            //var setKeywordProductName = "";

            if (!string.IsNullOrEmpty(productIds))
            {
                var allselectedprods = Regex.Split(productIds, ",");
                foreach (var t in allselectedprods)
                {
                    if (t == "") continue;
                    if (productId == Convert.ToInt32(t)) continue;

                    productId = Convert.ToInt32(t);
                    if (productId == 0) continue;

                    allproductId = (allproductId == "")
                        ? productId.ToString(CultureInfo.InvariantCulture)
                        : allproductId + "," + productId;

                }
            }

            #endregion

            #region AGENCY

            var agencyIds = advanceSearch.SelectedAgencies;


            var agencyId = 0;
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

                }
            }

            #endregion

            #region SECTOR

            var sectorIds = advanceSearch.SelectedSectors;

            var sectorId = 0;
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

                }
            }

            #endregion

            #region OWNERSHIP

            var ownershipIds = advanceSearch.SelectedOwnerships;

            var ownershipId = 0;
            var searchFilterOwnership = "";

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

                }
            }

            #endregion

            #region INFORMATION SOURCE

            var refIds = advanceSearch.RefIds;

            //var searchFilterInfoSource = "";
            if (!string.IsNullOrEmpty(refIds))
            {
                var refId = Convert.ToInt32(refIds);
                isAnyDataFound = true;

            }


            #endregion

            #region Tender Value
            int? tenderValFlag = advanceSearch.TenderValFlag != null ? Convert.ToInt32((object)advanceSearch.TenderValFlag) : 0;

            #endregion

            #region Reference No
            int? ourRefNo = advanceSearch.OurRefNo != null ? Convert.ToInt32((object)advanceSearch.OurRefNo) : 0;

            #endregion

            #region Tender Type
            int? tenderType = null;
            if (advanceSearch.TenderTypeId != null)
            { advanceSearch.TenderTypeId = Convert.ToInt32((object)advanceSearch.TenderTypeId); }

            #endregion

            #region Tender Status

            int? tenderStatusFlag = tsf = advanceSearch.TenderStatusFlag != null ? Convert.ToInt32((object)advanceSearch.TenderStatusFlag) : 0;

            if (tenderStatusFlag != 0)
            {
                isAnyDataFound = true;
                string tenderStatusText;
                switch (tenderStatusFlag)
                {
                    case (Int16)TenderStatusFlags.NewTenders:
                        tenderStatusText = TenderStatusValues.NewTenders;
                        break;
                    case (Int16)TenderStatusFlags.LiveTenders:
                        tenderStatusText = TenderStatusValues.LiveTenders;
                        break;
                    case (Int16)TenderStatusFlags.ClosedTenders:
                        tenderStatusText = TenderStatusValues.ClosedTenders;
                        break;
                    case (Int16)TenderStatusFlags.AllTenders:
                        tenderStatusText = TenderStatusValues.AllTenders;
                        break;
                    default:
                        tenderStatusText = TenderStatusValues.AllTenders;
                        break;
                }

            }

            #endregion

            #region Submission Date
            var subDateFrom = advanceSearch.SubDateFrom;
            var subDateTo = advanceSearch.SubDateTo;
            if (!string.IsNullOrEmpty(subDateFrom))
            {
                subDateFromDt = Convert.ToDateTime(subDateFrom);
            }
            if (!string.IsNullOrEmpty(subDateTo))
            {
                subDateToDt = Convert.ToDateTime(subDateTo);
            }

            #endregion

            #region Opening Date

            var opDateFrom = advanceSearch.OpDateFrom;
            var opDateTo = advanceSearch.OpDateTo;
            if (!string.IsNullOrEmpty(opDateFrom))
            {
                opDateFromDt = Convert.ToDateTime(opDateFrom);
            }
            if (!string.IsNullOrEmpty(opDateTo))
            {
                opDateToDt = Convert.ToDateTime(opDateTo);
            }

            #endregion

            int? icbNcb = advanceSearch.IcbNcb;

            #endregion

            tenderYear = string.IsNullOrEmpty(tenderYear) ? "" : tenderYear;
            //Session["TenderYear"] = tenderYear;

            if (tenderYear != "0" && tenderYear != "")
            { closedTenderTitle = tenderYear + " Closed Tender"; }
            else
            { closedTenderTitle = ""; }

            productIds = productIds == "0" ? "" : productIds;

            bool? isIcbNcb = null;
            if (icbNcb != -1)
            {
                isIcbNcb = icbNcb == 1 ? true : false;
            }

            var tStatusFlag = tenderStatusFlag == null ? 0 : tenderStatusFlag.Value;

            //if (Session["TotalSearchedTenders"] == null)
            //{ isSearchWithCount = true; }

            var tenaderList = this.GetAllSearchIndianTenderList(TenderBy, searchType, searchText, productIds, locationIds, indSubindIds, agencyIds, sectorIds, ownershipIds, refIds,
                tenderStatusFlag == 0 ? null : tenderStatusFlag, page, pageSize, isSearchWithCount, tenderYear, tenderType, tenderValFlag, subDateFromDt, subDateToDt, opDateFromDt,
                opDateToDt, ourRefNo == 0 ? null : ourRefNo, isIcbNcb, "", withinSearchedTexts, orderByType);

            List<SearchTenaderInfoWithAllDetail> tenaderInfoWithDetail = tenaderList.TenaderDetailSearch;
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
                //totalLive += totalFresh;

                //Session["TotalAllTenders"] = totalAll.ToString(CultureInfo.InvariantCulture);
                //Session["TotalSearchedTenders"] = totalCount.ToString(CultureInfo.InvariantCulture);
                //Session["TotalLiveTenders"] = totalLive.ToString(CultureInfo.InvariantCulture);
                //Session["TotalFreshTenders"] = totalFresh.ToString(CultureInfo.InvariantCulture);
                //Session["TotalClosedTenders"] = totalClosed.ToString(CultureInfo.InvariantCulture);
            }

            //var total = Convert.ToInt64(Session["TotalSearchedTenders"]);
            //totalAll = Convert.ToInt64(Session["TotalAllTenders"]);
            //totalLive = Convert.ToInt64(Session["TotalLiveTenders"]);
            //totalFresh = Convert.ToInt64(Session["TotalFreshTenders"]);
            //totalClosed = Convert.ToInt64(Session["TotalClosedTenders"]);


            var shoecurrentpage = (page / PageSize) + 1;
            //Session["searchProductID"] = searchProductId.ToString(CultureInfo.InvariantCulture);
            var tenderDetail = new TenderDetail
            {
                AllSearchTenaderInfoWithAllDetail = tenaderInfoWithDetail,

                SearchType = searchType,
                SearchProuctId = searchProductId,

                DisplayText = DisplayText,
                DisplayText2 = withinSearchedTexts,
                DisplayText3 = displayLastWithinSearchKeyword,
                AdvanceSearchPara = advanceSearch,
                ClosedTenderTitle = closedTenderTitle,
                DisplaySearchTextDetail = DisplaySearchTextDetail,

                TenderStatusFlag = tsf,
                TenderStatus = tsf,
                TenderYear = tenderYear,

                DisplayCurrentPage = shoecurrentpage,
                PageSize = pageSize,
                Newpagecnt = page,

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

                TotalLive = totalLive,
                TotalFresh = totalFresh,
                TotalClosed = totalClosed,
                Total = totalAll,

                OrderBy = orderByType == null ? 0 : orderByType.Value
            };

            return tenderDetail;
        }

        public TenderDetailInfo GetAllSearchIndianTenderList(int tenderBy, int searchType, string searchText,
             string productIds = "", string locationIds = "", string indSubindIds = "", string agencyIds = "", string sectorIds = "", string ownershipIds = "", string refIds = "",
              int? tenderStatus = null, int start = 0, int limit = 20, bool isSearchWithCount = true, string tenderYear = null, int? tenderType = null,
              int? TenderValueType = null, DateTime? subDateFrom = null, DateTime? subDateTo = null, DateTime? opDateFrom = null, DateTime? opDateTo = null,
              int? ourRefNo = null, bool? icbNcb = null, string OtherSearchText = null, string WithinSearchText = null, int? orderByType = null)
        {
            return _tenderInfo.GetAllSearchIndianTenderList(tenderBy, searchType, searchText, productIds, locationIds, indSubindIds, agencyIds, sectorIds, ownershipIds, refIds,
                 tenderStatus, start, limit, isSearchWithCount, tenderYear, tenderType, TenderValueType, subDateFrom, subDateTo, opDateFrom, opDateTo, ourRefNo, icbNcb, OtherSearchText,
                 WithinSearchText, orderByType);
        }

        public TenderDetails GetTenderInfoById(string refno, string tenderYear)
        {
            var tenderdet = new TenderDetails();
            int refNo = refno == "" ? 0 : Convert.ToInt32(refno);
            var currentYert = DateTime.Now.Year;// Convert.ToInt32(ConfigurationManager.AppSettings["LatestTenderYear"].ToString());
            int tenderYears;
            if (tenderYear == "")
                tenderYears = currentYert;
            else
            {
                tenderYears = Convert.ToInt32(tenderYear);
                if (tenderYears > currentYert)
                    tenderYears = currentYert;
            }
            tenderdet = _tenderInfo.GetTenderInfoById(refNo, tenderYears.ToString());
            return tenderdet;

        }


        #region INDIAN CATEGORY
        public List<StateCityForIndianTenders> GetStateCityForIndianTenders(string searchText)
        {
            return _tenderInfo.GetStateCityForIndianTenders(searchText);

        }
        public List<tbProduct> GetProductsForIndianTenders(string searchText, string wordSearch)
        {
            return _tenderInfo.GetProductsForIndianTenders(searchText, wordSearch);
        }
        public List<IndustrySubIndustryDetail> GetIndustrySubIndustryList(string searchText)
        {
            return _tenderInfo.GetIndustrySubIndustryList(searchText);
        }
        public List<AgencySectorOwhershipDetail> GetAgencySectorOwnershipForIndianTenders(string searchText)
        {
            return _tenderInfo.GetAgencySectorOwnershipForIndianTenders(searchText);
        }
        //public List<tbSector> GetSectorForIndianTenders(string searchText)
        //{
        //    return _tenderInfo.GetSectorForIndianTenders(searchText);
        //}
        //public List<tbOwnership> GetOwnershipForIndianTenders(string searchText)
        //{
        //    return _tenderInfo.GetOwnershipForIndianTenders(searchText);
        //}
        #endregion


        /*GLOBAL*/
        public TenderDetail GetSearchGlobalTenderResult(int regionId, int page, string searchText, int searchType, AdvanceSearchParameter advanceSearch,
            int searchProductId = 0, bool isSearchWithCount = true, int? orderByType = 0, List<string> WithinSearchWords = null, string OtherSearchText = "")
        {
            var DisplayText = searchText.ToString();
            var withinSearchedTexts = "";
            var displayLastWithinSearchKeyword = "";

            if (WithinSearchWords != null)
            {

                if (WithinSearchWords.Any())
                {
                    withinSearchedTexts = string.Join(", ", WithinSearchWords);
                    displayLastWithinSearchKeyword = WithinSearchWords.LastOrDefault().ToString();
                }
            }

            string DisplaySearchTextDetail = "";
            page = page == 1 ? 0 : page;

            string productIds = advanceSearch.SelectedProducts;
            string agencyIds = advanceSearch.SelectedAgencies;
            string countryIds = advanceSearch.SelectedCountries;

            countryIds = countryIds == "0" ? "" : countryIds;

            DateTime? subDateFromDt = null;
            DateTime? subDateToDt = null;
            DateTime? opDateFromDt = null;
            DateTime? opDateToDt = null;

            var mainSearchText = "";
            int pageSize = PageSize;

            long totalLive = 0;
            long totalFresh = 0;
            long totalClosed = 0;
            long totalCount = 0;
            long totalAll = 0;

            if (!string.IsNullOrEmpty(DisplayText))
            {
                var sType = "";
                switch (searchType)
                {
                    case 1:
                        sType = "Search By Keyword";
                        break;
                    case 2:
                        sType = "Search By Word";
                        break;
                    case 3:
                        sType = "Search By Exact Phrase";
                        break;
                }
                mainSearchText = sType + " : ";
                mainSearchText += "<b><span class='title'>" + DisplayText + " </span></b>";

                if (searchProductId != 0)
                {
                    DisplaySearchTextDetail = "Get complete Tenders information related to latest <span style=\"color:red;\"><b>" + DisplayText + "</b></span> from all over India at <a href=\"www.tenderassist247.com\">www.tenderassist247.com</a>. Search the best Live tenders from Indian government tenders,  Indian Public Sector Tenders, Indian Private Sector tenders, Indian online tenders, tender invitation notice, business tender notices, E tenders and bidding and Procurement Tenders.";
                }
            }

            var tsf = 0;

            var allCountry = "";
            var countryId = 0;

            if (regionId != 0 && string.IsNullOrEmpty(countryIds))
            {
                var countrylist = _tenderInfo.ListCountrybyRegion(regionId).Select(x => x.CountryId).ToList();
                if (countrylist.Any())
                {
                    allCountry = String.Join(",", countrylist.ToArray());
                    countryIds = allCountry;
                }
            }

            if (!string.IsNullOrEmpty(countryIds))
            {
                var allselectedcountries = Regex.Split(countryIds, ",");
                foreach (var t in allselectedcountries)
                {
                    if (t == "") continue;
                    if (countryId == Convert.ToInt32(t)) continue;

                    countryId = Convert.ToInt32(t);
                    if (countryId == 0) continue;

                    allCountry = (allCountry == "")
                        ? countryId.ToString(CultureInfo.InvariantCulture)
                        : allCountry + "," + countryId;
                }
            }

            #region Reference No
            int? ourRefNo = advanceSearch.OurRefNo != null ? Convert.ToInt32((object)advanceSearch.OurRefNo) : 0;

            #endregion

            #region Tender Type
            int? tenderType = null;
            if (advanceSearch.TenderTypeId != null)
            { advanceSearch.TenderTypeId = Convert.ToInt32(advanceSearch.TenderTypeId); }

            #endregion

            #region Tender Status

            int? tenderStatusFlag = tsf = advanceSearch.TenderStatusFlag != null ? Convert.ToInt32((object)advanceSearch.TenderStatusFlag) : 0;

            if (tenderStatusFlag != 0)
            {
                string tenderStatusText;
                switch (tenderStatusFlag)
                {
                    case (Int16)TenderStatusFlags.NewTenders:
                        tenderStatusText = TenderStatusValues.NewTenders;
                        break;
                    case (Int16)TenderStatusFlags.LiveTenders:
                        tenderStatusText = TenderStatusValues.LiveTenders;
                        break;
                    case (Int16)TenderStatusFlags.ClosedTenders:
                        tenderStatusText = TenderStatusValues.ClosedTenders;
                        break;
                    case (Int16)TenderStatusFlags.AllTenders:
                        tenderStatusText = TenderStatusValues.AllTenders;
                        break;
                    default:
                        tenderStatusText = TenderStatusValues.AllTenders;
                        break;
                }
            }

            #endregion

            #region Submission Date
            var subDateFrom = advanceSearch.SubDateFrom;
            var subDateTo = advanceSearch.SubDateTo;
            if (!string.IsNullOrEmpty(subDateFrom))
            {
                subDateFromDt = Convert.ToDateTime(subDateFrom);
            }
            if (!string.IsNullOrEmpty(subDateTo))
            {
                subDateToDt = Convert.ToDateTime(subDateTo);
            }

            #endregion

            #region Opening Date

            var opDateFrom = advanceSearch.OpDateFrom;
            var opDateTo = advanceSearch.OpDateTo;
            if (!string.IsNullOrEmpty(opDateFrom))
            {
                opDateFromDt = Convert.ToDateTime(opDateFrom);
            }
            if (!string.IsNullOrEmpty(opDateTo))
            {
                opDateToDt = Convert.ToDateTime(opDateTo);
            }

            #endregion


            var tStatusFlag = tenderStatusFlag == null ? 0 : tenderStatusFlag.Value;

            var tenaderList = this.GetAllSearchGlobalTenderList(searchType, searchText, countryIds, tenderStatusFlag == 0 ? null : tenderStatusFlag, page, pageSize,
                isSearchWithCount, productIds, agencyIds, tenderType, subDateFromDt, subDateToDt, opDateFromDt, opDateToDt, ourRefNo, withinSearchedTexts, "");

            List<SearchTenaderInfoWithAllDetail> tenaderInfoWithDetail = tenaderList.TenaderDetailSearch;
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

            var shoecurrentpage = (page / PageSize) + 1;
            var tenderDetail = new TenderDetail
            {
                AllSearchTenaderInfoWithAllDetail = tenaderInfoWithDetail,
                AdvanceSearchPara = advanceSearch,
                SearchType = searchType,
                SearchProuctId = searchProductId,
                DisplayText = DisplayText,
                DisplayText2 = withinSearchedTexts,
                DisplaySearchTextDetail = DisplaySearchTextDetail,

                TenderStatusFlag = tsf,
                TenderStatus = tsf,
                RegionId = regionId,

                DisplayCurrentPage = shoecurrentpage,
                PageSize = pageSize,
                Newpagecnt = page,

                SelectedCountry = countryIds ?? "",
                SelectedProduct = productIds ?? "",
                SelectedAgency = agencyIds ?? "",

                TotalLive = totalLive,
                TotalFresh = totalFresh,
                TotalClosed = totalClosed,
                Total = totalAll,
                IsGlobalTender = true
            };

            return tenderDetail;
        }


        public TenderDetailInfo GetAllSearchGlobalTenderList(int searchType, string searchText, string countryIds = "", int? tenderStatus = null, int start = 0, int limit = 20, bool isSearchWithCount = true,
             string productIds = "", string agencyIds = "", int? tenderType = null,
            DateTime? subDateFrom = null, DateTime? subDateTo = null, DateTime? opDateFrom = null, DateTime? opDateTo = null,
            int? ourRefNo = null, string WithinSearchText = null, string OtherSearchText = null)
        {
            return _tenderInfo.GetAllSearchGlobalTenderList(searchType, searchText, countryIds, tenderStatus, start, limit, isSearchWithCount,
                 productIds, agencyIds, tenderType, subDateFrom, subDateTo, opDateFrom, opDateTo, ourRefNo, WithinSearchText, OtherSearchText);
        }

        public TenderDetails GetGlobalTenderInfoById(string refno)
        {
            TenderDetails tenderdet = _tenderInfo.GetGlobalTenderInfoById(refno);
            return tenderdet;

        }

        public tabClientDetail GetUsersByUniqueId(Guid userUniqueNo)
        {
            return _tenderInfo.GetUsersByUniqueId(userUniqueNo);
        }

        #region AUTO COMPLETE
        public ActionResult AutocompleteSearchKeywords(string term)
        {
            if (term.Trim().Length > 1)
            {
                var op = term.LastIndexOf(",", StringComparison.Ordinal);
                term = term.Substring(op + 1);
            }

            var searchText = term.Trim().ToLower();
            var maincat = _tenderInfo.SearchAutoCompleteKeywords(searchText).ToArray();
            return Json(maincat, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public string AmountToWords(string totalamount)
        {
            //BEGIN
            totalamount = totalamount.Split('.')[0];
            string amountword = "0";
            int length = totalamount.Length;
            if ((length - 4) < 0)
            { amountword = totalamount.ToString(); return amountword; }
            switch (length - 4)
            {
                case 0:
                    string str2 = totalamount.Substring(1, 1).ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    amountword = totalamount.Substring(0, 1) + (object)'.' + str2 + " Thousand".ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    break;
                case 1:
                    amountword = totalamount.Substring(0, 2) + (object)'.' + totalamount.Substring(2, 1) + " Thousand".ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    break;
                case 2:
                    string str3 = totalamount.Substring(1, 2).ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    amountword = totalamount.Substring(0, 1) + (object)'.' + str3 + " Lacs".ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    break;
                case 3:
                    amountword = totalamount.Substring(0, 2) + (object)'.' + totalamount.Substring(2, 2) + " Lacs".ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    break;
                case 4:
                    string str4 = totalamount.Substring(1, 2).ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    amountword = totalamount.Substring(0, 1) + (object)'.' + str4 + " Crore".ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    break;
                case 5:
                    amountword = totalamount.Substring(0, 2) + (object)'.' + totalamount.Substring(2, 2) + " Crore".ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    break;
                default:
                    if (length >= 10)
                    {
                        int num = length - 10 + 3;
                        amountword = totalamount.Substring(0, num) + (object)'.' + totalamount.Substring(num, 2) + " Crore".ToString((IFormatProvider)CultureInfo.InvariantCulture);
                        break;
                    }
                    break;
            }
            return amountword;
        }
        public string Highlightsearchtext(string text, string keywords, int sarchType, string cssClass = "text-red-selected")
        {
            var fullMatch = sarchType != 1;
            string returndata;
            var words = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length >= 2).ToArray();

            if (!fullMatch)
            {
                returndata = words.Select(word => word.ToLower().Trim()).Aggregate(text.ToLower().Trim(), (current, pattern) => Regex.Replace(current, pattern, string.Format("<span class=\"{0}\">{1}</span>", cssClass, "$0"), RegexOptions.IgnorePatternWhitespace));
            }
            else
            {
                returndata = words.Select(word => "\\b" + word.ToLower().Trim() + "\\b").Aggregate(text.ToLower(), (current, pattern) => Regex.Replace(current, pattern, string.Format("<span class=\"{0}\">{1}</span>", cssClass, "$0"), RegexOptions.IgnorePatternWhitespace));
            }
            return returndata;
            //var fullMatch = sarchType != 1;
            //string returndata;
            //var words = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //if (!fullMatch)
            //{
            //    returndata = words.Select(word => word.ToLower().Trim()).Aggregate(text.ToLower().Trim(),
            //                 (current, pattern) =>
            //                 Regex.Replace(current, pattern, string.Format("<span class=\"{0}\">{1}</span>", cssClass, "$0"), RegexOptions.IgnoreCase));
            //}
            //else
            //{
            //    returndata = words.Select(word => "\\b" + word.ToLower().Trim() + "\\b").Aggregate(text.ToLower().Trim(),
            //                (current, pattern) =>
            //                Regex.Replace(current, pattern, string.Format("<span class=\"{0}\">{1}</span>", cssClass, "$0"), RegexOptions.IgnoreCase));
            //}
            //return returndata;



        }


        public int SubmitInquiryRegForms(InquiryRegFormFields RegFormParams)
        {
            var result = Convert.ToInt32(_tenderInfo.SubmitInquiryRegForms(RegFormParams));
            return result;
        }

        public string SubscribNowMailFormat()
        {
            string emailFormatFileName = GetEmailFormatFileName(1, 0);
            string str = string.Empty;
            using (StreamReader streamReader = new StreamReader(emailFormatFileName))
                str = streamReader.ReadToEnd();

            return this.FormatEmailContent()
                .Replace("{{EmailBody}}",
                str.Replace("{{SiteName}}", this.sitename));
        }
        public string SubscribNowMailFormat_Admin()
        {
            string emailFormatFileName = GetEmailFormatFileName(2, 0);
            string str = string.Empty;
            using (StreamReader streamReader = new StreamReader(emailFormatFileName))
                str = streamReader.ReadToEnd();
            return str;
        }

        private string salesmailtoLink = "mailto:" + ConfigurationManager.AppSettings["SalesEMailToLink"].ToString() + "?Subject=Quick%20Contact";
        private string salesemailId = ConfigurationManager.AppSettings["SalesEMailId"].ToString();
        public string url = ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
        private string imagesrc = ConfigurationManager.AppSettings["ApplicationUrl"].ToString() + "Images/logo.png";
        private string appName = ConfigurationManager.AppSettings["ProjectName"].ToString();
        private string caremailtoLink = "mailto:" + ConfigurationManager.AppSettings["CareEMailToLink"].ToString();
        private string careemailId = ConfigurationManager.AppSettings["CareEMailId"].ToString();
        private string sitename = ConfigurationManager.AppSettings["SiteName"].ToString();

        public string FormatEmailContent()
        {
            string emailFormatFileName = GetEmailFormatFileName(0, 0);
            string str = string.Empty;

            using (StreamReader streamReader = new StreamReader(emailFormatFileName))
                str = streamReader.ReadToEnd();

            return str.Replace("{{SalesMailToLink}}", this.salesmailtoLink)
                .Replace("{{SalesEmailId}}", this.salesemailId)
                .Replace("{{SiteUrl}}", this.url)
                .Replace("{{ImagePath}}", this.imagesrc)
                .Replace("{{Application}}", this.appName)
                .Replace("{{CareEMailIdLink}}", this.caremailtoLink)
                .Replace("{{CareEMailId}}", this.careemailId)
                .Replace("{{SiteName}}", this.sitename);
        }

        public string GetEmailFormatFileName(int type, int moduleType = 0)
        {
            string filepath = "";
            string filename = "";
            switch (moduleType)
            {
                default:
                    switch (type)
                    {
                        case 0:
                            filename = "MainEmailFormat.html";
                            break;
                        case 1:
                            filename = "SubscribNowEmailFormat.html";
                            break;
                        case 2:
                            filename = "Admin_NewSubscribtionEMailFormat.html";
                            break;
                        case 3:
                            filename = "SampleTenderEmailFormat.html";
                            break;
                        case 4:
                            filename = "SampleTenderDetailEmailFormat.html";
                            break;
                    }
                    break;

            }

            filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/HTMLEmailFormats/" + filename + "");

            return filepath;
        }




        /*USER*/
        public string MailUpdateUserInformation(string userFormFields, bool isUser)
        {
            var data = Regex.Split(userFormFields, "#-#");

            var sendMail = "";

            var username = data[0] + " " + data[1];
            if (isUser) //FOR USER
            {
                sendMail += @"<span style='font-size: 14px; color: #FF6600;'>You have requested to update your Information.</span>
                                <br />
                                <span style='font-size: 14px; color: #FF6600;'>Your new information as you have requested, are below:</span>
                                <br /><br />" + UserInformation(data) +
                                @"<div style='clear: both; height: 0; line-height: 0; font-size: 0; overflow: hidden; visibility: hidden;'></div>
                                <br />
                                <span style='font-size: 14px; color: #FF6600;'>Our team will be update your information within 24 hours. Please feel free to ask us if you have any query.</span>
                                <br />
                                <span style='font-size: 14px; color: #FF6600;'>Thanks for the request.</span>";
            }
            else //FOR ADMIN
            {
                sendMail += @"<span style='font-size: 14px; color: #FF6600;'>Our client, <b>" + username + "</b>, has requested to update the Information.</span>" +
                            @"<br />
                            <span style='font-size: 14px; color: #FF6600;'>Please update the information of the client, <b>" + username + "</b>, as below:</span><br /><br />" +
                            UserInformation(data) +
                            @"<div style='clear: both; height: 0; line-height: 0; font-size: 0; overflow: hidden; visibility: hidden;'></div>
                            <br />
                            <span style='font-size: 14px; color: #FF6600;'>Login credentials of the client, <b>" + username + "</b>, are as below.</span>" +
                            @"<br />
                            <table width='740px' border='0' align='left' cellpadding='0' cellspacing='0' style='border: 1px solid #dcdcdc;'>
                                <tr style='height: 25px;'>
                                    <td style='width: 150px; text-align: right;'>
                                        <span style='margin: 5px; font-size: 13px; color: #4274b1; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: left;'>LoginID :</span>
                                    </td>
                                    <td style='width: auto; margin-left: 10px;'>
                                        <span style='font-size: 13px; color: #4B4B4B; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: right;'>" + data[11] + @"</span>
                                    </td>
                                </tr>
                                <tr style='height: 25px;'>
                                    <td style='width: 150px; text-align: right;'>
                                        <span style='margin: 5px; font-size: 13px; color: #4274b1; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: left;'>Password :</span>
                                    </td>
                                    <td style='width: auto; margin-left: 10px;'>
                                        <span style='font-size: 13px; color: #4B4B4B; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: right;'>" + data[12] + @"</span>
                                    </td>
                                </tr>
                            </table>";
            }
            return sendMail;
        }
        private static string UserInformation(IList<string> data)
        {
            const string trstyle = "height: 25px;";
            const string ltdstyle = "width: 150px; text-align: right;";
            const string rtdstyle = "width: auto; margin-left: 10px;";
            const string lspanstyle = "margin: 5px; font-size: 13px; color: #4274b1; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: left;";
            const string rspanstyle = "font-size: 13px; color: #4B4B4B; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: right;";

            var setuserdata =
                @"<table width='740px' border='0' align='left' cellpadding='0' cellspacing='0' style='border: 1px solid #dcdcdc;'>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                             @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>First Name :</span></td>" +
                             @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[0] + "</span></td>" +
                         @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                             @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Last Name :</span></td>" +
                             @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[1] + "</span></td>" +
                         @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                             @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Company Name : </span></td>" +
                             @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[2] + "</span></td>" +
                        @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Designation : </span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[3] + "</span></td>" +
                        @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Address : </span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[4] + "</span></td>" +
                        @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Country : </span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[5] + "</span></td>" +
                        @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>State : </span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[6] + "</span></td>" +
                        @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>City : </span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[7] + "</span></td>" +
                        @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>PIN Code : </span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[8] + "</span></td>" +
                        @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Phone No : </span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[9] + "</span></td>" +
                        @"</tr>";
            setuserdata += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Email ID : </span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[10] + "</span></td>" +
                        @"</tr>";
            setuserdata += "</table>";
            return setuserdata;
        }
        public string CreateMail(int intClientId, string strClientName, string mailtitle, string head, string body, string sitemailid, string contactno, string locallive)
        {
            string logo = "";
            logo = "<img src='" + imagesrc + "' width='186px' />";

            var mailtositemailid = "mailto://" + sitemailid;

            var mailFormat = @"
<div style='width:750px;border:1px solid #4B4B4B;border-radius:3px;margin:4px 15px;float:left;'>
    <table border='0' style='font-family:Calibri;font-weight:bold;font-size:13px;color:#4B4B4B;width:100%;'>
        <tr>
            <td style='background-color:#037ca5;'>
		        <a href='" + url + "' Target='_blank'>" + logo + @"
                </a>
            </td>
		</tr>
        <tr>
			<td>
				<span><b>Dear Concerned, " + strClientName + @" </b>
                </span>
			</td>
		</tr>
        <tr>
			<td>
				<span>Welcome to 
                    <a style='color: #0572AF;' href='" + url + "' Target='_blank'>" + sitename + @"
                    </a>
            </span>
			</td>
		</tr>";

            #region titlehead

            if (mailtitle != "")
            {
                mailFormat += @"
        <tr>
			<td>
				<span>
                    " + mailtitle + @"
                </span>
			</td>
		</tr>";
            }

            if (head != "")
            {
                mailFormat += @"
        <tr>
			<td>
				<span>
					<label style='color: #037ca5;'><b>" + head + @"</b>
                    </label>
				</span>
			</td>
		</tr>";
            }

            #endregion

            mailFormat += @"        
        <tr>
			<td>
				<br />
                    " + body + @"
                <br /><br />
            </td>
		</tr>
        <tr>
			<td>
				 <span>
                    For any further assistance, email us at 
			        <a style='color: #0572AF;' href='" + mailtositemailid + "'>" + sitemailid + @"
                    </a> or contact " + contactno + @" for more details.
		        </span>
			</td>
		</tr>
        <tr>
			<td>
				<span>
                    <b>Warm Regards, </b>
                    <a style='color: #0572AF;' href='http://" + sitename + "' Target='_blank'>" + sitename + @"
                    </a>
                </span>
			</td>
		</tr>
        <tr>
			<td>
				<span style='font-size: 20px; font-family:Calibri; color: black; font-weight: bold; margin: 4px; float: left;'>
                    Help Desk
                </span>
			</td>
		</tr>
        <tr>
			<td>
				<span>
					<label style='color: #037ca5;'><b>Head Office : </b></label>D10/11, Aviskar Complex, 
				</span>
			</td>
		</tr>
        <tr>
			<td>
				<span> Behind Millenium Hotel, Motipura
				</span>
			</td>
		</tr>
		<tr>
			<td>
				<span> Himmatnagar, Gujarat - 383001. Gujarat.
				</span>
			</td>
		</tr>
		<tr>
			<td>
				<span>Contact No : " + contactno + @"
				</span>
			</td>
		</tr>
		<tr>
			<td>
				<span>Email : 
                    <a style='color: #037ca5; font-weight: bold;' href='" + mailtositemailid + "'>" + sitemailid + @"
                    </a>
                </span>
			</td>
		</tr>
    </table>
</div>";
            return mailFormat;
        }
        public string MailUpdateUserPasswrod(string userFormFields, bool isUser)
        {
            string[] data = Regex.Split(userFormFields, "#-#");

            string sendMail = "";

            var username = data[0];

            if (isUser) //FOR USER
            {
                sendMail += @"<span style='font-size: 14px; color: #FF6600;'>Based on your request, we have updated your Password.</span><br />
                              <span style='font-size: 14px; color: #FF6600;'>Your new Login credentials are as below:</span>
                              <br /><br />";
            }
            else //FOR ADMIN
            {
                sendMail += @"<span style='font-size: 14px; color: #FF6600;'>Our client, <b>" + username + "</b>, has updated the Login credentials.</span><br />" +
                            @"<span style='font-size: 14px; color: #FF6600;'>Please find the updated Login credentials for the client, <b>" +
                            username + "</b>, as below:</span><br /><br />";
            }

            sendMail += @"<table width='740px' border='0' align='left' cellpadding='0' cellspacing='0' style='border: 1px solid #dcdcdc;'>
                            <tr style='height: 25px;'>
                                <td style='width: 150px; text-align: right;'>
                                    <span style='margin: 5px; font-size: 13px; color: #4274b1; font-family: Arial, Helvetica, sans-serif;
                                        font-weight: bold; text-align: left;'>LoginID :</span>
                                </td>
                                <td style='width: auto; margin-left: 10px;'>
                                    <span style='font-size: 13px; color: #4B4B4B; font-family: Arial, Helvetica, sans-serif;
                                        font-weight: bold; text-align: right;'>" + data[1] + @"</span>
                                </td>
                            </tr>
                            <tr style='height: 25px;'>
                                <td style='width: 150px; text-align: right;'>
                                    <span style='margin: 5px; font-size: 13px; color: #4274b1; font-family: Arial, Helvetica, sans-serif;
                                        font-weight: bold; text-align: left;'>Password :</span>
                                </td>
                                <td style='width: auto; margin-left: 10px;'>
                                    <span style='font-size: 13px; color: #4B4B4B; font-family: Arial, Helvetica, sans-serif;
                                        font-weight: bold; text-align: right;'>" + data[2] + @"</span>
                                </td>
                            </tr>
                        </table>";
            return sendMail;
        }
        public string MailSendFeedback(string userFormFields)
        {
            string[] data = Regex.Split(userFormFields, "#-#");

            const string trstyle = "height: 25px;";
            const string ltdstyle = "width: 150px; text-align: right;";
            const string rtdstyle = "width: auto; margin-left: 10px;";
            const string lspanstyle = "margin: 5px; font-size: 13px; color: #4274b1; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: left;";
            const string rspanstyle = "font-size: 13px; color: #4B4B4B; font-family: Arial, Helvetica, sans-serif; font-weight: bold; text-align: right;";

            var sendMail = "";

            var username = data[5];
            sendMail += "<span style='font-size: 14px; color: #FF6600;'><b>" + username + "</b>, has send the feedback.</span><br />" +
                        @"<span style='font-size: 14px; color: #FF6600;'>Please find the feedback details as below:</span><br /><br />";

            sendMail += "<table width='740px' border='0' align='left' cellpadding='0' cellspacing='0' style='border: 1px solid #dcdcdc;'>";
            sendMail += "<tr style='" + trstyle + "'>" +
                             @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Name :</span></td>" +
                             @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[0] + "</span></td>" +
                         @"</tr>";
            sendMail += "<tr style='" + trstyle + "'>" +
                             @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Email :</span></td>" +
                             @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[1] + "</span></td>" +
                         @"</tr>";
            sendMail += "<tr style='" + trstyle + "'>" +
                             @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Subject :</span></td>" +
                             @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[2] + "</span></td>" +
                         @"</tr>";
            sendMail += "<tr style='" + trstyle + "'>" +
                             @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Suggestion about :</span></td>" +
                             @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[3] + "</span></td>" +
                         @"</tr>";
            sendMail += "<tr style='" + trstyle + "'>" +
                            @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Suggestion :</span></td>" +
                            @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + data[4] + "</span></td>" +
                        @"</tr>";
            sendMail += "<tr style='" + trstyle + "'>" +
                           @"<td style='" + ltdstyle + "'><span style='" + lspanstyle + "'>Posted On :</span></td>" +
                           @"<td style='" + rtdstyle + "'><span style='" + rspanstyle + "'>" + DateTime.Now.ToLongDateString() + "</span></td>" +
                       @"</tr>";

            sendMail += @"</table>";

            return sendMail;
        }
        public string SampleTenderMailFormat()
        {
            string emailFormatFileName = GetEmailFormatFileName(3, 0);
            string str = string.Empty;
            using (StreamReader streamReader = new StreamReader(emailFormatFileName))
                str = streamReader.ReadToEnd();
            return this.FormatEmailContent().Replace("{{EmailBody}}", str.Replace("{{SiteName}}", this.sitename));
        }



        #region SAMPLE TENDERS
        public TenderDetailInfo GetAllSearchTenderInfo_Client_SampleTender(int? permissionId, int? start, int? limit, int? tenderStatusFlag, Boolean? iscount,
            ref string ownershipId, ref string ownershipIdNotUsedIn, ref string sectorId, ref string sectorIdNotUsedIn,
            ref string agencyId, ref string agencyIdNotUsedIn, ref string indSub, ref string indSubNotUsedIn,
            ref string loc, ref string locNotUsedIn, ref string keyword1, ref string keyword2, ref string keyword3,
            ref string otherKeywords, ref string notUsedKeywords, ref string documentType, ref string isIcbncb,
            ref string tenderValue, ref double tenderValueFrom, ref double tenderValueTo,
            ref bool indianGlobal, ref string finalSearchText, string tenderyear, string OrderBys, string AscDesc, bool IsOnlyCount = false)
        {
            return _tenderInfo.GetAllSearchTenderInfo_Client_SampleTender(permissionId, start, limit, tenderStatusFlag, iscount,
            ref ownershipId, ref ownershipIdNotUsedIn, ref sectorId, ref sectorIdNotUsedIn,
            ref agencyId, ref agencyIdNotUsedIn, ref indSub, ref indSubNotUsedIn,
            ref loc, ref locNotUsedIn, ref keyword1, ref keyword2, ref keyword3,
            ref otherKeywords, ref notUsedKeywords, ref documentType, ref isIcbncb,
            ref tenderValue, ref tenderValueFrom, ref tenderValueTo,
            ref indianGlobal, ref finalSearchText, tenderyear, OrderBys, AscDesc, IsOnlyCount = false);
        }
        public string CreateTenderFormat(string ourrefno, int purpose, string eId, string displayText, Boolean indianGlobal)
        {
            //var selectedTenders = "";
            var newactiveDocLink = "";
            var closedLink = "";
            string empContactNo1 = "+91-97247 00247";
            string empContactNo2 = "+91-96870 00247";

            newactiveDocLink = url + "Indian-Tenders/Bid-Tenders-Details/";
            closedLink = newactiveDocLink;

            var tenaderInfoWithDetail = _tenderInfo.GetTenderInfoByOurRefNo(ourrefno, indianGlobal);
            UserMembershipDetail _tbUserMembershipDetail = new UserMembershipDetail();
            var empDetail = _tbUserMembershipDetail.GetLocalEmpEamilIdWithEmpId_Local(Convert.ToInt32(eId));

            if (empDetail != null)
            {
                empContactNo1 = "+91-" + empDetail.intContactNo.ToString();
                empContactNo2 = "+91-" + empDetail.intCompContactNo.ToString();
            }

            HomeController home = new HomeController();
            var filepath = GetEmailFormatFileName(4);
            var filedetail = string.Empty;
            using (StreamReader reader = new StreamReader(filepath))
            {
                filedetail = reader.ReadToEnd();
            }

            var emptyfiledetail = "";
            var tenderinfo = "";
            foreach (var item in tenaderInfoWithDetail)
            {
                emptyfiledetail = filedetail;

                var tenerstatus = "";

                #region Location
                var locations = Regex.Split(item.Location.Trim(), " - ");
                var city = new CultureInfo("en-US", false).TextInfo.ToTitleCase(locations[0].ToLower());
                var state = new CultureInfo("en-US", false).TextInfo.ToTitleCase(locations[1].ToLower());
                var location = "";
                if (state != "") location = city.Trim() + "-" + state.Trim();
                else location = city.Trim();
                #endregion Location

                #region Sector
                var sector = "";
                if (!(item.SectorName == null || item.SectorName.Trim() == ""))
                {
                    sector = new CultureInfo("en-US", false).TextInfo.ToTitleCase(item.SectorName.ToLower());
                }
                #endregion Sector

                #region Link
                var link = "";
                if (indianGlobal)
                {
                    link = newactiveDocLink + item.DueDate.Year + "/" + item.OurRefNo + "/";
                }
                #endregion Link

                var workdesc = Highlightsearchtext(item.WorkDesc, displayText, 1);

                switch (item.TenderStatusReturn)
                {
                    case 1:
                        tenerstatus = "ACTIVE";
                        break;
                    case 2:
                        tenerstatus = "NEW";
                        break;
                    case 3:
                        tenerstatus = "CLOSED";
                        break;
                }

                emptyfiledetail = emptyfiledetail.Replace("{{OurRefNo}}", item.OurRefNo.ToString());
                emptyfiledetail = emptyfiledetail.Replace("{{TenderStatus}}", tenerstatus);
                emptyfiledetail = emptyfiledetail.Replace("{{DueDate}}", item.DueDate.ToString("dd MMM, yyyy"));
                emptyfiledetail = emptyfiledetail.Replace("{{TenderValue}}", AmountToWords(item.TenderAmount.ToString()));
                emptyfiledetail = emptyfiledetail.Replace("{{Location}}", location);
                emptyfiledetail = emptyfiledetail.Replace("{{Sector}}", sector);
                emptyfiledetail = emptyfiledetail.Replace("{{TenderDetailLink}}", link);
                emptyfiledetail = emptyfiledetail.Replace("{{TenderDetailInfo}}", workdesc);
                emptyfiledetail = emptyfiledetail.Replace("{{ContactNo}}", empContactNo1.ToString() + " / " + empContactNo2.ToString());


                tenderinfo += emptyfiledetail;
            }

            return tenderinfo;
        }
        #endregion
    }
}