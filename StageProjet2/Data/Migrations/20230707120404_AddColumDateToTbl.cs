using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageProjet2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumDateToTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimePrix",
                table: "prixs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimePrix",
                table: "prixs");
        }
    }
}
