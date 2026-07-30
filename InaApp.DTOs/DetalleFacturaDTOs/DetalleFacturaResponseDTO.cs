


namespace InaApp.DTOs.DetalleFacturaDTOs
{
    public class DetalleFacturaResponseDTO
    {

        public int ProductoId { get; set; } //pK compuesta

        public string ProductoNombre { get; set; } = string.Empty;


        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        //campos calculados (no se persisten, se obtienen del producto o se calculan)
        public decimal Subtotal { get; set; } //Cantidad * Precio
        public decimal PorcentajeImpuesto { get; set; }          
        public decimal MontoImpuesto { get; set; }//Subtotal * PorcentajeImpuesto / 100
        public int DescuentoAplicado { get; set; }
        public decimal DescuentoMonto { get; set; }//Subtotal * DescuentoAplicado / 100
        public decimal TotalLinea { get; set; }//Subtotal + MontoImpuesto - DescuentoMonto
    }
}
