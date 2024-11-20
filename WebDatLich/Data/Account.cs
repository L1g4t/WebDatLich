namespace WebDatLich.Data
{
    public class Account
    {
        public int AccountId { get; set; }  // Khóa chính

        public string Username { get; set; } // Tên đăng nhập

        public string Password { get; set; } // Mật khẩu

        public string Role { get; set; } // Chức vụ (Admin, Customer, Employee)

        public int? CustomerId { get; set; } // Khóa ngoại, liên kết với bảng Customers
        public Customer Customer { get; set; } // Điều hướng sang bảng Customer

        public int? EmployeeId { get; set; } // Khóa ngoại, liên kết với bảng Employees
        public Employee Employee { get; set; } // Điều hướng sang bảng Employees
    }
}

