using System.ComponentModel.DataAnnotations;

namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class FacturaCreateViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EL id del cliente debe ser un numero positivo")]
        public int ClienteId { get; set; }//FK

        public string? ClienteNombre { get; set; } = string.Empty;

        public int ProductoId { get; set; }

        public string? ProductoNombre { get; set; } = string.Empty;

        public int Cantidad {  get; set; } 

        //porcentaje de descuento para la línea que se está agregando
        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        public int DescuentoAplicado { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        //jalo los detalles en forma de lista x si hay mas de un producto
        public List<DetalleFacturaViewModel> Detalles { get; set; } = new List<DetalleFacturaViewModel>();

        //totales calculados desde los detalles
        public decimal Subtotal { get; set; }            

        public decimal TotalImpuestos { get; set; }      
        
        public decimal DescuentoTotal { get; set; }        
        
        public decimal Total { get; set; }                 

    }
}