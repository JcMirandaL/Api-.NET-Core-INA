using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDescuentoAplicadoToDetalleFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DescuentoAplicado",
                table: "DetalleFactura",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescuentoAplicado",
                table: "DetalleFactura");
        }
    }
}
