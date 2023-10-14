using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageProjet2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrixTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "prixs",
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
                    table.PrimaryKey("PK_prixs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prixs");
        }
    }
}
