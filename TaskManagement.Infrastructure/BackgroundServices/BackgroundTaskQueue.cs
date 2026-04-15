using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace TaskManagement.Infrastructure.BackgroundServices
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(int taskId);
        ValueTask<int> DequeueAsync(CancellationToken cancellationToken);
    }

    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<int> _queue;

        public BackgroundTaskQueue()
        {
            var options = new BoundedChannelOptions(100) { FullMode = BoundedChannelFullMode.Wait };
            _queue = Channel.CreateBounded<int>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(int taskId) =>
            await _queue.Writer.WriteAsync(taskId);

        public async ValueTask<int> DequeueAsync(CancellationToken cancellationToken) =>
            await _queue.Reader.ReadAsync(cancellationToken);
    }
}
