﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nursery.Data;

namespace Nursery.Migrations
{
    [DbContext(typeof(NurseryContext))]
    [Migration("20220921153406_updateEntities")]
    partial class updateEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Nursery.Models.Adz", b =>
                {
                    b.Property<int>("AdzId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("AdzIsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("AdzOrderIndex")
                        .HasColumnType("int");

                    b.Property<string>("AdzPic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("EntityId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EntityTypeId")
                        .HasColumnType("int");

                    b.HasKey("AdzId");

                    b.HasIndex("CountryId");

                    b.HasIndex("EntityTypeId");

                    b.ToTable("Adz");
                });

            modelBuilder.Entity("Nursery.Models.AgeCategory", b =>
                {
                    b.Property<int>("AgeCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AgeCategoryTlAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AgeCategoryTlEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("from")
                        .HasColumnType("int");

                    b.Property<int>("to")
                        .HasColumnType("int");

                    b.HasKey("AgeCategoryId");

                    b.ToTable("AgeCategory");
                });

            modelBuilder.Entity("Nursery.Models.Area", b =>
                {
                    b.Property<int>("AreaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("AreaIsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("AreaOrderIndex")
                        .HasColumnType("int");

                    b.Property<string>("AreaTlAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AreaTlEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.HasKey("AreaId");

                    b.HasIndex("CityId");

                    b.ToTable("Area");
                });

            modelBuilder.Entity("Nursery.Models.Banner", b =>
                {
                    b.Property<int>("BannerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("BannerIsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("BannerOrderIndex")
                        .HasColumnType("int");

                    b.Property<string>("BannerPic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EntityTypeId")
                        .HasColumnType("int");

                    b.HasKey("BannerId");

                    b.HasIndex("EntityTypeId");

                    b.ToTable("Banner");
                });

            modelBuilder.Entity("Nursery.Models.Child", b =>
                {
                    b.Property<int>("ChildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AgeCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ChildEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChildImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChildName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChildPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NurseryMemberId")
                        .HasColumnType("int");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("ChildId");

                    b.HasIndex("AgeCategoryId");

                    b.HasIndex("NurseryMemberId");

                    b.HasIndex("ParentId");

                    b.ToTable("Child");
                });

            modelBuilder.Entity("Nursery.Models.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("CityIsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("CityOrderIndex")
                        .HasColumnType("int");

                    b.Property<string>("CityTlAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CityTlEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.HasKey("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("Nursery.Models.Configuration", b =>
                {
                    b.Property<int>("ConfigurationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Facebook")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instgram")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkedIn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twitter")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WhatsApp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConfigurationId");

                    b.ToTable("Configuration");
                });

            modelBuilder.Entity("Nursery.Models.ContactUs", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Msg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TransDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ContactId");

                    b.ToTable("ContactUs");
                });

            modelBuilder.Entity("Nursery.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("CountryIsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("CountryOrderIndex")
                        .HasColumnType("int");

                    b.Property<string>("CountryPic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryTlAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryTlEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("Nursery.Models.EntityType", b =>
                {
                    b.Property<int>("EntityTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EntityTypeTlar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityTypeTlen")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntityTypeId");

                    b.ToTable("EntityType");
                });

            modelBuilder.Entity("Nursery.Models.FAQ", b =>
                {
                    b.Property<int>("FAQId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AnswerAr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AnswerEn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionAr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionEn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FAQId");

                    b.ToTable("FAQ");
                });

            modelBuilder.Entity("Nursery.Models.Language", b =>
                {
                    b.Property<int>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LanguageId");

                    b.ToTable("Languages");

                    b.HasData(
                        new
                        {
                            LanguageId = 1,
                            Title = "English"
                        },
                        new
                        {
                            LanguageId = 2,
                            Title = "Arabic"
                        },
                        new
                        {
                            LanguageId = 3,
                            Title = "Both"
                        });
                });

            modelBuilder.Entity("Nursery.Models.Newsletter", b =>
                {
                    b.Property<int>("NewsletterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NewsletterId");

                    b.ToTable("Newsletter");
                });

            modelBuilder.Entity("Nursery.Models.NurseryImage", b =>
                {
                    b.Property<int>("NurseryImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NurseryId")
                        .HasColumnType("int");

                    b.Property<string>("Pic")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NurseryImageId");

                    b.HasIndex("NurseryId");

                    b.ToTable("NurseryImage");
                });

            modelBuilder.Entity("Nursery.Models.NurseryMember", b =>
                {
                    b.Property<int>("NurseryMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AgeCategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("AreaId")
                        .HasColumnType("int");

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("int");

                    b.Property<string>("Banner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CityId")
                        .HasColumnType("int");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Facebook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fax")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instagram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsSlider")
                        .HasColumnType("bit");

                    b.Property<int?>("Language")
                        .HasColumnType("int");

                    b.Property<string>("Lat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lng")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NurseryDescAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NurseryDescEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NurseryTlAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NurseryTlEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<string>("Phone1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("SpecialNeeds")
                        .HasColumnType("bit");

                    b.Property<bool?>("TransportationService")
                        .HasColumnType("bit");

                    b.Property<string>("Twitter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkTimeFrom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkTimeTo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NurseryMemberId");

                    b.HasIndex("AgeCategoryId");

                    b.HasIndex("AreaId");

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("NurseryMember");
                });

            modelBuilder.Entity("Nursery.Models.NurserySubscription", b =>
                {
                    b.Property<int>("NurserySubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Auth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("NurseryId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int>("PlanId")
                        .HasColumnType("int");

                    b.Property<int>("PlanTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PostDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Ref")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Result")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TrackID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TranID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("payment_type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NurserySubscriptionId");

                    b.HasIndex("NurseryId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("PlanId");

                    b.ToTable("NurserySubscription");
                });

            modelBuilder.Entity("Nursery.Models.PageContent", b =>
                {
                    b.Property<int>("PageContentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentAr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentEn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageTitleAr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageTitleEn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PageContentId");

                    b.ToTable("PageContent");
                });

            modelBuilder.Entity("Nursery.Models.Parent", b =>
                {
                    b.Property<int>("ParentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ParentAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentPhone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParentId");

                    b.ToTable("Parent");
                });

            modelBuilder.Entity("Nursery.Models.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PaymentMethodTlAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMethodTlEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethod");
                });

            modelBuilder.Entity("Nursery.Models.Plan", b =>
                {
                    b.Property<int>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<int?>("DurationInMonth")
                        .HasColumnType("int");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PlanTlAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlanTlEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.HasKey("PlanId");

                    b.HasIndex("CountryId");

                    b.ToTable("Plan");
                });

            modelBuilder.Entity("Nursery.Models.PlanType", b =>
                {
                    b.Property<int>("PlanTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlanTypeId");

                    b.ToTable("PlanTypes");

                    b.HasData(
                        new
                        {
                            PlanTypeId = 1,
                            Cost = 0.0,
                            Title = "Silver"
                        },
                        new
                        {
                            PlanTypeId = 2,
                            Cost = 5.0,
                            Title = "Gold"
                        });
                });

            modelBuilder.Entity("Nursery.Models.PublicDevice", b =>
                {
                    b.Property<int>("PublicDeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAndroiodDevice")
                        .HasColumnType("bit");

                    b.HasKey("PublicDeviceId");

                    b.HasIndex("CountryId");

                    b.ToTable("PublicDevice");
                });

            modelBuilder.Entity("Nursery.Models.PublicNotification", b =>
                {
                    b.Property<int>("PublicNotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EntityId")
                        .HasColumnType("int");

                    b.Property<int?>("EntityTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PublicNotificationId");

                    b.HasIndex("CountryId");

                    b.HasIndex("EntityTypeId");

                    b.ToTable("PublicNotification");
                });

            modelBuilder.Entity("Nursery.Models.PublicNotificationDevice", b =>
                {
                    b.Property<int>("PublicNotificationDeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("IsRead")
                        .HasColumnType("bit");

                    b.Property<int?>("PublicDeviceId")
                        .HasColumnType("int");

                    b.Property<int>("PublicNotificationId")
                        .HasColumnType("int");

                    b.HasKey("PublicNotificationDeviceId");

                    b.HasIndex("PublicDeviceId");

                    b.HasIndex("PublicNotificationId");

                    b.ToTable("PublicNotificationDevice");
                });

            modelBuilder.Entity("Nursery.Models.SystemConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("About")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Facebook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instagram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lng")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Policy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Terms")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twitter")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SystemConfiguration");
                });

            modelBuilder.Entity("Nursery.Models.Adz", b =>
                {
                    b.HasOne("Nursery.Models.Country", "Country")
                        .WithMany("Adz")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nursery.Models.EntityType", "EntityType")
                        .WithMany("Adz")
                        .HasForeignKey("EntityTypeId");

                    b.Navigation("Country");

                    b.Navigation("EntityType");
                });

            modelBuilder.Entity("Nursery.Models.Area", b =>
                {
                    b.HasOne("Nursery.Models.City", "City")
                        .WithMany("Area")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("Nursery.Models.Banner", b =>
                {
                    b.HasOne("Nursery.Models.EntityType", "EntityType")
                        .WithMany("Banner")
                        .HasForeignKey("EntityTypeId");

                    b.Navigation("EntityType");
                });

            modelBuilder.Entity("Nursery.Models.Child", b =>
                {
                    b.HasOne("Nursery.Models.AgeCategory", "AgeCategory")
                        .WithMany()
                        .HasForeignKey("AgeCategoryId");

                    b.HasOne("Nursery.Models.NurseryMember", "NurseryMember")
                        .WithMany("Child")
                        .HasForeignKey("NurseryMemberId");

                    b.HasOne("Nursery.Models.Parent", "Parent")
                        .WithMany("Child")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AgeCategory");

                    b.Navigation("NurseryMember");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Nursery.Models.City", b =>
                {
                    b.HasOne("Nursery.Models.Country", "Country")
                        .WithMany("City")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Nursery.Models.NurseryImage", b =>
                {
                    b.HasOne("Nursery.Models.NurseryMember", "Nursery")
                        .WithMany("NurseryImage")
                        .HasForeignKey("NurseryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nursery");
                });

            modelBuilder.Entity("Nursery.Models.NurseryMember", b =>
                {
                    b.HasOne("Nursery.Models.AgeCategory", "AgeCategory")
                        .WithMany("NurseryMember")
                        .HasForeignKey("AgeCategoryId");

                    b.HasOne("Nursery.Models.Area", "Area")
                        .WithMany("NurseryMember")
                        .HasForeignKey("AreaId");

                    b.HasOne("Nursery.Models.PaymentMethod", null)
                        .WithMany("NurseryMember")
                        .HasForeignKey("PaymentMethodId");

                    b.Navigation("AgeCategory");

                    b.Navigation("Area");
                });

            modelBuilder.Entity("Nursery.Models.NurserySubscription", b =>
                {
                    b.HasOne("Nursery.Models.NurseryMember", "Nursery")
                        .WithMany("NurserySubscription")
                        .HasForeignKey("NurseryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nursery.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("Nursery.Models.Plan", "Plan")
                        .WithMany("NurserySubscription")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nursery");

                    b.Navigation("PaymentMethod");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("Nursery.Models.Plan", b =>
                {
                    b.HasOne("Nursery.Models.Country", "Country")
                        .WithMany("Plan")
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Nursery.Models.PublicDevice", b =>
                {
                    b.HasOne("Nursery.Models.Country", "Country")
                        .WithMany("PublicDevice")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Nursery.Models.PublicNotification", b =>
                {
                    b.HasOne("Nursery.Models.Country", "Country")
                        .WithMany("PublicNotification")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nursery.Models.EntityType", "EntityType")
                        .WithMany()
                        .HasForeignKey("EntityTypeId");

                    b.Navigation("Country");

                    b.Navigation("EntityType");
                });

            modelBuilder.Entity("Nursery.Models.PublicNotificationDevice", b =>
                {
                    b.HasOne("Nursery.Models.PublicDevice", "PublicDevice")
                        .WithMany("PublicNotificationDevice")
                        .HasForeignKey("PublicDeviceId");

                    b.HasOne("Nursery.Models.PublicNotification", "PublicNotification")
                        .WithMany("PublicNotificationDevice")
                        .HasForeignKey("PublicNotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PublicDevice");

                    b.Navigation("PublicNotification");
                });

            modelBuilder.Entity("Nursery.Models.AgeCategory", b =>
                {
                    b.Navigation("NurseryMember");
                });

            modelBuilder.Entity("Nursery.Models.Area", b =>
                {
                    b.Navigation("NurseryMember");
                });

            modelBuilder.Entity("Nursery.Models.City", b =>
                {
                    b.Navigation("Area");
                });

            modelBuilder.Entity("Nursery.Models.Country", b =>
                {
                    b.Navigation("Adz");

                    b.Navigation("City");

                    b.Navigation("Plan");

                    b.Navigation("PublicDevice");

                    b.Navigation("PublicNotification");
                });

            modelBuilder.Entity("Nursery.Models.EntityType", b =>
                {
                    b.Navigation("Adz");

                    b.Navigation("Banner");
                });

            modelBuilder.Entity("Nursery.Models.NurseryMember", b =>
                {
                    b.Navigation("Child");

                    b.Navigation("NurseryImage");

                    b.Navigation("NurserySubscription");
                });

            modelBuilder.Entity("Nursery.Models.Parent", b =>
                {
                    b.Navigation("Child");
                });

            modelBuilder.Entity("Nursery.Models.PaymentMethod", b =>
                {
                    b.Navigation("NurseryMember");
                });

            modelBuilder.Entity("Nursery.Models.Plan", b =>
                {
                    b.Navigation("NurserySubscription");
                });

            modelBuilder.Entity("Nursery.Models.PublicDevice", b =>
                {
                    b.Navigation("PublicNotificationDevice");
                });

            modelBuilder.Entity("Nursery.Models.PublicNotification", b =>
                {
                    b.Navigation("PublicNotificationDevice");
                });
#pragma warning restore 612, 618
        }
    }
}
