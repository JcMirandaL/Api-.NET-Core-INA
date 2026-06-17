using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Entities
{
    [Table(name:"tbCategorias")]
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //get set es una propiedad de acceso que permite obtener o establecer el valor de una propiedad
        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es un campo obligatorio.")]
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "Campo obligatorio.")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo obligatorio.")]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo obligatorio.")]
        public string UsuarioCreacion { get; set; } = "admin";
        
        
        //relacion de 1 : N, 1 categoria : muchos productos
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
