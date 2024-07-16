using MockHttpServices.Models;

namespace MockHttpServices.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<TaskModel> GetAllTasks();
        void AddTask(TaskModel task);
    }
}
