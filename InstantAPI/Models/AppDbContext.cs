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

                entity.HasIndex(e => e.Login, "UQ__app_user__7838F27221125695")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAuteur).HasColumnName("Id_auteur");

                entity.Property(e => e.IdRole)
                    .HasColumnName("Id_role")
                    .HasDefaultValueSql("((1))");

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
                    .HasConstraintName("FK__app_user__Id_aut__3B75D760");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK__app_user__Id_rol__3A81B327");
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
                        l => l.HasOne<Image>().WithMany().HasForeignKey("IdImage").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_i__Id_im__4316F928"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("IdArticle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_i__Id_ar__4222D4EF"),
                        j =>
                        {
                            j.HasKey("IdArticle", "IdImage").HasName("PK__article___F2BDA3B6F97BD216");

                            j.ToTable("article_image");

                            j.IndexerProperty<int>("IdArticle").HasColumnName("Id_article");

                            j.IndexerProperty<int>("IdImage").HasColumnName("Id_image");
                        });

                entity.HasMany(d => d.IdTags)
                    .WithMany(p => p.IdArticles)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArticleTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("IdTag").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_t__Id_ta__3F466844"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("IdArticle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_t__Id_ar__3E52440B"),
                        j =>
                        {
                            j.HasKey("IdArticle", "IdTag").HasName("PK__article___9188955CE1384B4F");

                            j.ToTable("article_tag");

                            j.IndexerProperty<int>("IdArticle").HasColumnName("Id_article");

                            j.IndexerProperty<int>("IdTag").HasColumnName("Id_tag");
                        });
            });

            modelBuilder.Entity<Auteur>(entity =>
            {
                entity.ToTable("auteur");

                entity.HasIndex(e => e.Pseudo, "UQ__auteur__EA0EEA220116080B")
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

                entity.HasIndex(e => e.Filepath, "UQ__image__DFE356BEA65DD45B")
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

                entity.HasIndex(e => e.Name, "UQ__role__72E12F1B6B0BB981")
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

                entity.HasIndex(e => e.Mot, "UQ__tag__DF50CE3CAF5B8A24")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Mot)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mot");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
