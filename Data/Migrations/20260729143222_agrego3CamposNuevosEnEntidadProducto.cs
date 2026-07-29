using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class agrego3CamposNuevosEnEntidadProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DescuentoMaximo",
                table: "tbProductos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImpuestoAplicable",
                table: "tbProductos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PorcentajeImpuesto",
                table: "tbProductos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescuentoMaximo",
                table: "tbProductos");

            migrationBuilder.DropColumn(
                name: "ImpuestoAplicable",
                table: "tbProductos");

            migrationBuilder.DropColumn(
                name: "PorcentajeImpuesto",
                table: "tbProductos");
        }
    }
}
