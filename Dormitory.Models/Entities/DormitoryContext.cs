using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.Models.Entities;

public partial class DormitoryContext : DbContext
{
    public DormitoryContext()
    {
    }

    public DormitoryContext(DbContextOptions<DormitoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=QL_KXT;Username=postgres;Password=Abc123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Buildingid).HasName("buildings_pkey");

            entity.ToTable("buildings");

            entity.Property(e => e.Buildingid)
                .HasMaxLength(10)
                .HasColumnName("buildingid");
            entity.Property(e => e.Buildingname)
                .HasMaxLength(50)
                .HasColumnName("buildingname");
            entity.Property(e => e.Currentoccupancy)
                .HasDefaultValue(0)
                .HasColumnName("currentoccupancy");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Numberofrooms).HasColumnName("numberofrooms");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Contractid).HasName("contracts_pkey");

            entity.ToTable("contracts");

            entity.Property(e => e.Contractid)
                .HasMaxLength(10)
                .HasColumnName("contractid");
            entity.Property(e => e.Endtime).HasColumnName("endtime");
            entity.Property(e => e.Roomid)
                .HasMaxLength(10)
                .HasColumnName("roomid");
            entity.Property(e => e.Starttime).HasColumnName("starttime");
            entity.Property(e => e.Userid)
                .HasMaxLength(10)
                .HasColumnName("userid");

            entity.HasOne(d => d.Room).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.Roomid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contract_room");

            entity.HasOne(d => d.User).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contract_user");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.Paymentid)
                .HasMaxLength(10)
                .HasColumnName("paymentid");
            entity.Property(e => e.Contractid)
                .HasMaxLength(10)
                .HasColumnName("contractid");
            entity.Property(e => e.Paymentdate).HasColumnName("paymentdate");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(20)
                .HasColumnName("paymentmethod");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Unpaid'::character varying")
                .HasColumnName("paymentstatus");

            entity.HasOne(d => d.Contract).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Contractid)
                .HasConstraintName("fk_payments_contracts");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Roleid)
                .HasMaxLength(10)
                .HasColumnName("roleid");
            entity.Property(e => e.Roledescription)
                .HasMaxLength(100)
                .HasColumnName("roledescription");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Roomid).HasName("rooms_pkey");

            entity.ToTable("rooms");

            entity.Property(e => e.Roomid)
                .HasMaxLength(10)
                .HasColumnName("roomid");
            entity.Property(e => e.Buildingid)
                .HasMaxLength(10)
                .HasColumnName("buildingid");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Currentoccupancy)
                .HasDefaultValue(0)
                .HasColumnName("currentoccupancy");
            entity.Property(e => e.Roomnumber).HasColumnName("roomnumber");

            entity.HasOne(d => d.Building).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.Buildingid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_room_building");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Studentid).HasName("students_pkey");

            entity.ToTable("students");

            entity.Property(e => e.Studentid)
                .HasMaxLength(10)
                .HasColumnName("studentid");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(15)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Studentname)
                .HasMaxLength(50)
                .HasColumnName("studentname");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Userid)
                .HasMaxLength(10)
                .HasColumnName("userid");
            entity.Property(e => e.Roleid)
                .HasMaxLength(10)
                .HasColumnName("roleid");
            entity.Property(e => e.Studentid)
                .HasMaxLength(10)
                .HasColumnName("studentid");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("fk_users_roles");

            entity.HasOne(d => d.Student).WithMany(p => p.Users)
                .HasForeignKey(d => d.Studentid)
                .HasConstraintName("fk_users_students");
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.Violationid).HasName("violations_pkey");

            entity.ToTable("violations");

            entity.Property(e => e.Violationid)
                .HasMaxLength(10)
                .HasColumnName("violationid");
            entity.Property(e => e.Penaltyfee)
                .HasPrecision(12, 2)
                .HasColumnName("penaltyfee");
            entity.Property(e => e.Userid)
                .HasMaxLength(10)
                .HasColumnName("userid");
            entity.Property(e => e.Violationdate).HasColumnName("violationdate");
            entity.Property(e => e.Violationtype)
                .HasMaxLength(50)
                .HasColumnName("violationtype");

            entity.HasOne(d => d.User).WithMany(p => p.Violations)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_violations_users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
