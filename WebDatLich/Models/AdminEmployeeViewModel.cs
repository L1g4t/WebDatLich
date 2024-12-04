namespace WebDatLich.Models
{
    public class AdminEmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; } = null!;

        public string? Position { get; set; }

        public DateOnly? HireDate { get; set; }

        public string? PhoneNumber { get; set; }
        public int? TourGuideExperience { get; set; }
        public string? TourGuideLanguages { get; set; }
    }
}
