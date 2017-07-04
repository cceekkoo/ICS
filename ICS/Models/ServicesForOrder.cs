using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Models
{
    public class ServicesForOrder
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Value_ID { get; set; }
        public int Language_ID { get; set; }
        public bool Is_Selected { get; set; }
    }
}