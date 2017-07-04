using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Models.Merge
{
    public class OrderServiceMerge
    {
        public Order order { get; set; }
        public List<ServicesForOrder> servicesForOrder { get; set; }
    }
}