using AutoMapper;
using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.Common.Response;
using InaApp.DTOs.ClienteDTOs;
using InaApp.Entities;
using InaApp.Repository;


namespace InaApp.Services
{
    public class ClienteService : IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>
    { 
        //el service de producto no uso el tipado de IGenericRepository xq el repo de producto tiene metodos propios,
        //pero el repo de cliente no tiene metodos propios entonces si puedo usar el tipado de IGenericRepository
        //para poder usar los metodos genericos del repo
        private readonly ClienteRepository _clienteRepository;

        private readonly IMapper _mapper;

        public ClienteService(ClienteRepository clienteRepository, IMapper mapper)
        {
           _clienteRepository = clienteRepository;
            _mapper = mapper;
        }





        public async Task<Response<ClienteResponseDTO>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotNumberPositiveException($"El Id '{id}' del cliente debe ser un número positivo.");
            }

            var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(id);
            if (clienteExistente == null)
            {
                throw new NotFoundDbException($"El cliente con Id '{id}' no existe o esta inactivo en la base de datos.");
            }

            return new Response<ClienteResponseDTO>
            {
                Message = ("Cliente encontrado exitosamente:"),
                Data = _mapper.Map<ClienteResponseDTO>(clienteExistente),
                Success = true,
            };
        }

           
        public async Task<Response<List<ClienteResponseDTO>>> ObtenerTodosAsync()
        {
            var listaClientes = await _clienteRepository.ObtenerTodosAsync();

            if (listaClientes == null || listaClientes.Count == 0)
            {
                throw new NotFoundDbException("No se encontraron clientes activos en la base de datos.");
            }
            
            return new Response<List<ClienteResponseDTO>>
            {
                Message = "Clientes obtenidos exitosamente.",
                Data = _mapper.Map<List<ClienteResponseDTO>>(listaClientes),
                Success = true
            };

        }


        public async Task<Response<ClienteResponseDTO>> CrearAsync(ClienteCreateDTO entity)
        {

            var clienteExistente = await _clienteRepository.ObtenerPorCedulaAsync(entity.Cedula);
            if (clienteExistente != null)
            {
                throw new EntityExistDbException($"El cliente con cedula '{entity.Cedula}' ya existe en la base de datos.");
            }

           

            //PASO DE DTO A ENTIDAD
            //LO QUE ESTA ENTRE <> ES EL TIPO DESTINO
            Cliente clienteNuevo = _mapper.Map<Cliente>(entity);

            //GUARDO ENTIDAD MAPEADA
            clienteNuevo = await _clienteRepository.CrearAsync(clienteNuevo);

            return new Response<ClienteResponseDTO>
            {
                Message = "Cliente creado exitosamente.",
                Data = _mapper.Map<ClienteResponseDTO>(clienteNuevo),
                Success = true
            };

        }


        public async Task<Response<ClienteResponseDTO>> ActualizarAsync(ClienteUpdateDTO entity)
        {
            
            if (entity.Id <= 0)
            {
                throw new NotNumberPositiveException($"El Id '{entity.Id}' del cliente debe ser un número positivo.");
            }

            var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(entity.Id);
            if (clienteExistente == null)
            {
                throw new NotFoundDbException($"El cliente con Id '{entity.Id}' no existe o esta inactivo.");
            }

            if (clienteExistente != null && clienteExistente.Id != entity.Id)
            {
                throw new EntityExistDbException($"El cliente con Id '{entity.Id}' ya existe en la base de datos.");
            }

            
            //LO QUE ESTA ENTRE <> ES EL TIPO DESTINO
            Cliente clienteActualizar = _mapper.Map<Cliente>(entity);

            clienteActualizar = await _clienteRepository.ActualizarAsync(clienteActualizar);

            return new Response<ClienteResponseDTO>
            {
                Message = "Cliente actualizado exitosamente.",
                Data = _mapper.Map<ClienteResponseDTO>(clienteActualizar),
                Success = true
            };
        }


        public async Task<Response<bool>> EliminarAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotNumberPositiveException($"El Id '{id}' del cliente debe ser un número positivo.");
            }

            var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(id);
            if (clienteExistente == null)
            {
                throw new NotFoundDbException($"El cliente con Id '{id}' no existe o esta inactivo en la base de datos.");
            }

            return new Response<bool>
            {
                Message = "Cliente eliminado exitosamente.",
                Data = await _clienteRepository.EliminarAsync(id),
                Success = true
            };
        }


    }
}
