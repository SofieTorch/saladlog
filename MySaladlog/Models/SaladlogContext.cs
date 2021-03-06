using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MySaladlog.Models
{
    public partial class SaladlogContext : DbContext
    {
        public SaladlogContext()
        {
        }

        public SaladlogContext(DbContextOptions<SaladlogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<LikeArticle> LikeArticles { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagArticle> TagArticles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=WINDOWS-3FQTUR9\\SQLEXPRESS;Initial Catalog=DBWebBlog;User Id=sa;Password=Hemmo1996;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Path).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Article_User");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Comment1).IsUnicode(false);

                entity.Property(e => e.DateComment).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Article");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_User");
            });

            modelBuilder.Entity<LikeArticle>(entity =>
            {
                entity.HasKey(e => e.IdLikeArticle)
                    .HasName("Like_Article_pk")
                    .IsClustered(false);

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.LikeArticles)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Like_Article_Article");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.LikeArticles)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Like_Article_User");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagName).IsUnicode(false);
            });

            modelBuilder.Entity<TagArticle>(entity =>
            {
                entity.HasKey(e => e.IdTagArticle)
                    .HasName("Tag_Article_pk")
                    .IsClustered(false);

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.TagArticles)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tag_Article_Article");

                entity.HasOne(d => d.IdTagNavigation)
                    .WithMany(p => p.TagArticles)
                    .HasForeignKey(d => d.IdTag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tag_Article_Tag");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
