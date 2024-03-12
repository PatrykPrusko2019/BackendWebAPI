﻿using Microsoft.EntityFrameworkCore;

namespace BackendWebAPI.Entities
{
    public class DocumentDbContext : DbContext
    {
        public DbSet<AdmissionDocument> Documents { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Storage> Storages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdmissionDocument>()
                .Property(d => d.TargetWarehouse)
                .IsRequired();

            modelBuilder.Entity<Storage>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Provider>()
                .Property(s => s.CompanyName)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Product>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Label>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(40);
        }
    }
}
