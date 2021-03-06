﻿using System.Collections.Generic;

namespace ICS.Models.Merge
{
    public class ContactMerge
    {
        public string menu { get; set; }
        public string image { get; set; }
        public IEnumerable<Site_Contents> site_Contents { get; set; }
        public SendEmail sendEmail { get; set; }
        public string captchaLanguage { get; set; }
    }
}