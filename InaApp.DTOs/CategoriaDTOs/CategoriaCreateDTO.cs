using System.ComponentModel.DataAnnotations;


namespace InaApp.DTOs.CategoriaDTOs
{
    public class CategoriaCreateDTO
    {
        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
