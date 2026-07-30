namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class FacturaDetailsViewModel
    {
        public int Id { get; set; }

        public string ClienteNombre { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public decimal Subtotal { get; set; }

        public int Descuento { get; set; }

        //totales calculados
        public decimal TotalImpuestos { get; set; }    

        public decimal DescuentoTotal { get; set; }    

        public decimal Total { get; set; }

        public bool Estado { get; set; }

        public List<DetalleFacturaViewModel> Detalles { get; set; } = new List<DetalleFacturaViewModel>();

    }
}
 