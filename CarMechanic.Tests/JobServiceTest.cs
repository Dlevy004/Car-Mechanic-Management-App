using CarMechanic.Server.Data;
using CarMechanic.Shared.Enums;
using CarMechanic.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarMechanic.Server.Tests
{
    public class JobServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            return context;
        }

        private Job CreateTestJob(int id, int customerId, JobStage stage = JobStage.ToDo)
        {
            return new Job
            {
                Id = id,
                CustomerId = customerId,
                LicensePlateNumber = $"TES-{id:D3}",
                VehicleYear = DateTime.Now.AddYears(-5).Date,
                JobCategory = JobCategory.Engine,
                VehicleIssueDescription = $"Test issue {id}",
                Severity = 5,
                JobStage = stage
            };
        }

        [Fact]
        public async Task GetAllJobsAsync_ShouldReturnAllJobs()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var jobs = new List<Job> { CreateTestJob(1, 10), CreateTestJob(2, 20) };
            dbContext.Jobs.AddRange(jobs);
            await dbContext.SaveChangesAsync();
            var service = new JobService(dbContext);

            // Act
            var result = await service.GetAllJobsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddJobAsync_ShouldAddJobAndReturnIt()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var service = new JobService(dbContext);
            var newJob = CreateTestJob(0, 30);

            // Act
            var createdJob = await service.AddJobAsync(newJob);
            var jobFromDb = await dbContext.Jobs.FirstOrDefaultAsync(j => j.CustomerId == 30);

            // Assert
            Assert.NotNull(createdJob);
            Assert.Equal(30, createdJob.CustomerId);
            Assert.True(createdJob.Id > 0);
            Assert.NotNull(jobFromDb);
            Assert.Equal($"TES-{createdJob.Id:D3}", jobFromDb.LicensePlateNumber);
        }

        [Fact]
        public async Task GetJobByIdAsync_ShouldReturnCorrectJob_IfExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var job = CreateTestJob(5, 50);
            dbContext.Jobs.Add(job);
            await dbContext.SaveChangesAsync();
            var service = new JobService(dbContext);

            // Act
            var result = await service.GetJobByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal(50, result.CustomerId);
        }

        [Fact]
        public async Task GetJobByIdAsync_ShouldReturnNull_IfNotExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var service = new JobService(dbContext);

            // Act
            var result = await service.GetJobByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateJobAsync_ShouldUpdateJob_IfExistsAndStageIsValid()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var originalJob = CreateTestJob(1, 10, JobStage.ToDo);
            dbContext.Jobs.Add(originalJob);
            await dbContext.SaveChangesAsync();
            dbContext.Entry(originalJob).State = EntityState.Detached;

            var service = new JobService(dbContext);
            var updatedJobData = CreateTestJob(1, 10, JobStage.InProgress);
            updatedJobData.VehicleIssueDescription = "Updated Issue";


            // Act
            await service.UpdateJobAsync(updatedJobData);
            var jobFromDb = await dbContext.Jobs.FindAsync(1);

            // Assert
            Assert.NotNull(jobFromDb);
            Assert.Equal(JobStage.InProgress, jobFromDb.JobStage);
            Assert.Equal("Updated Issue", jobFromDb.VehicleIssueDescription);
        }

        [Fact]
        public async Task UpdateJobAsync_ShouldThrowInvalidOperationException_IfStageRegresses()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var originalJob = CreateTestJob(1, 10, JobStage.Done);
            dbContext.Jobs.Add(originalJob);
            await dbContext.SaveChangesAsync();
            dbContext.Entry(originalJob).State = EntityState.Detached;

            var service = new JobService(dbContext);
            var invalidUpdateData = CreateTestJob(1, 10, JobStage.InProgress);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateJobAsync(invalidUpdateData));
        }

        [Fact]
        public async Task UpdateJobAsync_ShouldThrowKeyNotFoundException_IfNotExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var service = new JobService(dbContext);
            var nonExistentJob = CreateTestJob(99, 999);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateJobAsync(nonExistentJob));
        }


        [Fact]
        public async Task DeleteJobAsync_ShouldRemoveJob_IfExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var jobToDelete = CreateTestJob(1, 10);
            dbContext.Jobs.Add(jobToDelete);
            await dbContext.SaveChangesAsync();
            var service = new JobService(dbContext);

            // Act
            await service.DeleteJobAsync(1);
            var jobFromDb = await dbContext.Jobs.FindAsync(1);

            // Assert
            Assert.Null(jobFromDb);
        }

        [Fact]
        public async Task DeleteJobAsync_ShouldDoNothing_IfNotExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var existingJob = CreateTestJob(1, 10);
            dbContext.Jobs.Add(existingJob);
            await dbContext.SaveChangesAsync();
            var service = new JobService(dbContext);

            // Act
            await service.DeleteJobAsync(99);
            var count = await dbContext.Jobs.CountAsync();


            // Assert
            Assert.Equal(1, count);
        }
    }
}