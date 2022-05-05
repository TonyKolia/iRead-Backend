using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace iRead.DBModels.Models
{
    public partial class iReadDBContext : DbContext
    {
        public iReadDBContext()
        {
        }

        public iReadDBContext(DbContextOptions<iReadDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BooksStock> BooksStocks { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<IdentificationMethod> IdentificationMethods { get; set; } = null!;
        public virtual DbSet<MemberContactInfo> MemberContactInfos { get; set; } = null!;
        public virtual DbSet<MemberPersonalInfo> MemberPersonalInfos { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserCategory> UserCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasMany(d => d.Authors)
                    .WithMany(p => p.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "BooksAuthor",
                        l => l.HasOne<Author>().WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BooksAuth__Autho__31EC6D26"),
                        r => r.HasOne<Book>().WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BooksAuth__BookI__30F848ED"),
                        j =>
                        {
                            j.HasKey("BookId", "AuthorId").HasName("PK__BooksAut__6AED6DC41F667287");

                            j.ToTable("BooksAuthors");
                        });

                entity.HasMany(d => d.Categories)
                    .WithMany(p => p.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "BooksCategory",
                        l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BooksCate__Categ__2B3F6F97"),
                        r => r.HasOne<Book>().WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BooksCate__BookI__2A4B4B5E"),
                        j =>
                        {
                            j.HasKey("BookId", "CategoryId").HasName("PK__BooksCat__9C7051A7CFCE2C24");

                            j.ToTable("BooksCategories");
                        });
            });

            modelBuilder.Entity<BooksStock>(entity =>
            {
                entity.HasKey(e => e.BookId)
                    .HasName("PK__BooksSto__3DE0C20753F93620");

                entity.ToTable("BooksStock");

                entity.Property(e => e.BookId).ValueGeneratedNever();

                entity.HasOne(d => d.Book)
                    .WithOne(p => p.BooksStock)
                    .HasForeignKey<BooksStock>(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BooksStoc__BookI__2E1BDC42");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IdentificationMethod>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MemberContactInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__MemberCo__1788CC4C532E2904");

                entity.ToTable("MemberContactInfo");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.MemberContactInfo)
                    .HasForeignKey<MemberContactInfo>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MemberCon__UserI__403A8C7D");
            });

            modelBuilder.Entity<MemberPersonalInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__MemberPe__1788CC4C4756821F");

                entity.ToTable("MemberPersonalInfo");

                entity.HasIndex(e => e.IdNumber, "UQ__MemberPe__62DF8033604741B8")
                    .IsUnique();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.IdNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.GenderNavigation)
                    .WithMany(p => p.MemberPersonalInfos)
                    .HasForeignKey(d => d.Gender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MemberPer__Gende__3D5E1FD2");

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.MemberPersonalInfos)
                    .HasForeignKey(d => d.IdType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MemberPer__IdTyp__5CD6CB2B");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.MemberPersonalInfo)
                    .HasForeignKey<MemberPersonalInfo>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MemberPer__UserI__3C69FB99");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK__Orders__Status__45F365D3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Orders__UserId__44FF419A");

                entity.HasMany(d => d.Books)
                    .WithMany(p => p.Orders)
                    .UsingEntity<Dictionary<string, object>>(
                        "OrderItem",
                        l => l.HasOne<Book>().WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__OrderItem__BookI__49C3F6B7"),
                        r => r.HasOne<Order>().WithMany().HasForeignKey("OrderId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__OrderItem__Order__48CFD27E"),
                        j =>
                        {
                            j.HasKey("OrderId", "BookId").HasName("PK__OrderIte__A04E57EFA458A14B");

                            j.ToTable("OrderItems");
                        });
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserCategoryNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserCategory)
                    .HasConstraintName("FK__Users__UserCateg__36B12243");
            });

            modelBuilder.Entity<UserCategory>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
