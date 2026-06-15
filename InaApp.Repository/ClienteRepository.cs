using InaApp.Common.Interfaces;
using InaApp.Data;
using InaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InaApp.Common.Enums.Enumeradores;

namespace InaApp.Repository
{
    public class ClienteRepository : IGenericRepository<Cliente>
    {
        private readonly ApplicationDbContex _context;

        public ClienteRepository(ApplicationDbContex context)
        {
            _context = context;
        }




        //crud
        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes.AsNoTracking()
                .Where(x => x.Id == id && x.Estado == true)
                .SingleOrDefaultAsync();
        }


        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _context.Clientes.AsNoTracking()
                .Where(x => x.Estado == true)
                .ToListAsync();
        }


        public async Task<Cliente> CrearAsync(Cliente entity)
        {
            _context.Clientes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public async Task<Cliente> ActualizarAsync(Cliente entity)
        {
            _context.Clientes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;

        }


        public async Task<bool> EliminarAsync(int id)
        {
            var cliente = await ObtenerPorIdAsync(id);

            cliente.Estado = false;

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return true;

        }


        public async Task<Cliente> ObtenerPorCedulaAsync(string cedula)
        {
            return await _context.Clientes.AsNoTracking()
                .Where(x => x.Cedula == cedula && x.Estado == true)
                .SingleOrDefaultAsync();

        }
    }
}
