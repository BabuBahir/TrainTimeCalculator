using System;
using Microsoft.EntityFrameworkCore;

namespace unclesam.Models
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> dbcontextoption)
            :base(dbcontextoption)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<NewsComment> NewsComment { get; set; }
    }
}

