//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace jobportal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class req
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public req()
        {
            this.jobsubmissions = new HashSet<jobsubmission>();
        }
    
        public int reqid { get; set; }
        public Nullable<int> companyid { get; set; }
        public string jobtitle { get; set; }
        public string description { get; set; }
        public string department { get; set; }
        public string location { get; set; }
        public Nullable<System.DateTime> dateopen { get; set; }
        public Nullable<int> Experience { get; set; }
        public Nullable<int> Salary { get; set; }
        public Nullable<int> NOP { get; set; }
        public Nullable<System.DateTime> doe { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> approvalstatus { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<jobsubmission> jobsubmissions { get; set; }
    }
}
