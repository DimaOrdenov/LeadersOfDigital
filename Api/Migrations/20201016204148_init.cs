using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    FacilityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subcategory = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitute = table.Column<double>(nullable: false),
                    Rating = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.FacilityId);
                });

            migrationBuilder.CreateTable(
                name: "Barriers",
                columns: table => new
                {
                    BarrierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarrierType = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    FacilityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barriers", x => x.BarrierId);
                    table.ForeignKey(
                        name: "FK_Barriers_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Disability",
                columns: table => new
                {
                    DisabilityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisabilityType = table.Column<int>(nullable: false),
                    BarrierId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disability", x => x.DisabilityId);
                    table.ForeignKey(
                        name: "FK_Disability_Barriers_BarrierId",
                        column: x => x.BarrierId,
                        principalTable: "Barriers",
                        principalColumn: "BarrierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Barriers_FacilityId",
                table: "Barriers",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Disability_BarrierId",
                table: "Disability",
                column: "BarrierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disability");

            migrationBuilder.DropTable(
                name: "Barriers");

            migrationBuilder.DropTable(
                name: "Facilities");
        }
    }
}
