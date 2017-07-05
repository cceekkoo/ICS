using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Models.AdminMerge
{
    public class AccountAdminMerge
    {
        public User user { get; set; }
        public User edituser { get; set; }
        public ChangePassword changePassword { get; set; }
    }
}