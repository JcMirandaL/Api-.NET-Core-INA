using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.ClienteDTOs;
using InaApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.Api.Controllers
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : Controller
    {
        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;

         
        public ClienteController(IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService)
        {
            _clienteService = clienteService;
        }




        // GET: ClienteController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var resCliente = await _clienteService.ObtenerPorIdAsync(id);

                return Ok(resCliente);

            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }

        }


        // GET: ClienteController
        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var resListaClientes = await _clienteService.ObtenerTodosAsync();

                return Ok(resListaClientes);

            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            { 
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }
        }


        

        // POST: ClienteController/Create
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] ClienteCreateDTO clientedto)
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

                var response = await _clienteService.CrearAsync(clientedto);

                return Created("Cliente creado correctamente", response);

           }
           catch (NotFoundDbException ex)
           {     
                return BadRequest(ex.Message);
           }
           catch (EntityExistDbException ex)
           {
                 return Conflict(ex.Message);
           }
           catch (NotNumberPositiveException ex)
           {
                return BadRequest(ex.Message);
           }
           catch (InvalidEnumException ex)
           {
                return BadRequest(ex.Message);
           }
           catch (Exception)
           {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
           }
        }

       

        // POST: ClienteController/Edit/5
        [HttpPut]
        public async Task<ActionResult> EditAsync([FromBody] ClienteUpdateDTO clienteDTO)
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

                var respose = await _clienteService.ActualizarAsync(clienteDTO);

                return Ok(respose);

            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EntityExistDbException ex)
            {
                return Conflict(ex.Message);
            }
            catch (InvalidEnumException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");

            }
        }

       

        // POST: ClienteController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _clienteService.EliminarAsync(id);

                return Ok(response);

            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundDbException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }
        }
    }
}
