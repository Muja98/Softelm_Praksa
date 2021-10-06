﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Contexts;

namespace api.Migrations
{
    [DbContext(typeof(FarmieContext))]
    [Migration("20200623114741_AddedAtributesForTransactionAndJobApplication")]
    partial class AddedAtributesForTransactionAndJobApplication
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("api.Entities.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BirthType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<int>("DaysInFirstMating")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExclusionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExclusionReason")
                        .HasColumnType("nvarchar(1600)")
                        .HasMaxLength(1600);

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("Hb")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdentificationNumber")
                        .HasColumnType("int");

                    b.Property<int>("LeftTits")
                        .HasColumnType("int");

                    b.Property<string>("Rb")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.Property<int>("RightTits")
                        .HasColumnType("int");

                    b.Property<string>("SelectionMark")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TatooNumber")
                        .HasColumnType("int");

                    b.Property<int?>("TypeOfAnimalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeOfAnimalId");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("api.Entities.AnimalEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AnimalId")
                        .HasColumnType("int");

                    b.Property<double>("Contribution")
                        .HasColumnType("float");

                    b.Property<string>("ContributionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfEvent")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<float>("Stake")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.ToTable("AnimalEvents");
                });

            modelBuilder.Entity("api.Entities.Breed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Breeds");
                });

            modelBuilder.Entity("api.Entities.Farm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FarmerId")
                        .HasColumnType("int");

                    b.Property<double>("Lat")
                        .HasColumnType("float");

                    b.Property<double>("Lng")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("FarmerId");

                    b.ToTable("Farms");
                });

            modelBuilder.Entity("api.Entities.JobAdvertisement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveFlag")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DateOfPuplication")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FarmId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.ToTable("JobAdvertisements");
                });

            modelBuilder.Entity("api.Entities.JobApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AnnouncementId")
                        .HasColumnType("int");

                    b.Property<string>("AnnouncementTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FarmName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MotivationLetter")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnnouncementId");

                    b.HasIndex("UserId");

                    b.ToTable("JobApplications");
                });

            modelBuilder.Entity("api.Entities.Judgement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Evaluation")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Judgements");
                });

            modelBuilder.Entity("api.Entities.Possession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FarmId")
                        .HasColumnType("int");

                    b.Property<double>("Lat")
                        .HasColumnType("float");

                    b.Property<double>("Lng")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.ToTable("Possessions");
                });

            modelBuilder.Entity("api.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FarmId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("InStock")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int?>("TypeId")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.HasIndex("TypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("api.Entities.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("api.Entities.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Agriculture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("PossessionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SeasonStarted")
                        .HasColumnType("datetime2");

                    b.Property<bool>("State")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("PossessionId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("api.Entities.SeasonEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Contribution")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ForProductId")
                        .HasColumnType("int");

                    b.Property<int?>("SeasonId")
                        .HasColumnType("int");

                    b.Property<float>("Stake")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ForProductId");

                    b.HasIndex("SeasonId");

                    b.ToTable("SeasonEvents");
                });

            modelBuilder.Entity("api.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FarmId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeAndDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("UnitPrice")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.HasIndex("ProductId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("api.Entities.TypeOfAnimal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BreedId")
                        .HasColumnType("int");

                    b.Property<int?>("FarmId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfAnimals")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BreedId");

                    b.HasIndex("FarmId");

                    b.ToTable("TypesOfAnimal");
                });

            modelBuilder.Entity("api.Entities.TypeOfAnimalEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Contribution")
                        .HasColumnType("float");

                    b.Property<string>("ContributionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfEvent")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<float>("Stake")
                        .HasColumnType("real");

                    b.Property<int?>("TypeOfAnimalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("TypeOfAnimalId");

                    b.ToTable("TypesOfAnimalEvents");
                });

            modelBuilder.Entity("api.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AdministratorFlag")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FarmerFlag")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WorkerFlag")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("api.Entities.WorkingTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CompletionDeadline")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FarmId")
                        .HasColumnType("int");

                    b.Property<int?>("WorkerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.HasIndex("WorkerId");

                    b.ToTable("WorkingTasks");
                });

            modelBuilder.Entity("api.Relations.ParentsRelations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ChildId")
                        .HasColumnType("int");

                    b.Property<int?>("FatherId")
                        .HasColumnType("int");

                    b.Property<int?>("MatherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChildId");

                    b.HasIndex("FatherId");

                    b.HasIndex("MatherId");

                    b.ToTable("ParentsRelations");
                });

            modelBuilder.Entity("api.Relations.WorksOn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FarmId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("Grade")
                        .HasColumnType("real");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WorkerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.HasIndex("WorkerId");

                    b.ToTable("WorksOn");
                });

            modelBuilder.Entity("api.Entities.Animal", b =>
                {
                    b.HasOne("api.Entities.TypeOfAnimal", "TypeOfAnimal")
                        .WithMany("Animals")
                        .HasForeignKey("TypeOfAnimalId");
                });

            modelBuilder.Entity("api.Entities.AnimalEvent", b =>
                {
                    b.HasOne("api.Entities.Animal", "Animal")
                        .WithMany("Events")
                        .HasForeignKey("AnimalId");
                });

            modelBuilder.Entity("api.Entities.Farm", b =>
                {
                    b.HasOne("api.Entities.User", "Farmer")
                        .WithMany("Farms")
                        .HasForeignKey("FarmerId");
                });

            modelBuilder.Entity("api.Entities.JobAdvertisement", b =>
                {
                    b.HasOne("api.Entities.Farm", "Farm")
                        .WithMany("Announcements")
                        .HasForeignKey("FarmId");
                });

            modelBuilder.Entity("api.Entities.JobApplication", b =>
                {
                    b.HasOne("api.Entities.JobAdvertisement", "Announcement")
                        .WithMany("JobApplications")
                        .HasForeignKey("AnnouncementId");

                    b.HasOne("api.Entities.User", "User")
                        .WithMany("JobApplications")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("api.Entities.Judgement", b =>
                {
                    b.HasOne("api.Entities.Product", "Product")
                        .WithMany("Judgements")
                        .HasForeignKey("ProductId");

                    b.HasOne("api.Entities.User", "User")
                        .WithMany("Judgements")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("api.Entities.Possession", b =>
                {
                    b.HasOne("api.Entities.Farm", "Farm")
                        .WithMany("Possessions")
                        .HasForeignKey("FarmId");
                });

            modelBuilder.Entity("api.Entities.Product", b =>
                {
                    b.HasOne("api.Entities.Farm", "Farm")
                        .WithMany("Products")
                        .HasForeignKey("FarmId");

                    b.HasOne("api.Entities.ProductType", "Type")
                        .WithMany("Products")
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("api.Entities.Season", b =>
                {
                    b.HasOne("api.Entities.Possession", "Possession")
                        .WithMany("Seasons")
                        .HasForeignKey("PossessionId");
                });

            modelBuilder.Entity("api.Entities.SeasonEvent", b =>
                {
                    b.HasOne("api.Entities.Product", "ForProduct")
                        .WithMany()
                        .HasForeignKey("ForProductId");

                    b.HasOne("api.Entities.Season", "Season")
                        .WithMany("SeasonEvents")
                        .HasForeignKey("SeasonId");
                });

            modelBuilder.Entity("api.Entities.Transaction", b =>
                {
                    b.HasOne("api.Entities.Farm", "Farm")
                        .WithMany("Transactions")
                        .HasForeignKey("FarmId");

                    b.HasOne("api.Entities.Product", "Product")
                        .WithMany("Transactions")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("api.Entities.TypeOfAnimal", b =>
                {
                    b.HasOne("api.Entities.Breed", "Breed")
                        .WithMany("TypesOfAnimal")
                        .HasForeignKey("BreedId");

                    b.HasOne("api.Entities.Farm", "Farm")
                        .WithMany("TypeOfAnimals")
                        .HasForeignKey("FarmId");
                });

            modelBuilder.Entity("api.Entities.TypeOfAnimalEvent", b =>
                {
                    b.HasOne("api.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("api.Entities.TypeOfAnimal", "TypeOfAnimal")
                        .WithMany("Events")
                        .HasForeignKey("TypeOfAnimalId");
                });

            modelBuilder.Entity("api.Entities.WorkingTask", b =>
                {
                    b.HasOne("api.Entities.Farm", "Farm")
                        .WithMany("WorkingTasks")
                        .HasForeignKey("FarmId");

                    b.HasOne("api.Entities.User", "Worker")
                        .WithMany("WorkingTasks")
                        .HasForeignKey("WorkerId");
                });

            modelBuilder.Entity("api.Relations.ParentsRelations", b =>
                {
                    b.HasOne("api.Entities.Animal", "Child")
                        .WithMany()
                        .HasForeignKey("ChildId");

                    b.HasOne("api.Entities.Animal", "Father")
                        .WithMany()
                        .HasForeignKey("FatherId");

                    b.HasOne("api.Entities.Animal", "Mather")
                        .WithMany()
                        .HasForeignKey("MatherId");
                });

            modelBuilder.Entity("api.Relations.WorksOn", b =>
                {
                    b.HasOne("api.Entities.Farm", "Farm")
                        .WithMany("WorksOn")
                        .HasForeignKey("FarmId");

                    b.HasOne("api.Entities.User", "Worker")
                        .WithMany("WorksOn")
                        .HasForeignKey("WorkerId");
                });
#pragma warning restore 612, 618
        }
    }
}
