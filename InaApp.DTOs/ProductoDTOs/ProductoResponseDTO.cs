using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.DTOs.Producto
{
    //solo los datos que quiero devolver en la respuesta x ejemplo consultar x id, etc.
    public class ProductoResponseDTO
    {
        public int Id { get; set; }
        
        public string Nombre { get; set; } = string.Empty;
        
        public decimal Precio { get; set; }
        
        public int Stock { get; set; }
       
        public string? Descripcion { get; set; } = string.Empty;
       
    }
}
