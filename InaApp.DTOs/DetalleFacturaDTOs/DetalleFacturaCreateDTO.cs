using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace InaApp.DTOs.DetalleFacturaDTOs
{
    public class DetalleFacturaCreateDTO
    {

        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El id del producto debe ser un numero positivo")]
        public int ProductoId { get; set; } //pK compuesta

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un numero positivo.")]
        public int Cantidad { get; set; }
    }
}
