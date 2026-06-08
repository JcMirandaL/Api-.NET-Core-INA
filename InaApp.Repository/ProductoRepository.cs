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
            return await _context.Productos
                .Where(x => x.Id == id && x.Estado == true)
                .SingleOrDefaultAsync();

        }


        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            try
            {
                //utilizo el contexto para acceder a la tabla de productos y traigo todos los productos de la base de datos, lo convierto a una listaaSINCRONA y lo retorno
                //expresion lanbda para filtrar los productos por estado, solo traigo los productos que esten activos (estado=true)
                return await _context.Productos.Where(x => x.Estado == true).ToListAsync();
            }
            catch (Exception ex)
            {
                //si hay un error lo capturo y lo lanzo para que se maneje en el controlador
                throw new Exception($"Error al obtener los productos: {ex.Message}");

            }
        }




        public async Task<Producto> CrearAsync(Producto entity)
        {
            try
            {
                _context.Productos.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al crear elemento: {ex.Message}");
            }   
        }


        public async Task<Producto> ActualizarAsync(Producto entity)
        {
            try
            {
                //llamo al context y le paso la entidad completa para que actualice los campos que se hayan modificado
                _context.Productos.Update(entity);
                //guardo los cambios en la base de datos y retorno la entidad actualizada
                await _context.SaveChangesAsync();

                //retorno la entidad actualizada
                return entity;

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al actualizar el elemento: {ex.Message}");
            }


        }

        
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                //creo variable q guardara el producto encointrado con el metodo obtenerPorIdAsync, le paso el id que se recibe como parametro
                var producto = await ObtenerPorIdAsync(id);

                if (producto == null)
                {
                    return false;
                }

                producto.Estado = false; // Cambia el estado a false para marcarlo como eliminado

                _context.Productos.Update(producto);// Actualiza el producto en el contexto
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
                return true;// Retorna true si la eliminación fue exitosa
            }
            //ex captura cualquier error que pueda ocurrir durante el proceso de eliminación y lanza una nueva excepción con un mensaje personalizado que incluye el mensaje original del error.
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el elemento: {ex.Message}");
            }
        }


    }
}
