using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using TenderAssist.Models;

namespace TenderAssist.CommonHelper
{
    public class Utility
    {
        /// <summary>
        /// Function: ToList
        /// Converts ObjectResult to Generic List
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Destination"></param>
        public static void ToList<T1, T2>(ObjectResult<T1> Source, List<T2> Destination) where T2 : new()
        {
            Destination.AddRange(Source.Select(CreateMapping<T1, T2>()));
        }

        /// <summary>
        /// CreateMapping
        /// Creates the mapping
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        private static Func<T1, T2> CreateMapping<T1, T2>() where T2 : new()
        {
            var typeOfSource = typeof(T1);
            var typeOfDestination = typeof(T2);

            // use reflection to get a list of the properties on the source and destination types
            var sourceProperties = typeOfSource.GetProperties();
            var destinationProperties = typeOfDestination.GetProperties();

            // join the source properties with the destination properties based on name
            var properties = from sourceProperty in sourceProperties
                             join destinationProperty in destinationProperties
                             on sourceProperty.Name.ToUpper() equals destinationProperty.Name.ToUpper()
                             select new { SourceProperty = sourceProperty, DestinationProperty = destinationProperty };


            return (x) =>
            {
                var y = new T2();

                foreach (var property in properties)
                {
                    var value = property.SourceProperty.GetValue(x, null);
                    property.DestinationProperty.SetValue(y, value, null);
                }

                return y;
            };
        }

        public static bool SendMail(string strTo, string strCc, string strBcc, string strSubject, string strBody, string fileName,
            string empFromId = "", string empPassword = "", string empSMTP = "", int empSMTPPort = 0)
        {
            bool flag = true;
            try
            {
                string SMTP_Host = string.IsNullOrEmpty(empSMTP) ? ConfigurationManager.AppSettings["SMTP_Host"].ToString() : empSMTP;
                string SMTP_Password = string.IsNullOrEmpty(empPassword) ? ConfigurationManager.AppSettings["SMTP_Password"].ToString() : empPassword;
                int SMTP_Port = empSMTPPort == 0 ? Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_Port"]) : empSMTPPort;
                string SMTP_EmailId = string.IsNullOrEmpty(empFromId) ? ConfigurationManager.AppSettings["SMTP_EmailId"].ToString() : empFromId;

                MailMessage message = new MailMessage();

                if (strCc != null && strCc.Trim().Length > 0)
                    message.CC.Add(strCc);

                if (strBcc != null && strBcc.Trim().Length > 0)
                    message.Bcc.Add(strBcc);

                if (strTo.Trim() == "")
                    strTo = SMTP_EmailId;

                message.To.Add(strTo);
                message.Subject = strSubject;

                if (SMTP_EmailId != null && SMTP_EmailId.Trim().Length > 0)
                    message.From = new MailAddress(SMTP_EmailId);

                message.BodyEncoding = Encoding.UTF8;
                message.Body = strBody;
                message.IsBodyHtml = true;

                if (fileName != null && fileName.Trim().Length > 0)
                    message.Attachments.Add(new Attachment(fileName));

                SmtpClient smtpClient = new SmtpClient()
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = false,
                    Host = SMTP_Host,
                    Port = SMTP_Port
                };

                if (!SMTP_EmailId.EndsWith("tenderassist247.com"))
                    smtpClient.EnableSsl = true;

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(SMTP_EmailId, SMTP_Password);
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        public static string GetRandomText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            for (int index = 0; index <= 5; ++index)
            {
                string str = Convert.ToString("012345679"[random.Next("012345679".Length)]);
                stringBuilder.Append(str);
            }
            return stringBuilder.ToString();
        }

        public static List<string> GetAllAlphabaticWord()
        {
            var alphabaticWord = new AlphabaticWord();
            var allWord = alphabaticWord.GetAllAlphabaticWord().ToList();
            var wordlist = new List<string>();
            for (var i = 0; i < allWord.Count(); i++)
            { wordlist.Add(allWord[i].AlphabaticWordName); }

            return wordlist;
        }

        public static class TenderTypeList
        {
            /*INDIAN*/
            public const int HomePage = -1;
            public const int IndianTenderListing = -2;
            public const int DetailPage = -3;
            public const int SearchTender = 0;
            public const int State = 1;
            public const int City = 2;
            public const int Industry = 3;
            public const int SubIndustry = 4;
            public const int Agency = 5;
            public const int Sector = 6;
            public const int Ownership = 7;
            public const int Keyword = 8;
            public const int Auction = 9;
            public const int Buyer = 10;
            public const int Seller = 11;
            public const int Contracts = 12;

            /*Forms*/
            public const int FreeSampleTender = 13;
            public const int TenderRegistration = 14;
            public const int GetGlobaltender = 15;
            public const int GetTenderResults = 16;
            public const int VendorManagementsupport = 17;
            public const int GetDigitalcertificate = 18;
            public const int Contactus = 19;
            public const int QuickcontactFromGoogle = 20;
            public const int DownloadTender = 21;
            public const int TenderBidding = 22;
            public const int SubscriptionSuccess = 23;

