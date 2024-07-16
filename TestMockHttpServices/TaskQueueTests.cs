using MockHttpServices.Models;
using MockHttpServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMockHttpServices
{
    public class TaskQueueTests
    {
        private readonly TaskQueue _taskQueue;

        public TaskQueueTests()
        {
            _taskQueue = new TaskQueue();
        }

        [Fact]
        public void Enqueue_TaskIsAddedToQueue()
        {
            // Arrange
            var task = new TaskModel { Id = 1, Duration = 15, IsCompleted = false };

            // Act
            _taskQueue.EnqueueTask(task);

            // Assert
            Assert.True(_taskQueue.TryDequeue(out var dequeuedTask));
            Assert.Equal(task, dequeuedTask);
        }

        [Fact]
        public void TryDequeue_QueueIsEmpty_ReturnsFalse()
        {
            // Act
            var result = _taskQueue.TryDequeue(out var task);

            // Assert
            Assert.False(result);
            Assert.Null(task);
        }

        [Fact]
        public void Clear_QueueIsCleared()
        {
            // Arrange
            _taskQueue.EnqueueTask(new TaskModel { Id = 1, Duration = 15, IsCompleted = false });
            _taskQueue.EnqueueTask(new TaskModel { Id = 2, Duration = 20, IsCompleted = false });

            // Act
            _taskQueue.Clear();

            // Assert
            Assert.False(_taskQueue.TryDequeue(out _));
        }
    }
}
