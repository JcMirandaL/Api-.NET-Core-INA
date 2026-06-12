using System.ComponentModel.DataAnnotations;
using static InaApp.Common.Enums.Enumeradores;


namespace InaApp.DTOs.ClienteDTOs
{
    public class ClienteCreateDTO
    {
        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(20, ErrorMessage = "La cedula NO debe exeder los 20 caracteres.")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio.")]
        [EnumDataType(typeof(TipoCedulaEnum), ErrorMessage = "Escoja una opcion valida")]
        public TipoCedulaEnum TipoCedula { get; set; }

        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El apellido 1 debe tener entre 3 y 50 caracteres.")]
        public string Apellido1 { get; set; } = string.Empty;

        //el ? es para q permita null
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El apellido 2 debe tener entre 3 y 50 caracteres.")]
        public string? Apellido2 { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo electronico debe tener un formato valido.")]
        [MaxLength(150, ErrorMessage = "El correo electronico debe tener un maximo de 150 caracteres.")]
        public string? Correo { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El telefono debe tener un formato valido.")]
        [MaxLength(20, ErrorMessage = "El telefono debe tener nun maximo de 20 caracteres.")]
        public string? Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "La fecha debe tener un formato valido.")]
        public DateOnly FechaNacimiento { get; set; }
    }
}
