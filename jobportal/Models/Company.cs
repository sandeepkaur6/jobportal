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
    using System.ComponentModel.DataAnnotations;

    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            this.reqs = new HashSet<req>();
        }

        public int Companyid { get; set; }
        [Required(ErrorMessage = "Company Name cannot be empty")]
        public string Companyname { get; set; }
        [Required(ErrorMessage = "Email address cannot be left blank")]
        [EmailAddress(ErrorMessage = "should be abc@xyz.com format")]

        public string email { get; set; }
        [Required(ErrorMessage = "Username cannot be empty")]
        [RegularExpression("^[a-zA-Z]+", ErrorMessage = "username should start with letter")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter the password")]
        [StringLength(18, MinimumLength = 8,ErrorMessage ="Password cannot be less than 8 characters")]
        public string Password { get; set; }
        public string Comweb { get; set; }
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter the phone number")]
        [RegularExpression("^[1-9][0-9]{9}$", ErrorMessage = "Please enter 10 digit mobile number")]
        public string phnumber { get; set; }
        public string IndustryType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<req> reqs { get; set; }
    }
}
