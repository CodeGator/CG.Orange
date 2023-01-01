﻿// <auto-generated />
using System;
using CG.Orange.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CG.Orange.Data.Sqlite.Migrations
{
    [DbContext(typeof(OrangeDbContext))]
    partial class OrangeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("CG.Orange.Data.Entities.ProviderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProcessorType")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "IX_Provider_Names")
                        .IsUnique();

                    b.HasIndex(new[] { "ProviderType", "ProcessorType" }, "IX_Providers");

                    b.ToTable("Providers", "Orange");
                });

            modelBuilder.Entity("CG.Orange.Data.Entities.ProviderPropertyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProviderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.HasIndex(new[] { "Key" }, "IX_ProviderProperties")
                        .IsUnique();

                    b.ToTable("ProviderProperties", "Orange");
                });

            modelBuilder.Entity("CG.Orange.Data.Entities.SettingFileEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("EnvironmentName")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ApplicationName", "EnvironmentName" }, "IX_SettingFiles1")
                        .IsUnique();

                    b.HasIndex(new[] { "IsDisabled" }, "IX_SettingsFiles");

                    b.ToTable("SettingFiles", "Orange");
                });

            modelBuilder.Entity("CG.Orange.Data.Entities.ProviderPropertyEntity", b =>
                {
                    b.HasOne("CG.Orange.Data.Entities.ProviderEntity", "Provider")
                        .WithMany("Properties")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("CG.Orange.Data.Entities.ProviderEntity", b =>
                {
                    b.Navigation("Properties");
                });
#pragma warning restore 612, 618
        }
    }
}
