using InaApp.Common.Interfaces;
using InaApp.Data;
using InaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Repository
{
    //IMPLEMENTO la I generica y le paso la entidad en este caso producto
    public class ProductoRepository : IGenericRepository<Producto>
    {
        //variable de tipo ApplicationDbContex para poder acceder a la base de datos, se tipa de tipo clase y el nombre con _ para indicar que es una variable privada
        private readonly ApplicationDbContex _context;

        //constructor
        public ProductoRepository(ApplicationDbContex context)
        {
            //hago la inyesccion de dependencia
            _context = context;
        }
         

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            //variable para al;macenar la entity q devuelve la base datos
            //AsNoTracking() se utiliza para indicar que no se va a realizar un seguimiento de los cambios en la entidad,
            //esto mejora el rendimiento cuando solo se necesita leer los datos sin modificarlos
            return await _context.Productos.AsNoTracking()
                .Where(x => x.Id == id && x.Estado == true)
                .SingleOrDefaultAsync();

        }



        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            //utilizo el contexto para acceder a la tabla de productos y traigo todos los productos de la base de datos, lo convierto a una listaaSINCRONA y lo retorno
            //expresion lanbda para filtrar los productos por estado, solo traigo los productos que esten activos (estado=true)
            return await _context.Productos.AsNoTracking()
                .Where(x => x.Estado == true).ToListAsync();
        }




        public async Task<Producto> CrearAsync(Producto entity)
        {  
            _context.Productos.Add(entity);
            await _context.SaveChangesAsync();
            return entity;   
        }


        public async Task<Producto> ActualizarAsync(Producto entity)
        {
            //llamo al context y le paso la entidad completa para que actualice los campos que se hayan modificado
            _context.Productos.Update(entity);
            //guardo los cambios en la base de datos y retorno la entidad actualizada
            await _context.SaveChangesAsync();

            //retorno la entidad actualizada
            return entity;
        }

        
        public async Task<bool> EliminarAsync(int id)
        {
            var producto = await ObtenerPorIdAsync(id);

            producto.Estado = false; // Cambia el estado a false para marcarlo como eliminado

            _context.Productos.Update(producto);// Actualiza el producto en el contexto
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return true;// Retorna true si la eliminación fue exitosa
        }



        public async Task<Producto> ObtenerPorNombreAsync(string nombre)
        {
            //el AsNoTracking() se utiliza para indicar que no se va a realizar un seguimiento de los cambios en la entidad,
            //esto mejora el rendimiento cuando solo se necesita leer los datos sin modificarlos
            return await _context.Productos.AsNoTracking()
                .Where(x => x.Nombre == nombre && x.Estado == true)
                .SingleOrDefaultAsync();
        }

    }
}
