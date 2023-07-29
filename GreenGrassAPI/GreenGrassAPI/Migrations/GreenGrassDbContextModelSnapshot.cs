﻿// <auto-generated />
using System;
using GreenGrassAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GreenGrassAPI.Migrations
{
    [DbContext(typeof(GreenGrassDbContext))]
    partial class GreenGrassDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GreenGrassAPI.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("LastFertilizingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastWateringDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NextFertilizingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NextWateringDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PlantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlantId")
                        .IsUnique();

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("GreenGrassAPI.Models.Plant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bursting")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CareInstructions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FertilizingFrequency")
                        .HasColumnType("int");

                    b.Property<int>("HumidityRangeMax")
                        .HasColumnType("int");

                    b.Property<int>("HumidityRangeMin")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lighting")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NotificationId")
                        .HasColumnType("int");

                    b.Property<string>("PottedSuggestions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prunning")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RepottingFrequency")
                        .HasColumnType("int");

                    b.Property<string>("SoilType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TemperatureRangeMax")
                        .HasColumnType("int");

                    b.Property<int>("TemperatureRangeMin")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WateringFrequency")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("GreenGrassAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GreenGrassAPI.Models.Notification", b =>
                {
                    b.HasOne("GreenGrassAPI.Models.Plant", "Plant")
                        .WithOne("Notification")
                        .HasForeignKey("GreenGrassAPI.Models.Notification", "PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("GreenGrassAPI.Models.Plant", b =>
                {
                    b.HasOne("GreenGrassAPI.Models.User", "User")
                        .WithMany("Plants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GreenGrassAPI.Models.Plant", b =>
                {
                    b.Navigation("Notification");
                });

            modelBuilder.Entity("GreenGrassAPI.Models.User", b =>
                {
                    b.Navigation("Plants");
                });
#pragma warning restore 612, 618
        }
    }
}
