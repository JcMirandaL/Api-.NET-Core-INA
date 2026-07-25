namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class DetalleFacturaViewModel
    {
        public int ProductoId { get; set; }

        public string ProductoNombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }
    }
}
 