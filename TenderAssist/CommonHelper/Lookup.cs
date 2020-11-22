using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using System;
using TenderAssist.Models.DBConnection;

namespace TenderAssist.Models
{

    public class DateOfBirthDRP
    {
        public List<SelectListItem> FillDay(string selectedval)
        {
            var drpday = new List<SelectListItem>();
            drpday.Insert(0, (new SelectListItem { Text = "", Value = "" }));
            for (int i = 1; i <= 31; i++)
            {
                drpday.Insert(i, (new SelectListItem { Selected = selectedval == i.ToString(CultureInfo.InvariantCulture), Text = i.ToString(CultureInfo.InvariantCulture), Value = i.ToString(CultureInfo.InvariantCulture) }));
            }
            return drpday;
        }

        public List<SelectListItem> FillMonth(string selectedval)
        {
            var drpmonth = new List<SelectListItem>();
            drpmonth.Insert(0, (new SelectListItem { Text = "", Value = "" }));
            for (int i = 1; i <= 12; i++)
            {
                string month = "";
                switch (i)
                {
                    case 1:
                        month = "Jan-01";
                        break;
                    case 2:
                        month = "Feb-02";
                        break;
                    case 3:
                        month = "Mar-03";
                        break;
                    case 4:
                        month = "Apr-04";
                        break;
                    case 5:
                        month = "May-05";
                        break;
                    case 6:
                        month = "Jun-06";
                        break;
                    case 7:
                        month = "Jul-07";
                        break;
                    case 8:
                        month = "Aug-08";
                        break;
                    case 9:
                        month = "Sep-09";
                        break;
                    case 10:
                        month = "Oct-10";
                        break;
                    case 11:
                        month = "Nov-11";
                        break;
                    case 12:
                        month = "Dec-12";
                        break;
                }
                drpmonth.Insert(i, (new SelectListItem { Selected = selectedval == i.ToString(CultureInfo.InvariantCulture), Text = month, Value = i.ToString(CultureInfo.InvariantCulture) }));
            }
            return drpmonth;
        }

        public List<SelectListItem> FillYear(string selectedval)
        {
            var drpyear = new List<SelectListItem>();
            drpyear.Insert(0, (new SelectListItem { Text = "", Value = "" }));

            int currentYeay = System.DateTime.Today.Year;
            int minYr = currentYeay - 25;
            //int MaxYr = currentYeay - 4;
            int cnt = minYr;
            for (int i = 1; i <= 22; i++)
            {
                drpyear.Insert(i, (new SelectListItem { Selected = selectedval == cnt.ToString(CultureInfo.InvariantCulture), Text = cnt.ToString(CultureInfo.InvariantCulture), Value = cnt.ToString(CultureInfo.InvariantCulture) }));
                cnt++;
            }
            return drpyear;
        }
    }

    public class SearchType
    {
        [DisplayName("Name")]
        public string SearchTypeName { get; set; }

        [DisplayName("Value")]
        public string SearchTypeValue { get; set; }

        public List<SearchType> FindAllSearchType()
        {
            var searchType = new List<SearchType>
            {
                new SearchType { SearchTypeName = "Keyword", SearchTypeValue = "1" },
                new SearchType { SearchTypeName = "Word", SearchTypeValue = "2" },
                new SearchType { SearchTypeName = "Exact Phrase", SearchTypeValue = "3" },
            };
            return searchType;
        }
        public List<SearchType> ClientFindAllSearchType()
        {
            var searchType = new List<SearchType>
            {
                new SearchType { SearchTypeName = "SearchWithin", SearchTypeValue = "1" },
                new SearchType { SearchTypeName = "State/City", SearchTypeValue = "2" },
                new SearchType { SearchTypeName = "OurRefNo", SearchTypeValue = "3" },
            };
            return searchType;
        }
    }

    public class TenderValType
    {
        [DisplayName("Name")]
        public string TenderValTypeName { get; set; }

        [DisplayName("Value")]
        public string TenderValTypeValue { get; set; }

