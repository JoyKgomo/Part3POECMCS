﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using POEPART2CMCSFINAL.Services;

#nullable disable

namespace POEPART2CMCSFINAL.Migrations
{
    [DbContext(typeof(ClaimContext))]
    [Migration("20241121145757_InitialCreate4")]
    partial class InitialCreate4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("POEPART2CMCSFINAL.Models.Claim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("AmountDue")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("DateClaimed")
                        .HasColumnType("TEXT");

                    b.Property<double>("HourlyRate")
                        .HasColumnType("REAL");

                    b.Property<int>("HoursWorked")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("Claims");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AmountDue = 2400.0,
                            DateClaimed = new DateTime(2024, 11, 21, 16, 57, 57, 162, DateTimeKind.Local).AddTicks(1281),
                            HourlyRate = 2000.0,
                            HoursWorked = 34,
                            UserID = 1,
                            status = "Pending"
                        },
                        new
                        {
                            Id = 2,
                            AmountDue = 2400.0,
                            DateClaimed = new DateTime(2024, 11, 21, 16, 57, 57, 162, DateTimeKind.Local).AddTicks(1297),
                            HourlyRate = 2000.0,
                            HoursWorked = 34,
                            UserID = 2,
                            status = "pending"
                        });
                });

            modelBuilder.Entity("POEPART2CMCSFINAL.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClaimId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("POEPART2CMCSFINAL.Models.Users", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "John",
                            password = "password1",
                            role = "Lecturer",
                            username = "john@123.com"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Joy",
                            password = "joykgomo",
                            role = "HR",
                            username = "joy@kg.com"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Harry",
                            password = "password2",
                            role = "Programme Coordinator",
                            username = "hary@123.com"
                        },
                        new
                        {
                            ID = 4,
                            Name = "Sam",
                            password = "password3",
                            role = "Academic Manager",
                            username = "sam@123.com"
                        });
                });

            modelBuilder.Entity("POEPART2CMCSFINAL.Models.Claim", b =>
                {
                    b.HasOne("POEPART2CMCSFINAL.Models.Users", "Users")
                        .WithMany("Claims")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("POEPART2CMCSFINAL.Models.Users", b =>
                {
                    b.Navigation("Claims");
                });
#pragma warning restore 612, 618
        }
    }
}
