using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.CategoriaDTOs;
using InaApp.DTOs.Producto;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.Api.Controllers
{
    //indica que esta clase es un controlador de API, lo que significa que se espera que maneje solicitudes HTTP y genere respuestas HTTP
    [ApiController]
    //cuando pongo [controller] toma solo el nombre del controller en minuscula en este caso categoria omitiendo la parte de Controller del nombre. ruta= https://localhost:5001/api/producto
    [Route("api/[controller]")]//define la ruta pricipal para las solicitudes HTTP, htps://localhost:5001/api/categoria
    public class CategoriaController : Controller //hereda de la clase Controller para poder usar sus metodos y propiedades
    {
        //variable para guardar esa instancia, se tipa de tipo interface y el nombre con _ para indicar que es una variable privada
        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;//solo de lectura xq no se va a modidficar


        //constructor para inicializar variable del service
        public CategoriaController(IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> categoriaService)
        {
            //asigno la instancia que me llego por el param, detro del constructor
            //a la variable privada para poder usarla en los metodos del controlador
            _categoriaService = categoriaService;
        }


        //OBTENER TODOS LAS CATEGORIAS
        [HttpGet]//decorador para decir que la ruta es de un get
        //el ActionResult es para indicar que devuelce un status code
        public async Task<ActionResult> IndexAsync()//index es obtener todos
        {
            try
            {
                //llamo al metodo del service, var es una variable que se infiere el tipo de dato automaticamente 
                var resListaCategoria = await _categoriaService.ObtenerTodosAsync();

                return Ok(resListaCategoria);//devuelvo un status code 200 con la lista de categorias
            
            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);//devuelvo un status code 404 con el mensaje de error personalizado
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }
        }

        //obtener por ID
        [HttpGet("{id}")]//decorador para decir que la ruta es de un
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var resCategoria = await _categoriaService.ObtenerPorIdAsync(id);
                return Ok(resCategoria);//devuelvo un status code 200 con la categoria encontrada
            
            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);//devuelvo un status code 400 con el mensaje de error personali 
            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);//devuelvo un status code 404 con el mensaje de error personali 
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }
        }


        //crear categoria
        [HttpPost]
        //le digo que obtenga categoria del body de la solicitud y que es de tipo CategoriaCreateDTO y la variabl categoriaDTO
        //Task = metodo asincrono, ActionResult = devuelve un status code
        public async Task<ActionResult> CreateAsync([FromBody] CategoriaCreateDTO categoriaDTO)
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


                //llamo al metodo del service para crear categoria, le paso el DTO que me llego por el body
                var resCategoriaCreada = await _categoriaService.CrearAsync(categoriaDTO);
                return Created("Categoria creada correctamente: ", resCategoriaCreada);//devuelvo un status code 200 con la categoria creada
            
            }
            catch (EntityExistDbException ex)
            {
                //409 ya existe un registro
                return Conflict(ex.Message);//devuelvo un status code 400 con el mensaje de error personalizado
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }
        }



        [HttpPut]
        // GET: ProductoController/Edit/5
        public async Task<ActionResult> EditAsync([FromBody] CategoriaUpdateDTO categoriaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var resCategoriaActualizada = await _categoriaService.ActualizarAsync(categoriaDTO);
                return Ok(resCategoriaActualizada);
            
            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);//devuelvo un status code 400 con el mensaje de error personalizado
            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);//devuelvo un status code 404 con el mensaje de error personalizado
            }
            catch (EntityExistDbException ex)
            {
                return Conflict(ex.Message);//devuelvo un status code 409 con el mensaje de error personalizado
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }
        }


        //ELIMINAR POR ID(BORRADO LOGICO)
        // GET: ProductoController/Delete/5
        [HttpDelete("{id}")] // le paso x parametro el id en la ruta, htps://localhost:5001/api/producto/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var resEliminacion = await _categoriaService.EliminarAsync(id);
                return Ok(resEliminacion);//devuelvo un status code 200 con el resultado de la eliminacion (true o false)
            
            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);//devuelvo un status code 400 con el mensaje de error personalizado
            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);//devuelvo un status code 404 con el mensaje de error personalizado
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }

        }



    }
}