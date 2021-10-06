using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class ImageAddedInProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Species = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Surname = table.Column<string>(maxLength: 30, nullable: false),
                    EMail = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    AdministratorFlag = table.Column<bool>(nullable: false),
                    FarmerFlag = table.Column<bool>(nullable: false),
                    WorkerFlag = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Location = table.Column<string>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    Lng = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FarmerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_Users_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobAdvertisements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfPuplication = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: false),
                    ActiveFlag = table.Column<bool>(nullable: false),
                    FarmId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAdvertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobAdvertisements_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Possessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(maxLength: 1000, nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    Lng = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FarmId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Possessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Possessions_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Price = table.Column<float>(nullable: false),
                    Unit = table.Column<string>(nullable: false),
                    InStock = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    FarmId = table.Column<int>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TypesOfAnimal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfAnimals = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    FarmId = table.Column<int>(nullable: true),
                    BreedId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesOfAnimal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypesOfAnimal_Breeds_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypesOfAnimal_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkingTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false),
                    DateOfCreation = table.Column<DateTime>(nullable: false),
                    CompletionDeadline = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<bool>(nullable: false),
                    WorkerId = table.Column<int>(nullable: true),
                    FarmId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingTasks_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingTasks_Users_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorksOn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Grade = table.Column<float>(nullable: false),
                    WorkerId = table.Column<int>(nullable: true),
                    FarmId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorksOn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorksOn_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorksOn_Users_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MotivationLetter = table.Column<string>(nullable: false),
                    AnnouncementId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplications_JobAdvertisements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "JobAdvertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    State = table.Column<bool>(nullable: false),
                    SeasonStarted = table.Column<DateTime>(nullable: false),
                    Agriculture = table.Column<string>(nullable: true),
                    PossessionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Possessions_PossessionId",
                        column: x => x.PossessionId,
                        principalTable: "Possessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Judgements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(nullable: false),
                    Evaluation = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Judgements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Judgements_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Judgements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(nullable: true),
                    TimeAndDate = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<float>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    FarmId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificationNumber = table.Column<int>(nullable: false),
                    Hb = table.Column<string>(nullable: false),
                    Rb = table.Column<string>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    TypeOfAnimalId = table.Column<int>(nullable: true),
                    Gender = table.Column<bool>(nullable: false),
                    ExclusionDate = table.Column<DateTime>(nullable: false),
                    ExclusionReason = table.Column<string>(maxLength: 1600, nullable: true),
                    DaysInFirstMating = table.Column<int>(nullable: false),
                    LeftTits = table.Column<int>(nullable: false),
                    RightTits = table.Column<int>(nullable: false),
                    SelectionMark = table.Column<string>(nullable: false),
                    RegistrationNumber = table.Column<int>(nullable: false),
                    TatooNumber = table.Column<int>(nullable: false),
                    BirthType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_TypesOfAnimal_TypeOfAnimalId",
                        column: x => x.TypeOfAnimalId,
                        principalTable: "TypesOfAnimal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TypesOfAnimalEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Stake = table.Column<float>(nullable: false),
                    DateOfEvent = table.Column<DateTime>(nullable: false),
                    ContributionType = table.Column<string>(nullable: false),
                    Contribution = table.Column<double>(nullable: false),
                    TypeOfAnimalId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesOfAnimalEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypesOfAnimalEvents_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypesOfAnimalEvents_TypesOfAnimal_TypeOfAnimalId",
                        column: x => x.TypeOfAnimalId,
                        principalTable: "TypesOfAnimal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeasonEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Stake = table.Column<float>(nullable: false),
                    Contribution = table.Column<float>(nullable: false),
                    ForProductId = table.Column<int>(nullable: true),
                    SeasonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeasonEvents_Products_ForProductId",
                        column: x => x.ForProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeasonEvents_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimalEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 1000, nullable: false),
                    DateOfEvent = table.Column<DateTime>(nullable: false),
                    Stake = table.Column<float>(nullable: false),
                    ContributionType = table.Column<string>(nullable: false),
                    Contribution = table.Column<double>(nullable: false),
                    AnimalId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalEvents_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParentsRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildId = table.Column<int>(nullable: true),
                    MatherId = table.Column<int>(nullable: true),
                    FatherId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentsRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentsRelations_Animals_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParentsRelations_Animals_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParentsRelations_Animals_MatherId",
                        column: x => x.MatherId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalEvents_AnimalId",
                table: "AnimalEvents",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_TypeOfAnimalId",
                table: "Animals",
                column: "TypeOfAnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_FarmerId",
                table: "Farms",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_FarmId",
                table: "JobAdvertisements",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_AnnouncementId",
                table: "JobApplications",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Judgements_ProductId",
                table: "Judgements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Judgements_UserId",
                table: "Judgements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsRelations_ChildId",
                table: "ParentsRelations",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsRelations_FatherId",
                table: "ParentsRelations",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsRelations_MatherId",
                table: "ParentsRelations",
                column: "MatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Possessions_FarmId",
                table: "Possessions",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmId",
                table: "Products",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeId",
                table: "Products",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonEvents_ForProductId",
                table: "SeasonEvents",
                column: "ForProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonEvents_SeasonId",
                table: "SeasonEvents",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_PossessionId",
                table: "Seasons",
                column: "PossessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FarmId",
                table: "Transactions",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ProductId",
                table: "Transactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TypesOfAnimal_BreedId",
                table: "TypesOfAnimal",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_TypesOfAnimal_FarmId",
                table: "TypesOfAnimal",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_TypesOfAnimalEvents_ProductId",
                table: "TypesOfAnimalEvents",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TypesOfAnimalEvents_TypeOfAnimalId",
                table: "TypesOfAnimalEvents",
                column: "TypeOfAnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTasks_FarmId",
                table: "WorkingTasks",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTasks_WorkerId",
                table: "WorkingTasks",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorksOn_FarmId",
                table: "WorksOn",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_WorksOn_WorkerId",
                table: "WorksOn",
                column: "WorkerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalEvents");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "Judgements");

            migrationBuilder.DropTable(
                name: "ParentsRelations");

            migrationBuilder.DropTable(
                name: "SeasonEvents");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TypesOfAnimalEvents");

            migrationBuilder.DropTable(
                name: "WorkingTasks");

            migrationBuilder.DropTable(
                name: "WorksOn");

            migrationBuilder.DropTable(
                name: "JobAdvertisements");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "TypesOfAnimal");

            migrationBuilder.DropTable(
                name: "Possessions");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "Breeds");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
