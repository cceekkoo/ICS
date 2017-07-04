using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Models.Merge
{
    public class OrdersMerge
    {
        public string menu { get; set; }
        public OrderServiceMerge orderServiceMerge { get; set; }
        public IEnumerable<Site_Contents> site_Contents { get; set; }
        public string captchaLanguage { get; set; }
    }
}