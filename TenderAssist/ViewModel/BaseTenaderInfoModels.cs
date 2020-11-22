using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TenderAssist.ViewModel
{
    public class TenderDetailInfo
    {
        public List<SearchTenaderInfoWithAllDetail> TenaderDetailSearch { get; set; }
        public List<TenderCount> TenaderDetailCount { get; set; }
    }


    [NotMapped]
    public class SearchTenaderInfoWithAllDetail
    {
        public int Rn { get; set; }
        public int OurRefNo { get; set; }
        public int StateId { get; set; }
        public string Location { get; set; }
        public string WorkDesc { get; set; }
        public DateTime? Dt { get; set; }
        public DateTime DueDate { get; set; }
        public string Language { get; set; }
        public int RefId { get; set; }
        public string RefSource { get; set; }
        public string Ncbicb { get; set; }
        public bool? IsCorrigendum { get; set; }
        public decimal TenderAmount { get; set; }

        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public string SectorName { get; set; }
        public int OwnershipId { get; set; }
        public string OwnershipName { get; set; }
        public Guid? Rannumber { get; set; }
        //public int TotalRec { get; set; }
        public int TenderStatusReturn { get; set; }
        //public int MostCount { get; set; }
        public DateTime OpenDate { get; set; }
        public decimal EMD { get; set; }
        public decimal DocCost { get; set; }
        //public string NewRandNo { get; set; }
    }

    public class TenderCount
    {
        public int TenderStatusReturn { get; set; }
        public Int64 TotalRecord { get; set; }
    }


    public class StateCityForIndianTenders
    {
        public int LocId { get; set; }
        public int StateId { get; set; }

        public string StateName { get; set; }
        public string Location { get; set; }

    }

    public class IndustrySubIndustryDetail
    {
        public int SubIndustryId { get; set; }
        public int IndustryId { get; set; }

        public string SubIndustryName { get; set; }
        public string IndustryName { get; set; }
    }


    public class AgencySectorOwhershipDetail
    {
        public int AgencyId { get; set; }
        public int SectorId { get; set; }
        public int OwnershipId { get; set; }

        public string AgencyName { get; set; }
        public string SectorName { get; set; }
        public string OwnershipName { get; set; }
    }


    public class UserFavouriteTenderList
    {
        public Int32 IntFavTenderId { get; set; }
        public Int32 IntClientId { get; set; }
        public Int64 IntOurRefNo { get; set; }
        public Int64 IntTenderType { get; set; }
        public DateTime DatEnterDate { get; set; }
    }
}
