//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TenderAssist.Models.DBConnection
{
    using System;
    using System.Collections.Generic;
    
    public partial class TenderInfo_Global_Active
    {
        public int OurRefNo { get; set; }
        public System.Guid Rannumber { get; set; }
        public string TenderNo { get; set; }
        public string WorkDesc { get; set; }
        public string TenderSummary { get; set; }
        public decimal TenderAmount { get; set; }
        public decimal EarnestAmount { get; set; }
        public decimal DocCost { get; set; }
        public int CurrencyId { get; set; }
        public Nullable<System.DateTime> PurFromDate { get; set; }
        public Nullable<System.DateTime> PurToDate { get; set; }
        public System.DateTime SubmDate { get; set; }
        public System.DateTime OpenDate { get; set; }
        public int LocId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public int AgencyId { get; set; }
        public int DocumentId { get; set; }
        public int ContactId { get; set; }
        public int RefId { get; set; }
        public System.DateTime PubDate { get; set; }
        public string Language { get; set; }
        public bool isICB { get; set; }
        public bool ishosted { get; set; }
        public bool isCorrigendum { get; set; }
        public int WorkId { get; set; }
        public string TenderWebsite { get; set; }
        public Nullable<System.DateTime> dt { get; set; }
        public int EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> isClassified { get; set; }
    }
}
