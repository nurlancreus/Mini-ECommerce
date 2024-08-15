﻿using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Contexts
{
    public class MiniECommerceDbContext(DbContextOptions<MiniECommerceDbContext> contextOptions) : DbContext(contextOptions)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<AppFile>()
            .Property(p => p.Storage)
            .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changedEntities = ChangeTracker
        .Entries<BaseEntity>()
        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entity in changedEntities)
            {
                var now = DateTime.UtcNow + TimeSpan.FromHours(4);

                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedAt = now;
                }
                else if (entity.State == EntityState.Modified)
                {
                    entity.Entity.UpdatedAt = now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<AppFile> ApplicationFiles { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
    }
}
