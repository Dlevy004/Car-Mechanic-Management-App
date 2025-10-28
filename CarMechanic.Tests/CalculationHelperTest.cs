using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarMechanic.Shared.Models;
using CarMechanic.Shared.Enums;
using CarMechanic.Shared.Utils;

namespace CarMechanic.Tests
{
    public class CalculationHelperTest
    {
        [Fact]
        public void Test_CalculateWorkHourEstimation()
        {
            // Arrange
            var job = new Job
            {
                JobCategory = JobCategory.Engine, // motor: 8 óra
                VehicleYear = DateTime.Now.AddYears(-2), // korszerű: szorzó 0.5
                Severity = 7 // közepes súlyosság: szorzó 0.6
            };
            double expectedEstimation = 8 * 0.5 * 0.6;

            // Act
            var actualEstimation = CalculationHelper.CalculateWorkHourEstimation(job);

            // Assert
            Assert.Equal(expectedEstimation, actualEstimation);
        }

        [Theory]
        [InlineData(JobCategory.Brakes, 7, 5, 4 * 1 * 0.6)] // Fék (4), 7 év (1), 5-ös súly. (0.6) = 2.4
        [InlineData(JobCategory.Suspension, 15, 9, 6 * 1.5 * 0.8)] // Futómű (6), 15 év (1.5), 9-es súly. (0.8) = 7.2
        [InlineData(JobCategory.Chassis, 25, 10, 3 * 2 * 1)] // Karosszéria (3), 25 év (2), 10-es súly. (1) = 6.0
        public void Test_CalculateWorkHourEstimation_VariousInputs(
            JobCategory category, int vehicleAge, int severity, double expectedEstimation
        )
        {
            // Arrange
            var job = new Job
            {
                JobCategory = category,
                VehicleYear = DateTime.Now.AddYears(-vehicleAge),
                Severity = severity
            };

            // Act
            var actualEstimation = CalculationHelper.CalculateWorkHourEstimation(job);

            // Assert
            Assert.Equal(expectedEstimation, actualEstimation);
        }

        [Fact]
        public void Test_CalculateWorkHourEstimation_UnknownCategory()
        {
            // Arrange
            var job = new Job
            {
                JobCategory = (JobCategory)999, // Ismeretlen kategória
                VehicleYear = DateTime.Now.AddYears(-10), // 10 év (1)
                Severity = 5 // közepes súlyosság: szorzó 0.6
            };
            double expectedEstimation = 0 * 1 * 0.6; // Várható eredmény: 0

            // Act
            var actualEstimation = CalculationHelper.CalculateWorkHourEstimation(job);

            // Assert
            Assert.Equal(expectedEstimation, actualEstimation);
        }
    }
}
