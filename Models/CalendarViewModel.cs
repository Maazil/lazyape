using System.Collections.Generic;

namespace lazyape.Models
{
    /// <summary>
    /// CalendarViewModel for sending data to the calendar on the front-end
    /// </summary>
    public class CalendarViewModel
    {
        //Tasks we are going to send to the front
        private List<Task> Tasks { get; set; }

        //Calendar view model constructor
        public CalendarViewModel()
        {
            Tasks = new List<Task>();
        }

        //Add the task to the view model 
        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        //Return the tasks the view model have
        public List<Task> GetTasks()
        {
            return Tasks;
        }
    }
}