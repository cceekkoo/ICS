using System.Collections.Generic;

namespace ICS.Models.Merge
{
    public class FooterPartialMerge
    {
        public IEnumerable<Site_Contents> site_Contents { get; set; }
        public About_Translate about { get; set; }
        public IEnumerable<Address_Translate> address { get; set; }
        public IEnumerable<Contact> contact { get; set; }
        public IEnumerable<Social> social { get; set; }
    }
}