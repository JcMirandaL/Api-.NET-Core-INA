using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class facturaDetalleFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbFactura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    descuento = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbFactura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbFactura_tbClientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "tbClientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleFactura",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleFactura", x => new { x.FacturaId, x.ProductoId });
                    table.ForeignKey(
                        name: "FK_DetalleFactura_tbFactura_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "tbFactura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleFactura_tbProductos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "tbProductos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFactura_ProductoId",
                table: "DetalleFactura",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbFactura_ClienteId",
                table: "tbFactura",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleFactura");

            migrationBuilder.DropTable(
                name: "tbFactura");
        }
    }
}
