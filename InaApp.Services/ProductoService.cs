//los using son para importar las clases de otros proyectos o librerias
using InaApp.Common.Interfaces;
using InaApp.Common.Exceptions;
using InaApp.Entities;
using InaApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Services
{
    //los : son para implementar una interfaz o heredar de una clase, en este caso se implementa la interfaz generioca y se le pasa la entidad producto
    public class ProductoService : IGenericService<Producto>
    {
        //variable guarda instancia del repo, del tipo de la clase 'ProductoRepository, el nombre con _ para indicar que es una variable privada
        private readonly ProductoRepository _productoRepository;


        //inyecto al repo en el constructor, el constructor es un metodo especial que se ejecuta cuando se crea una instancia de la clase,
        //se usa para inicializar las variables o propiedades de la clase
        //para inyectar hay que definir la inyaccion en el program.cs, en este caso se inyecta el repo con su interface para que se pueda usar en el service
        //x parametro llega la instancia del repo que se inyecto en el program.cs, y se le asigna a la variable privada para poder usarla en los metodos del repo
        public ProductoService(ProductoRepository productoRepository)
        {
            //asigno a la variable privada la instancia que me llego por el param, detro del constructor para poder usarla en los metodos del repo
            _productoRepository = productoRepository;
        }




        public async Task<List<Producto>> ObtenerTodosAsync()

        {
            var listaProductos = await _productoRepository.ObtenerTodosAsync();

            if (listaProductos is null || listaProductos.Count == 0)
            {
                throw new NotFoundDbException("No se encontraron productos en la base de datos.");
            }

            return listaProductos; 
        }


        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
            {
                //msj perzonalizado 
                throw new NotNumberPositiveException($"El Id del producto debe ser mayor a cero. Id Ingresado: {id}.");
            }


            var producto = await _productoRepository.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                //exception personalizada y le paso el string template como poarametro
                //string tmplate = enacdenar texto sin concatenar con el + 
                throw new NotFoundDbException($"El producto con Id {id} no existe.");
            }

            return producto;
        }


        public async Task<Producto> CrearAsync(Producto entity)
        {
            var productoExistente = await _productoRepository.ObtenerPorNombreAsync(entity.Nombre);

            if (productoExistente != null) 
            { 
                throw new EntityExistDbException($"El producto con nombre '{entity.Nombre}' ya existe en la base de datos.");
            }

            if (entity.Stock <= 0)
            {
                throw new NotNumberPositiveException($"El stock debe ser mayor a 0. Stock Ingresado: {entity.Stock}");
            }

            if (entity.Precio <= 0)
            {
                throw new NotNumberPositiveException($"El precio debe ser mayor a 0. Precio Ingresado: {entity.Precio}");
            }


            //llamo al metodo del rewpo y le paso la entidad q llega x params
            return await _productoRepository.CrearAsync(entity);
        }


        public async Task<Producto> ActualizarAsync(Producto entity)
        {
            
            if (entity.Id <= 0)
            {
                throw new NotNumberPositiveException($"El Id '{entity.Id}' debe ser un numero positivo");
            }
            
            var productoExistente = await _productoRepository.ObtenerPorIdAsync(entity.Id);
            if (productoExistente == null)
            {
                throw new NotFoundDbException($"El prodcuto con Id '{entity.Id}' no existe o esta inactivo.");
            }

            var nombreExistente = await _productoRepository.ObtenerPorNombreAsync(entity.Nombre);
            if (nombreExistente != null && nombreExistente.Id != entity.Id)
            {
                throw new EntityExistDbException($"El producto con nombre '{entity.Nombre}' ya existe en la base de datos.");
            }

            if (entity.Stock < 0)
            {
                throw new NotNumberPositiveException($"El stock debe ser mayor a 0. Stock Ingresado: {entity.Stock}");
            }

            if (entity.Precio <= 0)
            {
                throw new NotNumberPositiveException($"El precio debe ser mayor a 0. Precio Ingresado: {entity.Precio}");
            }

            
            return await _productoRepository.ActualizarAsync(entity);
        }



        public async Task<bool> EliminarAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotNumberPositiveException($"El Id del producto debe ser mayor a cero. Id Ingresado: {id}.");
            }

            //variable q guardara el producto encointrado en el repo, le paso el id como parametro
            var producto = await _productoRepository.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                throw new NotFoundDbException($"El producto con el Id {id} no existe.");
            }
            
            return await _productoRepository.EliminarAsync(id);
        }

      
    }

}
