using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventManagerADV.Models;

namespace EventManagerADV.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EventVolunteer> EventVolunteers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventVolunteer>()
                .HasKey(ev => new { ev.EventId, ev.VolunteerId });

            modelBuilder.Entity<EventVolunteer>()
                .HasOne(ev => ev.Event)
                .WithMany(e => e.EventVolunteers)
                .HasForeignKey(ev => ev.EventId);

            modelBuilder.Entity<EventVolunteer>()
                .HasOne(ev => ev.Volunteer)
                .WithMany(v => v.EventVolunteers)
                .HasForeignKey(ev => ev.VolunteerId);
        }
    }
}
