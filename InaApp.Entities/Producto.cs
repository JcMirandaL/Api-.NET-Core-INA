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

        //get set es una propiedad de acceso que permite obtener o establecer el valor de una propiedad
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        [Range(0.01, 9999.99)]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, 999)]
        public int Stock { get; set; }

        //string empy lo inicia en cero y evita el warning de propiedad null
        [StringLength(500)]
        public string? Descripcion { get; set; } = string.Empty;

        [Required]
        public bool Estado { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        [Required]
        public string UsuarioCreacion { get; set; } = "admin";



    }
}
