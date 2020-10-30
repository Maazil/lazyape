using System;
using System.Collections.Generic;
using lazyape.Controllers;
using lazyape.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lazyape.Data
{
    public static class LazyApeDbInitializer
    {
        /// <summary>
        /// Initializer function to set the test cases we want before it run.
        /// </summary>
        /// <param name="context">Database context to use</param>
        /// <param name="um">User manager to use</param>
        /// <param name="development">bool to know if we are in development or not.</param>
        public static void Initializer(LazyApeDbContext context, UserManager<ApplicationUser> um, bool development)
        {
            // Run migrations if we're not in development mode
            if (!development)
            {
                //Migration is commented out since we do not have any yet.
                //context.Database.Migrate();
                //return;
            }

            // If we are in development mode the code below is run.
            context.Database.EnsureDeleted();

            // Make sure the database and tables exist
            context.Database.EnsureCreated();

            //Test user 1
            var user = new ApplicationUser { UserName = "user@uia.no", Email = "user@uia.no"};
            um.CreateAsync(user, "Password1.").Wait();
            
            //Test user 2
            var user2 = new ApplicationUser { UserName = "user2@uia.no", Email = "user2@uia.no"};
            um.CreateAsync(user2, "Password1.").Wait();

            //Save the user to the database
            context.SaveChanges();

            //Test courses we want added
            context.AddRange(
        new Course{Active = true,Code = "Dat219",HoursDone = 0, User = user, UserId = user.Id},
                    new Course{Active = true,Code = "Dat202",HoursDone = 0, User = user, UserId = user.Id},
                    new Course{Active = true,Code = "Dat219",HoursDone = 0, User = user2, UserId = user2.Id},
                    new Course{Active = true,Code = "Dat202",HoursDone = 0, User = user2, UserId = user2.Id}
            );
            
            //Save courses to the database
            context.SaveChanges();

            //Test task 1
            var task = new Task();

            task.Title = "C++ Time";
            task.Start = DateTime.Now;
            task.End = DateTime.Now.AddMinutes(50);
            task.User = user;
            task.UserId = user.Id;
            task.Type = Task.TaskType.NORMAL;

            context.Add(task);
            
            //Test task 2 
            var task2 = new Task();

            task2.Title = "ID course";
            task2.Start = DateTime.Now.AddHours(2);
            task2.End = DateTime.Now.AddHours(3);
            task2.User = user;
            task.UserId = user.Id;
            task.Type = Task.TaskType.NORMAL;

            context.Add(task2);
            
            // Save Changes to the database
            context.SaveChanges();
            
            // Add test settings to test user 1
            var setting = new Setting();
            setting.User = user;
            setting.UserId = user.Id;
            setting.DarkMode = false;
            setting.StartTime = new DateTime(StaticDataModel.IgnoreDate.Year,
                                            StaticDataModel.IgnoreDate.Month, 
                                            StaticDataModel.IgnoreDate.Day,
                                            8,0,0 );
            
            setting.EndTime = new DateTime(StaticDataModel.IgnoreDate.Year,
                                            StaticDataModel.IgnoreDate.Month, 
                                            StaticDataModel.IgnoreDate.Day,
                                            20,0,0 );

            setting.VisibleTimeFrom = new DateTime(StaticDataModel.IgnoreDate.Year,
                                                    StaticDataModel.IgnoreDate.Month, 
                                                    StaticDataModel.IgnoreDate.Day,
                                                8,0,0 );

            
            setting.VisibleTimeTo = new DateTime(StaticDataModel.IgnoreDate.Year,
                                                StaticDataModel.IgnoreDate.Month, 
                                                StaticDataModel.IgnoreDate.Day,
                                                20,0,0 );
            

            context.Add(setting);
            
            // Save Changes to the database
            context.SaveChanges();
        }
    }
}