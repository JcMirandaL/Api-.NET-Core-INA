using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.Api.Controllers
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : Controller
    {
        private readonly IGenericService<Cliente> _clienteService;

         
        public ClienteController(IGenericService<Cliente> clienteService)
        {
            _clienteService = clienteService;
        }




        // GET: ClienteController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                return Ok(cliente);

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


        // GET: ClienteController
        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var listaClientes = await _clienteService.ObtenerTodosAsync();

                return Ok(listaClientes);

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


        

        // POST: ClienteController/Create
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] Cliente cliente)
        {
           try
           {
                var result = await _clienteService.CrearAsync(cliente);

                return Created("Cliente creado correctamente", result);

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
           catch (Exception ex)
           {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador" + ex.Message);
           }
        }

       

        // POST: ClienteController/Edit/5
        [HttpPut]
        public async Task<ActionResult> EditAsync([FromBody] Cliente cliente)
        {
            try
            {
                await _clienteService.ActualizarAsync(cliente);

                return Ok("Cliente actualizado correctamente");

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
            catch (Exception ex)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador" + ex.Message);

            }
        }

       

        // POST: ClienteController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _clienteService.EliminarAsync(id);

                return Ok("Cliente eliminado correctamente");

            }
            catch (NotNumberPositiveException ex)
            {
                return BadRequest(ex.Message);
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
