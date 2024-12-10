﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebDatLich.Data;

public partial class CsdlDuLichContext : DbContext
{
    public CsdlDuLichContext()
    {
    }

    public CsdlDuLichContext(DbContextOptions<CsdlDuLichContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourGuide> TourGuides { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-5H24H4P;Initial Catalog=CSDL_DuLich;User ID=Sql;Password=123456;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__Accounts__F3DBC57334CD2C4B");

            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");

            entity.HasOne(d => d.Customer).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Accounts__custom__60A75C0F");

            entity.HasOne(d => d.Employee).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Accounts__employ__619B8048");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__5DE3A5B1E7DB8E54");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.BookingDate).HasColumnName("booking_date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.TourId).HasColumnName("tour_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__custom__5629CD9C");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__tour_i__571DF1D5");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB859277D034");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.DestinationId).HasName("PK__Destinat__5501539108393012");

            entity.Property(e => e.DestinationId).HasColumnName("destination_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DestinationName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("destination_name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C52E0BA8EDF39BEC");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("position");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__7A6B2B8C16B8AD1F");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.Comments)
                .HasColumnType("text")
                .HasColumnName("comments");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TourId).HasColumnName("tour_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedbacks__custo__5CD6CB2B");

            entity.HasOne(d => d.Tour).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedbacks__tour___5DCAEF64");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__ED1FC9EA1BC8DF3B");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment_method");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__bookin__59FA5E80");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.TourId).HasName("PK__Tours__4B16B9E668FB5C36");

            entity.Property(e => e.TourId).HasColumnName("tour_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DestinationId).HasColumnName("destination_id");
            entity.Property(e => e.EndDay).HasColumnName("end_day");
            entity.Property(e => e.GuideId).HasColumnName("guide_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.StartDay).HasColumnName("start_day");
            entity.Property(e => e.TourName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tour_name");

            entity.HasOne(d => d.Destination).WithMany(p => p.Tours)
                .HasForeignKey(d => d.DestinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tours__destinati__5070F446");

            entity.HasOne(d => d.Guide).WithMany(p => p.Tours)
                .HasForeignKey(d => d.GuideId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tours__guide_id__5165187F");
        });

        modelBuilder.Entity<TourGuide>(entity =>
        {
            entity.HasKey(e => e.GuideId).HasName("PK__TourGuid__04A822417F30DD0C");

            entity.Property(e => e.GuideId).HasColumnName("guide_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");
            entity.Property(e => e.LanguageSpoken)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("language_spoken");

            entity.HasOne(d => d.Employee).WithOne(p => p.TourGuides)
                .HasForeignKey<TourGuide>(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TourGuide__emplo__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
