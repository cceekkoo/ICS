using System.ComponentModel.DataAnnotations;

namespace ICS.Models
{
    public class ChangePassword
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "***")]
        [StringLength(50, ErrorMessage = "Şifrə 5-50 simvol aralığında ola bilər", MinimumLength = 5)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "***")]
        [Compare("NewPassword", ErrorMessage = "Şifrələr eyni deyil")]
        public string ReNewPassword { get; set; }
    }
}