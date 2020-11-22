using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TenderAssist.CommonHelper;
using TenderAssist.Models.DBConnection;
using TenderAssist.ViewModel;

namespace TenderAssist.Models
{

    public class TenderInformation
    {
        readonly TenderAssistEntities _db = new TenderAssistEntities();
        readonly LocalTenderAssistEntities _dblocal = new LocalTenderAssistEntities();

        #region tbCountry
        /*CACHED COUNTRY*/
        private List<tbCountry> AllCountry()
        {
            List<tbCountry> countrylist;
            if (!GetCache("countryList", out countrylist))
            {
                countrylist = (from co in _db.tbCountries
                               where co.CountryId != 0
                               select co).OrderBy(x => x.CountryId).ToList();
                AddCache(countrylist, "countryList", 30);
            }
            return countrylist;
        }
        public List<tbCountry> ListCountry()
        {
            var countrylist = AllCountry();
            return countrylist;
        }
        public tbCountry GetCountry(int id)
        {
            var countrydet = (from c in AllCountry()
                              where c.CountryId == id
                              select c).FirstOrDefault();

            return countrydet;
        }
        public tbCountry GetCountryWithCity(int locid)
        {
            var countrydet = (from c in _db.tbCountries
                              join s in _db.tbStates on c.CountryId equals s.CountryId
                              join l in _db.tbLocations on s.StateId equals l.StateId
                              where l.LocId == locid
                              select c).FirstOrDefault();

            return countrydet;
        }
        public List<tbCountry> ListCountrybyRegion(int regionId)
        {
            return (from c in AllCountry()
                    join r in _db.tbCountryRegionAssocations on c.CountryId equals r.CountryId
                    where r.RegionId == regionId
                    select c
                    ).ToList();
        }
        public List<tbCountry> ListGlobalCountry()
        {
            return (from c in AllCountry()
                    join r in _db.tbCountryRegionAssocations on c.CountryId equals r.CountryId
                    select c
                    ).ToList();
        }
        public List<tbCountry> ListGlobalCountryBySearch(string countryName)
        {
            var countrylist = (from ct in ListGlobalCountry()
                               where ct.CountryId != 0 && ct.CountryName.ToLower().StartsWith(countryName.ToLower())
                               select ct).OrderBy(x => x.CountryName).ToList();

            return countrylist;
        }
        #endregion

        #region tbState
        /*CACHED STATE*/
        private List<tbState> AllState()
        {
            List<tbState> statelist;
            if (!GetCache("stateList", out statelist))
            {
                statelist = (from st in _db.tbStates
                             where st.StateId != 0
                             select st).OrderBy(x => x.StateName).ToList();
                AddCache(statelist, "stateList", 30);
            }
            return statelist;
        }
        public tbState GetState(int id)
        {
            var statedet = (from st in AllState()
                            where st.StateId == id
                            select st).FirstOrDefault();

            return statedet;
        }
        public List<tbState> ListState()
        {
            var statelist = AllState();
            return statelist;
        }
        public List<tbState> ListStateByCountry(int countryId)
        {
            var statelist = (from st in AllState()
                             where st.CountryId == countryId
                             select st).OrderBy(x => x.StateName).ToList();

            return statelist;
        }
        public List<tbState> ListStateBySearch(int countryId, string stateName = "")
        {
            var statelist = (from st in AllState()
                             where st.CountryId == countryId && st.StateName.ToLower().StartsWith(stateName.ToLower())
                             select st).OrderBy(x => x.StateName).ToList();

            return statelist;
        }
        public tbState GetStateWithCity(int locid)
        {
            var statedet = (from s in _db.tbStates
                            join l in _db.tbLocations on s.StateId equals l.StateId
                            where l.LocId == locid
                            select s).FirstOrDefault();

            return statedet;
        }
        #endregion

        #region tbLocation
        /*CACHED CITY*/
        private List<tbLocation> AllCity()
        {
            List<tbLocation> citylist;
            if (!GetCache("cityList", out citylist))
            {
                citylist = (from ct in _db.tbLocations
                            select ct).OrderBy(x => x.Location).ToList();
                AddCache(citylist, "cityList", 30);
            }
            return citylist;
        }

