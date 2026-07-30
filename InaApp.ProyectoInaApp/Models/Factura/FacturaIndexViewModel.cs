
namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class FacturaIndexViewModel
    {
        public int Id { get; set; }

        public string ClienteNombre { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        public decimal Subtotal { get; set; }

        public decimal TotalImpuestos { get; set; }

        public decimal DescuentoTotal { get; set; }

        public decimal Total { get; set; }

        public bool Estado { get; set; }

    }
}
