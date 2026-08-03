
using static InaApp.Common.Enums.Enumeradores;

namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class FacturaIndexViewModel
    {
        public int Id { get; set; }

        public string ClienteNombre { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        public int Cantidad { get; set; }


        public decimal Subtotal { get; set; }

        public decimal TotalImpuestos { get; set; }

        public decimal DescuentoTotal { get; set; }

        public decimal Total { get; set; }

        public bool Estado { get; set; }

        public TipoDocumentoEnum TipoDocumento { get; set; }
        
        public int? FacturaReferenciaId { get; set; }
    }
}
