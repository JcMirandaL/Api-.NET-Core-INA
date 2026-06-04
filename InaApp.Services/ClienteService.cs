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
        private readonly IGenericRepository<Cliente> _clienteRepository;


        public ClienteService(IGenericRepository<Cliente> clienteRepository)
        {
           _clienteRepository = clienteRepository;
        }



        public Task<Cliente> ActualizarAsync(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> CrearAsync(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cliente>> ObtenerTodosAsync()
        {
            _clienteRepository.ObtenerTodosAsync();
            return null;
        }
    }
}
