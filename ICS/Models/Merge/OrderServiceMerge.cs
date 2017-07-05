using System.Collections.Generic;

namespace ICS.Models.Merge
{
    public class OrderServiceMerge
    {
        public Order order { get; set; }
        public List<ServicesForOrder> servicesForOrder { get; set; }
    }
}