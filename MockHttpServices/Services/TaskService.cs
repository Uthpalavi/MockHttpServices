
using MockHttpServices.Models;
using MockHttpServices.Repositories;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MockHttpServices.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskQueue _taskQueue;
        private readonly ITaskRepository _taskRepository;
        private int _taskIdCounter = 0;

        public TaskService(ITaskQueue taskQueue, ITaskRepository taskRepository)
        {
            _taskQueue = taskQueue;
            _taskRepository = taskRepository;
        }

        public IEnumerable<TaskModel> GetTasks() => _taskRepository.GetAllTasks();


        public void AddTasks(IEnumerable<int> taskDurations)
        {
            foreach (var duration in taskDurations)
            {
                var task = new TaskModel
                {
                    Id = ++_taskIdCounter,
                    Duration = duration,
                    IsCompleted = false
                };
                _taskRepository.AddTask(task);
                _taskQueue.EnqueueTask(task);
            }
        }


        public async Task ProcessTasks()
        {
            var tasks = new List<Task>();

            while (_taskQueue.TryDequeue(out var task))
            {
                tasks.Add(RunTask(task));
            }

            await Task.WhenAll(tasks);
        }

        private async Task RunTask(TaskModel task)
        {
            await Task.Delay(task.Duration * 1000);
            task.IsCompleted = true;
        }

        public void StopProcessing()
        {
            _taskQueue.Clear();
        }
    }

    public interface ITaskQueue
    {
        void EnqueueTask(TaskModel task);
        bool TryDequeue(out TaskModel task);
        void Clear();
    }

    public class TaskQueue : ITaskQueue
    {
        private readonly ConcurrentQueue<TaskModel> _taskQueue = new();

        public void EnqueueTask(TaskModel task)
        {
            _taskQueue.Enqueue(task);
        }

        public bool TryDequeue(out TaskModel task)
        {
            return _taskQueue.TryDequeue(out task);
        }

        public void Clear()
        {
            _taskQueue.Clear();
        }
    }
}