        public List<tbLocation> ListCityByState(int stateId)
        {
            var citylist = (from ct in AllCity()
                            where ct.StateId == stateId
                            select ct).OrderBy(x => x.Location).ToList();

            return citylist;
        }
        public List<tbLocation> ListAllCity()
        {
            var citylist = AllCity();
            return citylist;
        }
        public List<tbLocation> ListCityByCountry(int countryId)
        {
            var citylist = (from ct in AllCity()
                            join st in AllState() on ct.StateId equals st.StateId
                            where st.CountryId == countryId
                            select ct).OrderBy(x => x.Location).ToList();

            return citylist;
        }
        public tbLocation GetCity(int id)
        {
            var citydet = (from ct in AllCity()
                           where ct.LocId == id
                           select ct).FirstOrDefault();

            return citydet;
        }
        public List<tbLocation> ListCityBySearch(int countryId, string locationname)
        {
            var citylist = (from ct in ListCityByCountry(countryId)
                            where ct.LocId != 0 && ct.Location.ToLower().StartsWith(locationname.ToLower())
                            select ct).OrderBy(x => x.Location).ToList();

            return citylist;
        }
        #endregion

        #region tbAgencyIndian
        /*CACHED AGENCY*/
        private List<tbAgencyIndian> AllAgency()
        {
            List<tbAgencyIndian> agencylist;
            if (!GetCache("agencyList", out agencylist))
            {
                agencylist = (from agc in _db.tbAgencyIndians
                              select agc).OrderBy(x => x.AgencyName).ToList();
                AddCache(agencylist, "agencyList", 3);
            }
            return agencylist;
        }
        public List<tbAgencyIndian> ListAgencyMaster()
        {
            var categorylist = AllAgency();
            return categorylist;
        }
        public List<tbAgencyIndian> SearchAgencyMaster(string agencyname)
        {
            var categorylist = (from agc in AllAgency()
                                where agc.AgencyId != 0 && agc.AgencyName.ToLower().StartsWith(agencyname.ToLower())
                                select agc).OrderBy(x => x.AgencyName).ToList();

            return categorylist;
        }
        public tbAgencyIndian GetAgencyDetailById(int id)
        {
            var agcdet = (from st in AllAgency()
                          where st.AgencyId == id
                          select st).FirstOrDefault();

            return agcdet;
        }

        #endregion

        #region tbReference
        /*CACHED Reference*/
        private List<tbReference> AllReference()
        {
            List<tbReference> referencelist;
            if (!GetCache("referenceList", out referencelist))
            {
                referencelist = (from cat in _db.tbReferences
                                 where cat.RefId != 0
                                 select cat).OrderBy(x => x.RefSource).ToList();
                AddCache(referencelist, "referenceList", 30);
            }
            return referencelist;
        }
        public tbReference GetReference(int id)
        {
            var refdet = (from st in AllReference()
                          where st.RefId == id
                          select st).FirstOrDefault();

            return refdet;
        }
        public List<tbReference> ListAllReference()
        {
            var referencelist = AllReference();
            return referencelist;
        }
        #endregion

        #region tbIndustry
        /*CACHED Industry*/
        private List<tbIndustry> AllIndustry()
        {
            List<tbIndustry> industrylist;
            if (!GetCache("industryList", out industrylist))
            {
                industrylist = (from cat in _db.tbIndustries
                                where cat.IndustryId != 0
                                select cat).OrderBy(x => x.IndustryName).ToList();
                AddCache(industrylist, "industryList", 30);
            }
            return industrylist;
        }
        public List<tbIndustry> ListIndustry()
        {
            var indlist = AllIndustry();
            return indlist;
        }
        public tbIndustry GetIndustryById(int id)
        {
            var subindlist = (from i in AllIndustry()
                              where i.IndustryId == id
                              select i).OrderBy(x => x.IndustryName).FirstOrDefault();

            return subindlist;
        }
        public List<tbIndustry> ListIndustryBySearch(string indName)
        {
            var indlist = (from ind in AllIndustry()
                           where ind.IndustryName.ToLower().StartsWith(indName.ToLower())
                           select ind).OrderBy(x => x.IndustryName).ToList();

            return indlist;
        }
        #endregion

