using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICS.Models
{
    public class Login
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "***")]
        public string Username { get; set; }
        [Required(ErrorMessage = "***")]
        public string Password { get; set; }
    }
}