using Microsoft.AspNetCore.Identity;

namespace WebDatLich.Models
{
    // Kế thừa từ IdentityUser để thêm thuộc tính tùy chỉnh cho người dùng
    public class ApplicationUser : IdentityUser
    {
        // Bạn có thể thêm các thuộc tính tùy chỉnh cho người dùng ở đây
        public string FullName { get; set; }  // Ví dụ: tên đầy đủ
    }
}

