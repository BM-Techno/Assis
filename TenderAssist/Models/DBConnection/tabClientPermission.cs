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
    
    public partial class tabClientPermission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tabClientPermission()
        {
            this.tabClientPermissionWithAgencies = new HashSet<tabClientPermissionWithAgency>();
            this.tabClientPermissionWithIndSubIndustries = new HashSet<tabClientPermissionWithIndSubIndustry>();
            this.tabClientPermissionWithLocations = new HashSet<tabClientPermissionWithLocation>();
            this.tabClientPermissionWithOwnerships = new HashSet<tabClientPermissionWithOwnership>();
            this.tabClientPermissionWithProducts = new HashSet<tabClientPermissionWithProduct>();
            this.tabClientPermissionWithSectors = new HashSet<tabClientPermissionWithSector>();
        }
    
        public int intPermissionId { get; set; }
        public int intClientId { get; set; }
        public string strPermissionName { get; set; }
        public bool bitActive { get; set; }
        public bool bitBuy { get; set; }
        public bool bitCell { get; set; }
        public bool bitContract { get; set; }
        public bool bitICB { get; set; }
        public bool bitIndianOrGlobal { get; set; }
        public int intPurpose { get; set; }
        public string strOtherKeywords { get; set; }
        public string strNotUsedKeywords { get; set; }
        public bool bitTenderAmount { get; set; }
        public decimal moneyTenderAmountFrom { get; set; }
        public decimal moneyTenderAmountTo { get; set; }
        public int intEntryBy { get; set; }
        public System.DateTime dtEntryDate { get; set; }
        public Nullable<int> intUpdateBy { get; set; }
        public Nullable<System.DateTime> dtUpdateDate { get; set; }
        public string MachineName { get; set; }
        public string IPAddressOfSQLServer { get; set; }
        public string IPAddressOfClient { get; set; }
    
        public virtual tabClientDetail tabClientDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabClientPermissionWithAgency> tabClientPermissionWithAgencies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabClientPermissionWithIndSubIndustry> tabClientPermissionWithIndSubIndustries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabClientPermissionWithLocation> tabClientPermissionWithLocations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabClientPermissionWithOwnership> tabClientPermissionWithOwnerships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabClientPermissionWithProduct> tabClientPermissionWithProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabClientPermissionWithSector> tabClientPermissionWithSectors { get; set; }
    }
}
