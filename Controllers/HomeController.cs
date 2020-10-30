using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lazyape.Models;

namespace lazyape.Controllers
{
    /// <summary>
    /// A simple controller that take control over which site to show before LOGGED IN
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Show the index page
        /// </summary>
        /// <returns>Returns the correct view</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Show the privacy page
        /// </summary>
        /// <returns>Returns the correct view</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Show the about page
        /// </summary>
        /// <returns>Returns the correct view</returns>
        public IActionResult About()
        {
            return View();
        }
        
        // <summary>
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
