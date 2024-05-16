using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServProgLab7and8.Models;

public partial class VideogamesContext : DbContext
{
    public VideogamesContext()
    {
    }

    public VideogamesContext(DbContextOptions<VideogamesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GamePlatform> GamePlatforms { get; set; }

    public virtual DbSet<GamePublisher> GamePublishers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RegionSale> RegionSales { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("DataSource=Database\\\\videogames.db;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("game");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.GameName)
                .HasDefaultValueSql("NULL")
                .HasColumnName("game_name");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");

            //entity.HasOne(d => d.Genre).WithMany(p => p.Games).HasForeignKey(d => d.GenreId);
        });

        modelBuilder.Entity<GamePlatform>(entity =>
        {
            entity.ToTable("game_platform");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.GamePublisherId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("game_publisher_id");
            entity.Property(e => e.PlatformId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("platform_id");
            entity.Property(e => e.ReleaseYear)
                .HasDefaultValueSql("NULL")
                .HasColumnName("release_year");

            entity.HasOne(d => d.GamePublisher).WithMany(p => p.GamePlatforms).HasForeignKey(d => d.GamePublisherId);

            entity.HasOne(d => d.Platform).WithMany(p => p.GamePlatforms).HasForeignKey(d => d.PlatformId);
        });

        modelBuilder.Entity<GamePublisher>(entity =>
        {
            entity.ToTable("game_publisher");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.GameId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("game_id");
            entity.Property(e => e.PublisherId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("publisher_id");

            //entity.HasOne(d => d.Game).WithMany(p => p.GamePublishers).HasForeignKey(d => d.GameId);

            entity.HasOne(d => d.Publisher).WithMany(p => p.GamePublishers).HasForeignKey(d => d.PublisherId);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("genre");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.GenreName)
                .HasDefaultValueSql("NULL")
                .HasColumnName("genre_name");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.ToTable("platform");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.PlatformName)
                .HasDefaultValueSql("NULL")
                .HasColumnName("platform_name");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable("publisher");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.PublisherName)
                .HasDefaultValueSql("NULL")
                .HasColumnName("publisher_name");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("region");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.RegionName)
                .HasDefaultValueSql("NULL")
                .HasColumnName("region_name");
        });

        modelBuilder.Entity<RegionSale>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("region_sales");

            entity.Property(e => e.GamePlatformId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("game_platform_id");
            entity.Property(e => e.NumSales).HasColumnName("num_sales");
            entity.Property(e => e.RegionId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("region_id");

            entity.HasOne(d => d.GamePlatform).WithMany().HasForeignKey(d => d.GamePlatformId);

            entity.HasOne(d => d.Region).WithMany().HasForeignKey(d => d.RegionId);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasNoKey().ToTable("auth_person");

            entity.Property(e => e.Login)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
