using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.Text.Json;
using TaskManagement.BusinessLogic.DTOs;
using TaskManagement.BusinessLogic.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.BackgroundServices;
using TaskManagement.Shared.Enums;


namespace Task_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppTaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IAppTaskService _taskService;
        private readonly IMapper _mapper;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IDistributedCache _distributedCache;

        public AppTaskController(ITaskRepository taskRepository, IAppTaskService taskService, IMapper mapper, IBackgroundTaskQueue backgroundTaskQueue, IDistributedCache distributedCache)
        {
            _taskRepository = taskRepository;
            _taskService = taskService;
            _mapper = mapper;
            _backgroundTaskQueue = backgroundTaskQueue;
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] AppTaskCreateUpdateModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var isDuplicate = await _taskService.IsDuplicateAsync(request.Title, userId, DateTime.UtcNow.Date);
            if (isDuplicate)
            {
                return BadRequest("You have already created a task with this title today.");
            }

            var task = _mapper.Map<AppTask>(request);
            task.UserId = userId;
            task.CreatedAt = DateTime.UtcNow;
            task.Status =AppTaskStatus.Pending;

            await _taskRepository.AddAsync(task);
            await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(task.Id);

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, _mapper.Map<AppTaskModel>(task));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            string cacheKey = $"task_{id}";
            AppTask? task = null;

            var cachedTask = await _distributedCache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedTask))
            {
                task = JsonSerializer.Deserialize<AppTask>(cachedTask);
            }
            else
            {
                task = await _taskRepository.GetByIdAsync(id);

                if (task != null)
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };
                    await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(task), cacheOptions);
                }
            }

            if (task == null)
                return NotFound(new { message = "Task not found" });

            if (task.UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
                return Forbid();

            return Ok(_mapper.Map<AppTaskModel>(task));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskRepository.GetMany().Where(t => t.UserId == User.FindFirst(ClaimTypes.NameIdentifier)!.Value).ToListAsync();

            return Ok(_mapper.Map<IEnumerable<AppTaskModel>>(tasks));
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] AppTaskStatus status)
        {
            var original = await _taskRepository.GetByIdAsync(id);
            if (original == null)
                return NotFound(new { message = "Task not found" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (original.UserId != userId)
                return Forbid();

            original.Status = status;
            await _taskRepository.UpdateAsync(original);

            await _distributedCache.RemoveAsync($"task_{id}");

            return Ok(new { message = "Task status updated successfully" });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateTask(int id,[FromBody] AppTaskCreateUpdateModel task)
        {
            var orginal = await _taskRepository.GetByIdAsync(id);
            if (orginal == null)
                return NotFound(new { message = "Task not found" });

            if (orginal.UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            await _taskRepository.UpdateAsync(_mapper.Map<AppTask>(task));

            return Ok(new { message = "Task status updated successfully" });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var orginal = await _taskRepository.GetByIdAsync(id);
            if (orginal == null)
                return NotFound(new { message = "Task not found" });

            if (orginal.UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            await _taskRepository.DeleteByIdAsync(id);

            return Ok(new { message = "Task deleted successfully" });
        }
    }
}
