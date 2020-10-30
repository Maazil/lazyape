using System;
using System.Collections.Generic;
using System.Text;
using lazyape.Models;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lazyape.Data
{
    public class LazyApeDbContext : IdentityDbContext<ApplicationUser>
    {
        //Constructor for datebase context
        public LazyApeDbContext(DbContextOptions<LazyApeDbContext> options)
            : base(options)
        {
        }
        
        //A datebase set for all objects we want to add in the database 
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Course> Courses { get; set; } 
        
        public DbSet<Token> Tokens { get; set; }
       
        
    }
}
