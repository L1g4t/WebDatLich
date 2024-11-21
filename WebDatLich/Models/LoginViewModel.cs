using System.ComponentModel.DataAnnotations;

namespace WebDatLich.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Username must be at least 3 characters long.", MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")] 
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

