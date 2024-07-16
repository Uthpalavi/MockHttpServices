using MockHttpServices.Models;
using MockHttpServices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMockHttpServices
{
    public class TaskRepositoryTests
    {
        private readonly TaskRepository _taskRepository;

        public TaskRepositoryTests()
        {
            _taskRepository = new TaskRepository();
        }

        [Fact]
        public void AddTask_TaskIsAddedToList()
        {
            // Arrange
            var task = new TaskModel { Id = 1, Duration = 15, IsCompleted = false };

            // Act
            _taskRepository.AddTask(task);

            // Assert
            var tasks = _taskRepository.GetAllTasks();
            Assert.Single(tasks);
            Assert.Contains(task, tasks);
        }

        [Fact]
        public void GetAllTasks_ReturnsAllAddedTasks()
        {
            // Arrange
            var task1 = new TaskModel { Id = 1, Duration = 15, IsCompleted = false };
            var task2 = new TaskModel { Id = 2, Duration = 20, IsCompleted = false };
            _taskRepository.AddTask(task1);
            _taskRepository.AddTask(task2);

            // Act
            var tasks = _taskRepository.GetAllTasks();

            // Assert
            Assert.Equal(2, tasks.Count());
            Assert.Contains(task1, tasks);
            Assert.Contains(task2, tasks);
        }
    }
}
