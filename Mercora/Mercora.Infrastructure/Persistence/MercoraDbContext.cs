using System;
using System.Collections.Generic;
using Mercora.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Mercora.Infrastructure.Persistence;

public partial class MercoraDbContext : DbContext
{
    public MercoraDbContext()
    {
    }

    public MercoraDbContext(DbContextOptions<MercoraDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItems> CartItems { get; set; }

    public virtual DbSet<Carts> Carts { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Inventory> Inventory { get; set; }

    public virtual DbSet<OrderItems> OrderItems { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<Payments> Payments { get; set; }

    public virtual DbSet<ProductImages> ProductImages { get; set; }

    public virtual DbSet<ProductVariants> ProductVariants { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<UserRoles> UserRoles { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=mercora;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItems>(entity =>
        {
            entity.Property(e => e.AddedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_CartItems_AddedAtUtc");
            entity.Property(e => e.UpdatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_CartItems_UpdatedAtUtc");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasConstraintName("FK_CartItems_Carts");

            entity.HasOne(d => d.Variant).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Variants");
        });

        modelBuilder.Entity<Carts>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UX_Carts_User_Active")
                .IsUnique()
                .HasFilter("([Status]=(0))");

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Carts_CreatedAtUtc");
            entity.Property(e => e.UpdatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Carts_UpdatedAtUtc");

            entity.HasOne(d => d.User).WithOne(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Users");
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Categories_CreatedAtUtc");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Categories_IsActive");
            entity.Property(e => e.UpdatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Categories_UpdatedAtUtc");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory).HasConstraintName("FK_Categories_Parent");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.Property(e => e.VariantId).ValueGeneratedNever();
            entity.Property(e => e.UpdatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Inventory_UpdatedAtUtc");

            entity.HasOne(d => d.Variant).WithOne(p => p.Inventory).HasConstraintName("FK_Inventory_ProductVariants");
        });

        modelBuilder.Entity<OrderItems>(entity =>
        {
            entity.Property(e => e.LineTotal).HasComputedColumnSql("([UnitPrice]*[Quantity])", true);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Variant).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Variants");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Orders_CreatedAtUtc");
            entity.Property(e => e.CurrencyCode).IsFixedLength();
            entity.Property(e => e.GrandTotal).HasComputedColumnSql("((([Subtotal]-[DiscountTotal])+[TaxTotal])+[ShippingTotal])", true);

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<Payments>(entity =>
        {
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Payments_CreatedAtUtc");
            entity.Property(e => e.CurrencyCode).IsFixedLength();
            entity.Property(e => e.UpdatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Payments_UpdatedAtUtc");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments).HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<ProductImages>(entity =>
        {
            entity.HasIndex(e => e.ProductId, "UX_ProductImages_OnePrimaryPerProduct")
                .IsUnique()
                .HasFilter("([IsPrimary]=(1))");

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_ProductImages_CreatedAtUtc");

            entity.HasOne(d => d.Product).WithOne(p => p.ProductImages).HasConstraintName("FK_ProductImages_Products");
        });

        modelBuilder.Entity<ProductVariants>(entity =>
        {
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_ProductVariants_CreatedAtUtc");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_ProductVariants_IsActive");
            entity.Property(e => e.UpdatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_ProductVariants_UpdatedAtUtc");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants).HasConstraintName("FK_ProductVariants_Products");
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Products_CreatedAtUtc");
            entity.Property(e => e.CurrencyCode)
                .IsFixedLength()
                .HasDefaultValue("EUR", "DF_Products_Currency");
            entity.Property(e => e.UpdatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Products_UpdatedAtUtc");

            entity.HasMany(d => d.Category).WithMany(p => p.Product)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategories",
                    r => r.HasOne<Categories>().WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_ProductCategories_Categories"),
                    l => l.HasOne<Products>().WithMany()
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_ProductCategories_Products"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId");
                        j.HasIndex(new[] { "CategoryId" }, "IX_ProductCategories_CategoryId");
                    });
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Roles_CreatedAtUtc");
        });

        modelBuilder.Entity<UserRoles>(entity =>
        {
            entity.Property(e => e.AssignedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_UserRoles_AssignedAtUtc");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasConstraintName("FK_UserRoles_Users");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Users_CreatedAtUtc");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Users_IsActive");
            entity.Property(e => e.UpdatedAtUtc).HasDefaultValueSql("(sysutcdatetime())", "DF_Users_UpdatedAtUtc");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
