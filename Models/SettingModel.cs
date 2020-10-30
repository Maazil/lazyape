using System;

namespace lazyape.Models
{
    /// <summary>
    /// Settings object
    /// </summary>
    public class Setting
    {
        //Id of settings object in the database
        public int Id { get; set; } 
        
        //bool to show if it should activate the dark mode
        public bool DarkMode { get; set; }
        
        //This is default for whole week. When the user is available.
        //This is for the auto generator
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; } 
        
        //The time interval calender view will show normally 
        public DateTime VisibleTimeTo { get; set; }
        public DateTime VisibleTimeFrom { get; set; }
        //User connection 
        //Foreign key
        public string UserId { get; set; } 
        //Navigation property
        public ApplicationUser User { get; set; }
    }
}