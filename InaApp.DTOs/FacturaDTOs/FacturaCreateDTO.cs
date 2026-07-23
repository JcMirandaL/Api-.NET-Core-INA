


using InaApp.DTOs.DetalleFacturaDTOs;
using System.ComponentModel.DataAnnotations;

namespace InaApp.DTOs.FacturaDTOs
{
    public class FacturaCreateDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EL id del cliente debe ser un numero positivo")]
        public int ClienteId { get; set; }//FK

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El descuento debe ser igual o mayor a 0.")]
        public int descuento { get; set; }

        [Required]

        //jalo los detalles en forma de lista x si hay mas de un producto
        public List<DetalleFacturaCreateDTO> Detalles { get; set; } = new List<DetalleFacturaCreateDTO>();
    }
}
