using System.Diagnostics;
using lazyape.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lazyape.Controllers
{
    /// <summary>
    /// A simple controller that give us the calendar page
    /// </summary>
    [Authorize]
    public class CalendarController : Controller
    {
        /// <summary>
        /// GET - Index page - aka calendar page
        /// </summary>
        /// <returns>Returns the correct view</returns>
        public IActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// If a error happened then show it
        /// </summary>
        /// <returns>Returns a view with the the error code</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}