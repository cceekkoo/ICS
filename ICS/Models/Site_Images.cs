//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICS.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class Site_Images
    {
        public int ID { get; set; }
        public string image { get; set; }
        [Required(ErrorMessage = "***")]
        public string Description { get; set; }
    }
}
