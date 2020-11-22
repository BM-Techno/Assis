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
    
    public partial class Proc_GetTenderInformationById_Result
    {
        public Nullable<int> AgencyId { get; set; }
        public Nullable<int> OwnershipId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> LocId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> DocumentId { get; set; }
        public Nullable<System.DateTime> Dt { get; set; }
        public Nullable<System.DateTime> SubmDate { get; set; }
        public Nullable<System.DateTime> OpenDate { get; set; }
        public Nullable<System.DateTime> PubDate { get; set; }
        public Nullable<bool> IsCorrigendum { get; set; }
        public string Corrigendum { get; set; }
        public Nullable<bool> IsICB { get; set; }
        public string NCBICB { get; set; }
        public string WorkDesc { get; set; }
        public Nullable<int> OurRefNo { get; set; }
        public string TenderNo { get; set; }
        public string TenderSummary { get; set; }
        public Nullable<decimal> TenderAmount { get; set; }
        public Nullable<decimal> EarnestAmount { get; set; }
        public Nullable<decimal> DocCost { get; set; }
        public Nullable<int> RefId { get; set; }
        public string RandomNumber { get; set; }
        public string Location { get; set; }
        public string AgencyName { get; set; }
        public string SectorName { get; set; }
        public string OwnershipName { get; set; }
        public string Language { get; set; }
        public string RefSource { get; set; }
    }
}