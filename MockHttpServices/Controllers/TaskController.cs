using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockHttpServices.Services;

namespace MockHttpServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            var tasks = _taskService.GetTasks();
            return Ok(tasks);
        }

        [HttpPost("submit_tasks")]
        public IActionResult SubmitTasks([FromBody] List<int> taskDurations)
        {
            if (taskDurations == null)
            {
                return BadRequest("Missing 'task_durations_in_seconds' array.");
            }

            if (taskDurations.Count == 0)
            {
                return BadRequest("Task durations array is empty.");
            }

            if (taskDurations.Any(d => d.GetType() != typeof(int)))
            {
                return BadRequest("Some Values in the array are not int.");
            }

            if (!taskDurations.All(d => (d >= 10 && d <= 25)))
            {
                return BadRequest("Invalid task durations. All durations must be between 10 and 25 seconds.");
            }

            try
            {
                _taskService.AddTasks(taskDurations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Accepted();
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartProcessing()
        {
            await _taskService.ProcessTasks();
            return Ok();
        }

        [HttpPost("stop")]
        public IActionResult StopProcessing()
        {
            _taskService.StopProcessing();
            return Ok();
        }
    }
}
