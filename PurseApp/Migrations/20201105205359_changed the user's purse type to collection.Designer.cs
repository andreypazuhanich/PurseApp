﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PurseApp;

namespace PurseApp.Migrations
{
    [DbContext(typeof(PurseAppDbContext))]
    [Migration("20201105205359_changed the user's purse type to collection")]
    partial class changedtheuserspursetypetocollection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PurseApp.Models.Account", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("CurrencyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PurseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AccountId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("PurseId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PurseApp.Models.Currency", b =>
                {
                    b.Property<Guid>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            CurrencyId = new Guid("4be468fa-9ebf-4d9c-80dd-bfe54ecf4b29"),
                            Name = "RUB"
                        });
                });

            modelBuilder.Entity("PurseApp.Models.Purse", b =>
                {
                    b.Property<Guid>("PurseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PurseId");

                    b.HasIndex("UserId");

                    b.ToTable("Purses");
                });

            modelBuilder.Entity("PurseApp.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("INN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PurseApp.Models.Account", b =>
                {
                    b.HasOne("PurseApp.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.HasOne("PurseApp.Models.Purse", null)
                        .WithMany("Accounts")
                        .HasForeignKey("PurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PurseApp.Models.Purse", b =>
                {
                    b.HasOne("PurseApp.Models.User", null)
                        .WithMany("Purses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
