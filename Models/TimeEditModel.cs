using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace lazyape.Models
{   
    public class TimeEdit
    {
        /// <summary>
        /// Help variable to add the correct data over to the task
        /// </summary>
        public class ReservationTimeEdit
        {
            //Id of reservation time edit var
            [JsonProperty("id")]
            public int id { get; set; }
            //Start date
            [JsonProperty("startdate")]
            public string startDate { get; set; }
            //start time
            [JsonProperty("starttime")]
            public string startTime { get; set; }
            //End date
            [JsonProperty("enddate")]
            public string endDate { get; set; }
            //End time
            [JsonProperty("endtime")]
            public string endTime { get; set; }
            //colums
            [JsonProperty("columns")]
            public List<string> columns { get; set; }
            /// <summary>
            /// Function to convert reservationTimeEdit object to task model
            /// </summary>
            /// <returns>Task model with the information from the reservationTimeEdit </returns>
            public Task convertToTask()
            {
                //New task to store it in
                Task task = new Task();

                //Convert the start and end date to the right format
                DateTime sDate = DateTime.ParseExact(startDate, "dd.MM.yyyy", null);
                DateTime eDate = DateTime.ParseExact(endDate, "dd.MM.yyyy",null);
                
                //Add the start and end to the new reservation
                task.Start = Convert.ToDateTime(sDate.ToString("yyyy-MM-dd") + " " + startTime);
                task.End = Convert.ToDateTime(eDate.ToString("yyyy-MM-dd") + " " + endTime);

                //Set right type
                task.Type = Task.TaskType.THIRDPARTY;
                
                //TODO Make this pick the right 1 automatic
                //Set the title on the task
                task.Title = columns[0] + " - " + columns[5] + " - " + columns[2];
                
                //Return task
                return task;
            }
        }
        
        //##############################
        //Below is the TimeEdit elements
        //##############################
        
        //Columnheaders
        [JsonProperty("columnheaders")]
        public List<string> Columnheaders { get; set; }
        
        //Info 
        [JsonProperty("info")]
        public List<int> Info { get; set; }
        
        //Reservation
        [JsonProperty("reservations")]
        public List<Task> Reservations { get; set; }

        /// <summary>
        /// TimeEdit constructor
        /// </summary>
        public TimeEdit()
        {
            //Make the new lists
            Columnheaders = new List<string>();
            Info = new List<int>();
            Reservations = new List<Task>();
        }
    }
}