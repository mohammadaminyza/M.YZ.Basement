﻿// <auto-generated />
using System;
using M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zamin.MiniBlog.Infra.Data.Sql.Commands.Common;

namespace M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.Migrations
{
    [DbContext(typeof(MiniblogDbContext))]
    [Migration("20210313204811_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("M.YZ.Basement.Infra.Events.Outbox.OutBoxEventItem", b =>
                {
                    b.Property<long>("OutBoxEventItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("AccuredByUserId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("AccuredOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("AggregateId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AggregateName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AggregateTypeName")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EventName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("EventPayload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventTypeName")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("bit");

                    b.HasKey("OutBoxEventItemId");

                    b.ToTable("OutBoxEventItems");
                });

            modelBuilder.Entity("M.YZ.Basement.MiniBlog.Core.Domain.People.Entities.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<Guid>("BusinessId")
                        .IsUnicode(true)
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasAlternateKey("BusinessId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("M.YZ.Basement.MiniBlog.Core.Domain.Writers.Entities.Writer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<Guid>("BusinessId")
                        .IsUnicode(true)
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ModifiedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasAlternateKey("BusinessId");

                    b.ToTable("Writers");
                });
#pragma warning restore 612, 618
        }
    }
}