        public List<TenderValType> FindAllTenderValType()
        {
            var tenderValType = new List<TenderValType>
            {
                new TenderValType { TenderValTypeName= "[SELECT TENDER VALUE]", TenderValTypeValue = "0" },
                new TenderValType { TenderValTypeName= "Ref Documents",TenderValTypeValue = "1" },
                new TenderValType { TenderValTypeName= "Below 1 Lakh", TenderValTypeValue= "2" },
                new TenderValType { TenderValTypeName= "1 Lakh to 1 Crore",TenderValTypeValue = "3" },
                new TenderValType { TenderValTypeName= "1 Crore to 10 Crore",TenderValTypeValue = "4" },
                new TenderValType { TenderValTypeName= "10 Crore to 100 Crore",TenderValTypeValue = "5" }
            };
            return tenderValType;
        }
    }

    public class AlphabaticWord
    {
        [DisplayName("Name")]
        public string AlphabaticWordName { get; set; }

        [DisplayName("Value")]
        public string AlphabaticWordValue { get; set; }

        public List<AlphabaticWord> GetAllAlphabaticWord()
        {
            var alphabaticWord = new List<AlphabaticWord>
            {
                new AlphabaticWord { AlphabaticWordName= "A",AlphabaticWordValue = "A" },
                new AlphabaticWord { AlphabaticWordName= "B", AlphabaticWordValue= "B" },
                new AlphabaticWord { AlphabaticWordName= "C",AlphabaticWordValue = "C" },
                new AlphabaticWord { AlphabaticWordName= "D",AlphabaticWordValue = "D" },
                new AlphabaticWord { AlphabaticWordName= "E", AlphabaticWordValue= "E" },
                new AlphabaticWord { AlphabaticWordName= "F",AlphabaticWordValue = "F" },
                new AlphabaticWord { AlphabaticWordName= "G",AlphabaticWordValue = "G" },
                new AlphabaticWord { AlphabaticWordName= "H", AlphabaticWordValue= "H" },
                new AlphabaticWord { AlphabaticWordName= "I",AlphabaticWordValue = "I" },
                new AlphabaticWord { AlphabaticWordName= "J",AlphabaticWordValue = "J" },
                new AlphabaticWord { AlphabaticWordName= "K", AlphabaticWordValue= "K" },
                new AlphabaticWord { AlphabaticWordName= "L",AlphabaticWordValue = "L" },
                new AlphabaticWord { AlphabaticWordName= "M",AlphabaticWordValue = "M" },
                new AlphabaticWord { AlphabaticWordName= "N", AlphabaticWordValue= "N" },
                new AlphabaticWord { AlphabaticWordName= "O",AlphabaticWordValue = "O" },
                new AlphabaticWord { AlphabaticWordName= "P",AlphabaticWordValue = "P" },
                new AlphabaticWord { AlphabaticWordName= "Q", AlphabaticWordValue= "Q" },
                new AlphabaticWord { AlphabaticWordName= "R",AlphabaticWordValue = "R" },
                new AlphabaticWord { AlphabaticWordName= "S",AlphabaticWordValue = "S" },
                new AlphabaticWord { AlphabaticWordName= "T", AlphabaticWordValue= "T" },
                new AlphabaticWord { AlphabaticWordName= "U",AlphabaticWordValue = "U" },
                new AlphabaticWord { AlphabaticWordName= "V",AlphabaticWordValue = "V" },
                new AlphabaticWord { AlphabaticWordName= "W", AlphabaticWordValue= "W" },
                new AlphabaticWord { AlphabaticWordName= "X",AlphabaticWordValue = "X" },
                new AlphabaticWord { AlphabaticWordName= "Y", AlphabaticWordValue= "Y" },
                new AlphabaticWord { AlphabaticWordName= "Z",AlphabaticWordValue = "Z" }
            };
            return alphabaticWord;
        }
    }

    public class TenderType
    {
        [DisplayName("Name")]
        public string TenderTypeName { get; set; }

        [DisplayName("Value")]
        public string TenderTypeValue { get; set; }

        public List<TenderType> FindAllTenderType()
        {
            var searchType = new List<TenderType>
            {
                new TenderType { TenderTypeName= "[SELECT TENDER TYPE]", TenderTypeValue = "0" },
                new TenderType { TenderTypeName= "Buy", TenderTypeValue = "1" },
                new TenderType { TenderTypeName= "Sell", TenderTypeValue = "2" },
                new TenderType { TenderTypeName= "Contract", TenderTypeValue = "3" },
            };
            return searchType;
        }
    }

