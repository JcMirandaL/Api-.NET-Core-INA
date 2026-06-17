using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Relaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "tbProductos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbCategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbCategorias", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbProductos_CategoriaId",
                table: "tbProductos",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbProductos_tbCategorias_CategoriaId",
                table: "tbProductos",
                column: "CategoriaId",
                principalTable: "tbCategorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbProductos_tbCategorias_CategoriaId",
                table: "tbProductos");

            migrationBuilder.DropTable(
                name: "tbCategorias");

            migrationBuilder.DropIndex(
                name: "IX_tbProductos_CategoriaId",
                table: "tbProductos");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "tbProductos");
        }
    }
}
