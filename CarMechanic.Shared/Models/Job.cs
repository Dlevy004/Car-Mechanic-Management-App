using System.ComponentModel.DataAnnotations;
using CarMechanic.Shared.Enums;

namespace CarMechanic.Shared.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please choose a customer.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "The license plate number is required.")]
        [RegularExpression(@"^[A-Z]{3}-\d{3}$", ErrorMessage = "The license plate format is incorrect (e.g. ABC-123).")]
        public string LicensePlateNumber { get; set; }

        [Range(1900, 2100, ErrorMessage = "The year of manufacture must be between 1900 and 2100.")]
        public int VehicleYear { get; set; }
        public JobCategory JobCategory { get; set; }

        [Required(ErrorMessage = "The issue description is required.")]
        public string VehicleIssueDescription { get; set; }

        [Range(1, 10, ErrorMessage = "Severity should be a value between 1 and 10.")]
        public int Severity { get; set; }
        public JobStage JobStage { get; set; }

    }
}
