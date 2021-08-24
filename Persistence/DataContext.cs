using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser> //derived from entity framework class DbContext
    //not using repository pattern to handle logic, rather clean architecture and SQRS
    {//generate constructor
    //passing options -base constructor which we passing instructions to from DbContext 
    //we will create a migration
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        //adding property of type DbSet, takes type parameter Activity
        //we're gonna call our table activities
        public DbSet<Activity> Activities { get; set; }
    }
}