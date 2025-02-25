using Microsoft.EntityFrameworkCore;
using SmartEventPlanningPlatform.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace SmartEventPlanningPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Message and User relationship
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse mesajlar korunur.

            // Message and Event relationship
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Event)
                .WithMany(e => e.Messages)
                .HasForeignKey(m => m.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Etkinlik silinirse mesajlar da silinir.

            // Participant and User relationship
            modelBuilder.Entity<Participant>()
                .HasOne(p => p.User)
                .WithMany(u => u.Participants)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict

            // Participant and Event relationship
            modelBuilder.Entity<Participant>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Etkinlik silinirse katılımcılar silinir
            modelBuilder.Entity<UserPoints>()
               .HasOne(up => up.User)
               .WithMany(u => u.UserPoints)
               .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<ActivityHistory>()
                .HasOne(ah => ah.User)
                .WithMany(u => u.ActivityHistories)
                .HasForeignKey(ah => ah.UserId);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Participant> Participants { get; set; }

        public DbSet<UserPoints> UserPoints { get; set; }

        public DbSet<ActivityHistory> ActivityHistory { get; set; }
    }
}
