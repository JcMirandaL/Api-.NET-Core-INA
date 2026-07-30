using InaApp.Common.Interfaces;
using InaApp.Data;
using InaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace InaApp.Repository
{
    public class FacturaRepository : IGenericRepository<Factura>
    {

        //variable de tipo ApplicationDbContex para poder acceder a la base de datos, se tipa de tipo clase y el nombre con _ para indicar que es una variable privada
        private readonly ApplicationDbContex _context;

        //constructor
        public FacturaRepository(ApplicationDbContex context)
        {
            //hago la inyesccion de dependencia 
            _context = context;
        }



        //CRUD
        public async Task<Factura?> ObtenerPorIdAsync(int id)
        {
            return await _context.Factura
                .Include(x => x.Cliente)
                .Include(x => x.Detalles)
                .ThenInclude(x => x.Producto)//el then include es para traer lkos datos de producto desde detalles
                .AsNoTracking()
                .Where(x => x.Id == id && x.Estado)
                .SingleOrDefaultAsync();
        }


        public async Task<List<Factura>> ObtenerTodosAsync()
        {
            return await _context.Factura
                .Include(x => x.Cliente)
                .Include(x => x.Detalles)
                .ThenInclude(x => x.Producto)//el then include es para traer lkos datos de producto desde detalles
                .AsNoTracking()
                .Where(x => x.Estado)
                .ToListAsync();
        }


        public async Task<Factura> CrearAsync(Factura entity)
        {
            _context.Factura.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public async Task<Factura> ActualizarAsync(Factura entity)
        {
            _context.Factura.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        //metodos no utilizados pero para cumplir con interfaz
        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }



    }
}
