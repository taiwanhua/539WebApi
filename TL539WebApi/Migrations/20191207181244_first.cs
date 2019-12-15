using Microsoft.EntityFrameworkCore.Migrations;

namespace TL539WebApi.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WinNumbers",
                columns: table => new
                {
                    WinNumberID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(nullable: true),
                    DrawOrder1 = table.Column<string>(nullable: true),
                    DrawOrder2 = table.Column<string>(nullable: true),
                    DrawOrder3 = table.Column<string>(nullable: true),
                    DrawOrder4 = table.Column<string>(nullable: true),
                    DrawOrder5 = table.Column<string>(nullable: true),
                    ASC1 = table.Column<string>(nullable: true),
                    ASC2 = table.Column<string>(nullable: true),
                    ASC3 = table.Column<string>(nullable: true),
                    ASC4 = table.Column<string>(nullable: true),
                    ASC5 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WinNumbers", x => x.WinNumberID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WinNumbers");
        }
    }
}
