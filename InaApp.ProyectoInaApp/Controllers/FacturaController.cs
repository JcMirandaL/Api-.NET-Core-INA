using AutoMapper;
using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.ClienteDTOs;
using InaApp.DTOs.FacturaDTOs;
using InaApp.DTOs.Producto;
using InaApp.ProyectoInaApp.Models.Factura;
using InaApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.ProyectoInaApp.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IGenericService<FacturaResponseDTO, FacturaCreateDTO, FacturaUpdateDTO> _facturaService;
        private readonly ClienteService _clienteService;
        private readonly ProductoService _productoService;
        private readonly IMapper _mapper;


        public FacturaController(
            IGenericService<FacturaResponseDTO, FacturaCreateDTO, FacturaUpdateDTO> facturaService,
            ClienteService clienteService,
            ProductoService productoService,
            IMapper mapper
        )
        {
            _facturaService = facturaService;
            _clienteService = clienteService;
            _productoService = productoService;
            _mapper = mapper;
        }



        // GET: FacturaController
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var listFacturas = await _facturaService.ObtenerTodosAsync();

                var listViewModel = _mapper.Map<List<FacturaIndexViewModel>>(listFacturas.Data);

                return View(listViewModel);

            }
            catch (NotFoundDbException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: FacturaController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var factura = await _facturaService.ObtenerPorIdAsync(id);

                //PASO DE DTO A VIEWMODEL
                var facturaViewModel = _mapper.Map<FacturaDetailsViewModel>(factura.Data);

                return View(facturaViewModel);
            }
            catch (NotNumberPositiveException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: FacturaController/Create
        [HttpGet]
        public ActionResult CreateAsync()
        {
            var model = new FacturaCreateViewModel();
            return View(model);
        }

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FacturaCreateViewModel facturaVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarNombresAsync(facturaVM);
                    return View(facturaVM);
                }

                //paso de ViewModel a DTO para enviarlo al servicio, incluyendo la lista de detalles
                var facturaDTO = _mapper.Map<FacturaCreateDTO>(facturaVM);

                var response = await _facturaService.CrearAsync(facturaDTO);

                if (!response.Success)
                {
                    await CargarNombresAsync(facturaVM);

                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(facturaVM);
                }

                TempData["SuccessMessage"] = "Factura creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarNombresAsync(facturaVM);
                return View(facturaVM);
            }
            catch (InsufficientStockException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarNombresAsync(facturaVM);
                return View(facturaVM);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: FacturaController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdAsync(id);

                if (!response.Success)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                var facturaDeleteVM = _mapper.Map<FacturaIndexViewModel>(response.Data);

                return View(facturaDeleteVM);

            }
            catch (NotNumberPositiveException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: FacturaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync(int id, IFormCollection collection)
        {
            try
            {
                var response = await _facturaService.EliminarAsync(id);

                if (!response.Success)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Producto eliminado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (NotNumberPositiveException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        public async Task<IActionResult> BuscarClientes(string? termino)
        {
            var response = await _clienteService.BuscarAsync(termino);

            var viewModel = new BusquedaClienteModalViewModel
            {
                //aqui le digo que si la respuesta es nula, le asigne una lista vacía para evitar errores en la vista
                Items = response.Data ?? new List<ClienteResponseDTO>(),
                //si el termino es nulo, le asigno una cadena vacía para evitar errores en la vista
                Termino = termino ?? ""
            };

            return PartialView("_ResultadoBusquedaClientes", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarProductos(string? termino)
        {
            var response = await _productoService.BuscarAsync(termino);

            var viewModel = new BusquedaProductoModalViewModel
            {
                Items = response.Data ?? new List<ProductoResponseDTO>(),
                Termino = termino ?? ""
            };

            return PartialView("_ResultadoBusquedaProductos", viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(FacturaCreateViewModel model)
        {
            try
            {
                await CargarNombresAsync(model);

                if (model.ProductoId <= 0)
                {
                    ModelState.AddModelError("ProductoId", "Seleccione un producto antes de agregar.");
                    return View("Create", model);
                }
                if (model.Cantidad <= 0)
                {
                    ModelState.AddModelError("Cantidad", "La cantidad debe ser mayor a 0.");
                    return View("Create", model);
                }

                //buscar el producto seleccionado
                var producto = await _productoService.ObtenerPorIdAsync(model.ProductoId);

                if (producto.Data == null)
                {
                    TempData["ErrorMessage"] = "Producto no encontrado.";
                    return View("Create", model);
                }

                //aplico cap: si el descuento ingresado supera el maximo del producto, se usa el maximo
                int descuentoFinal = model.DescuentoAplicado;
                if (descuentoFinal > producto.Data.DescuentoMaximo)
                    descuentoFinal = producto.Data.DescuentoMaximo;

                //calculo valores de la linea
                decimal subt = model.Cantidad * producto.Data.Precio;
                decimal impuesto = subt * producto.Data.PorcentajeImpuesto / 100m;
                decimal descMonto = subt * descuentoFinal / 100m;
                decimal totalLinea = subt + impuesto - descMonto;

                // Verificar si el producto ya existe en la factura
                var detalleExistente = model.Detalles
                    .FirstOrDefault(x => x.ProductoId == model.ProductoId);

                if (detalleExistente != null)
                {
                    // Solo aumenta la cantidad y recalcula todo
                    detalleExistente.Cantidad += model.Cantidad;
                    detalleExistente.Subtotal = detalleExistente.Cantidad * detalleExistente.Precio;
                    detalleExistente.MontoImpuesto = detalleExistente.Subtotal * detalleExistente.PorcentajeImpuesto / 100m;
                    detalleExistente.DescuentoMonto = detalleExistente.Subtotal * detalleExistente.DescuentoAplicado / 100m;
                    detalleExistente.TotalLinea = detalleExistente.Subtotal + detalleExistente.MontoImpuesto - detalleExistente.DescuentoMonto;
                }
                else
                {
                    //crear la línea
                    var detalle = new DetalleFacturaViewModel
                    {
                        ProductoId = producto.Data.Id,
                        ProductoNombre = producto.Data.Nombre,
                        Precio = producto.Data.Precio,
                        Cantidad = model.Cantidad,
                        Subtotal = subt,
                        PorcentajeImpuesto = producto.Data.PorcentajeImpuesto,
                        MontoImpuesto = impuesto,
                        DescuentoAplicado = descuentoFinal,
                        DescuentoMonto = descMonto,
                        TotalLinea = totalLinea
                    };

                    //agregar a la lista
                    model.Detalles.Add(detalle);
                }

                //recalcular todos los totales de la factura
                RecalcularTotales(model);

                //Limpiar ModelState para que la vista refleje los nuevos valores.
                ModelState.Remove("ProductoId");
                ModelState.Remove("Cantidad");
                ModelState.Remove("ProductoNombre");
                ModelState.Remove("DescuentoAplicado");

                model.ProductoId = 0;
                model.Cantidad = 0;
                model.ProductoNombre = "";
                model.DescuentoAplicado = 0;

                return View("Create", model);
            }
            catch (NotNumberPositiveException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarNombresAsync(model);
                return View("Create", model);
            }
            catch (NotFoundDbException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarNombresAsync(model);
                return View("Create", model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                await CargarNombresAsync(model);
                return View("Create", model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> RemoveProduct(FacturaCreateViewModel model, [FromQuery] int productoId)
        {
            try
            {
                await CargarNombresAsync(model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                return View("Create", model);
            }

            var detalle = model.Detalles
                .FirstOrDefault(x => x.ProductoId == productoId);

            if (detalle != null)
            {
                model.Detalles.Remove(detalle);
            }


            RecalcularTotales(model);

            ModelState.Remove("ProductoId");
            ModelState.Remove("Cantidad");
            ModelState.Remove("ProductoNombre");
            ModelState.Remove("DescuentoAplicado");

            model.ProductoId = 0;
            model.Cantidad = 0;
            model.ProductoNombre = "";
            model.DescuentoAplicado = 0;

            return View("Create", model);
        }


        //recalcula todos los totales de la factura desde los detalles
        private void RecalcularTotales(FacturaCreateViewModel model)
        {
            model.Subtotal = model.Detalles.Sum(x => x.Subtotal);
            model.TotalImpuestos = model.Detalles.Sum(x => x.MontoImpuesto);
            model.DescuentoTotal = model.Detalles.Sum(x => x.DescuentoMonto);
            model.Total = model.Subtotal + model.TotalImpuestos - model.DescuentoTotal;
        }

        private async Task CargarNombresAsync(FacturaCreateViewModel vModel)
        {
            if (vModel.ClienteId > 0)
            {
                try
                {
                    var cliente = await _clienteService.ObtenerPorIdAsync(vModel.ClienteId);
                    if (cliente.Success && cliente.Data != null)
                    {
                        vModel.ClienteNombre =
                            $"{cliente.Data.Nombre} {cliente.Data.Apellido1} {cliente.Data.Apellido2}".Trim();
                    }
                }
                catch { }
            }

            if (vModel.ProductoId > 0)
            {
                try
                {
                    var producto = await _productoService.ObtenerPorIdAsync(vModel.ProductoId);
                    if (producto.Success && producto.Data != null)
                    {
                        vModel.ProductoNombre = producto.Data.Nombre;
                    }
                }
                catch { }
            }
        }


        // GET: FacturaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FacturaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}