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
    
    public partial class tbOwnership
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbOwnership()
        {
            this.tbAgencyIndians = new HashSet<tbAgencyIndian>();
        }
    
        public int OwnershipId { get; set; }
        public string OwnershipName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbAgencyIndian> tbAgencyIndians { get; set; }
    }
}
