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

    public partial class Address_Translate
    {
        public int ID { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public int Value_ID { get; set; }
        public int Language_ID { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Language Language { get; set; }
    }
}
