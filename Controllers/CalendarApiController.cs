using System;
using System.Collections.Generic;
using System.Linq;
using lazyape.Data;
using lazyape.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace lazyape.Controllers
{
    /// <summary>
    /// The calendar Api is a facade for other systems.
    /// Its job is to control all with the calendar to do. 
    /// </summary>
    [ApiController]
    //To we have added so the one uses the api can add its own user,
    //this api is locked to our users.
    [Authorize]
    [Route("api/calendar")]
    public class CalendarApiController : ControllerBase
    {

        //TimeEdit Controller
        private readonly TimeEditApiController _te;
        //Auto Generator Controller
        private TaskAutoGeneratorController _auto;
        //Settings Api Controller
        private SettingsApiController _se;    
        //Feide Api Controller
        private FeideApiController _feideApi;

        //Database and user manager        
        private readonly LazyApeDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Calendar Api Controllers Constructor
        /// </summary>
        /// <param name="db">The database context you want it to use and send to the other controllers.</param>
        /// <param name="userManager">The user manager it is going to use and send to the other controllers.</param>
        public CalendarApiController(LazyApeDbContext db, UserManager<ApplicationUser> userManager)
        {
            //Set db and user manager
            _db = db;
            _userManager = userManager;

            //Set timeEdit api controller
            _te = new TimeEditApiController();
            //Set autoGenerator controller
            _auto = new TaskAutoGeneratorController(db, userManager);
            //Set settings api controller
            _se = new SettingsApiController(db,userManager);
            //Set Feide api controller
            _feideApi = new FeideApiController(db, userManager);
        }
        
 
        //TODO Should probably be a async function
        /// <summary>
        ///  This get all the information the calendar need on a reload. 
        /// </summary>
        /// <returns> All the tasks the user own at this point.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            //connect user to his courses
            //also add tokens to user in here
            try
            {
                _feideApi.GetUserInfo(_userManager.GetUserId(User));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //Variable to store the data we are going to send to the front-end.
            CalendarViewModel tasks = new CalendarViewModel();
            
            //List to store the courses the user have in.
            List<string> courses = new List<string>();
            
            //Get course codes from the user
            List<Course> courseObjects = _db.Courses.Where(w => w.UserId == _userManager.GetUserAsync(User).Result.Id).ToList();
            
            //Loop through the list and add the course cods 
            foreach (var obj in courseObjects)
            {
                courses.Add(obj.Code);
            }
            
            //Add TimeEdit reservation to tasks in the calendar 
            //Get all reservation from the te controller
            foreach (var res in _te.GetAll(courses).Result)
            {
                //Take out every task out from the list
                foreach (var te in res.Reservations)
                {
                    //Add the task to the te
                    tasks.AddTask(te);
                }
            }
            
            //Getting the ones from the database.
            var list = _db.Tasks.Where(w=> w.UserId == _userManager.GetUserAsync(User).Result.Id).ToList();
            foreach (var t in list)
            {
                tasks.AddTask(t);
            }

            //Return OK code with the task payload
            return Ok(tasks.GetTasks());
        }
        
        /// <summary>
        ///  Find the task it is given the id to. 
        /// </summary>
        /// <param name="id"> Id of task it want to find.</param>
        /// <returns> The task asked for or error message with not found</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id) 
        {
            //Search for the given task
            var task = _db.Tasks.Find(id);
                 
            //Check if the task exists, return 404 if it doesn't
            if (task == null)
                return NotFound();
               
            //return 200 Ok with the task
            return Ok(task);
        }
        
        /// <summary>
        /// Add a task to the database. 
        /// </summary>
        /// <param name="task"> The task the user want to add</param>
        /// <returns> Returns the task if add, and if not error message</returns>
        [HttpPost]
        public IActionResult Post(Task task)
        {
            //Check if the task have another id then 0
            if (task.Id != 0)
            {
                //If it is id is something else tell them something is wrong. 
                //This is added for error check from the front-end. 
                return BadRequest();
            }
            //Check if all the information is added. 
            if (ModelState.IsValid)
            {
                //Makes a connection between task and user
                task.User = _userManager.GetUserAsync(User).Result; 
                task.UserId = _userManager.GetUserAsync(User).Result.Id;
                
                //Add task type
                task.Type = Task.TaskType.NORMAL;
               
                //Add task to database and saves it
                _db.Add(task);
                _db.SaveChanges();

            }
     
            //Return 200 ok with the new task
            return Ok(task);
        }
        
        /// <summary>
        /// Edit a task.
        /// </summary>
        /// <param name="task">Task chosen to edit.</param>
        /// <returns>Returns the task it did edit or error message.</returns>
        [HttpPut("{id}")]
        public IActionResult Put(Task task)
        {
            //Check if task exists
            if (!_db.Tasks.Any(p => p.Id == task.Id))
            {
                return NotFound();
            }

            //Connect the user and user id to the edit task.
            // We dod not connect it in the front-end so we take it here instead.
            task.User = _userManager.GetUserAsync(User).Result;
            task.UserId = _userManager.GetUserId(User);
                
            //Update the task in the database and saves it
            _db.Update(task);
            _db.SaveChanges();
            
            //Return 200 ok with the new task
            return Ok(task);
        }

        /// <summary>
        /// Delete task with the id given.
        /// </summary>
        /// <param name="id">Id of task it is going to delete</param>
        /// <returns> The task deleted or error message</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Search for the given task
            var task = _db.Tasks.Find(id);
                 
            //Check if the task exists, return 404 if it doesn't
            if (task == null)
                return NotFound();
     
            //Remove task from the database
            _db.Remove(task);
            _db.SaveChanges();
                 
            //return 200 Ok with the task
            return Ok(task);
        }
        
        /// <summary>
        /// It gets the TimeEdit task from a test period to show it work.
        /// The reason for this is that when sensor is going to check over this. Their is no task to get.
        /// So to show it work we made our own test case to show.
        /// </summary>
        /// <returns> Returns the task given by TimeEdit in the test period.</returns>
        [HttpGet]
        [Route("test/timeedit")]
        public IActionResult GetTestTimeEditTasks()
        {
            //Variable to store the data we are going to send to the front-end.
            CalendarViewModel tasks = new CalendarViewModel();
            
            //List to store the course codes in
            List<string> courses = new List<string>();

            //Get course codes from the user
            List<Course> courseObjects = _db.Courses.Where(w => w.UserId == _userManager.GetUserAsync(User).Result.Id).ToList();
            
            //Loop through and add all the course codes.
            foreach (var obj in courseObjects)
            {
                courses.Add(obj.Code);
            }
            
            //Add TimeEdit reservation to tasks in the calendar 
            //Get all reservation from the te controller
            foreach (var res in _te.GetAllTest(courses).Result)
            {
                //Take out every task out from the list
                foreach (var te in res.Reservations)
                {
                    //Add the task to the te
                    tasks.AddTask(te);
                }
            }

            //Return OK code with the task payload
            return Ok(tasks.GetTasks());
        }

        /// <summary>
        ///  It is generating the auto generated tasks in the test period.
        ///  The reason for this is that when sensor is going to check over this. Their is no task to generate.
        ///  So to show it work we made our own test case to show.
        /// </summary>
        /// <returns> All the auto generated tasks the user need.</returns>
        [HttpGet]
        [Route("test/auto")]
        public IActionResult GetAutoGeneratedTasks()
        {
            //Variable to store the data we are going to send to the front-end.
            CalendarViewModel tasks = new CalendarViewModel();

            //Get auto generated tasks from database on current user
            List<Task> test = _db.Tasks
                .Where(w => w.Type == Task.TaskType.AUTO && w.UserId == _userManager.GetUserId(User)).ToList();
            
            //Check if the test list is not empty
            if (test.Count != 0)
            {
                return Ok(test);
            }
            //AutoGenerate For users
            _auto.AutoGenerateTest(_userManager.GetUserId(User));

            //Get auto generated tasks from database on current user
            List<Task> te = _db.Tasks
                .Where(w => w.Type == Task.TaskType.AUTO && w.UserId == _userManager.GetUserId(User)).ToList();

            //Return OK code with the task payload
            return Ok(te);
        }

        /// <summary>
        /// Get all the settings the user have.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("settings")]
        public IActionResult GetAllSettings()
        {
            // Redirect the user to the the settings api GetAll function.
            return RedirectToAction("GetAll", "SettingsApi");
        }
        
        /// <summary>
        /// Edit the settings the user have
        /// </summary>
        /// <param name="setting">Settings object with the new settings</param>
        /// <returns>Return the settings after the changes</returns>
        [HttpPut]
        [Route("settings/{id}")]
        public IActionResult EditSettings(Setting setting)
        {
            //Redirect the user to the settings api EditSettings function. 
            return RedirectToAction("EditSettings", "SettingsApi", setting);
        }
    }
}