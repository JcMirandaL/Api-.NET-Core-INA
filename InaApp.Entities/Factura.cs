

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InaApp.Entities
{
    [Table(name: "tbFactura")]
    public class Factura
    {
        //propiedades = variables o atributos de una clase/objeto
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EL id del cliente debe ser un numero positivo")]
        public int ClienteId { get; set; }//FK

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El descuento debe ser igual o mayor a 0.")]
        public int descuento { get; set; }

        [Required(ErrorMessage = "El estado es un campo obligatorio.")]
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "Campo obligatorio.")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo obligatorio.")]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo obligatorio.")]
        public string UsuarioCreacion { get; set; } = "admin";


        //relacion un cliente muchas facturas
        public Cliente Cliente { get; set; } = null!;

        //relacion 1 : N, 1 Factura Muchos detalles
        public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();

    }
}
