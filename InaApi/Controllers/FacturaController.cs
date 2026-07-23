using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.FacturaDTOs;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.Api.Controllers
{
    [ApiController]
    [Route("api/factura")]
    public class FacturaController : Controller
    {
        private readonly IGenericService<FacturaResponseDTO, FacturaCreateDTO, FacturaUpdateDTO> _facturaService;

        public FacturaController(IGenericService<FacturaResponseDTO, FacturaCreateDTO, FacturaUpdateDTO> facturaService)
        {
            _facturaService = facturaService;
        }




        [HttpGet]
        // GET: FacturaController
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var resListaFactura = await _facturaService.ObtenerTodosAsync();

                return Ok(resListaFactura);

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


        [HttpGet("{id}")]
        // GET: FacturaController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var resFactura = await _facturaService.ObtenerPorIdAsync(id);

                return Ok(resFactura);

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

       

        // POST: FacturaController/Create
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] FacturaCreateDTO facturaDTO)
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

                var response = await _facturaService.CrearAsync(facturaDTO);

                return Created("Factura creada correctamente", response);

            }
            catch (NotFoundDbException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InsufficientStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DiscountOutRange ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TotalOutRange ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error en el servidor. Contacte con el administrador");
            }
        }


       
        // POST: FacturaController/Edit/5
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
        // POST: FacturaController/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
