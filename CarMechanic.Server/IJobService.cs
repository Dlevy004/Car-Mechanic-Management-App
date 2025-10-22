using CarMechanic.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarMechanic.Server
{
    public interface IJobService
    {
        Task<List<Job>> GetAllJobsAsync();
        Task<Job> GetJobByIdAsync(int id);
        Task<Job> AddJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(int id);
    }
}