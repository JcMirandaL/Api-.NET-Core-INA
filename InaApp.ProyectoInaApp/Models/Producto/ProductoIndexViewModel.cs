using static InaApp.Common.Enums.Enumeradores;

namespace InaApp.ProyectoInaApp.Models.Producto
{
    public class ProductoIndexViewModel
    {
        public int Id { get; set; }

        public string CategoriaNombre { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        //public string? Descripcion { get; set; } = string.Empty;

        public TipoImpuestoAplicable ImpuestoAplicable { get; set; }

        public decimal PorcentajeImpuesto { get; set; }

        public int DescuentoMaximo { get; set; }
    }
}
