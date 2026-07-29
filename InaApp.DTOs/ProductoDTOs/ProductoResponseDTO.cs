using static InaApp.Common.Enums.Enumeradores;

namespace InaApp.DTOs.Producto
{
    //solo los datos que quiero devolver en la respuesta x ejemplo consultar x id, etc.
    public class ProductoResponseDTO
    {
        public int Id { get; set; }
        
        public int CategoriaId { get; set; }

        public string CategoriaNombre { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;
        
        public decimal Precio { get; set; }
        
        public int Stock { get; set; }
       
        public string? Descripcion { get; set; } = string.Empty;

        public TipoImpuestoAplicable ImpuestoAplicable { get; set; }

        public decimal PorcentajeImpuesto { get; set; } = 0;

        public int DescuentoMaximo { get; set; } = 0;

    }
}
