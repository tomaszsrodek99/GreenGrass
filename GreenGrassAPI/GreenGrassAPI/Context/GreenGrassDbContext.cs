using Microsoft.EntityFrameworkCore;
using GreenGrassAPI.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GreenGrassAPI.Context
{
    public class GreenGrassDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public GreenGrassDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plant>()
                .HasOne(u => u.Notification)
                .WithOne(b => b.Plant)
                .HasForeignKey<Notification>(b => b.PlantId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GreenGrass;Trusted_Connection=True;",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Identity"));
        }
    }
}