        #region tbSubIndustry
        /*CACHED SubIndustry*/
        private List<tbSubIndustry> AllSubIndustry()
        {
            List<tbSubIndustry> subindustrylist;
            if (!GetCache("subindustryList", out subindustrylist))
            {
                subindustrylist = (from ct in _db.tbSubIndustries
                                   select ct).OrderBy(x => x.SubIndustryName).ToList();
                AddCache(subindustrylist, "subindustryList", 30);
            }
            return subindustrylist;
        }
        public List<tbSubIndustry> ListSubIndustryByInd(int indId)
        {
            var subindlist = (from ct in AllSubIndustry()
                              where ct.IndustryId == indId
                              select ct).OrderBy(x => x.SubIndustryName).ToList();

            return subindlist;
        }
        public List<tbSubIndustry> ListAllSubIndustry()
        {
            var subindlist = (from ct in AllSubIndustry()
                              select ct).OrderBy(x => x.SubIndustryName).ToList();

            return subindlist;
        }
        public tbSubIndustry GetSubIndById(int id)
        {
            var subindlist = (from ct in AllSubIndustry()
                              where ct.SubIndustryId == id
                              select ct).OrderBy(x => x.SubIndustryName).FirstOrDefault();

            return subindlist;
        }
        public tbSubIndustry GetSubIndByIndId(int id)
        {
            var subindlist = (from ct in AllSubIndustry()
                              where ct.IndustryId == id
                              select ct).OrderBy(x => x.SubIndustryName).FirstOrDefault();

            return subindlist;
        }
        public List<tbSubIndustry> ListSubIndustryBySearch(string subindName)
        {
            var subindlist = (from ind in AllSubIndustry()
                              where ind.SubIndustryName.ToLower().StartsWith(subindName.ToLower())
                              select ind).OrderBy(x => x.SubIndustryName).ToList();

            return subindlist;
        }
        #endregion

        #region tbProducts
        /*CACHED PRODUCTS*/
        private List<tbProduct> AllProducts()
        {
            List<tbProduct> productlist;
            if (!GetCache("productList", out productlist))
            {
                productlist = (from p in _db.tbProducts
                               select p).OrderBy(x => x.ProductsName).ToList();
                AddCache(productlist, "productList", 1);
            }
            return productlist;
        }
        public List<tbProduct> ListAllProducts()
        {
            var productlist = AllProducts();
            return productlist;
        }
        public tbProduct GetProductById(int id)
        {
            var productlist = (from p in AllProducts()
                               where p.ProductsId == id
                               select p).OrderBy(x => x.ProductsName).FirstOrDefault();

            return productlist;
        }
        public List<tbProduct> GetExactProduct(string productname)
        {
            List<tbProduct> productlist;
            productlist = (from p in AllProducts()
                           where p.ProductsId != 0 && p.ProductsName.ToLower().Trim().Equals(productname.ToLower().Trim())
                           select p).OrderBy(x => x.ProductsName).ToList();
            return productlist;
        }
        public List<tbProduct> SearchProducts(string productname)
        {
            List<tbProduct> productlist;
            if (productname.Trim() != "")
            {

                productlist = (from p in AllProducts()
                               where p.ProductsId != 0 && p.ProductsName.ToLower().StartsWith(productname.ToLower())
                               select p).OrderBy(x => x.ProductsName).ToList();
            }
            else
            {
                productlist = (from p in AllProducts()
                               where p.ProductsId != 0
                               select p).OrderBy(x => x.ProductsName).ToList();
            }
            return productlist;
        }
        public List<tbProduct> GetRelatedProducts(int productId)
        {
            var firstOrDefault = AllProducts().FirstOrDefault(x => x.ProductsId == productId);
            if (firstOrDefault != null)
            {
                var subindbyprod = firstOrDefault.SubIndustryId;
                var productlist = (from pr in AllProducts()
                                   where subindbyprod == pr.SubIndustryId
                                   select pr).OrderBy(x => x.ProductsName).ToList();


                return productlist;
            }
            return null;
        }
        #endregion

        #region tbSector
        /*CACHED SECTOR*/
        private List<tbSector> AllCompanySector()
        {
            List<tbSector> sectorlist;
            if (!GetCache("sectorList", out sectorlist))
            {
                sectorlist = (from sect in _db.tbSectors
                              where sect.SectorId != 0
                              select sect).OrderBy(x => x.SectorName).ToList();
                AddCache(sectorlist, "sectorList", 3);
            }
            return sectorlist;
        }

        public List<tbSector> ListSector()
        {
            return AllCompanySector();
        }

        public tbSector GetSectorByAgencyId(int agcid)
        {
            var sectdet = (from st in AllCompanySector()
                           join agc in AllAgency() on st.SectorId equals agc.SectorId
                           where agc.AgencyId == agcid
                           select st).FirstOrDefault();
            return sectdet;
        }

        public tbSector GetSectorById(int id)
        {
            var sectdet = (from st in AllCompanySector()
                           where st.SectorId == id
                           select st).FirstOrDefault();
            return sectdet;
        }

