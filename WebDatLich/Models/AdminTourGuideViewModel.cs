using System.ComponentModel.DataAnnotations;

namespace WebDatLich.Models
{
    public class AdminTourGuideViewModel
    {
        public int EmployeeId { get; set; }

        public int ExperienceYears { get; set; }

        public string LanguageSpoken { get; set; }
    }
}
