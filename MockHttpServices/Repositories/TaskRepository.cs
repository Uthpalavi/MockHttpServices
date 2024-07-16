using MockHttpServices.Models;
using System.Threading.Tasks;

namespace MockHttpServices.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly List<TaskModel> _tasks = new();

        public void AddTask(TaskModel task)
        {
            _tasks.Add(task);
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            return _tasks;
        }
    }
}
