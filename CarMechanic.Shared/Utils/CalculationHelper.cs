using CarMechanic.Shared.Enums;
using CarMechanic.Shared.Models;

namespace CarMechanic.Shared.Utils
{
    public static class CalculationHelper
    {
        public static double CalculateWorkHourEstimation(Job job)
        {
            double categoryHours = job.JobCategory switch
            {
                JobCategory.Chassis => 3,
                JobCategory.Engine => 8,
                JobCategory.Suspension => 6,
                JobCategory.Brakes => 4,
                _ => 0
            };

            int carAge = DateTime.Now.Year - job.VehicleYear.Year;
            double ageMultiplier = carAge switch
            {
                var age when age >= 0 && age < 5 => 0.5,
                var age when age >= 5 && age < 10 => 1,
                var age when age >= 10 && age < 20 => 1.5,
                _ => 2
            };

            double severityMultiplier = job.Severity switch
            {
                var s when s >= 1 && s <= 2 => 0.2,
                var s when s >= 3 && s <= 4 => 0.4,
                var s when s >= 5 && s <= 7 => 0.6,
                var s when s >= 8 && s <= 9 => 0.8,
                10 => 1,
                _ => 0
            };

            return categoryHours * ageMultiplier * severityMultiplier;
        }
    }
}
