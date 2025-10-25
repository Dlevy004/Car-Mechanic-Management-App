using CarMechanic.Server;
using CarMechanic.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarMechanic.Server.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    [Authorize(Roles = "Admin,Mechanic")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Job>>> GetAllJobsAsync()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJobByIdAsync(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound(); // 404 Not Found
            }
            return Ok(job);
        }

        [HttpPost]
        public async Task<ActionResult<Job>> PostAsync([FromBody] Job job)
        {
            var createdJob = await _jobService.AddJobAsync(job);
            return CreatedAtAction("GetJobById", new { id = createdJob.Id }, createdJob);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Job job)
        {
            if (id != job.Id)
            {
                return BadRequest(); // 400 Bad Request
            }

            await _jobService.UpdateJobAsync(job);
            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _jobService.DeleteJobAsync(id);
            return NoContent(); // 204 No Content
        }
    }
}