    public class TenderStatus
    {
        [DisplayName("Name")]
        public string TenderStatusName { get; set; }

        [DisplayName("Value")]
        public string TenderStatusValue { get; set; }

        public List<TenderStatus> FindAllTenderStatus()
        {
            var tenderType = new List<TenderStatus>
            {
                new TenderStatus { TenderStatusName= "[SELECT TENDER STATUS]", TenderStatusValue = "0" },
                new TenderStatus { TenderStatusName= "Active", TenderStatusValue = "1" },
                new TenderStatus { TenderStatusName= "New", TenderStatusValue = "2" },
                new TenderStatus { TenderStatusName= "Close", TenderStatusValue = "3" },
            };
            return tenderType;
        }
    }

    public class OrderBy
    {
        [DisplayName("Name")]
        public string OrderByName { get; set; }

        [DisplayName("Value")]
        public string OrderByValue { get; set; }

        public List<OrderBy> GetAllOrderType()
        {
            var orderType = new List<OrderBy>
            {
                new OrderBy { OrderByName= "Date : Newest", OrderByValue = "0" },
                new OrderBy { OrderByName= "Date : Oldest", OrderByValue = "1" },
                new OrderBy { OrderByName= "Price: Low to High", OrderByValue = "2" },
                new OrderBy { OrderByName= "Price: High to Low", OrderByValue = "3" },
            };
            return orderType;
        }
    }

    public class LoginStatus
    {
        public bool CheckLoginSession()
        {
            if (HttpContext.Current.Session["ClientID"] != null)
            {
                HttpContext.Current.Session.Add("ClientID", HttpContext.Current.Session["ClientID"].ToString());

                if (HttpContext.Current.Session["ClientInfo"] == null)
                {
                    UserMembershipDetail _userInfo = new UserMembershipDetail();
                    var userinfo = _userInfo.GetUsersById(Convert.ToInt32(HttpContext.Current.Session["ClientID"].ToString()));

                    if (userinfo != null)
                    {
                        HttpContext.Current.Session.Add("ClientInfo", userinfo);
                    }
                }
                return true;
            }
            return false;
        }

        public string GetUserName()
        {
            var username = string.Empty;
            string fname = "";
            string lname = "";

            if (HttpContext.Current.Session["ClientInfo"] != null)
            {
                var userinfo = (tabClientDetail)HttpContext.Current.Session["ClientInfo"];

                if (userinfo != null)
                {
                    if (!string.IsNullOrEmpty(userinfo.strFName))
                        fname = userinfo.strFName.Trim();
                    if (!string.IsNullOrEmpty(userinfo.strLName))
                        lname = userinfo.strLName.Trim();

                    username = fname + " " + lname;

                    if (username.Trim() == "")
                    { username = userinfo.strEmailId1; }
                }
            }
            return username;

        }

        public string GetUserLastLogin()
        {
            var lastlogindate = "";

            if (HttpContext.Current.Session["ClientInfo"] != null)
            {
                var userinfo = (tabClientDetail)HttpContext.Current.Session["ClientInfo"];

                if (userinfo != null)
                {
                    lastlogindate = userinfo.dtLastLoginDate != null ? userinfo.dtLastLoginDate.Value.ToString("dd MMM, yyyy") : "";
                }
            }
            return lastlogindate;

        }
    }

    public class GetListItems
    {
        private readonly TenderInformation _tenderInfo = new TenderInformation();
        private static int TotalGetData = 20;
        Random random = new Random();

