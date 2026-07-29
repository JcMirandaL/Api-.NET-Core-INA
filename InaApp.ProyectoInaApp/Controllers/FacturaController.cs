using AutoMapper;
using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.ClienteDTOs;
using InaApp.DTOs.FacturaDTOs;
using InaApp.DTOs.Producto;
using InaApp.ProyectoInaApp.Models.Factura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InaApp.ProyectoInaApp.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IGenericService<FacturaResponseDTO, FacturaCreateDTO, FacturaUpdateDTO> _facturaService;
        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;
        private readonly IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly IMapper _mapper;


        public FacturaController(
            IGenericService<FacturaResponseDTO, FacturaCreateDTO, FacturaUpdateDTO> facturaService,
            IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService,
            IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoService,
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
                //Se usa TempData porque es un GET redirige a Index. ModelState se pierde en redirect xq solo vive en la peticion actual.
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
            catch (NotNumberPositiveException ex)//exeption personalizada q se lanza desde el servicio si el id es negativo.
            {
                //Se usa TempData porque es un GET redirige a Index. ModelState se pierde en redirect.
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio
            {
                //Se usa TempData porque es un GET redirige a Index. ModelState se pierde en redirect.
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
        public async Task<ActionResult> CreateAsync()
        {
            //Creo un nuevo objeto FacturaCreateViewModel para pasarlo a la vista
            var model = new FacturaCreateViewModel();

            //Llamo método CargarSelectListAsync para llenar propiedades Clientes y Productos(selectList)
            await CargarSelectListAsync(model);

            //Paso el modelo a la vista
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
                    await CargarSelectListAsync(facturaVM);
                    return View(facturaVM);
                }

                //paso de ViewModel a DTO para enviarlo al servicio, incluyendo la lista de detalles
                var facturaDTO = _mapper.Map<FacturaCreateDTO>(facturaVM);

                var response = await _facturaService.CrearAsync(facturaDTO);

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y devuelvo la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!response.Success)
                {
                    await CargarSelectListAsync(facturaVM);

                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(facturaVM);
                }

                TempData["SuccessMessage"] = "Factura creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarSelectListAsync(facturaVM);
                return View(facturaVM);
            }
            catch (InsufficientStockException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarSelectListAsync(facturaVM);
                return View(facturaVM);
            }
            catch (DiscountOutRange ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarSelectListAsync(facturaVM);
                return View(facturaVM);
            }
            catch (TotalOutRange ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarSelectListAsync(facturaVM);
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

                //paso de ResponseDTO a ViewModel para poder mostrarlo en la vista y el
                //Data es xq los datos vienen encapsulados en un objeto de tipo ResponseDTO,
                var facturaDeleteVM = _mapper.Map<FacturaIndexViewModel>(response.Data);

                return View(facturaDeleteVM);

            }
            catch (NotNumberPositiveException ex)//exeption personalizada q se lanza desde el servicio si el id es negativo.
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio
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
                //llamo service eliminar y le paso el id del a eliminar
                var response = await _facturaService.EliminarAsync(id);

                //si el servicio devuelve un error, agrego un mensaje de error al TempData
                //y redirijo a la vista Index para que se muestre la lista de facturas
                if (!response.Success)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                //si todo sale bien, guardo un mensaje de éxito en TempData
                //cuando hay otra peticion se pierde el mensaje
                TempData["SuccessMessage"] = "Producto eliminado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (NotNumberPositiveException ex)//exeption personalizada q se lanza desde el servicio si el id es negativo.
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio
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




        private async Task CargarSelectListAsync(FacturaCreateViewModel vModel)
        {
            //obtengo todos los clientes
            var clientes = await _clienteService.ObtenerTodosAsync();
            //asigno a la propiedad Clientes del ViewModel una nueva SelectList con los datos obtenidos de clientes.Data
            vModel.Clientes = new SelectList(clientes.Data, "Id", "Nombre");

            //obtengo todos los productos
            var productos = await _productoService.ObtenerTodosAsync();
            //asigno a la propiedad Productos del ViewModel una nueva SelectList con los datos obtenidos de productos.Data
            vModel.Productos = new SelectList(productos.Data, "Id", "Nombre");
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(FacturaCreateViewModel model)
        {
            try
            {
                //volver a cargar los combos
                await CargarSelectListAsync(model);

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

                // Verificar si el producto ya existe en la factura
                var detalleExistente = model.Detalles
                    .FirstOrDefault(x => x.ProductoId == model.ProductoId);

                if (detalleExistente != null)
                {
                    // Solo aumenta la cantidad
                    detalleExistente.Cantidad += model.Cantidad;
                    detalleExistente.Subtotal = detalleExistente.Cantidad * detalleExistente.Precio;
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
                        Subtotal = model.Cantidad * producto.Data.Precio
                    };

                    //agregar a la lista
                    model.Detalles.Add(detalle);
                }

                //recalcular subtotal
                model.Subtotal = model.Detalles.Sum(x => x.Subtotal);

                //recalcular total
                model.Total = model.Subtotal - (model.Subtotal * model.Descuento / 100);

                //Limpiar ModelState para que la vista refleje los nuevos valores.
                //En ASP.NET Core, ModelState tiene prioridad sobre el modelo.
                //Sin esto, el select y el input no se limpian porque ModelState conserva el valor del POST anterior.
                ModelState.Remove("ProductoId");
                ModelState.Remove("Cantidad");

                //retorno a la vista Create con el modelo actualizado
                return View("Create", model);
            }
            catch (NotNumberPositiveException ex)
            {
                //Se usa ModelState porque el error ocurre en un form POST.
                //El usuario necesita ver el error EN el form para corregirlo y reintentar.
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarSelectListAsync(model);
                return View("Create", model);
            }
            catch (NotFoundDbException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarSelectListAsync(model);
                return View("Create", model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                await CargarSelectListAsync(model);
                return View("Create", model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> RemoveProduct(FacturaCreateViewModel model, [FromQuery] int productoId)
        {
            try
            {
                await CargarSelectListAsync(model);
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


            model.Subtotal = model.Detalles.Sum(x => x.Subtotal);

            model.Total = model.Subtotal - (model.Subtotal * model.Descuento / 100);

            //Limpiar ModelState para que la vista refleje los nuevos valores.
            //En ASP.NET Core, ModelState tiene prioridad sobre el modelo.
            //Sin esto, el select y el input no se limpian porque ModelState conserva el valor del POST anterior.
            ModelState.Remove("ProductoId");
            ModelState.Remove("Cantidad");

            //retorno a la vista Create con el modelo actualizado
            return View("Create", model);
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
