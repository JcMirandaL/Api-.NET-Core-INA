using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.Producto;
using InaApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InaApp.Api.Controllers
{
    //la clase controller es la clase base para todos los controladores en ASP.NET Core MVC, proporciona metodos y propiedades
    //para manejar las solicitudes HTTP y generar respuestas HTTP

    //indica que esta clase es un controlador de API, lo que significa que se espera que maneje solicitudes HTTP y genere respuestas HTTP
    [ApiController]
    //cuando pongo [controller] toma solo el nombre del controller en minuscula en este caso producto omitiendo la parte de Controller del nombre. ruta= https://localhost:5001/api/producto
    [Route("api/[controller]")]//define la ruta pricipal para las solicitudes HTTP, htps://localhost:5001/api/producto
    public class ProductoController : Controller//hereda de la clase Controller para poder usar sus metodos y propiedades 
    {

        //variable para guardar esa instancia, se tipa de tipo interface y el nombre con _ para indicar que es una variable privada
        private readonly IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;//solo de lectura xq no se va a modidficar


        //inyecto al service en el constructor, el constructor es un metodo especial que se ejecuta cuando se crea una instancia de la clase,
        //se usa para inicializar las variables o propiedades de la clase
        //para inyectar hay que definir la inyaccion en el program.cs, en este caso se inyecta el service con su interface para que se pueda usar en el controlador, y se le asigna la instancia que me llega por el parametro del constructor a la variable privada para poder usarla en los metodos del controlador
        public ProductoController(IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoService)
        {
            //asigno la instancia que me llego por el param, detro del constructor a la variable privada para poder usarla en los metodos del controlador
            _productoService = productoService;
        }




        // CRUD BASICO 
        //OBTENER TODOS LOS PRODUCTOS
        [HttpGet]//decorador para decir que la ruta es de un get
        //el ActionResult es para indicar que devuelce un status code 
        public async Task<ActionResult> IndexAsync()//index es obtener todos
        {
            try
            {
                //llamo al metodo del service, var es una variable que se infiere el tipo de dato automaticamente 
                var lista = await _productoService.ObtenerTodosAsync();


                //200 = ok, puedo usar StatusCode o directamente el Ok  
                return StatusCode(200, lista);
            }
            catch (NotFoundDbException ex)
            {
                //devuelve 404(no encontrado) y el msj perzonalizado 
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador" + ex.Message);
            }
        }


        //OBTENER POR ID
        [HttpGet( "{id}" )]
        // GET: htps://localhost:5001/api/producto/5
        //details es obtener por id o x detalle ese parametro de la ruta debe llamrse igaul al parametro del metodo
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerPorIdAsync(id);

                return Ok(producto);
            }
            //except perzonalizadas, si no entra en niguno va al ultimo al 500
            catch (NotNumberPositiveException ex)
            {
                //duvuelve 400(Datos incorrectos) y el msj perzonalizado
                return BadRequest(ex.Message); 
            }
            catch (NotFoundDbException ex)
            {
                //devuelve 404(no encontrado) y el msj perzonalizado 
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {   //500 error interno del servido
                return StatusCode(500, "Error en el servidor. Contacte con el administrador: " + ex.Message);
            }
        }



        // GET: ProductoController/Create
        [HttpPost]
        //le digo que obtenga el product del body de la solicitud y que es de tipo Producto y la variabl producto,
        //Task = metodo asincrono, ActionResult = devuelve un status code
        public async Task<ActionResult> CreateAsync([FromBody] ProductoCreateDTO productoDTO)
        {
            try
            {
                //el modelState es una propiedad de la clase Controller que contiene el estado de validacion del modelo,
                //si el modelo(entidad en cuestion) no es valido devuelve un 400 con el detalle de los errores de validacion
                //las validaciones que usa son los decoradores del modelo entity
                if (!ModelState.IsValid)
                {
                    //devuelvo el modelState q tiene los msjs de errors
                    return BadRequest(ModelState);
                }

                var result = await _productoService.CrearAsync(productoDTO);

                //Created es = 201, es lo mismo q StatusCode(201, result)
                return Created("Producto creado correctamente", result);
                 
            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EntityExistDbException ex)
            {
                //409 = conflic (datos duplicados)
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador" + ex.Message);
            }
        }



        //ACTUALIZAR POR ID
        [HttpPut]
        // GET: ProductoController/Edit/5
        public async Task<ActionResult> EditAsync([FromBody] ProductoUpdateDTO productoDTO)
        {
            try
            {
                //el modelState es una propiedad de la clase Controller que contiene el estado de validacion del modelo,
                //si el modelo(entidad en cuestion) no es valido devuelve un 400 con el detalle de los errores de validacion
                //las validaciones que usa son los decoradores del modelo entity
                if (!ModelState.IsValid)
                {
                    //devuelvo el modelState q tiene los msjs de errors
                    return BadRequest(ModelState);
                }

                //llamo al metodo del service y le paso el producto encontrado
                await _productoService.ActualizarAsync(productoDTO);
                
                return Ok("Producto actualizado correctamente");
            }
            catch (EntityExistDbException ex)
            {
                return Conflict(ex.Message);
            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador" + ex.Message);
            }
        }



        //ELIMINAR POR ID(BORRADO LOGICO)
        // GET: ProductoController/Delete/5
        [HttpDelete("{id}")] // le paso x parametro el id en la ruta, htps://localhost:5001/api/producto/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _productoService.EliminarAsync(id);

                //si es 200 msj positivo SiNo msj negativo
                return Ok("Producto eliminado correctamente" + result);
            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador" + ex.Message);
            }
        }

      
    }
}
