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
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }

        //configuring many to many (overriding OnModelCreating)
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //telling primary key
            builder.Entity<ActivityAttendee>(x => x.HasKey( aa => new { aa.AppUserId, aa.ActivityId }));

            //configuring the entity itself
            builder.Entity<ActivityAttendee>()
            .HasOne( u => u.AppUser )
            .WithMany( a => a.Activities )
            .HasForeignKey( aa => aa.AppUserId );

            builder.Entity<ActivityAttendee>()
            .HasOne( u => u.Activity )
            .WithMany( a => a.Attendees )
            .HasForeignKey( aa => aa.ActivityId );

            builder.Entity<Comment>()
            .HasOne(a => a.Activity)
            .WithMany(c => c.Comments)
            .OnDelete(DeleteBehavior.Cascade); //if we delete an activity, it will cacasde to delete comments for that activity
        
            builder.Entity<UserFollowing>( b => {
                b.HasKey(k => new {k.ObserverId, k.TargetId});

                b.HasOne(o => o.Observer)
                    .WithMany(f => f.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(o => o.Target)
                    .WithMany(f => f.Followers)
                    .HasForeignKey(o => o.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}