        public List<tbSector> SearchCompanySectorMaster(string sectorname)
        {
            var sectorlist = (from sect in AllCompanySector()
                              where sect.SectorId != 0 && sect.SectorName.ToLower().StartsWith(sectorname.ToLower())
                              select sect).OrderBy(x => x.SectorName).ToList();

            return sectorlist;
        }

        #endregion

        #region tbOwnership
        public List<tbOwnership> ListOwnership()
        {
            var ownershiplist = (from ow in _db.tbOwnerships
                                 where ow.OwnershipId != 0
                                 select ow).OrderBy(x => x.OwnershipName).ToList();

            return ownershiplist;
        }

        public tbOwnership GetOwnershipById(int id)
        {
            var ownershipdet = (from ow in _db.tbOwnerships
                                where ow.OwnershipId == id
                                select ow).FirstOrDefault();
            return ownershipdet;
        }
        #endregion


        #region INDIAN CATEGORY
        public List<StateCityForIndianTenders> GetStateCityForIndianTenders(string searchText)
        {
            List<StateCityForIndianTenders> itemList = new List<StateCityForIndianTenders>();
            var result = _db.Proc_Site_GetStateCityForIndianTenders(searchText);
            Utility.ToList(result, itemList);

            return itemList;

        }
        public List<tbProduct> GetProductsForIndianTenders(string searchText, string wordSearch)
        {
            List<tbProduct> itemList = new List<tbProduct>();
            var result = _db.Proc_Site_GetProductForIndianTenders(searchText, wordSearch);
            Utility.ToList(result, itemList);

            return itemList;
        }
        public List<IndustrySubIndustryDetail> GetIndustrySubIndustryList(string searchText)
        {
            List<IndustrySubIndustryDetail> itemList = new List<IndustrySubIndustryDetail>();
            var result = _db.Proc_Site_GetIndustrySubIndustryList(searchText);
            Utility.ToList(result, itemList);

            return itemList;
        }
        public List<AgencySectorOwhershipDetail> GetAgencySectorOwnershipForIndianTenders(string searchText)
        {
            List<AgencySectorOwhershipDetail> itemList = new List<AgencySectorOwhershipDetail>();
            var result = _db.Proc_Site_GetOwnershipSectorAgencyForIndianTenders(searchText);
            Utility.ToList(result, itemList);

            return itemList;
        }
        //public List<tbSector> GetSectorForIndianTenders(string searchText)
        //{
        //    List<tbSector> itemList = new List<tbSector>();
        //    var result = _db.Proc_Site_GetSectorForIndianTenders(searchText);
        //    Utility.ToList(result, itemList);

        //    return itemList;
        //}
        //public List<tbOwnership> GetOwnershipForIndianTenders(string searchText)
        //{
        //    List<tbOwnership> itemList = new List<tbOwnership>();
        //    var result = _db.Proc_Site_GetOwnershipForIndianTenders(searchText);
        //    Utility.ToList(result, itemList);

        //    return itemList;
        //}
        #endregion


        #region CACHING RELATED CODE

        public static void AddCache<T>(T o, string key, int days)
        {
            System.Web.HttpContext.Current.Cache.Insert(key, o, null, DateTime.Now.AddDays(days), TimeSpan.Zero);
        }

        public static void ClearCache(string key)
        {
            System.Web.HttpContext.Current.Cache.Remove(key);
        }

        public static bool CacheExists(string key)
        {
            return System.Web.HttpContext.Current.Cache[key] != null;
        }

