using System.ComponentModel.DataAnnotations;

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