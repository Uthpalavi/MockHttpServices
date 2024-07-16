using MockHttpServices.Models;
using System.Threading.Tasks;

namespace MockHttpServices.Services
{
    public interface ITaskService
    {
        public IEnumerable<TaskModel> GetTasks();
        public void AddTasks(IEnumerable<int> taskDurations);
        public Task ProcessTasks();
        public void StopProcessing();

    }
}
