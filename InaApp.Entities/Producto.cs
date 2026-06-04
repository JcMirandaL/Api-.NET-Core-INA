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

    //[Table(name:"tbProductos")]
    public class Producto
    {
        //propiedades = variables o atributos de una clase/objeto
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; }

        public string Nombre { get; set; }
        
        public decimal Precio { get; set; }
        
        public int Stock { get; set; }
        
        public string Descripcion { get; set; }
        
        public bool Estado { get; set; }





    }
}
