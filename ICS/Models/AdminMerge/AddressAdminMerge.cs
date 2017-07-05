using System.Collections.Generic;

namespace ICS.Models.AdminMerge
{
    public class AddressAdminMerge
    {
        public IEnumerable<Address_Translate> addresses { get; set; }
        public Address_Translate address_Translate { get; set; }
        public IEnumerable<Language> languages { get; set; }
        public int defaultLanguageID { get; set; }
    }
}