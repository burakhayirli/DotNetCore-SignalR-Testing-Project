using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRProject.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        //DESKTOP-34I9MBF\SQLEXPRESS
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
