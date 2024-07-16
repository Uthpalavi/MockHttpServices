namespace MockHttpServices.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public bool IsCompleted { get; set; }

        //public bool IsValid()
        //{
        //    return Duration >= 10 && Duration <= 25;
        //}
    }
}
