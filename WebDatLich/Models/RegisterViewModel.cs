using System.ComponentModel.DataAnnotations;

namespace WebDatLich.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
    }
}

