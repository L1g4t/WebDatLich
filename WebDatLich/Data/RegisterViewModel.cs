using System.ComponentModel.DataAnnotations;

namespace WebDatLich.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Username must be at least 3 characters long.", MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}

