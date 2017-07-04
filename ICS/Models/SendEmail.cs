using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICS.Models
{
    public class SendEmail
    {
        public int id { get; set; }
        [Required(ErrorMessage = "***")]
        public string Name { get; set; }
        [Required(ErrorMessage = "***")]
        [EmailAddress(ErrorMessage = "***")]
        public string Email { get; set; }
        [Required(ErrorMessage = "***")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "***")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}