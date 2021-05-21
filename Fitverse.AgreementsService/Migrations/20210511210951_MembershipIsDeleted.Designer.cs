﻿// <auto-generated />
using System;
using Fitverse.AgreementsService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fitverse.AgreementsService.Migrations
{
    [DbContext(typeof(AgreementsContext))]
    [Migration("20210511210951_MembershipIsDeleted")]
    partial class MembershipIsDeleted
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Fitverse.AgreementsService.Models.Agreement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AgreementId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndingDate")
                        .HasColumnType("Date");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("MembershipId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("StartingDate")
                        .HasColumnType("Date");

                    b.Property<int>("TerminationPeriod")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId")
                        .IsUnique();

                    b.ToTable("Agreements");
                });

            modelBuilder.Entity("Fitverse.AgreementsService.Models.Installment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AgreementId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("Date");

                    b.Property<DateTime>("EndingDate")
                        .HasColumnType("Date");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<DateTime>("StartingDate")
                        .HasColumnType("Date");

                    b.HasKey("Id");

                    b.ToTable("Installments");
                });

            modelBuilder.Entity("Fitverse.AgreementsService.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId")
                        .IsUnique();

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Fitverse.AgreementsService.Models.Membership", b =>
                {
                    b.Property<int>("MembershipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<float>("InstallmentPrice")
                        .HasColumnType("real");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("PeriodType")
                        .HasColumnType("int");

                    b.Property<int>("TerminationPeriod")
                        .HasColumnType("int");

                    b.HasKey("MembershipId");

                    b.ToTable("Memberships");
                });
#pragma warning restore 612, 618
        }
    }
}
