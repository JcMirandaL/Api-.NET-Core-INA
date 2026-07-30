using InaApp.DTOs.ClienteDTOs;

namespace InaApp.ProyectoInaApp.Models.Factura
{
    public class BusquedaClienteModalViewModel
    {
        public List<ClienteResponseDTO> Items { get; set; } = new();
        public string? Termino { get; set; } = string.Empty;
    }
}