using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Entities
{
    //NIVELES DE ACCESO:
    //public cualquier clase o metodo puede acceder a esta clase
    //internal solo las clases dentro de este proyecto pueden acceder a esta clase
    //private solo las clases o metodos dentro de esta clase pueden acceder a esta clase
    //protected solo las clases dentro de esta clase y las clases que hereden de esta clase pueden acceder a esta clase

    [Table(name:"tbProductos")]
    public class Producto
    {
        //propiedades = variables o atributos de una clase/objeto
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required (ErrorMessage = "Campo obligatorio.")]
        public int CategoriaId { get; set; } //FK

        //get set es una propiedad de acceso que permite obtener o establecer el valor de una propiedad
        [Required (ErrorMessage = "El nombre es un campo obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Required (ErrorMessage = "El precio es un campo obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        [Required (ErrorMessage = "El stock es un campo obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser un numero positivo.")]
        public int Stock { get; set; }

        //string empy lo inicia en cero y evita el warning de propiedad null
        [StringLength(500, ErrorMessage = "La descripcion NO debe superar un maximo de 500 caracterres.")]
        public string? Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es un campo obligatorio.")]
        public bool Estado { get; set; } = true;

        [Required (ErrorMessage = "Campo obligatorio.")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo obligatorio.")]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo obligatorio.")]
        public string UsuarioCreacion { get; set; } = "admin";


        //relacion de 1 : N, 1 categoria : muchos productos
        //el ! en el null es para indicar que esta propiedad no puede ser null, es decir, que siempre debe tener un valor asignado, esto es necesario porque la propiedad Categoria es de tipo referencia y puede ser null, pero al poner el ! le decimos al compilador que no va a ser null
        //y que siempre va a tener un valor asignado, esto es importante para evitar errores de null reference exception en tiempo de ejecucion
        public Categoria Categoria { get; set; } = null!;



    }
}
