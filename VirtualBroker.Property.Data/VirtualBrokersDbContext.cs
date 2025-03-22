using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VirtualBroker.Property.Core;

namespace VirtualBroker.Property.Data;

public partial class VirtualBrokersDbContext : DbContext
{
    public VirtualBrokersDbContext()
    {
    }

    public VirtualBrokersDbContext(DbContextOptions<VirtualBrokersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<APIRequests_Zillow> APIRequests_Zillows { get; set; }

    public virtual DbSet<Properties_Zillow> Properties_Zillows { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         => optionsBuilder.UseSqlServer("Name=VirtualBroker");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<APIRequests_Zillow>(entity =>
        {
            entity.ToTable("APIRequests_Zillow");

            entity.HasIndex(e => e.Code, "IX_APIRequests_Zillow").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ApiKey).HasMaxLength(1000);
            entity.Property(e => e.ApiHost).HasMaxLength(1000);
            entity.Property(e => e.Code)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RequestUri).HasMaxLength(2048);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.APIRequests_ZillowCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_APIRequests_Zillow_Users");

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.APIRequests_ZillowUpdatedByUsers)
                .HasForeignKey(d => d.UpdatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_APIRequests_Zillow_Users1");
            entity.HasQueryFilter(f => !f.IsDeleted);
        });

        modelBuilder.Entity<Properties_Zillow>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Properties_Zillow");

            entity.HasIndex(e => e.Zpid, "IX_Properties_Zillow")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AffiliatedLink).HasMaxLength(1000);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Zpid).HasMaxLength(50);

            entity.HasOne(d => d.Api).WithMany(p => p.Properties_Zillows)
                .HasForeignKey(d => d.ApiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Properties_Zillow_APIRequests_Zillow");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Properties_ZillowCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Properties_Zillow_Users");

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.Properties_ZillowUpdatedByUsers)
                .HasForeignKey(d => d.UpdatedByUserId)
                .HasConstraintName("FK_Properties_Zillow_Users1");
            entity.HasQueryFilter(f => !f.IsDeleted);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.RoleCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roles_Users");

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.RoleUpdatedByUsers)
                .HasForeignKey(d => d.UpdatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roles_Users1");

            entity.HasMany(d => d.Users).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolesUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolesUsers_Users"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolesUsers_Roles"),
                    j =>
                    {
                        j.HasKey("RoleId", "UserId");
                        j.ToTable("RolesUsers");
                    });
            entity.HasQueryFilter(f => !f.IsDeleted);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(500);
            entity.Property(e => e.FirstName).HasMaxLength(500);
            entity.Property(e => e.LastName).HasMaxLength(500);
            entity.Property(e => e.ObjectId).HasMaxLength(50);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.InverseCreatedByUser).HasForeignKey(d => d.CreatedByUserId);

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.InverseUpdatedByUser).HasForeignKey(d => d.UpdatedByUserId);
            entity.HasQueryFilter(f => !f.IsDeleted);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
