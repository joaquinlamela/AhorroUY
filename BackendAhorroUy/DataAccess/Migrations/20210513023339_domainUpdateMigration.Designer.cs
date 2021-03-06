// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(ContextObl))]
    [Migration("20210513023339_domainUpdateMigration")]
    partial class domainUpdateMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Domain.Market", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Market");
                });

            modelBuilder.Entity("Domain.MarketHours", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ClosingTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DayOfTheWeek")
                        .HasColumnType("int");

                    b.Property<Guid?>("MarketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OpeningTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MarketId");

                    b.ToTable("MarketHours");
                });

            modelBuilder.Entity("Domain.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Domain.ProductMarket", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MarketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("CurrentPrice")
                        .HasColumnType("float");

                    b.Property<int>("QuantityAvailable")
                        .HasColumnType("int");

                    b.Property<double>("RegularPrice")
                        .HasColumnType("float");

                    b.HasKey("ProductId", "MarketId");

                    b.HasIndex("MarketId");

                    b.ToTable("ProductsMarkets");
                });

            modelBuilder.Entity("Domain.Token", b =>
                {
                    b.Property<string>("TokenValue")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TokenValue");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.UserSession", b =>
                {
                    b.Property<Guid>("Token")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Token");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("Domain.MarketHours", b =>
                {
                    b.HasOne("Domain.Market", "Market")
                        .WithMany("Hours")
                        .HasForeignKey("MarketId");

                    b.Navigation("Market");
                });

            modelBuilder.Entity("Domain.Product", b =>
                {
                    b.HasOne("Domain.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Domain.ProductMarket", b =>
                {
                    b.HasOne("Domain.Market", "Market")
                        .WithMany("ProductsMarkets")
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Product", "Product")
                        .WithMany("ProductsMarkets")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Market");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Market", b =>
                {
                    b.Navigation("Hours");

                    b.Navigation("ProductsMarkets");
                });

            modelBuilder.Entity("Domain.Product", b =>
                {
                    b.Navigation("ProductsMarkets");
                });
#pragma warning restore 612, 618
        }
    }
}
