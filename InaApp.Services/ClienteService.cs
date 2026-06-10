using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Services
{
    public class ClienteService : IGenericService<Cliente>
    { 
        //el service de producto no uso el tipado de IGenericRepository xq el repo de producto tiene metodos propios,
        //pero el repo de cliente no tiene metodos propios entonces si puedo usar el tipado de IGenericRepository
        //para poder usar los metodos genericos del repo
        private readonly IGenericRepository<Cliente> _clienteRepository;


        public ClienteService(IGenericRepository<Cliente> clienteRepository)
        {
           _clienteRepository = clienteRepository;
        }





        public async Task<Cliente> ObtenerPorIdAsync(int id)
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

            return clienteExistente; 
        }

           
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var listaClientes = await _clienteRepository.ObtenerTodosAsync();

            if (listaClientes == null || listaClientes.Count == 0)
            {
                throw new NotFoundDbException("No se encontraron clientes activos en la base de datos.");
            }

            return listaClientes; 
            
        }


        public async Task<Cliente> CrearAsync(Cliente entity)
        {

            var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(entity.Id);
            if (clienteExistente != null)
            {
                throw new EntityExistDbException($"El cliente con Id '{entity.Id}' ya existe en la base de datos.");
            }

            // Enum.IsDefined(entity.TipoCedula) devuelve true si el valor existe en el enum, false si no
            //lo niego para decir que si el valor no existe en el enum, entonces lanzo la excepcion
            if (!Enum.IsDefined(entity.TipoCedula))
            {
                throw new InvalidEnumException($"El valor '{entity.TipoCedula}' no es un valor válido para el campo TipoCedula del cliente.");
            }


            return await _clienteRepository.CrearAsync(entity);

        }


        public async Task<Cliente> ActualizarAsync(Cliente entity)
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

            // Enum.IsDefined(entity.TipoCedula) devuelve true si el valor existe en el enum, false si no
            //lo niego para decir que si el valor no existe en el enum, entonces lanzo la excepcion
            if (!Enum.IsDefined(entity.TipoCedula))
            {
                throw new InvalidEnumException($"El valor '{entity.TipoCedula}' no es un valor válido para el campo TipoCedula del cliente.");
            }

            return await _clienteRepository.ActualizarAsync(entity);
        }


        public async Task<bool> EliminarAsync(int id)
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

            return await _clienteRepository.EliminarAsync(id); 
        }


    }
}