        public List<SelectListItem> GetStateList(int CountryId, string searchName = "")
        {
            var states = string.IsNullOrEmpty(searchName)
                ? _tenderInfo.ListStateByCountry(CountryId).OrderBy(x => random.Next()).Take(TotalGetData).ToList()
                : _tenderInfo.ListStateBySearch(CountryId, searchName).ToList();

            var stateList = new List<SelectListItem>();
            for (var i = 0; i < states.Count(); i++)
            {
                stateList.Insert(i, new SelectListItem { Text = states[i].StateName, Value = states[i].StateId.ToString(CultureInfo.InvariantCulture) });
            }


            return stateList;
        }
        public List<SelectListItem> GetCityList(int CountryId, string searchName = "")
        {
            var city = string.IsNullOrEmpty(searchName)
                ? _tenderInfo.ListCityByCountry(CountryId).OrderBy(x => random.Next()).Take(TotalGetData).ToList()
                : _tenderInfo.ListCityBySearch(CountryId, searchName).ToList();

            var cityList = new List<SelectListItem>();
            for (var i = 0; i < city.Count(); i++)
            {
                cityList.Insert(i, new SelectListItem { Text = city[i].Location, Value = city[i].LocId.ToString(CultureInfo.InvariantCulture) });
            }

            return cityList;
        }
        public List<SelectListItem> GetIndustryList(string searchName = "")
        {
            var listAllIndustory = string.IsNullOrEmpty(searchName)
                ? _tenderInfo.ListIndustry().OrderBy(x => random.Next()).Take(TotalGetData).ToList()
                : _tenderInfo.ListIndustryBySearch(searchName).ToList();

            var indList = new List<SelectListItem>();
            for (var i = 0; i < listAllIndustory.Count(); i++)
            {
                indList.Insert(i, new SelectListItem { Text = listAllIndustory[i].IndustryName, Value = listAllIndustory[i].IndustryId.ToString(CultureInfo.InvariantCulture) });
            }

            return indList;
        }
        public List<SelectListItem> GetSubIndustryList(string searchName = "")
        {
            var listAllSubIndustory = string.IsNullOrEmpty(searchName)
                ? _tenderInfo.ListAllSubIndustry().OrderBy(x => random.Next()).Take(TotalGetData).ToList()
                : _tenderInfo.ListSubIndustryBySearch(searchName).ToList();

            var subindList = new List<SelectListItem>();
            for (var i = 0; i < listAllSubIndustory.Count(); i++)
            {
                subindList.Insert(i, new SelectListItem { Text = listAllSubIndustory[i].SubIndustryName, Value = listAllSubIndustory[i].SubIndustryId.ToString(CultureInfo.InvariantCulture) });
            }

            return subindList;
        }
        public List<SelectListItem> GetAgencyList(string searchName = "")
        {
            var listAllAgency = string.IsNullOrEmpty(searchName)
                ? _tenderInfo.ListAgencyMaster().OrderBy(x => random.Next()).Take(TotalGetData).ToList()
                : _tenderInfo.SearchAgencyMaster(searchName).ToList();

            var agencyList = new List<SelectListItem>();
            for (var i = 0; i < listAllAgency.Count(); i++)
            {
                agencyList.Insert(i, new SelectListItem { Text = listAllAgency[i].AgencyName, Value = listAllAgency[i].AgencyId.ToString(CultureInfo.InvariantCulture) });
            }

            return agencyList;
        }
        public List<SelectListItem> GetSectorList(string searchName = "")
        {
            var listAllsector = string.IsNullOrEmpty(searchName)
                ? _tenderInfo.ListSector().ToList().OrderBy(x => random.Next()).Take(TotalGetData).ToList()
                : _tenderInfo.SearchCompanySectorMaster(searchName).ToList();

            var sectorList = new List<SelectListItem>();

            for (var i = 0; i < listAllsector.Count(); i++)
            {
                sectorList.Insert(i, new SelectListItem { Text = listAllsector[i].SectorName, Value = listAllsector[i].SectorId.ToString(CultureInfo.InvariantCulture) });
            }

            return sectorList;
        }
        public List<SelectListItem> GetProductList(string searchName = "")
        {
            var listAllproducts = string.IsNullOrEmpty(searchName)
                ? _tenderInfo.ListAllProducts().ToList().OrderBy(x => random.Next()).Take(TotalGetData).ToList()
                : _tenderInfo.SearchProducts(searchName).ToList();

            var sectorList = new List<SelectListItem>();

            for (var i = 0; i < listAllproducts.Count(); i++)
            {
                sectorList.Insert(i, new SelectListItem { Text = listAllproducts[i].ProductsName, Value = listAllproducts[i].ProductsId.ToString(CultureInfo.InvariantCulture) });
            }

            return sectorList;
        }
        public List<SelectListItem> GetOwnershipList()
        {
            var listAllownership = _tenderInfo.ListOwnership().ToList();
            var ownershipList = new List<SelectListItem>();

            for (var i = 0; i < listAllownership.Count(); i++)
            {
                ownershipList.Insert(i, new SelectListItem { Text = listAllownership[i].OwnershipName, Value = listAllownership[i].OwnershipId.ToString(CultureInfo.InvariantCulture) });
            }

            return ownershipList;
        }
        public List<SelectListItem> GetKeywordList(string searchName = "")
        {
            var listAllProducts = string.IsNullOrEmpty(searchName)
               ? _tenderInfo.SearchProducts(searchName).OrderBy(x => random.Next()).Take(TotalGetData).ToList()
               : _tenderInfo.SearchProducts(searchName).ToList();

            var productList = new List<SelectListItem>();
            for (var i = 0; i < listAllProducts.Count(); i++)
            {
                productList.Insert(i, new SelectListItem { Text = listAllProducts[i].ProductsName, Value = listAllProducts[i].ProductsId.ToString(CultureInfo.InvariantCulture) });
            }

            return productList;
        }
        public List<SelectListItem> GetGlobalCountryList(string searchName = "")
        {
            var country = string.IsNullOrEmpty(searchName)
                ? _tenderInfo.ListGlobalCountry().OrderBy(x => random.Next()).Take(TotalGetData).ToList()
                : _tenderInfo.ListGlobalCountryBySearch(searchName).ToList();

            var countryList = new List<SelectListItem>();
            for (var i = 0; i < country.Count(); i++)
            {
                countryList.Insert(i, new SelectListItem { Text = country[i].CountryName, Value = country[i].CountryId.ToString(CultureInfo.InvariantCulture) });
            }

            return countryList;
        }


