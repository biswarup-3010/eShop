using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using eShopDAL.Models;

namespace eShopDAL.Data
{
    public partial class EShopContext : DbContext
    {
        public EShopContext()
        {
        }

        public EShopContext(DbContextOptions<EShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
        public virtual DbSet<Purchase> Purchases { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserInterest> UserInterests { get; set; } = null!;
        public virtual DbSet<Wishlist> Wishlists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Biswa@2001;database=eShopDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.3.0-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.HasIndex(e => e.ProductId, "ProductID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.AddedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasDefaultValueSql("'1'");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("cart_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("cart_ibfk_1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Status)
                    .HasColumnType("enum('Pending','Shipped','Delivered','Cancelled')")
                    .HasDefaultValueSql("'Pending'");

                entity.Property(e => e.TotalAmount).HasPrecision(10, 2);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("orders_ibfk_1");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasIndex(e => e.OrderId, "OrderID");

                entity.HasIndex(e => e.ProductId, "ProductID");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PriceAtPurchase).HasPrecision(10, 2);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("orderdetails_ibfk_1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("orderdetails_ibfk_2");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "CategoryID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Price).HasPrecision(10, 2);

                entity.Property(e => e.ProductName).HasMaxLength(100);

                entity.Property(e => e.Stock).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("products_ibfk_1");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ProductId, "ProductID");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.IsPrimary).HasDefaultValueSql("'0'");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("productimages_ibfk_1");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase");

                entity.HasIndex(e => e.OrderId, "OrderID");

                entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PaymentMethod).HasMaxLength(50);

                entity.Property(e => e.PaymentStatus)
                    .HasColumnType("enum('Success','Failed','Pending')")
                    .HasDefaultValueSql("'Pending'");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("purchase_ibfk_1");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasIndex(e => e.ProductId, "ProductID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.Comment).HasColumnType("text");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("reviews_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("reviews_ibfk_1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.RoleName, "RoleName")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "Email")
                    .IsUnique();

                entity.HasIndex(e => e.RoleId, "RoleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Address).HasColumnType("text");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.PasswordHash).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("users_ibfk_1");
            });

            modelBuilder.Entity<UserInterest>(entity =>
            {
                entity.HasKey(e => e.InterestId)
                    .HasName("PRIMARY");

                entity.ToTable("UserInterest");

                entity.HasIndex(e => e.CategoryId, "CategoryID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.InterestId).HasColumnName("InterestID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.InterestedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("userinterest_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("userinterest_ibfk_1");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.ToTable("Wishlist");

                entity.HasIndex(e => e.ProductId, "ProductID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.WishlistId).HasColumnName("WishlistID");

                entity.Property(e => e.AddedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("wishlist_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("wishlist_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
