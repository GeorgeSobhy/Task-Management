using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Shared.Enums;

namespace TaskManagement.Infrastructure.BackgroundServices
{
    public class TaskProcessingWorker : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TaskProcessingWorker> _logger;

        public TaskProcessingWorker(
            IBackgroundTaskQueue taskQueue,
            IServiceScopeFactory scopeFactory,
            ILogger<TaskProcessingWorker> logger)
        {
            _taskQueue = taskQueue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Task Worker is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var taskId = await _taskQueue.DequeueAsync(stoppingToken);

                    await Task.Delay(5000, stoppingToken);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Resolve the repository inside the scope
                        var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                        var task = await taskRepository.GetByIdAsync(taskId);

                        if (task != null)
                        {
                            task.Status = AppTaskStatus.Done;
                            await taskRepository.UpdateAsync(task);

                            _logger.LogInformation("Task {TaskId} successfully processed and updated to 'Done'.", taskId);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing a background task.");
                }
            }
        }
    }
}