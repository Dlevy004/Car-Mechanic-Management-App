using CarMechanic.Server.Data;
using CarMechanic.Server;
using CarMechanic.Shared.Models;
using Microsoft.EntityFrameworkCore;
using CarMechanic.Shared.Enums;

namespace CarMechanic.Server
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;

        public JobService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Job> AddJobAsync(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<Job> GetJobByIdAsync(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task UpdateJobAsync(Job job)
        {
            var existingJob = await _context.Jobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == job.Id);

            if (existingJob == null)
            {
                throw new KeyNotFoundException($"Job with Id {job.Id} not found.");
            }

            if (job.JobStage < existingJob.JobStage)
            {
                throw new InvalidOperationException("Cannot regress job stage to a previous stage.");
            }

            _context.Entry(job).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}