using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InstantAPI.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<Auteur> Auteurs { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Test> Tests { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\;Database=DbBlog;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("app_user");

                entity.HasIndex(e => e.Login, "UQ__app_user__7838F2720B0DD406")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAuteur).HasColumnName("Id_auteur");

                entity.Property(e => e.IdRole).HasColumnName("Id_role");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Login)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.HasOne(d => d.IdAuteurNavigation)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.IdAuteur)
                    .HasConstraintName("FK__app_user__Id_aut__3A81B327");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__app_user__Id_rol__398D8EEE");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("article");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAuteur).HasColumnName("Id_auteur");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Texte)
                    .IsUnicode(false)
                    .HasColumnName("texte");

                entity.Property(e => e.Titre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("titre");

                entity.HasOne(d => d.IdAuteurNavigation)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.IdAuteur)
                    .HasConstraintName("FK__article__Id_aute__29572725");

                entity.HasMany(d => d.IdImages)
                    .WithMany(p => p.IdArticles)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArticleImage",
                        l => l.HasOne<Image>().WithMany().HasForeignKey("IdImage").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_i__Id_im__4222D4EF"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("IdArticle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_i__Id_ar__412EB0B6"),
                        j =>
                        {
                            j.HasKey("IdArticle", "IdImage").HasName("PK__article___F2BDA3B6B8017445");

                            j.ToTable("article_image");

                            j.IndexerProperty<int>("IdArticle").HasColumnName("Id_article");

                            j.IndexerProperty<int>("IdImage").HasColumnName("Id_image");
                        });

                entity.HasMany(d => d.IdTags)
                    .WithMany(p => p.IdArticles)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArticleTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("IdTag").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_t__Id_ta__3E52440B"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("IdArticle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_t__Id_ar__3D5E1FD2"),
                        j =>
                        {
                            j.HasKey("IdArticle", "IdTag").HasName("PK__article___9188955CD2D754EE");

                            j.ToTable("article_tag");

                            j.IndexerProperty<int>("IdArticle").HasColumnName("Id_article");

                            j.IndexerProperty<int>("IdTag").HasColumnName("Id_tag");
                        });
            });

            modelBuilder.Entity<Auteur>(entity =>
            {
                entity.ToTable("auteur");

                entity.HasIndex(e => e.Pseudo, "UQ__auteur__EA0EEA224006EE83")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Nom)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("prenom");

                entity.Property(e => e.Pseudo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("pseudo");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image");

                entity.HasIndex(e => e.Filepath, "UQ__image__DFE356BEC2216E98")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Filepath)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("filepath");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Titre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("titre");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.HasIndex(e => e.Name, "UQ__role__72E12F1B1C6E244A")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tag");

                entity.HasIndex(e => e.Mot, "UQ__tag__DF50CE3CB8B3F602")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Mot)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mot");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("test");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Titre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("titre");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
