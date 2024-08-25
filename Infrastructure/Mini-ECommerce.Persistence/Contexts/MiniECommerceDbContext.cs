using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Base;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Contexts
{
    public class MiniECommerceDbContext(DbContextOptions<MiniECommerceDbContext> contextOptions) : IdentityDbContext<AppUser, AppRole, string>(contextOptions)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<AppFile>()
          .Property(p => p.Storage)
            .HasConversion<string>();

            //     modelBuilder.Entity<Product>()
            //.HasMany(e => e.ProductImageFiles)
            // .WithMany(e => e.Products);


            
            modelBuilder.Entity<Product>()
          .HasMany(p => p.ProductImageFiles)
          .WithMany(pi => pi.Products)
          .UsingEntity<ProductProductImageFile>(
            j => j
          .HasOne(pp => pp.ProductImageFile)
          .WithMany(pi => pi.ProductProductImageFiles)
          .HasForeignKey(pp => pp.ProductImageFileId),
            j => j
          .HasOne(pp => pp.Product)
          .WithMany(p => p.ProductProductImageFiles)
          .HasForeignKey(pp => pp.ProductId)
            );

            modelBuilder.Entity<ProductProductImageFile>()
           .Property(p => p.IsMain)
           .HasDefaultValue(false);

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
        public DbSet<ProductProductImageFile> ProductProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
    }
}
