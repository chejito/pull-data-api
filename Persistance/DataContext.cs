using Microsoft.EntityFrameworkCore;
using PullDataApi.Models;
using Serilog;
using System.Reflection;

namespace PullDataApi.Persistance
{
    public class DataContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            Log.Information("DbContext configured");
            /*builder.Entity<Category>().HasData(DefaultData.CategoryList);
            builder.Entity<User>().HasData(DefaultData.UserList);
            builder.Entity<Movement>().HasData(DefaultData.MovementList);*/
            Log.Information("Initial database seeding done");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // in memory database used for simplicity, change to a real db for production applications
            //options.UseInMemoryDatabase("TestDb");
        }
    }
}
