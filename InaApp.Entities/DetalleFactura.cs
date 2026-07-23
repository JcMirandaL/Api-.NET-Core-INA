

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InaApp.Entities
{
    public class DetalleFactura
    {
        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El id de la factura debe ser un numero positivo")]
        public int FacturaId { get; set; } //pK compuesta

        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El id del producto debe ser un numero positivo")]
        public int ProductoId { get; set; } //pK compuesta

      
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un numero positivo.")]
        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "El precio es un campo obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }


        //el ! en el null es para indicar que esta propiedad no puede ser null, es decir, que siempre debe tener un valor asignado
        //esto es necesario porque la propiedad Categoria es de tipo referencia y puede ser null, pero al poner el ! le decimos al compilador que no va a ser null
        //relacion de N : 1, muchos detales : 1 factura
        public Factura Factura { get; set; } = null!;
        //relacion de N : 1, muchos detales : 1 producto
        public Producto Producto { get; set; } = null!;
    }
}
