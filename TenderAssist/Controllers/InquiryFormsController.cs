using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TenderAssist.CommonHelper;
using TenderAssist.Models;
using TenderAssist.Models.DBConnection;
using TenderAssist.ViewModel;
using static TenderAssist.CommonHelper.Utility;

namespace TenderAssist.Controllers
{
    public class InquiryFormsController : Controller
    {
        private readonly TenderInformation _tenderInfo;
        private readonly GetListItems _getListItems;
        private readonly CommonController _common;


        public InquiryFormsController()
        {
            _tenderInfo = new TenderInformation();
            _getListItems = new GetListItems();
            _common = new CommonController();
        }

        public ActionResult IndianTenders()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.IndianTender,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.IndianTender,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }
        public ActionResult GlobalTenders()
        {
            var tenderDetail = new TenderDetail
            {
                CountryList = _getListItems.CountryList(),
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.GlobalTender,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.GlobalTender,
                FormType = FormType.RegistrationForm
            };

            return View("InquiryForm", tenderDetail);
        }
        public ActionResult TenderResult()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.TenderResult,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.TenderResult,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }
        public ActionResult TenderAssist()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.TenderAssist,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.TenderAssist,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }
        public ActionResult result()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.result,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.Result,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }
        public ActionResult project()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.project,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.Project,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }
        public ActionResult VendorEmpanelment()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.VendorEmpanelment,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.VendorEmpanelment,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }
        public ActionResult Subcontracting()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.SubContractor,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.SubContractor,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }
        public ActionResult DigitalSignature()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.DigitalSignature,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.DigitalSignature,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }

        public ActionResult ISOConsultant()
        {
            var tenderDetail = new TenderDetail
            {
                StateList = _getListItems.StateList(),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.ISOConsultant,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.ISOConsultant,
                FormType = FormType.OtherForms
            };

            return View("InquiryForm", tenderDetail);
        }


        public ActionResult NewRegistration()
        {
            var tenderDetail = new TenderDetail
            {
                CountryList = _getListItems.CountryList(),
                StateList = _getListItems.StateList(0),
                CityList = _getListItems.CityList(0),
                Subscribetype = SubscribType.Registration,
                DownloadTenderRefNo = 0,
                FormTitle = SubscribTypeDisplsyText.Registration,
                FormType = FormType.RegistrationForm
            };

            return View("RegistrationForm", tenderDetail);
        }

        [HttpPost]
        public ActionResult SubmitInquiryDetail(FormCollection coll)
        {
            var msg = "ok";
            string userEmail;
            var inqForm = new tbInquiryRegForm();
            var inqFormUserId = 0;

            var ModuleType = 0;
            var InquiryTypeId = 0;
            var LinkId = 0;
            var BrowserLink = "";

            var tYear = "";
            var tenderId = 0;

            var encryptedOurRefNo = "";
            var encryptedtYear = "";
            var encryptedtUserId = "";
            var isDowloadDoc = false; var isExportExcelDoc = false;

            var DownloadDocumentUrl = "";

            var PopUpFormControlId = "";
            var InquiryTypeName = "";

            string adminFromId = ConfigurationManager.AppSettings["SMTP_EmailId"].ToString();

            var LoginUserId = Session["ClientID"] == null ? 0 : Convert.ToInt32(Session["ClientID"]);

            try
            {
                //var ClientIpAddress = RequestHelpers.GetClientIpAddress();

                var FormType = coll.Get("FormType").ToString().ToLower().Trim();
                InquiryTypeName = coll.Get("InquiryTypeName").ToString();
                var captchaHiddenId = "hdn_Captcha_Code";

                var captchaCode = coll.Get(captchaHiddenId);
                var txtCaptcha = coll.Get("txtCaptcha" + PopUpFormControlId);
                BrowserLink = coll.Get("BrowserLink").ToString();

                if (Convert.ToInt32(FormType) != Utility.FormType.PayOnline)
                {
                    if (captchaCode == null || (txtCaptcha.Trim() != captchaCode.ToString().Trim()))
                    {
                        msg = "captchaerror"; userEmail = "";
                        return Json(new { msg, userEmail, inqFormUserId }, JsonRequestBehavior.AllowGet);
                    }
                }

                InquiryRegFormFields RegFormParams = new InquiryRegFormFields()
                {
                    InquiryTypeID = Convert.ToInt32(coll.Get("InquiryTypeID")),
                    intClientPurpose = 1,
                    OurRefNo = Convert.ToInt32(coll.Get("TenderID")),
                    NewID = Convert.ToInt32(coll.Get("NewID")),
                    ModuleType = Convert.ToInt32(coll.Get("ModuleType")),

                    Name = coll.Get("txtName"),
                    MobNo = coll.Get("txtMobileNo"),
                    EmailID = coll.Get("txtEmailId"),
                    InterestedTenders = coll.Get("txtInterestedTenders"),
                    CompName = FormType != Utility.FormType.ContactForm.ToString() ? coll.Get("txtCompanyName") : "",

                    //Designation = !isContactForm ? coll.Get("txtDesignation") : "",

                    //Address = FormType == Utility.FormType.RegistrationForm.ToString() ? coll.Get("txtAddress") : "",
                    //PhoneNo = !isContactForm ? coll.Get("txtPhoneNo") : "",
                    //Website = !isContactForm ? coll.Get("txtWebsite") : "",

                    Country = FormType == Utility.FormType.RegistrationForm.ToString() ? Convert.ToInt32(coll.Get("drpCountry")) : 0,
                    State = Convert.ToInt32(coll.Get("drpState")),
                    City = (FormType == Utility.FormType.RegistrationForm.ToString() || FormType == Utility.FormType.OtherForms.ToString()) ? Convert.ToInt32(coll.Get("drpCity")) : 0,

                    Flag = 0,
                    LinkId = 0,//string.IsNullOrEmpty(coll.Get("LinkId" + PopUpFormControlId)) ? 0 : Convert.ToInt32(coll.Get("LinkId" + PopUpFormControlId)),
                    BrowserLink = BrowserLink,
                    FormTitle = InquiryTypeName,//= string.IsNullOrEmpty(coll.Get("FormTitle" + PopUpFormControlId)) ? "" : coll.Get("FormTitle" + PopUpFormControlId).ToString(),
                    //ClientIPAddress = ClientIpAddress

                };

                tenderId = RegFormParams.OurRefNo;
                InquiryTypeId = RegFormParams.InquiryTypeID;
                ModuleType = RegFormParams.ModuleType;
                userEmail = RegFormParams.EmailID;

                var userName = RegFormParams.Name;
                var userContactNo = RegFormParams.MobNo.ToString();
                var userProductInfo = string.IsNullOrEmpty(RegFormParams.InterestedTenders) ? "" : RegFormParams.InterestedTenders;

                var FormTitle = RegFormParams.FormTitle;

                inqFormUserId = _common.SubmitInquiryRegForms(RegFormParams);
                if (inqFormUserId == 0)
                {
                    msg = "error";
                    userEmail = "";

                    return Json(new { msg, userEmail, inqFormUserId, InquiryTypeId, tenderId, tYear, encryptedOurRefNo, encryptedtYear, encryptedtUserId },
                        JsonRequestBehavior.AllowGet);
                }

                string country = "";
                string state = "";
                string city = "";

                tYear = string.IsNullOrEmpty(coll.Get("TenderYear"))
                    ? DateTime.Now.Year.ToString()
                    : coll.Get("TenderYear").ToString();

                if (RegFormParams.State != 0)
                {
                    var statedet = _tenderInfo.GetState(RegFormParams.State);
                    state = statedet == null ? "" : statedet.StateName;
                }
                if (RegFormParams.Country != 0)
                {
                    var countrydet = _tenderInfo.GetCountry(RegFormParams.Country);
                    country = countrydet == null ? "" : countrydet.CountryName;
                }
                if (RegFormParams.City != 0)
                {
                    var citydet = _tenderInfo.GetCity(RegFormParams.City);
                    city = citydet == null ? "" : citydet.Location;
                }


                string appName = ConfigurationManager.AppSettings["ProjectName"].ToString();

                string messageBody = "";
                string userFormFields;
                string subject = "";
                string title = "";

                string filepath = "";
                string filedetail = "";

                subject = "Your initial request for " + InquiryTypeName + " :: " + appName;
                messageBody = _common.SubscribNowMailFormat();

                //filepath = GetHTMLEmailFormatFileName(CommonController.TenderTypeList.UserInfoDetail);
                //filedetail = string.Empty;
                //using (StreamReader reader = new StreamReader(filepath))
                //{
                //    filedetail = reader.ReadToEnd();
                //}

                //filedetail = filedetail.Replace("{{Name}}", userName);
                //filedetail = filedetail.Replace("{{Email}}", userEmail);
                //filedetail = filedetail.Replace("{{ContactNo}}", userContactNo);
                //filedetail = filedetail.Replace("{{ProductDetails}}", userProductInfo);
                //messageBody = messageBody.Replace("{{UserInfoDetail}}", filedetail);

                messageBody = messageBody.Replace("{{Name}}", userName);

                Utility.SendMail(userEmail, "", "", subject, messageBody, "");


                #region SEND MAIL TO ADMIN

                subject = appName + " : request to register for Tenders Subscription ";

                messageBody = _common.SubscribNowMailFormat_Admin();
                messageBody = messageBody.Replace("{{Name}}", RegFormParams.Name);
                messageBody = messageBody.Replace("{{Designation}}", RegFormParams.Designation);
                messageBody = messageBody.Replace("{{CompanyName}}", RegFormParams.CompName);
                messageBody = messageBody.Replace("{{Address}}", RegFormParams.Address);
                messageBody = messageBody.Replace("{{MobileNo}}", RegFormParams.MobNo);
                messageBody = messageBody.Replace("{{PhoneNo}}", RegFormParams.PhoneNo);
                messageBody = messageBody.Replace("{{Country}}", country);
                messageBody = messageBody.Replace("{{State}}", state);
                messageBody = messageBody.Replace("{{City}}", city);
                messageBody = messageBody.Replace("{{InterestedTenders}}", !string.IsNullOrEmpty(RegFormParams.InterestedTenders) ? RegFormParams.InterestedTenders.Replace("#-#", "; ") : "");
                messageBody = messageBody.Replace("{{EmailID}}", RegFormParams.EmailID);
                messageBody = messageBody.Replace("{{Website}}", RegFormParams.Website);

                Utility.SendMail(adminFromId, "", ConfigurationManager.AppSettings["Director_FromEmailId"], subject, messageBody, "");

                #endregion

            }
            catch (Exception errorex)
            {
                msg = "error" + errorex.InnerException;
                userEmail = "";
                LoginUserId = 0;

            }
            return Json(new
            {
                msg,
                userEmail,
                inqFormUserId,
                InquiryTypeId,
                tenderId,
                tYear,
                encryptedOurRefNo,
                encryptedtYear,
                encryptedtUserId,
                isDowloadDoc,
                isExportExcelDoc,
                DownloadDocumentUrl,
                LoginUserId
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Thanks(string email)
        {
            var tenderDetail = new TenderDetail();
            ViewBag.EmailId = email;

            return View(tenderDetail);
        }
        public JsonResult GetStateByCountry(int countryId)
        {
            var stateList = _getListItems.StateList(countryId);
            return this.Json(new { StateList = stateList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCityByState(int stateId)
        {
            var cityList = _getListItems.CityList(stateId);
            return this.Json(new { CityList = cityList }, JsonRequestBehavior.AllowGet);
        }

    }
}