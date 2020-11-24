using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace danielg_projectOne.DataModel
{
    public partial class danielGProj0DBContext : DbContext
    {
        public danielGProj0DBContext()
        {
        }

        public danielGProj0DBContext(DbContextOptions<danielGProj0DBContext> options)
            : base(options)
        {

        }

        public virtual DbSet<AggInventory> AggInventories { get; set; }
        public virtual DbSet<AggOrder> AggOrders { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<GenOrder> GenOrders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AggInventory>(entity =>
            {
                entity.HasKey(e => new { e.StoreId, e.Product })
                    .HasName("PK_StoreIdProduct");

                entity.ToTable("AggInventory", "Proj");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.Product).HasMaxLength(100);

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.AggInventories)
                    .HasForeignKey(d => d.Product)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryProductName");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.AggInventories)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryStoreID");
            });

            modelBuilder.Entity<AggOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.Product })
                    .HasName("PK_OrderIdProduct");

                entity.ToTable("AggOrders", "Proj");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Product).HasMaxLength(100);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.AggOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderID");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.AggOrders)
                    .HasForeignKey(d => d.Product)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductName");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "Proj");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<GenOrder>(entity =>
            {
                entity.ToTable("GenOrder", "Proj");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.GenOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerID");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.GenOrders)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreID");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Product__737584F754066CBD");

                entity.ToTable("Product", "Proj");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store", "Proj");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
