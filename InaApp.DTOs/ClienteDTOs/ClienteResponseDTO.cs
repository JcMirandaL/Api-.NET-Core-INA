using static InaApp.Common.Enums.Enumeradores;



namespace InaApp.DTOs.ClienteDTOs
{
    public class ClienteResponseDTO
    {
        public int Id { get; set; }

        public string Cedula { get; set; } = string.Empty;

        public TipoCedulaEnum TipoCedula { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido1 { get; set; } = string.Empty;

        //el ? es para q permita null
        public string? Apellido2 { get; set; } = string.Empty;

        public string? Correo { get; set; } = string.Empty;

        public string? Telefono { get; set; } = string.Empty;

        public DateOnly FechaNacimiento { get; set; }
    }
}
