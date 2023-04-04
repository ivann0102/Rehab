using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RehabCV.Models;

namespace RehabCV.Database
{
    public class RehabCVContext : IdentityDbContext
    {
        public RehabCVContext(DbContextOptions<RehabCVContext> options)
            : base(options)
        { }
        public DbSet<Child> Children { get; set; }
        public DbSet<Queue> Queues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rehabilitation> Rehabilitations { get; set; }
        public DbSet<Reserve> Reserves { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<NumberOfChildren> NumberOfChildren { get; set;}
        public DbSet<Card> Cards{ get;set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Child>()
                .HasOne(x => x.Rehabilitation)
                .WithOne(x => x.Child)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reserve>()
                .HasMany(x => x.Children)
                .WithOne(x => x.Reserve); 
            modelBuilder.Entity<Group>()
                .HasMany(x => x.Children)
                .WithOne(x => x.Group)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Rehabilitation>()
                .HasOne(x => x.Queue)
                .WithOne(x => x.Rehabilitation)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Child)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
                

            modelBuilder.Entity<Card>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Child)
                .WithMany(ch => ch.Cards)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Therapist)
                .WithMany(th => th.Cards)
                .HasForeignKey(c => c.TherapistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
