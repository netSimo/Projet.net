using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageProjet2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFacturesTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "factures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Etat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AncienValeur = table.Column<double>(type: "float", nullable: false),
                    NouvelleValeur = table.Column<double>(type: "float", nullable: false),
                    FactureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    prix = table.Column<double>(type: "float", nullable: false),
                    AdherentId = table.Column<int>(type: "int", nullable: true),
                    CompteurId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_factures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_factures_adherents_AdherentId",
                        column: x => x.AdherentId,
                        principalTable: "adherents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_factures_compteurs_CompteurId",
                        column: x => x.CompteurId,
                        principalTable: "compteurs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_factures_AdherentId",
                table: "factures",
                column: "AdherentId");

            migrationBuilder.CreateIndex(
                name: "IX_factures_CompteurId",
                table: "factures",
                column: "CompteurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "factures");
        }
    }
}
