using InaApp.Common.Interfaces;
using InaApp.Data;
using InaApp.Entities;
using Microsoft.EntityFrameworkCore;


namespace InaApp.Repository
{
    public class CategoriaRepository : IGenericRepository<Categoria>
    {
        private readonly ApplicationDbContex _context;

        public CategoriaRepository(ApplicationDbContex context)
        {
            _context = context;
        }





        public async Task<Categoria?> ObtenerPorIdAsync(int id)
        {
            return await _context.Categorias
                .AsNoTracking()
                .Where(x => x.Id == id && x.Estado == true)
                .SingleOrDefaultAsync();
        }


        public async Task<List<Categoria>> ObtenerTodosAsync()
        {
            return await _context.Categorias
                .AsNoTracking()
               .Where(x => x.Estado == true)
               .ToListAsync();
        }

        
        public async Task<Categoria> CrearAsync(Categoria entity)
        {
            _context.Categorias.Add(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        
        public async Task<Categoria> ActualizarAsync(Categoria entity)
        {
            //llamo al context y le paso la entidad completa para que actualice los campos que se hayan modificado
            _context.Categorias.Update(entity);
            //guardo los cambios en la base de datos
            await _context.SaveChangesAsync();

            //retorno la entidad actualizada
            return entity;
        }



        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<Categoria?> ObtenerPorNombreAsync(string nombre)
        {
            return await _context.Categorias
                .AsNoTracking()
                .Where(x => x.Nombre == nombre && x.Estado == true)
                .SingleOrDefaultAsync();
        }


    }
}
