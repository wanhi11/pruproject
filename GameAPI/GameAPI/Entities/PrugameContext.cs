using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GameAPI.Entities;

public partial class PrugameContext : DbContext
{
    public PrugameContext()
    {
    }

    public PrugameContext(DbContextOptions<PrugameContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LeaderBoard> LeaderBoards { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            GetConnectionString());
    }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LeaderBoard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaderBo__3213E83F86D22382");

            entity.ToTable("LeaderBoard");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.LeaderBoards)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaderBoa__useri__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PK__user__CBA1B2578C710055");

            entity.ToTable("User");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}