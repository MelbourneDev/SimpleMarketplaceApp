﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleMarketplaceApp.Data;

#nullable disable

namespace SimpleMarketplaceApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SimpleMarketplaceApp.Models.Category", b =>
                {
                    b.Property<int>("categoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("categoryDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("categoryName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("categoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("SimpleMarketplaceApp.Models.Item", b =>
                {
                    b.Property<int>("itemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateListed")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("itemId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SimpleMarketplaceApp.Models.ItemImage", b =>
                {
                    b.Property<int>("imageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("imageURL")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("itemID")
                        .HasColumnType("int");

                    b.HasKey("imageID");

                    b.HasIndex("itemID");

                    b.ToTable("ItemImages");
                });

            modelBuilder.Entity("SimpleMarketplaceApp.Models.Transaction", b =>
                {
                    b.Property<int>("transactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("buyerId")
                        .HasColumnType("int");

                    b.Property<int>("itemId")
                        .HasColumnType("int");

                    b.Property<int>("sellerId")
                        .HasColumnType("int");

                    b.Property<decimal>("transactionAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("transactionDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("transactionID");

                    b.HasIndex("buyerId");

                    b.HasIndex("itemId");

                    b.HasIndex("sellerId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("SimpleMarketplaceApp.Models.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("userId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SimpleMarketplaceApp.Models.Item", b =>
                {
                    b.HasOne("SimpleMarketplaceApp.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleMarketplaceApp.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimpleMarketplaceApp.Models.ItemImage", b =>
                {
                    b.HasOne("SimpleMarketplaceApp.Models.Item", "Item")
                        .WithMany("ItemImages")
                        .HasForeignKey("itemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("SimpleMarketplaceApp.Models.Transaction", b =>
                {
                    b.HasOne("SimpleMarketplaceApp.Models.User", "Buyer")
                        .WithMany()
                        .HasForeignKey("buyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleMarketplaceApp.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("itemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleMarketplaceApp.Models.User", "Seller")
                        .WithMany()
                        .HasForeignKey("sellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("Item");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("SimpleMarketplaceApp.Models.Item", b =>
                {
                    b.Navigation("ItemImages");
                });
#pragma warning restore 612, 618
        }
    }
}