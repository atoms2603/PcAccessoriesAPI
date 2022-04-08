﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PcAccessories.Entities.Entities;
using PcAccessories.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAccessories.EFCore.Data
{
    public class PcAccessoriesDbContext : IdentityDbContext<User,Role,Guid>
    {
        public PcAccessoriesDbContext(DbContextOptions<PcAccessoriesDbContext> options) : base(options)
        {

        }

        public DbSet<Slide> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductLove> ProductLoves { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens").HasKey(x => x.UserId);

            modelBuilder.Entity<Slide>(entity =>
            {
                entity.Property(e => e.SlideId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.BrandId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(e => e.ProductImageId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ProductLove>(entity =>
            {
                entity.Property(e => e.ProductLoveId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.InvoiceId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.Property(e => e.InvoiceDetailId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InvoiceId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });



            modelBuilder.Entity<Slide>().HasIndex(x => x.SlideId).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(x => x.CategoryId).IsUnique();
            modelBuilder.Entity<Brand>().HasIndex(x => x.BrandId).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(x => x.ProductId).IsUnique();
            modelBuilder.Entity<ProductImage>().HasIndex(x => x.ProductImageId).IsUnique();
            modelBuilder.Entity<ProductLove>().HasIndex(x => x.ProductLoveId).IsUnique();
            modelBuilder.Entity<Invoice>().HasIndex(x => x.InvoiceId).IsUnique();
            modelBuilder.Entity<InvoiceDetail>().HasIndex(x => x.InvoiceDetailId).IsUnique();

            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "huyt4242@gmail.com",
                NormalizedEmail = "huyt4242@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "admin"),
                SecurityStamp = string.Empty,
                Name = "Atoms",
                Address = "HCM",
                PhoneNumber = "0342553542"
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

            modelBuilder.Entity<Slide>().HasData(
              new Slide() { SlideId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00D1"), Image = "1.png", Status = (byte)PcAccessoriesEnum.SlideStatus.Active },
              new Slide() { SlideId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00D2"), Image = "2.png", Status = (byte)PcAccessoriesEnum.SlideStatus.Active },
              new Slide() { SlideId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00D3"), Image = "3.png", Status = (byte)PcAccessoriesEnum.SlideStatus.Active },
              new Slide() { SlideId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00D4"), Image = "4.png", Status = (byte)PcAccessoriesEnum.SlideStatus.Active },
              new Slide() { SlideId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00D5"), Image = "5.png", Status = (byte)PcAccessoriesEnum.SlideStatus.Active }
              );
        }
    }
}
