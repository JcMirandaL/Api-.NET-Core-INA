


namespace InaApp.DTOs.DetalleFacturaDTOs
{
    public class DetalleFacturaResponseDTO
    {

        public int ProductoId { get; set; } //pK compuesta

        public string ProductoNombre { get; set; } = string.Empty;


        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Subtotal { get; set; }
    }
}
