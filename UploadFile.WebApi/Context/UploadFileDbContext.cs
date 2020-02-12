using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using UploadFile.WebApi.Models;

namespace UploadFile.WebApi.Context
{
    public partial class UploadFileDbContext : DbContext
    {
        private readonly string _connectionString;

        public UploadFileDbContext()
        {
        }

        public UploadFileDbContext(DbContextOptions<UploadFileDbContext> options,  IConfiguration config)
            : base(options)
        {
            _connectionString = config["UploadFileDb"];
        }

        // setando os models 
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Document> Document { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);

                // para gerar uma nova migração comentar a linha de cima e descomentar a linha de baixo
                // optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=UploadFile;Username=postgres;Password=postgre");
            }
        }

        // mapeamento das entidaudes via fluente api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // mepeando Enitdade usuario
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(100);

                entity.Property(e => e.Removed)
                    .HasColumnName("removed")
                    .HasDefaultValueSql("false");
            });

            // mepeando Enitdade documento
            modelBuilder.Entity<Document>(entity => 
            {
                entity.ToTable("documents");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.File)
                    .IsRequired()
                    .HasColumnName("file");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("file_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_user_id");
            });

            // popula com usuario sysadmin
            modelBuilder.Entity<User>().HasData(
                new User {
                    Id = 1,
                    Name = "Sys Admin",
                    Email = "sysadmin@email.com",
                    Password = "123456",
                    Removed = false
                }
            );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}