            /*GLOBAL*/
            public const int GlobalDetailPage = -3;
            public const int MiddleEastCountryRegion = 1;
            public const int EuropeanCountryRegion = 2;
            public const int AfricanCountryRegion = 3;
            public const int AsianCountryRegion = 4;
            public const int SaarCountryRegion = 5;
            public const int AustraliaOceaniaCountryRegion = 6;
            public const int SouthAmericaCountryRegion = 7;
            public const int NorthAmericaCountryRegion = 8;
            public const int GlobalCountry = 10;

        }
        public static List<int> GlobalCountryRegionList()
        {
            List<int> regionList = new List<int>()
            {
                TenderTypeList.MiddleEastCountryRegion,
                TenderTypeList.EuropeanCountryRegion,
                TenderTypeList.AfricanCountryRegion,
                TenderTypeList.AsianCountryRegion,
                TenderTypeList.SaarCountryRegion,
                TenderTypeList.AustraliaOceaniaCountryRegion,
                TenderTypeList.SouthAmericaCountryRegion,
                TenderTypeList.NorthAmericaCountryRegion
        };
            return regionList;
        }
        public static class TenderTypeDisplayText
        {
            /*INDIAN*/

            public static string IndianTender = "indian-tenders";

            public const string State = "state tenders";
            public const string City = "city tenders";
            public const string Locations = "tenders by state/city";

            public const string Industry = "industry tenders";
            public const string SubIndustry = "sub-industry tenders";
            public const string Category = "tenders by industry/sub-industry";

            public const string Agency = "agency tenders";
            public const string Sector = "company sector tenders";
            public const string Ownership = "ownership tenders";
            public const string AgencySectorOwnership = "tenders by ownership/sector/agency";

            public const string Keyword = "keyword tenders";
            public const string AdvanceSearch = "advance search senders";

            public static string GlobalTender = "global-tenders";
            public static string DisplayMiddleEastCountryName = "Middle East Countries";
            public static string DisplayEuropeanCountryName = "European Countries";
            public static string DisplayAfricanCountryName = "African Countries";
            public static string DisplayAsianCountryName = "Asian Countries";
            public static string DisplaySaarCountryName = "Saar Countries";
            public static string DisplayAustraliaOceaniaCountryName = "Australia Oceania Countries";
            public static string DisplaySouthAmericaCountryName = "South America Countries";
            public static string DisplayNorthAmericaCountryName = "North America Countries";

        }
        public static class TenderWordNameList
        {
            public static string StateWord = "tenders-from-";
            public static string CityWord = "tenders-in-";
            public static string IndustryWord = "tenders-about-";
            public static string SubIndustryWord = "tenders-of-";
            public static string AgencyWord = "tenders-of-";
            public static string OwnershipWord = "tenders-by-";
            public static string SectorWord = "tenders-by-";
            public static string KeywordWord = "tenders-for-";
            public static string AuctionTender = "tender-category";
            public static string TypesOfTender = "tender-type";
            public static string GlobalCountryWord = "tenders-from-";
        }
        public class SubscribType
        {
            public const int IndianTender = 1;
            public const int GlobalTender = 2;
            public const int TenderResult = 3;
            public const int TenderAssist = 4;
            public const int VendorEmpanelment = 5;
            public const int SubContractor = 6;
            public const int DigitalSignature = 7;
            public const int Projects = 8;

            public const int Registration = 9;
            public const int FreeTenders = 10;
            public const int ContactUs = 11;
            public const int DownloadTender = 12;
            public const int SampleTenders = 13;
            public const int PayOnline = 14;
            public const int BidWithTenderAssist = 15;
            //public const int BidTenderGetAssist = 16;
            public const int ISOConsultant = 17;

            public const int Renewal = 18;

            public const int ClientFeedback = 100;
            public const int result = 50;
            public const int project = 51;

        }
        public class SubscribTypeDisplsyText
        {
            public const string IndianTender = "Indian Tenders";
            public const string GlobalTender = "Global Tenders";
            public const string TenderAssist = "Bid Tender Get Assistance";
            public const string SubContractor = "Subcontractor";
            public const string Registration = "New Subscription";
            public const string DownloadTender = "Download Document";
            public const string TenderResult = "Get Tender Awarded";
            public const string Projects = "";
            public const string FreeTenders = "Tender Hosting";
            public const string DigitalSignature = "Get Tender Digital Signature Certificate";
            public const string ContactUs = "contact us";
            public const string VendorEmpanelment = "Vendor Empanelment";
            public const string SampleTenders = "Get sample tenders";
            public const string Result = "Result";
            public const string Project = "Project";
            public const string PayOnline = "Pay Online";
            public const string BidWithTenderAssist = "Bid With Tender Assist";
            //public const string BidTenderGetAssist = "Bid Tender Get Assistance";
            public const string ISOConsultant = "ISO consultant";

            public const string Renewal = "User - Renewal";

            public const string ClientFeedback = "Client Feedback";


        }

        public static class LinkUrls
        {
            public static string TenderByKeywordUrl = ConfigurationManager.AppSettings["TenderByKeywordUrl"].ToString();
            public static string TenderByAdvSearchUrl = ConfigurationManager.AppSettings["TenderByAdvSearchUrl"].ToString();
            public static string TenderDetailUrl = ConfigurationManager.AppSettings["TenderDetailUrl"].ToString();
            public static string BidWithAssistUrl = ConfigurationManager.AppSettings["BidWithAssistUrl"].ToString();
            public static string GlobalTenderDetailUrl = ConfigurationManager.AppSettings["GlobalTenderDetailUrl"].ToString();
        }

        public class FormType
        {
            public const int RegistrationForm = 1;
            public const int ContactForm = 2;
            public const int OtherForms = 3;
            public const int PopupForms = 4;
            public const int PayOnline = 5;
        }
    }

}