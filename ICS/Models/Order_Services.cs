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
    using System;
    using System.Collections.Generic;
    
    public partial class Order_Services
    {
        public int ID { get; set; }
        public int Order_ID { get; set; }
        public int Service_ID { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Service Service { get; set; }
    }
}