using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Models.Merge
{
    public class CustomersMerge
    {
        public string menu { get; set; }
        public string image { get; set; }
        public IEnumerable<Customer> customers { get; set; }
    }
}