        public static bool GetCache<T>(string key, out T value)
        {
            try
            {
                if (!CacheExists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T)System.Web.HttpContext.Current.Cache[key];
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }

        #endregion


        public TenderDetailInfo GetAllSearchIndianTenderList(int tenderBy, int searchType, string searchText,
             string productIds = "", string locationIds = "", string indSubindIds = "", string agencyIds = "", string sectorIds = "", string ownershipIds = "", string refIds = "",
              int? tenderStatus = null, int start = 0, int limit = 20, bool isSearchWithCount = true, string tenderYear = null, int? tenderType = null,
              int? TenderValueType = null, DateTime? subDateFrom = null, DateTime? subDateTo = null, DateTime? opDateFrom = null, DateTime? opDateTo = null,
              int? ourRefNo = null, bool? icbNcb = null, string OtherSearchText = null, string WithinSearchText = null, int? orderByType = null)
        {
            List<SearchTenaderInfoWithAllDetail> tenderList = new List<SearchTenaderInfoWithAllDetail>();
            List<TenderCount> tenderCount = new List<TenderCount>();

            if (orderByType == null)
            { orderByType = 3; }

            var resultData = _db.Proc_TA_SearchIndianTenderList(tenderBy, searchType, searchText, productIds, locationIds, indSubindIds, agencyIds, sectorIds, ownershipIds, refIds,
               tenderStatus, start, limit, false, tenderYear, tenderType, TenderValueType, subDateFrom, subDateTo, opDateFrom, opDateTo, ourRefNo, icbNcb,
               OtherSearchText, WithinSearchText, orderByType);
            Utility.ToList(resultData, tenderList);

            if (isSearchWithCount)
            {
                var resultCount = _db.Proc_TA_SearchIndianTenderCount(tenderBy, searchType, searchText, productIds, locationIds, indSubindIds, agencyIds, sectorIds, ownershipIds, refIds,
                   tenderStatus, start, limit, true, tenderYear, tenderType, TenderValueType, subDateFrom, subDateTo, opDateFrom, opDateTo, ourRefNo, icbNcb,
                   OtherSearchText, WithinSearchText);
                Utility.ToList(resultCount, tenderCount);
            }


            TenderDetailInfo tenders = new TenderDetailInfo()
            {
                TenaderDetailSearch = tenderList,
                TenaderDetailCount = tenderCount
            };

            return tenders;
        }
        public TenderDocFile GetTenderDocFilesById(string refno)
        {
            int refNo = refno == "" ? 0 : Convert.ToInt32(refno);
            return _db.TenderDocFiles.Where(x => x.OurRefNo == refNo)
                        .Select((t => t)).SingleOrDefault();
        }

        public List<SearchProductList> SearchAutoCompleteKeywords(string searchText)
        {
            List<SearchProductList> itemList = new List<SearchProductList>();
            var result = _db.Proc_Site_SearchProductsNameAsKeyWord(searchText);
            Utility.ToList(result, itemList);

            return itemList;
        }

        public TenderDetails GetTenderInfoById(int refno, string tenderYear)
        {
            var data = _db.Proc_GetTenderInformationById(refno, tenderYear);

            var tenderdet = data.Select(c => ConvertGetTenderInfoById(c)).FirstOrDefault<TenderDetails>();
            return tenderdet;
        }
        public TenderDetails ConvertGetTenderInfoById(Proc_GetTenderInformationById_Result result)
        {
            var tenderdet = new TenderDetails()
            {
                AgencyId = result.AgencyId.Value,
                OwnershipId = result.OwnershipId.Value,
                SectorId = result.SectorId.Value,
                StateId = result.StateId.Value,
                LocId = result.LocId.Value,
                CountryId = result.CountryId.Value,
                DocumentId = result.DocumentId.Value,
                dt = result.Dt.Value,
                SubmDate = result.SubmDate.Value,
                OpenDate = result.OpenDate.Value,
                PubDate = result.PubDate.Value,
                Corrigendum = result.Corrigendum,
                Ncbicb = result.NCBICB,
                WorkDesc = result.WorkDesc,
                OurRefNo = result.OurRefNo.Value,
                TenderNo = result.TenderNo,
                TenderSummary = result.TenderSummary,
                TenderAmount = result.TenderAmount.Value,
                EarnestAmount = result.EarnestAmount.Value,
                DocCost = result.DocCost.Value,
                RefId = result.RefId.Value,
                RandomNumber = result.RandomNumber,
                Location = result.Location,
                //AgencyName = result.AgencyName,
                //SectorName = result.SectorName,
                //OwnershipName = result.OwnershipName,
                Language = result.Language,
                RefSource = result.RefSource,
            };

            return tenderdet;
        }


        public TenderDetails GetGlobalTenderInfoById(string refno)
        {
            int refNo = refno == "" ? 0 : Convert.ToInt32(refno);
            var detail = (from global in _db.TenderInfo_Global
                         where global.OurRefNo == refNo
                         select new TenderDetails()
                         {
                             AgencyId = global.AgencyId,
                             StateId = global.StateId,
                             LocId = global.LocId,
                             CountryId = global.CountryId,
                             DocumentId = global.DocumentId,
                             dt = global.dt,
                             SubmDate = global.SubmDate,
                             OpenDate = global.OpenDate,
                             PubDate = global.PubDate,
                             Corrigendum = global.isCorrigendum ? "Corrigendum" : "",
                             WorkDesc = global.WorkDesc,
                             OurRefNo = global.OurRefNo,
                             TenderNo = global.TenderNo,
                             TenderSummary = global.TenderSummary,
                             TenderAmount = global.TenderAmount,
                             EarnestAmount = global.EarnestAmount,
                             DocCost = global.DocCost,
                             RefId = global.RefId,
                             RandomNumber = global.Rannumber.ToString(),
                             //Location = global.Location,
                             //AgencyName = result.AgencyName,
                             //SectorName = result.SectorName,
                             //OwnershipName = result.OwnershipName,
                             Language = global.Language,
                             //RefSource = global.RefSource,
                         }).SingleOrDefault();
            return detail;
        }

        public object SubmitInquiryRegForms(InquiryRegFormFields RegFormParams)
        {
            var result = _db.Proc_Site_TBInquiryRegForm_Add(RegFormParams.InquiryTypeID, RegFormParams.Name, RegFormParams.Designation, RegFormParams.CompName, RegFormParams.Address,
                RegFormParams.Country, RegFormParams.State, RegFormParams.City, RegFormParams.MobNo, RegFormParams.PhoneNo, RegFormParams.InterestedTenders, RegFormParams.Website,
                RegFormParams.intClientPurpose, RegFormParams.EmailID, RegFormParams.OurRefNo, RegFormParams.NewID, RegFormParams.Flag, RegFormParams.ModuleType, RegFormParams.LinkId,
                RegFormParams.BrowserLink, RegFormParams.FormTitle, RegFormParams.ClientIPAddress, "");


            var reqid = result.Select(c => GetInquiryId(c)).FirstOrDefault<int>();


            return reqid.ToString();
        }
        public int GetInquiryId(Nullable<int> result)
        {
            return result.Value;
        }




        /*For User*/
        public TenderDetailInfo GetAllSearchTenderInfo_Client(int? permissionId, int? start, int? limit, int? tenderStatusFlag,
            DateTime? enterDate, string searchText, int? OurRefNo, string ownershipId, string sectorId, string agencyId, string indSub, string loc,
            string keyword1, string keyword2, string keyword3, string otherKeywords, string notUsedKeywords,
            string documentType, string isIcbncb, string tenderValue, decimal? tenderValueFrom, decimal? tenderValueTo, Boolean indianGlobal,
            Boolean IsOnlyCount, string WithinSearchText = "",
            DateTime? subDateFrom = null, DateTime? subDateTo = null, DateTime? opDateFrom = null, DateTime? opDateTo = null, string globalCountryIds = "")//int? orderByType = null
        {
            List<SearchTenaderInfoWithAllDetail> tenderList = new List<SearchTenaderInfoWithAllDetail>();
            List<TenderCount> tenderCount = new List<TenderCount>();

            var resultData = _db.Proc_TA_ClientTenderList(permissionId, start, limit, tenderStatusFlag == null ? "" : tenderStatusFlag.Value.ToString(), enterDate, searchText, OurRefNo, ownershipId, sectorId, agencyId, indSub, loc,
                keyword1, keyword2, keyword3, otherKeywords, notUsedKeywords, documentType, isIcbncb, tenderValue, tenderValueFrom, tenderValueTo, indianGlobal, WithinSearchText,
                subDateFrom, subDateTo, opDateFrom, opDateTo, globalCountryIds);
            Utility.ToList(resultData, tenderList);

            if (IsOnlyCount)
            {
                var resultCount = _db.Proc_TA_ClientTenderCount(permissionId, start, limit, tenderStatusFlag == null ? "" : tenderStatusFlag.Value.ToString(), enterDate, searchText, OurRefNo, ownershipId, sectorId, agencyId, indSub, loc,
                keyword1, keyword2, keyword3, otherKeywords, notUsedKeywords, documentType, isIcbncb, tenderValue, tenderValueFrom, tenderValueTo, indianGlobal, WithinSearchText,
                subDateFrom, subDateTo, opDateFrom, opDateTo, globalCountryIds);
                Utility.ToList(resultCount, tenderCount);
            }

            TenderDetailInfo tenders = new TenderDetailInfo()
            {
                TenaderDetailSearch = tenderList,
                TenaderDetailCount = tenderCount
            };

            return tenders;
        }


        public tabClientDetail GetUsersByUniqueId(Guid userUniqueNo)
        {
            return _db.tabClientDetails.Where(u => u.uniUniqueNo.ToString() == userUniqueNo.ToString()).FirstOrDefault();
        }


        #region FOR USER : TENDER INFO WITH CRITERIA & COUNT Sample Tender

        public TenderDetailInfo GetAllSearchTenderInfo_Client_SampleTender(int? permissionId, int? start, int? limit, int? tenderStatusFlag, bool? iscount,
            ref string ownershipId, ref string ownershipIdNotUsedIn, ref string sectorId, ref string sectorIdNotUsedIn,
            ref string agencyId, ref string agencyIdNotUsedIn, ref string indSub, ref string indSubNotUsedIn,
            ref string loc, ref string locNotUsedIn, ref string keyword1, ref string keyword2, ref string keyword3,
            ref string otherKeywords, ref string notUsedKeywords, ref string documentType, ref string isIcbncb,
            ref string tenderValue, ref double tenderValueFrom, ref double tenderValueTo,
            ref Boolean indianGlobal, ref string finalSearchText, string tenderyear, string OrderBys, string AscDesc, bool IsOnlyCount = false)//int? orderByType = null
        {
            List<SearchTenaderInfoWithAllDetail> tenderList = new List<SearchTenaderInfoWithAllDetail>();
            List<TenderCount> tenderCount = new List<TenderCount>();

            var resultData = _dblocal.Proc_TA_ClientSampleTender_List(permissionId, start, limit,
                tenderStatusFlag == null ? "" : tenderStatusFlag.Value.ToString(),
                new System.Data.Entity.Core.Objects.ObjectParameter("OwnershipID", ownershipId),
                new System.Data.Entity.Core.Objects.ObjectParameter("OwnershipIDNotUsedIn", ownershipIdNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("SectorID", sectorId),
                new System.Data.Entity.Core.Objects.ObjectParameter("SectorIDNotUsedIn", sectorIdNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("AgencyID", agencyId),
                new System.Data.Entity.Core.Objects.ObjectParameter("AgencyIDNotUsedIn", agencyIdNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("IndSub", indSub),
                new System.Data.Entity.Core.Objects.ObjectParameter("IndSubNotUsedIn", indSubNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("Loc", loc),
                new System.Data.Entity.Core.Objects.ObjectParameter("LocNotUsedIn", locNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("Keyword1", keyword1),
                new System.Data.Entity.Core.Objects.ObjectParameter("Keyword2", keyword2),
                new System.Data.Entity.Core.Objects.ObjectParameter("Keyword3", keyword3),
                new System.Data.Entity.Core.Objects.ObjectParameter("OtherKeywords", otherKeywords),
                new System.Data.Entity.Core.Objects.ObjectParameter("NotUsedKeywords", notUsedKeywords),
                new System.Data.Entity.Core.Objects.ObjectParameter("DocumentType", documentType),
                new System.Data.Entity.Core.Objects.ObjectParameter("IsICBNCB", isIcbncb == null ? "" : isIcbncb),
                new System.Data.Entity.Core.Objects.ObjectParameter("bitTenderAmount", tenderValue),
                new System.Data.Entity.Core.Objects.ObjectParameter("TenderValueFrom", tenderValueFrom),
                new System.Data.Entity.Core.Objects.ObjectParameter("TenderValueTo", tenderValueTo),
                new System.Data.Entity.Core.Objects.ObjectParameter("IndianGlobal", indianGlobal),
                new System.Data.Entity.Core.Objects.ObjectParameter("finalSearchText", finalSearchText),
                tenderyear, OrderBys, AscDesc);
            Utility.ToList(resultData, tenderList);

            if (iscount.Value)
            {
                var resultCount = _dblocal.Proc_TA_ClientSampleTender_Count(permissionId, start, limit,
                tenderStatusFlag == null ? "" : tenderStatusFlag.Value.ToString(),
                new System.Data.Entity.Core.Objects.ObjectParameter("OwnershipID", ownershipId),
                new System.Data.Entity.Core.Objects.ObjectParameter("OwnershipIDNotUsedIn", ownershipIdNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("SectorID", sectorId),
                new System.Data.Entity.Core.Objects.ObjectParameter("SectorIDNotUsedIn", sectorIdNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("AgencyID", agencyId),
                new System.Data.Entity.Core.Objects.ObjectParameter("AgencyIDNotUsedIn", agencyIdNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("IndSub", indSub),
                new System.Data.Entity.Core.Objects.ObjectParameter("IndSubNotUsedIn", indSubNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("Loc", loc),
                new System.Data.Entity.Core.Objects.ObjectParameter("LocNotUsedIn", locNotUsedIn),
                new System.Data.Entity.Core.Objects.ObjectParameter("Keyword1", keyword1),
                new System.Data.Entity.Core.Objects.ObjectParameter("Keyword2", keyword2),
                new System.Data.Entity.Core.Objects.ObjectParameter("Keyword3", keyword3),
                new System.Data.Entity.Core.Objects.ObjectParameter("OtherKeywords", otherKeywords),
                new System.Data.Entity.Core.Objects.ObjectParameter("NotUsedKeywords", notUsedKeywords),
                new System.Data.Entity.Core.Objects.ObjectParameter("DocumentType", documentType),
                new System.Data.Entity.Core.Objects.ObjectParameter("IsICBNCB", isIcbncb == null ? "" : isIcbncb),
                new System.Data.Entity.Core.Objects.ObjectParameter("bitTenderAmount", tenderValue),
                new System.Data.Entity.Core.Objects.ObjectParameter("TenderValueFrom", tenderValueFrom),
                new System.Data.Entity.Core.Objects.ObjectParameter("TenderValueTo", tenderValueTo),
                new System.Data.Entity.Core.Objects.ObjectParameter("IndianGlobal", indianGlobal),
                new System.Data.Entity.Core.Objects.ObjectParameter("finalSearchText", finalSearchText),
                tenderyear, OrderBys, AscDesc);
                Utility.ToList(resultCount, tenderCount);
            }

            TenderDetailInfo tenders = new TenderDetailInfo()
            {
                TenaderDetailSearch = tenderList,
                TenaderDetailCount = tenderCount
            };

            return tenders;
        }

        public List<SearchTenaderInfoWithAllDetail> GetTenderInfoByOurRefNo(string ourrefNos, Boolean? type)
        {
            List<SearchTenaderInfoWithAllDetail> tenderList = new List<SearchTenaderInfoWithAllDetail>();
            var resultData = _db.Proc_Site_GetTenderInfoByOurRefNo_New(ourrefNos, type);
            Utility.ToList(resultData, tenderList);
            return tenderList;
        }
        public void InsertSendSampleTenders(int intClientId)
        {
            _dblocal.Proc_Site_I_SendSampleTenders(intClientId);
            //SqlParameter sqlParameter = new SqlParameter();
            //sqlParameter.ParameterName = "@intClientId";
            //sqlParameter.Value = (object)intClientId;
            //sqlParameter.SqlDbType = SqlDbType.Int;
            //object[] objArray = new object[1]
            //  {
            //    (object) sqlParameter
            //  };

            //this._dblocal.CommandTimeout = new int?(0);
            //this._dblocal.ExecuteStoreQuery<SearchTenaderInfoWithAllDetail>("exec Proc_Site_I_SendSampleTenders @intClientId", objArray);
        }
        #endregion




        /*For Global*/
        public TenderDetailInfo GetAllSearchGlobalTenderList(int searchType, string searchText, string countryIds = "",
            int? tenderStatus = null, int start = 0, int limit = 20, bool isSearchWithCount = true,
            string productIds = "", string agencyIds = "", int? tenderType = null,
            DateTime? subDateFrom = null, DateTime? subDateTo = null, DateTime? opDateFrom = null, DateTime? opDateTo = null,
            int? ourRefNo = null, string WithinSearchText = null, string OtherSearchText = null)
        {
            List<SearchTenaderInfoWithAllDetail> tenderList = new List<SearchTenaderInfoWithAllDetail>();
            List<TenderCount> tenderCount = new List<TenderCount>();


            var resultData = _db.Proc_TA_SearchGlobalTenderList(searchType, searchText, countryIds, tenderStatus, start, limit, productIds, agencyIds, tenderType,
                subDateFrom, subDateTo, opDateFrom, opDateTo, ourRefNo, WithinSearchText);
            Utility.ToList(resultData, tenderList);

            if (isSearchWithCount)
            {
                var resultCount = _db.Proc_TA_SearchGlobalTenderCount(searchType, searchText, countryIds, productIds, agencyIds, tenderType,
                subDateFrom, subDateTo, opDateFrom, opDateTo, ourRefNo, WithinSearchText);
                Utility.ToList(resultCount, tenderCount);
            }


            TenderDetailInfo tenders = new TenderDetailInfo()
            {
                TenaderDetailSearch = tenderList,
                TenaderDetailCount = tenderCount
            };

            return tenders;
        }

    }
}