﻿using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.Migrations
{
    [ExcludeFromCodeCoverage]

    [DbContext(typeof(ContextObl))]
    partial class ContextOblModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Domain.Coupon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("MarketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MarketId");

                    b.HasIndex("UserId");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("Domain.Market", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ClosingTime")
                        .HasColumnType("datetime2");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoCoupon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpeningTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Market");
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

            modelBuilder.Entity("Domain.Purchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("MarketAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MarketName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
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

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Token");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("ProductUser", b =>
                {
                    b.Property<Guid>("FavoriteOfId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FavoritesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FavoriteOfId", "FavoritesId");

                    b.HasIndex("FavoritesId");

                    b.ToTable("ProductUser");
                });

            modelBuilder.Entity("Domain.Coupon", b =>
                {
                    b.HasOne("Domain.Market", "Market")
                        .WithMany()
                        .HasForeignKey("MarketId");

                    b.HasOne("Domain.User", null)
                        .WithMany("Coupons")
                        .HasForeignKey("UserId");

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

            modelBuilder.Entity("Domain.Purchase", b =>
                {
                    b.HasOne("Domain.User", null)
                        .WithMany("Purchases")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ProductUser", b =>
                {
                    b.HasOne("Domain.User", null)
                        .WithMany()
                        .HasForeignKey("FavoriteOfId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Product", null)
                        .WithMany()
                        .HasForeignKey("FavoritesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Market", b =>
                {
                    b.Navigation("ProductsMarkets");
                });

            modelBuilder.Entity("Domain.Product", b =>
                {
                    b.Navigation("ProductsMarkets");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Navigation("Coupons");

                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
