using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    FacilityId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                        .Annotation("Sqlite:Autoincrement", true),
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

            migrationBuilder.CreateIndex(
                name: "IX_Barriers_FacilityId",
                table: "Barriers",
                column: "FacilityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barriers");

            migrationBuilder.DropTable(
                name: "Facilities");
        }
    }
}
