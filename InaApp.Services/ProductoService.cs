//los using son para importar las clases de otros proyectos o librerias
using InaApp.Common.Interfaces;
using InaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Services
{
    //los : son para implementar una interfaz o heredar de una clase, en este caso se implementa la interfaz IProductoService
    public class ProductoService : IProductoService
    {
        //variable para guardar la instancia del repositorio, se tipa de tipo interface y el nombre con _ para indicar que es una variable privada
        private readonly IProductoRepository _productoRepository;



        //inyecto al repo en el constructor, el constructor es un metodo especial que se ejecuta cuando se crea una instancia de la clase,
        //se usa para inicializar las variables o propiedades de la clase
        //para inyectar hay que definir la inyaccion en el program.cs, en este caso se inyecta el repo con su interface para que se pueda usar en el service
        //x parametro llega la instancia del repo que se inyecto en el program.cs, y se le asigna a la variable privada para poder usarla en los metodos del repo
        public ProductoService(IProductoRepository productoRepository)
        {
            //asigno a la variable privada la instancia que me llego por el param, detro del constructor para poder usarla en los metodos del repo
            _productoRepository = productoRepository;
        }




        //task significa que el metodo es asincrono xq espera respuesta de base datos, y el Async es para que se sepa que ese metodo es asincrono
        public Task<Producto> ActualizarAsync(Producto producto)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> CrearAsync(Producto producto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Producto>> ObtenerTodosAsync()
        {
            _productoRepository.ObtenerTodosAsync();

            return null;
        }
    }

}
