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

    public partial class Contact
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "***")]
        public string Text { get; set; }
        [Required(ErrorMessage = "***")]
        [StringLength(10, ErrorMessage = "10 simvoldan �oxdur")]
        public string icon { get; set; }
    }
}
