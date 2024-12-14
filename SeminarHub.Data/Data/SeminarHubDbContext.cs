using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.Data.Models;

namespace SeminarHub.Data.Data
{
    public class SeminarHubDbContext : IdentityDbContext
    {
        public SeminarHubDbContext(DbContextOptions<SeminarHubDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreatedAsync();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Seminar> Seminars { get; set; }
        public DbSet<SeminarParticipant> SeminarsParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Seminar>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Seminars)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SeminarParticipant>()
                .HasKey(e => new { e.SeminarId, e.ParticipantId });

            builder.Entity<SeminarParticipant>()
                .HasOne(e => e.Seminar)
                .WithMany()
                .HasForeignKey(e => e.SeminarId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SeminarParticipant>()
                .HasOne(e => e.Participant)
                .WithMany()
                .HasForeignKey(e => e.ParticipantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
               .Entity<Category>()
               .HasData(new Category()
               {
                   Id = 1,
                   Name = "Technology & Innovation"
               },
               new Category()
               {
                   Id = 2,
                   Name = "Business & Entrepreneurship"
               },
               new Category()
               {
                   Id = 3,
                   Name = "Science & Research"
               },
               new Category()
               {
                   Id = 4,
                   Name = "Arts & Culture"
               });

            base.OnModelCreating(builder);
        }
    }
}