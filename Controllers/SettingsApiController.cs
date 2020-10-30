using System;
using System.Linq;
using System.Threading.Tasks;
using lazyape.Data;
using lazyape.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace lazyape.Controllers
{
    /// <summary>
    /// Settings Api Controller take control over all the settings the user can set.
    /// </summary>
    [ApiController]
    //To we have added so the one uses the api can add its own user,
    //this api is locked to our users.
    [Authorize]
    [Route("api/settings")]
    public class SettingsApiController : ControllerBase
    {
        //The database context and user manager
        private readonly LazyApeDbContext _db;
        private readonly UserManager<ApplicationUser> _um;

        /// <summary>
        /// The settings api controllers construnctor
        /// </summary>
        /// <param name="db">The database context to use</param>
        /// <param name="um">The user manager to use</param>
        public SettingsApiController(LazyApeDbContext db, UserManager<ApplicationUser> um)
        {
            //Add the values given for database context and user manager
            _db = db;
            _um = um;
        }
        
        /// <summary>
        ///  Get all the settings the user have
        /// </summary>
        /// <returns> Returns a settings object with all the settings the user have saved</returns>
        [HttpGet]
        [Route("get")]
        //URL Call: https://localhost:5001/api/settings/get 
        public  IActionResult GetAll()
        {
            //Get all the settings the user have saved.
            var setting = _db.Settings.FirstOrDefault(w => w.UserId == _um.GetUserId(User));

            //Return the settings and ok code
            return Ok(setting);
        }

        /// <summary>
        /// Function for getting darkmode. 
        /// </summary>
        /// <returns>Dark mode value</returns>
        [HttpGet]
        [Route("get/darkMode")]
        //URL Call: https://localhost:5001/api/settings/get/darkmode 
        public IActionResult GetDarkMode()
        {
            //Get dark mode
            var setting = _db.Settings.FirstOrDefault(w => w.UserId == _um.GetUserId(User));
            
            //Return the dark mode value
            return Ok(setting.DarkMode);
        }

        /// <summary>
        /// Edit the settings for the current logged in user.
        /// </summary>
        /// <param name="settings">A settings object with all the new values.</param>
        /// <returns> Returns the settings object after implementing all the settings.</returns>
        [HttpPut]
        [Route("put/{id}")]
        //URL Call: https://localhost:5001/api/settings/put/1
        public IActionResult EditSettings(Setting settings)
        {
            //Check if task exists
            if (!_db.Settings.Any(w => w.Id == settings.Id))
            {
                return NotFound();
            }
            
            //Update the setting
            _db.Settings.Update(settings);
            
            //Save changes
            _db.SaveChanges();

            // Return setting
            return Ok(settings);
        }

    }

}