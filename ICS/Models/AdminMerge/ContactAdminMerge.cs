using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Models.AdminMerge
{
    public class ContactAdminMerge
    {
        public IEnumerable<Contact> contacts { get; set; }
        public Contact contact { get; set; }
    }
}