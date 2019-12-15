﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TL539WebApi.DbContexts;

namespace TL539WebApi.Migrations
{
    [DbContext(typeof(TL539WebApiContext))]
    [Migration("20191207181244_first")]
    partial class first
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TL539WebApi.Entities.WinNumber", b =>
                {
                    b.Property<int>("WinNumberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ASC1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ASC2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ASC3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ASC4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ASC5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DrawOrder1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DrawOrder2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DrawOrder3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DrawOrder4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DrawOrder5")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WinNumberID");

                    b.ToTable("WinNumbers");
                });
#pragma warning restore 612, 618
        }
    }
}
