//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlantNest_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class AdminLogin
    {
        public int AdminID { get; set; }

        [Display(Name = "Name")]
        public string AdName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
