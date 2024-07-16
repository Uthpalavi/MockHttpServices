using MockHttpServices.Models;
using MockHttpServices.Repositories;
using MockHttpServices.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMockHttpServices
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly Mock<ITaskQueue> _mockQueue;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _mockQueue = new Mock<ITaskQueue>();
            _taskService = new TaskService(_mockQueue.Object, _mockRepository.Object);
        }

        [Fact]
        public void AddTasks_ValidDurations_AddsTasksToRepository()
        {
            var durations = new List<int> { 10, 20 };

            _taskService.AddTasks(durations);

            _mockRepository.Verify(r => r.AddTask(It.IsAny<TaskModel>()), Times.Exactly(2));
        }


        [Fact]
        public async Task ProcessTasks_TasksInQueue_CompletesTasks()
        {
            // Arrange
            var task1 = new TaskModel { Id = 1, Duration = 1 };
            var task2 = new TaskModel { Id = 2, Duration = 2 };
            var tasksQueue = new Queue<TaskModel>(new[] { task1, task2 });

            _mockQueue.Setup(q => q.TryDequeue(out It.Ref<TaskModel>.IsAny))
                      .Returns((out TaskModel task) =>
                      {
                          if (tasksQueue.Count > 0)
                          {
                              task = tasksQueue.Dequeue();
                              return true;
                          }
                          task = null;
                          return false;
                      });

            // Act
            await _taskService.ProcessTasks();

            // Assert
            Assert.True(task1.IsCompleted);
            Assert.True(task2.IsCompleted);
        }

        [Fact]
        public void StopProcessing_ClearsQueue()
        {
            _taskService.AddTasks(new List<int> { 10, 20 });
            _taskService.StopProcessing();

            var tasks = _taskService.GetTasks();
            Assert.Empty(tasks);
        }
    }
}
