using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Queue>()
                .HasNoKey();
            modelBuilder.Entity<Rehabilitation>()
                .HasNoKey();
            modelBuilder.Entity<Reserve>()
                .HasNoKey();
        }
    }
}
