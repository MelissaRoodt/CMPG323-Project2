﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace Ecopower_project2_33509735.Models
{
    public partial class ecopower_dbContext : DbContext
    {
        public ecopower_dbContext()
        {
        }

        public ecopower_dbContext(DbContextOptions<ecopower_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.CellPhone).HasMaxLength(50);

                entity.Property(e => e.CustomerName).HasMaxLength(50);

                entity.Property(e => e.CustomerSurname).HasMaxLength(50);

                entity.Property(e => e.CustomerTitle).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.Property(e => e.DeliveryAddress).HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Customers");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailsId);

                entity.Property(e => e.OrderDetailsId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrderDetailsID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Products1");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).ValueGeneratedNever();

                entity.Property(e => e.ProductDescription).HasMaxLength(50);

                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
