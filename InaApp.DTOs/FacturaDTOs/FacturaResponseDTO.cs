using InaApp.DTOs.DetalleFacturaDTOs;


namespace InaApp.DTOs.FacturaDTOs
{
    public class FacturaResponseDTO
    {
        public int Id { get; set; }
        
        public int ClienteId { get; set; }//FK

        public string ClienteNombre { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        //jalo los detalles en forma de lista x si hay mas de un producto
        public List<DetalleFacturaResponseDTO> Detalles { get; set; } = new List<DetalleFacturaResponseDTO>();

        public decimal Subtotal { get; set; }

        public int descuento { get; set; }

        //totales calculados desde las líneas de detalle
        public decimal TotalImpuestos { get; set; }  

        public decimal DescuentoTotal { get; set; }  

        public decimal Total { get; set; }

        public bool Estado { get; set; }

    }
}