        public List<SelectListItem> CityList(int StateId = 0)
        {

            #region City

            var cityList = new List<SelectListItem>();
            cityList.Insert(0, (new SelectListItem { Text = "[CITY]", Value = "0" }));

            if (StateId != 0)
            {
                var city = _tenderInfo.ListCityByState(StateId).ToList();
                for (var i = 1; i <= city.Count(); i++)
                {
                    cityList.Insert(i, new SelectListItem
                    {
                        Text = city[i - 1].Location,
                        Value = city[i - 1].LocId.ToString(CultureInfo.InvariantCulture)
                    });
                }
            }
            #endregion

            return cityList;
        }
        public List<SelectListItem> StateList(int CountryId = 0)
        {
            CountryId = CountryId == 0
                ? Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"])
                : CountryId;

            #region State
            var states = _tenderInfo.ListStateByCountry(CountryId).ToList();
            var stateList = new List<SelectListItem>();
            stateList.Insert(0, (new SelectListItem { Text = "[STATE]", Value = "0" }));
            for (var i = 1; i <= states.Count(); i++)
            {
                stateList.Insert(i, new SelectListItem { Text = states[i - 1].StateName, Value = states[i - 1].StateId.ToString(CultureInfo.InvariantCulture) });
            }
            #endregion

            return stateList;
        }
        public List<SelectListItem> CountryList()
        {
            var IndiaCountryID = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaCountryID"]);
            #region Country
            var country = _tenderInfo.ListCountry().ToList();
            var countryList = new List<SelectListItem>();
            countryList.Insert(0, (new SelectListItem { Text = "[COUNTRY]", Value = "0" }));
            for (var i = 1; i <= country.Count(); i++)
            {
                countryList.Insert(i, new SelectListItem
                {
                    Text = country[i - 1].CountryName,
                    Value = country[i - 1].CountryId.ToString(CultureInfo.InvariantCulture),
                    Selected = country[i - 1].CountryId == IndiaCountryID ? true : false
                });
            }
            #endregion

            return countryList;
        }
    }


    public enum TenderStatusFlags
    {
        AllTenders = 0,
        LiveTenders = 1,
        NewTenders = 2,
        ClosedTenders = 3
    }

    public class TenderStatusValues
    {
        public static string NewTenders = ConfigurationManager.AppSettings["NewTenders"];
        public static string LiveTenders = ConfigurationManager.AppSettings["LiveTenders"];
        public static string ClosedTenders = ConfigurationManager.AppSettings["ClosedTenders"];
        public static string AllTenders = ConfigurationManager.AppSettings["AllTenders"];
    }
}
