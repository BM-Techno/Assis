using System;

namespace TenderAssist.ViewModel
{
    public class TenderDetails
    {
        public int OurRefNo { get; set; }
        public string TenderNo { get; set; }
        public string WorkDesc { get; set; }
        public string TenderSummary { get; set; }

        public decimal TenderAmount { get; set; }
        public decimal EarnestAmount { get; set; }
        public decimal DocCost { get; set; }

        public Nullable<DateTime> PurFromDate { get; set; }
        public Nullable<DateTime> PurToDate { get; set; }
        public DateTime SubmDate { get; set; }
        public DateTime PubDate { get; set; }
        public DateTime OpenDate { get; set; }
        public Nullable<DateTime> dt { get; set; }

        public int LocId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public int AgencyId { get; set; }
        public int SectorId { get; set; }
        public int OwnershipId { get; set; }
        public int DocumentId { get; set; }
        public int RefId { get; set; }

        public string Location { get; set; }

        public string Language { get; set; }
        public string RefSource { get; set; }
        public string Ncbicb { get; set; }
        public string Corrigendum { get; set; }
        public string RandomNumber { get; set; }
    }
}
