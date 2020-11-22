using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TenderAssist.Models.DBConnection;

namespace TenderAssist.Controllers
{
    public class LogController : Controller
    {
        private readonly CommonController _common;

        public LogController()
        {
            this._common = new CommonController();
        }

        public ActionResult Index(string un)
        {
            if (string.IsNullOrEmpty(un))
                return RedirectToAction("Index", "Home");

            tabClientDetail usersByUniqueId = _common.GetUsersByUniqueId(new Guid(un));
            if (usersByUniqueId == null)
                return (ActionResult)this.RedirectToAction("Index", "Home");

            this.Session["ClientId"] = (object)usersByUniqueId.intClientId;
            this.Session["Purpose"] = (object)usersByUniqueId.intClientPurpose;
            this.Session["PageTitle"] = (object)"";
            this.Session["IsActiveUser"] = (object)usersByUniqueId.intActive.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            this.Session["PageTitle"] = (object)"TenderAssist247 :: Indian Tenders";
            this.Session["IndianPermissionId"] = (object)null;

            return RedirectToAction("MyDashboard", "User");
        }
    }
}