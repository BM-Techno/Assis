using System.Web;
using System.Web.Optimization;

namespace TenderAssist
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/common-js").Include(
                    "~/Scripts/CommonScript.js"));

            #region SUBSCRIB NOW            
            bundles.Add(new ScriptBundle("~/bundles/inquiry-script").Include(
                      "~/Scripts/Inquiry.js"));
            #endregion

            #region Search
            bundles.Add(new ScriptBundle("~/bundles/search-script").Include(
                       "~/Scripts/Search/MainSearch.js",
                       "~/Scripts/Search/AutoComplete.js"));
            bundles.Add(new ScriptBundle("~/bundles/advancesearch-js").Include(
                   "~/Scripts/Search/AdvanceSearch.js"));
            bundles.Add(new ScriptBundle("~/bundles/advance-search-datepicker-script").Include(
               "~/Scripts/Search/AdvSearch_DatePicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/load-dataformenu-script").Include(
              "~/Scripts/Search/GetDataForMenu.js"));

            #endregion

            #region GLOBAL
            bundles.Add(new ScriptBundle("~/bundles/global-search-script").Include(
             "~/Scripts/Search/GlobalSearch.js"));
            #endregion


            #region PAY ONLINE            
            bundles.Add(new ScriptBundle("~/bundles/pay-online-script").Include(
                      "~/Scripts/PayOnline.js"));
            #endregion


            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/bootstrap.css",
                       "~/Content/TA/CSS/TAStyle.css"));
            bundles.Add(new StyleBundle("~/Content/other-css").Include(
                     "~/Content/TA/CSS/font-awesome.min.css",
                     "~/Content/TA/CSS/animate.css",
                     "~/Content/TA/CSS/animate.min.css",
                     "~/Content/TA/CSS/overwrite.css",
                     "~/Content/TA/CSS/dropdown.css"));

            #region AutoComplete

            bundles.Add(new StyleBundle("~/Content/autocomplete-css").Include(
                        "~/Content/PlugIn/AutoComplete/autocomplete-0.3.0.css",
                        "~/Content/PlugIn/AutoComplete/autocomplete-0.3.0.min.css"));
            #endregion

            
            /*User*/
            bundles.Add(new ScriptBundle("~/bundles/client-search-js").Include(
                  "~/Scripts/User/ClientSearch.js"));

            bundles.Add(new ScriptBundle("~/bundles/indian-service-js").Include(
                       "~/Scripts/User/IndianService.js"));

            bundles.Add(new ScriptBundle("~/bundles/global-service-js").Include(
                      "~/Scripts/User/GlobalService.js"));

            bundles.Add(new ScriptBundle("~/bundles/user-login-script").Include(
                     "~/Scripts/User/Login.js"));

            bundles.Add(new StyleBundle("~/Content/user-css").Include(
                      "~/Content/TA/CSS/UserStyle.css"));
        }
    }
}
