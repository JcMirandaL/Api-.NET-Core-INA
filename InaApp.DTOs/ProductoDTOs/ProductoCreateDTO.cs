using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.DTOs.Producto
{
    public class ProductoCreateDTO
    {
        //get set es una propiedad de acceso que permite obtener o establecer el valor de una propiedad
        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es un campo obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es un campo obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser un numero positivo.")]
        public int Stock { get; set; }

        //string empy lo inicia en cero y evita el warning de propiedad null
        [StringLength(500, ErrorMessage = "La descripcion NO debe superar un maximo de 500 caracterres.")]
        public string? Descripcion { get; set; } = string.Empty;

       
    }
}
