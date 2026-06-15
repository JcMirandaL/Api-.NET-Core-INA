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
using InaApp.DTOs.Producto;
using AutoMapper;

namespace InaApp.Services
{
    //los : son para implementar una interfaz o heredar de una clase, en este caso se implementa la interfaz generioca y se le pasa la entidad producto
    public class ProductoService : IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>
    {
        //variable guarda instancia del repo, del tipo de la clase 'ProductoRepository, el nombre con _ para indicar que es una variable privada
        private readonly ProductoRepository _productoRepository;
        //variable para inyectar mapper
        private readonly IMapper _mapper;


        //inyecto al repo en el constructor, el constructor es un metodo especial que se ejecuta cuando se crea una instancia de la clase,
        //se usa para inicializar las variables o propiedades de la clase
        //para inyectar hay que definir la inyaccion en el program.cs, en este caso se inyecta el repo con su interface para que se pueda usar en el service
        //x parametro llega la instancia del repo que se inyecto en el program.cs, y se le asigna a la variable privada para poder usarla en los metodos del repo
        public ProductoService(ProductoRepository productoRepository, IMapper mapper)
        {
            //asigno a la variable privada la instancia que me llego por el param, detro del constructor para poder usarla en los metodos del repo
            _productoRepository = productoRepository;
            _mapper = mapper;
        }




        public async Task<List<ProductoResponseDTO>> ObtenerTodosAsync()

        {
            var listaProductos = await _productoRepository.ObtenerTodosAsync();

            if (listaProductos is null || listaProductos.Count == 0)
            {
                throw new NotFoundDbException("No se encontraron productos en la base de datos.");
            }

            var listaProductosResponse = _mapper.Map<List<ProductoResponseDTO>>(listaProductos);

            return listaProductosResponse;
        }


        public async Task<ProductoResponseDTO> ObtenerPorIdAsync(int id)
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

            //paso de entity a responseDTO, mapea las propiedades con el mismo nombre y tipo de dato, si no son iguales hay que configurar el mapeo en el profile de automapper
            var productoResponse = _mapper.Map<ProductoResponseDTO>(producto);

            //devuelvo el DTO mapeado con los datos del producto encontrado, el mapeo se encarga de asignar los valores a las propiedades del responseDTO
            return productoResponse;
        }


        public async Task<ProductoResponseDTO> CrearAsync(ProductoCreateDTO entity)
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

            //creo una nueva instancia de producto y le asigno los valores que llegan por el parametro, para luego pasarla al repo y guardarla en la base de datos
            //MANERA VIEJA DE MAPEAR, CREANDO UNA NUEVA INSTANCIA DE LA ENTIDAD Y ASIGNANDO LOS VALORES MANUALMENTE
            //Producto nuevoProducto = new Producto
            //{
            //    Nombre = entity.Nombre,
            //    Precio = entity.Precio,
            //    Stock = entity.Stock,
            //    Descripcion = entity.Descripcion,
            //};
            //nuevoProducto = await _productoRepository.CrearAsync(nuevoProducto);
            //ProductoResponseDTO productoResponse = new ProductoResponseDTO
            //{
            //    Id = nuevoProducto.Id,
            //    Nombre = nuevoProducto.Nombre,
            //    Precio = nuevoProducto.Precio,
            //    Stock = nuevoProducto.Stock,
            //    Descripcion = nuevoProducto.Descripcion
            //};
            ////llamo al metodo del rewpo y le paso la entidad q llega x params
            //return productoResponse;


            //forma mas sensilla usando la libreria automapper
            //pasa de createDTO a entity, mapea las propiedades con el mismo nombre y tipo de dato, si no son iguales hay que configurar el mapeo en el profile de automapper
            //LO QUE ESTA ENTRE <> ES EL TIPO DESTINO
            Producto nuevoProducto = _mapper.Map<Producto>(entity);

            nuevoProducto = await _productoRepository.CrearAsync(nuevoProducto);

            //pasa de entity a responseDTO, mapea las propiedades con el mismo nombre y tipo de dato, si no son iguales hay que configurar el mapeo en el profile de automapper
            ProductoResponseDTO productoResponse = _mapper.Map<ProductoResponseDTO>(nuevoProducto);

            //retorna el responseDTO con los datos del nuevo producto creado, el mapeo se encarga de asignar los valores a las propiedades del responseDTO
            return productoResponse;
        }


        public async Task<ProductoResponseDTO> ActualizarAsync(ProductoUpdateDTO entity)
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

            //passo de DTO a Entity
            //LO QUE ESTA ENTRE <> ES EL TIPO DESTINO
            Producto productoActualizar = _mapper.Map<Producto>(entity);

            //actualice con la entidad mapeada
            productoActualizar = await _productoRepository.ActualizarAsync(productoActualizar);

            //paso de entity a responseDTO
            ProductoResponseDTO productoResponse = _mapper.Map<ProductoResponseDTO>(productoActualizar);

            //devuelvo el DTO mapeado con los datos del producto actualizado
            return productoResponse;
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
