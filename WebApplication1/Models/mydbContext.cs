﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class mydbContext : DbContext
{
    public mydbContext(DbContextOptions<mydbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cabinet> Cabinets { get; set; }

    public virtual DbSet<Cabinettype> Cabinettypes { get; set; }

    public virtual DbSet<Day> Days { get; set; }

    public virtual DbSet<NumberPair> NumberPairs { get; set; }

    public virtual DbSet<Pair> Pairs { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Studentgroup> Studentgroups { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Subjectlesson> Subjectlessons { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Week> Weeks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(e => e.IdCabinet).HasName("PRIMARY");

            entity.ToTable("cabinet");

            entity.HasIndex(e => e.IdType, "fk_cabinet_cabinettype");

            entity.Property(e => e.IdCabinet).HasColumnName("id_cabinet");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.NameCabinet)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name_cabinet");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Cabinets)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("cabinet_ibfk_1");
        });

        modelBuilder.Entity<Cabinettype>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("PRIMARY");

            entity.ToTable("cabinettype");

            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.TypeCabinet)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Type_Cabinet");
        });

        modelBuilder.Entity<Day>(entity =>
        {
            entity.HasKey(e => e.IdDay).HasName("PRIMARY");

            entity.ToTable("day");

            entity.HasIndex(e => e.IdWeek, "fk_day_week");

            entity.Property(e => e.IdDay).HasColumnName("id_day");
            entity.Property(e => e.DayWeek)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("day_week");
            entity.Property(e => e.IdWeek).HasColumnName("id_week");

            entity.HasOne(d => d.IdWeekNavigation).WithMany(p => p.Days)
                .HasForeignKey(d => d.IdWeek)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("day_ibfk_1");
        });

        modelBuilder.Entity<NumberPair>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("number_pair");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NumberPair1).HasColumnName("number_pair");
        });

        modelBuilder.Entity<Pair>(entity =>
        {
            entity.HasKey(e => new { e.IdPair, e.IdTeacher, e.IdCabinet, e.IdGroup, e.IdDay, e.IdTypeLesson, e.IdSubject, e.IdSheduleNumber })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            entity.ToTable("pair");

            entity.HasIndex(e => e.IdCabinet, "fk_pair_cabinet");

            entity.HasIndex(e => e.IdDay, "fk_pair_day");

            entity.HasIndex(e => e.IdGroup, "fk_pair_studentgroup");

            entity.HasIndex(e => e.IdSubject, "fk_pair_subject");

            entity.HasIndex(e => e.IdTeacher, "fk_pair_teacher");

            entity.HasIndex(e => e.IdTypeLesson, "fk_pair_typelesson");

            entity.HasIndex(e => e.IdSheduleNumber, "id_shedule_number");

            entity.Property(e => e.IdPair)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_pair");
            entity.Property(e => e.IdTeacher).HasColumnName("id_teacher");
            entity.Property(e => e.IdCabinet).HasColumnName("id_cabinet");
            entity.Property(e => e.IdGroup).HasColumnName("id_group");
            entity.Property(e => e.IdDay).HasColumnName("id_day");
            entity.Property(e => e.IdTypeLesson).HasColumnName("id_type_lesson");
            entity.Property(e => e.IdSubject).HasColumnName("id_subject");
            entity.Property(e => e.IdSheduleNumber).HasColumnName("id_shedule_number");

            entity.HasOne(d => d.IdCabinetNavigation).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.IdCabinet)
                .HasConstraintName("fk_pair_cabinet");

            entity.HasOne(d => d.IdDayNavigation).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.IdDay)
                .HasConstraintName("fk_pair_day");

            entity.HasOne(d => d.IdGroupNavigation).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.IdGroup)
                .HasConstraintName("fk_pair_studentgroup");

            entity.HasOne(d => d.IdSheduleNumberNavigation).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.IdSheduleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pair_ibfk_7");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.IdSubject)
                .HasConstraintName("fk_pair_subject");

            entity.HasOne(d => d.IdTeacherNavigation).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.IdTeacher)
                .HasConstraintName("fk_pair_teacher");

            entity.HasOne(d => d.IdTypeLessonNavigation).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.IdTypeLesson)
                .HasConstraintName("fk_pair_typelesson");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.IdSemester).HasName("PRIMARY");

            entity.ToTable("semester");

            entity.Property(e => e.IdSemester).HasColumnName("id_semester");
            entity.Property(e => e.NumberSemester).HasColumnName("number_semester");
        });

        modelBuilder.Entity<Studentgroup>(entity =>
        {
            entity.HasKey(e => e.IdGroup).HasName("PRIMARY");

            entity.ToTable("studentgroup");

            entity.Property(e => e.IdGroup).HasColumnName("id_group");
            entity.Property(e => e.Course)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("course");
            entity.Property(e => e.NameGroup)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name_group");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.IdSubject).HasName("PRIMARY");

            entity.ToTable("subject");

            entity.Property(e => e.IdSubject).HasColumnName("id_subject");
            entity.Property(e => e.NameSubject)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name_subject");
        });

        modelBuilder.Entity<Subjectlesson>(entity =>
        {
            entity.HasKey(e => e.IdTypeless).HasName("PRIMARY");

            entity.ToTable("subjectlesson");

            entity.Property(e => e.IdTypeless).HasColumnName("id_typeless");
            entity.Property(e => e.NameOfTypeLesson)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("name_of_type_lesson");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.IdTeacher).HasName("PRIMARY");

            entity.ToTable("teacher");

            entity.Property(e => e.IdTeacher).HasColumnName("id_teacher");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.NameTeacher)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name_teacher");
        });

        modelBuilder.Entity<Week>(entity =>
        {
            entity.HasKey(e => e.IdWeek).HasName("PRIMARY");

            entity.ToTable("week");

            entity.HasIndex(e => e.IdSemester, "fk_week_semester");

            entity.Property(e => e.IdWeek).HasColumnName("id_week");
            entity.Property(e => e.IdSemester).HasColumnName("id_semester");
            entity.Property(e => e.TypeWeek)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("type_week");

            entity.HasOne(d => d.IdSemesterNavigation).WithMany(p => p.Weeks)
                .HasForeignKey(d => d.IdSemester)
                .HasConstraintName("fk_week_semester");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}