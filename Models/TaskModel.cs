using System;

namespace lazyape.Models
{
    /// <summary>
    /// Task object
    /// </summary>
    public class Task
    {
        //Enum for task type
        public enum TaskType
        {
            NORMAL,
            AUTO,
            THIRDPARTY
        }
        
        //Id for task object in database
        public int Id { get; set; }
        //Start date on task
        public DateTime Start { get; set; }
        //End date on task
        public DateTime End { get; set; }
        //Title on task
        public string Title { get; set; }
        
        //Foreign key
        public string UserId { get; set; } 
        //Navigation property
        public ApplicationUser User { get; set; }
        
        //Task type
        public TaskType Type { get; set; }
    }
}