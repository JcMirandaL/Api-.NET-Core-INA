

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class FacturaCreateViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EL id del cliente debe ser un numero positivo")]
        public int ClienteId { get; set; }//FK

        public SelectList? Clientes { get; set; }

        public int ProductoId { get; set; }

        public SelectList? Productos { get; set; }

        public int Cantidad {  get; set; } 

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El descuento debe ser igual o mayor a 0.")]
        public int Descuento { get; set; }

        [Required]

        //jalo los detalles en forma de lista x si hay mas de un producto
        public List<DetalleFacturaViewModel> Detalles { get; set; } = new List<DetalleFacturaViewModel>();

        //
        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

    }
}
