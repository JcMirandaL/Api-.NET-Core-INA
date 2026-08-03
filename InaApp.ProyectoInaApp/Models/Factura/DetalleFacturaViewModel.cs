namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class DetalleFacturaViewModel
    {
        public int ProductoId { get; set; }

        public string ProductoNombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        //cantidad original de la factura de referencia (para NC), solo display
        public int CantidadOriginal { get; set; }

        //campos calculados
        public decimal Subtotal { get; set; }//Cantidad * Precio

        public decimal PorcentajeImpuesto { get; set; }//del producto
        
        public decimal MontoImpuesto { get; set; }//Subtotal * PorcentajeImpuesto / 100

        public int DescuentoAplicado { get; set; }  
        
        public decimal DescuentoMonto { get; set; } //Subtotal * DescuentoAplicado / 100
        
        public decimal TotalLinea { get; set; }                  // Subtotal + MontoImpuesto - DescuentoMonto
    }
}
 