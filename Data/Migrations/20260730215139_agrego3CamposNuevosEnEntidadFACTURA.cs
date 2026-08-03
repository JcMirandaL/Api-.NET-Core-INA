using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class agrego3CamposNuevosEnEntidadFACTURA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacturaReferenciaId",
                table: "tbFactura",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoNotaCredito",
                table: "tbFactura",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoDocumento",
                table: "tbFactura",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbFactura_FacturaReferenciaId",
                table: "tbFactura",
                column: "FacturaReferenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbFactura_tbFactura_FacturaReferenciaId",
                table: "tbFactura",
                column: "FacturaReferenciaId",
                principalTable: "tbFactura",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbFactura_tbFactura_FacturaReferenciaId",
                table: "tbFactura");

            migrationBuilder.DropIndex(
                name: "IX_tbFactura_FacturaReferenciaId",
                table: "tbFactura");

            migrationBuilder.DropColumn(
                name: "FacturaReferenciaId",
                table: "tbFactura");

            migrationBuilder.DropColumn(
                name: "MotivoNotaCredito",
                table: "tbFactura");

            migrationBuilder.DropColumn(
                name: "TipoDocumento",
                table: "tbFactura");
        }
    }
}
