


using InaApp.DTOs.DetalleFacturaDTOs;
using System.ComponentModel.DataAnnotations;
using static InaApp.Common.Enums.Enumeradores;

namespace InaApp.DTOs.FacturaDTOs
{
    public class FacturaCreateDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EL id del cliente debe ser un numero positivo")]
        public int ClienteId { get; set; }//FK
         
        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public TipoDocumentoEnum TipoDocumento { get; set; } = TipoDocumentoEnum.FacturaElectronica;
        
        public int? FacturaReferenciaId { get; set; }
        
        [MaxLength(500)]
        public string? MotivoNotaCredito { get; set; }

        //jalo los detalles en forma de lista x si hay mas de un producto
        public List<DetalleFacturaCreateDTO> Detalles { get; set; } = new List<DetalleFacturaCreateDTO>();
    }
}
