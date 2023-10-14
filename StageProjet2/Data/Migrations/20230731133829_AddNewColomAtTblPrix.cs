using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageProjet2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColomAtTblPrix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "etat",
                table: "prixs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PrixView",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prixtranche1 = table.Column<double>(type: "float", nullable: false),
                    Prixtranche2 = table.Column<double>(type: "float", nullable: false),
                    Prixtranche3 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrixView", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrixView");

            migrationBuilder.DropColumn(
                name: "etat",
                table: "prixs");
        }
    }
}
