using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageProjet2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdherentTblAndCompteurtbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "compteurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marque = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compteurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "adherents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Num = table.Column<int>(type: "int", nullable: false),
                    CompteurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adherents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_adherents_compteurs_CompteurId",
                        column: x => x.CompteurId,
                        principalTable: "compteurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_adherents_CompteurId",
                table: "adherents",
                column: "CompteurId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adherents");

            migrationBuilder.DropTable(
                name: "compteurs");
        }
    }
}
