using InaApp.DTOs.Producto;

namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class BusquedaProductoModalViewModel
    {
        public List<ProductoResponseDTO> Items { get; set; } = new();
        public string? Termino { get; set; } = string.Empty;
    }
}