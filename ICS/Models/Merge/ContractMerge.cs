using System.Collections.Generic;

namespace ICS.Models.Merge
{
    public class ContractMerge
    {
        public string menu { get; set; }
        public Contract contract { get; set; }
        public IEnumerable<Site_Contents> site_Contents { get; set; }
        public string captchaLanguage { get; set; }
    }
}