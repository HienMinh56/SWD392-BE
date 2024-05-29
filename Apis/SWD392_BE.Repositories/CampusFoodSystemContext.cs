using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;

namespace SWD392_BE.Repositories;

public partial class CampusFoodSystemContext : DbContext
{
    public CampusFoodSystemContext()
    {
    }

    public CampusFoodSystemContext(DbContextOptions<CampusFoodSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<AreaSession> AreaSessions { get; set; }

    public virtual DbSet<Campus> Campuses { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;uid=sa;pwd=12345;database=CampusFoodSystem;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Area__3214EC07B26BFC36");

            entity.ToTable("Area");

            entity.HasIndex(e => e.AreaId, "UQ__Area__70B82049341DE850").IsUnique();

            entity.Property(e => e.AreaId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AreaSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AreaSess__3214EC076D60CD96");

            entity.ToTable("AreaSession");

            entity.HasIndex(e => e.AreaSessionId, "UQ__AreaSess__E80F50256BAAC6F0").IsUnique();

            entity.Property(e => e.AreaId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AreaSessionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SessionId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.AreaSessions)
                .HasPrincipalKey(p => p.AreaId)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AreaSession_Area");

            entity.HasOne(d => d.Session).WithMany(p => p.AreaSessions)
                .HasPrincipalKey(p => p.SessionId)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AreaSession_Session");
        });

        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campus__3214EC07A5723796");

            entity.ToTable("Campus");

            entity.HasIndex(e => e.CampusId, "UQ__Campus__FD598DD7CC2976B3").IsUnique();

            entity.Property(e => e.AreaId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampusId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Campuses)
                .HasPrincipalKey(p => p.AreaId)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Campus_Area");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Food__3214EC078EFC4B87");

            entity.ToTable("Food");

            entity.HasIndex(e => e.FoodId, "UQ__Food__856DB3EA0A736D4B").IsUnique();

            entity.Property(e => e.FoodId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StoreId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Store).WithMany(p => p.Foods)
                .HasPrincipalKey(p => p.StoreId)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Food_Store");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07C8EDD9E8");

            entity.ToTable("Order");

            entity.HasIndex(e => e.OrderId, "UQ__Order__C3905BCED0AE1AF1").IsUnique();

            entity.Property(e => e.AreaSessionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StoreId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransationId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AreaSession).WithMany(p => p.Orders)
                .HasPrincipalKey(p => p.AreaSessionId)
                .HasForeignKey(d => d.AreaSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_AreaSession");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasPrincipalKey(p => p.StoreId)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Store");

            entity.HasOne(d => d.Transation).WithMany(p => p.Orders)
                .HasPrincipalKey(p => p.TransationId)
                .HasForeignKey(d => d.TransationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Transaction");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasPrincipalKey(p => p.UserId)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07531AB5AA");

            entity.ToTable("OrderDetail");

            entity.HasIndex(e => e.OrderDetailId, "UQ__OrderDet__D3B9D36D90383D15").IsUnique();

            entity.Property(e => e.FoodId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderDetailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Food).WithMany(p => p.OrderDetails)
                .HasPrincipalKey(p => p.FoodId)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Food");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasPrincipalKey(p => p.OrderId)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Session__3214EC07E8C8F427");

            entity.ToTable("Session");

            entity.HasIndex(e => e.SessionId, "UQ__Session__C9F49291B4A69BBB").IsUnique();

            entity.Property(e => e.SessionId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Store__3214EC07F087FC19");

            entity.ToTable("Store");

            entity.HasIndex(e => e.StoreId, "UQ__Store__3B82F100A9787477").IsUnique();

            entity.Property(e => e.AreaId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StoreId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Stores)
                .HasPrincipalKey(p => p.AreaId)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Store_Area");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0711B818F5");

            entity.ToTable("Transaction");

            entity.HasIndex(e => e.TransationId, "UQ__Transact__B1E7315490DC15CB").IsUnique();

            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.TransationId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasPrincipalKey(p => p.UserId)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07644E0073");

            entity.ToTable("User");

            entity.HasIndex(e => e.UserId, "UQ__User__1788CC4D63AAACC1").IsUnique();

            entity.Property(e => e.AccessToken).IsUnicode(false);
            entity.Property(e => e.CampusId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken).IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Campus).WithMany(p => p.Users)
                .HasPrincipalKey(p => p.CampusId)
                .HasForeignKey(d => d.CampusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Campus");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
