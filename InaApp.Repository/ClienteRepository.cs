using InaApp.Common.Interfaces;
using InaApp.Data;
using InaApp.Entities;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes
                .AsNoTracking()
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


        public Task<bool> EliminarAsync(int id)
        {
           throw new NotImplementedException();

        }


        public async Task<Cliente?> ObtenerPorCedulaAsync(string cedula)
        {
            return await _context.Clientes.AsNoTracking()
                .Where(x => x.Cedula == cedula && x.Estado == true)
                .SingleOrDefaultAsync();

        }

        public async Task<List<Cliente>> BuscarAsync(string? termino, int limite)
        {
            return await _context.Clientes
                .AsNoTracking()
                .Where(x => x.Estado == true &&
                //si el termino es nulo o vacio, se devuelve todos los clientes, de lo contrario se filtra por nombre, apellido1, apellido2 o cedula
                    (string.IsNullOrWhiteSpace(termino) ||
                     x.Nombre.Contains(termino) ||
                     x.Apellido1.Contains(termino) ||
                     (x.Apellido2 != null && x.Apellido2.Contains(termino)) ||
                     x.Cedula.Contains(termino)))
                .OrderBy(x => x.Nombre)
                .Take(limite)
                .ToListAsync();
        }
    }
}
