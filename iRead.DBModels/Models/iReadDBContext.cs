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
        public virtual DbSet<Favorite> Favorites { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<IdentificationMethod> IdentificationMethods { get; set; } = null!;
        public virtual DbSet<MemberContactInfo> MemberContactInfos { get; set; } = null!;
        public virtual DbSet<MemberPersonalInfo> MemberPersonalInfos { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Publisher> Publishers { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserCategory> UserCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1253_CI_AI");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Surname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.ImagePath).HasMaxLength(500);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ISBN")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Title).HasMaxLength(500);

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

                entity.HasMany(d => d.Publishers)
                    .WithMany(p => p.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "BooksPublisher",
                        l => l.HasOne<Publisher>().WithMany().HasForeignKey("PublisherId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BooksPubl__Publi__72C60C4A"),
                        r => r.HasOne<Book>().WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BooksPubl__BookI__71D1E811"),
                        j =>
                        {
                            j.HasKey("BookId", "PublisherId").HasName("PK__BooksPub__992695FD2B24A1FA");

                            j.ToTable("BooksPublishers");
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
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BookId })
                    .HasName("PK__Favorite__7456C06C5CDC01EE");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Favorites__BookI__7A672E12");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Favorites__UserI__797309D9");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<IdentificationMethod>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<MemberContactInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__MemberCo__1788CC4C532E2904");

                entity.ToTable("MemberContactInfo");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Telephone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

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

                entity.Property(e => e.IdNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Surname).HasMaxLength(255);

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
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BookId })
                    .HasName("PK__Ratings__7456C06C0FCC6B5A");

                entity.Property(e => e.Rating1).HasColumnName("Rating");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ratings__BookId__76969D2E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ratings__UserId__75A278F5");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.Salt)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.HasOne(d => d.UserCategoryNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserCategory)
                    .HasConstraintName("FK__Users__UserCateg__36B12243");

                entity.HasMany(d => d.Authors)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserFavoriteAuthor",
                        l => l.HasOne<Author>().WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserFavor__Autho__7849DB76"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserFavor__UserI__7755B73D"),
                        j =>
                        {
                            j.HasKey("UserId", "AuthorId").HasName("PK__UserFavo__4085638F1CF9EA1E");

                            j.ToTable("UserFavoriteAuthors");
                        });

                entity.HasMany(d => d.Categories)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserFavoriteCategory",
                        l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserFavor__Categ__74794A92"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserFavor__UserI__73852659"),
                        j =>
                        {
                            j.HasKey("UserId", "CategoryId").HasName("PK__UserFavo__B6185FEC7F14C88E");

                            j.ToTable("UserFavoriteCategories");
                        });
            });

            modelBuilder.Entity<UserCategory>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
