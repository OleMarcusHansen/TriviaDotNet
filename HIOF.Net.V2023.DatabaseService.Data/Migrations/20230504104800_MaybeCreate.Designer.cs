﻿// <auto-generated />
using System;
using HIOF.Net.V2023.DatabaseService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HIOF.Net.V2023.DatabaseService.Data.Migrations
{
    [DbContext(typeof(HighScoreDbContext))]
    [Migration("20230504104800_MaybeCreate")]
    partial class MaybeCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HIOF.Net.V2023.DatabaseService.Data.HighScore", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Correct")
                        .HasColumnType("int");

                    b.Property<int>("Wrong")
                        .HasColumnType("int");

                    b.HasKey("Id", "Category");

                    b.ToTable("HighScores");
                });

            modelBuilder.Entity("HIOF.Net.V2023.DatabaseService.Data.UserData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Correct")
                        .HasColumnType("int");

                    b.Property<int>("Wrong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserData");
                });

            modelBuilder.Entity("HIOF.Net.V2023.DatabaseService.Data.HighScore", b =>
                {
                    b.HasOne("HIOF.Net.V2023.DatabaseService.Data.UserData", "User")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
