using AutoMapper;
using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.ClienteDTOs;
using InaApp.ProyectoInaApp.Models.Cliente;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.ProyectoInaApp.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }




        // GET: ClienteController
        public async Task<ActionResult> Index()
        {
            try
            {
                var listaClientes = await _clienteService.ObtenerTodosAsync();

                //el .Dta es xq el resultado de la api viene en un objeto q tiene 3 propiedades: data, message y success.
                var ListViewMoel = _mapper.Map<List<ClienteIndexViewModel>>(listaClientes.Data);

                return View(ListViewMoel);
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio si no se encuentra ningun cliente en la base de datos.
            {
                //el TempData es para guardar temporalmente un mensaje de error y mostrarlo en la vista Index.
                TempData["ErrorMessage"] = ex.Message;
                //redirecciono a la vista Index para que se muestre el mensaje de error.
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ClienteController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                var clienteViewModel = _mapper.Map<ClienteIndexViewModel>(cliente.Data);

                return View(clienteViewModel);
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

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        //el antiforgerytoken es para evitar ataques de tipo CSRF (Cross-Site Request Forgery) y se debe usar en todos los formularios que envian datos al servidor.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(ClienteCreateViewModel clienteCreateVM)
        {
            try
            {
                //para validar que los datos ingresados en el formulario cumplan con las reglas de validación definidas en el modelo de vista(DataAnotations)
                if (!ModelState.IsValid)
                {
                    //si no es valido, devuelve la vista con los datos ingresados para que el usuario pueda corregirlos
                    return View(clienteCreateVM);
                }

                //paso de ViewModel a DTO para enviarlo al servicio
                var clienteCreateDTO = _mapper.Map<ClienteCreateDTO>(clienteCreateVM);

                var newCliente = await _clienteService.CrearAsync(clienteCreateDTO);

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y devuelvo la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!newCliente.Success)
                {
                    ModelState.AddModelError(string.Empty, newCliente.Message);
                    return View(clienteCreateVM);
                }

                TempData["SuccessMessage"] = "Cliente creado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (EntityExistDbException ex)//exeption personalizada q se lanza desde el servicio si el cliente ya existe en la base de datos.
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(clienteCreateVM);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";

                return View(clienteCreateVM);
            }
        }

        // GET: ClienteController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                //si el servicio devuelve un error, agrego un mensaje de error al TempData y redirecciono a la vista Index para que se muestre el mensaje de error.
                if (!cliente.Success)
                {
                    TempData["ErrorMessage"] = cliente.Message;
                    return RedirectToAction(nameof(Index));
                }

                //paso de DTO a ViewModel para enviarlo a la vista
                var clienteEditVM = _mapper.Map<ClienteEditViewModel>(cliente.Data);

                return View(clienteEditVM);
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

        // POST: ClienteController/Edit/5
        //es un post xq los forms en html solo acepta 2 metodos post y get, y el edit es un form q envia datos al servidor para actualizar un cliente. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ClienteEditViewModel clienteEditVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(clienteEditVM);
                }

                //paso de ViewModel a DTO para enviarlo al servicio
                var clienteUpdateDTO = _mapper.Map<ClienteUpdateDTO>(clienteEditVM);

                var updatedCliente = await _clienteService.ActualizarAsync(clienteUpdateDTO);

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y
                //devuelvo la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!updatedCliente.Success)
                {
                    ModelState.AddModelError(string.Empty, updatedCliente.Message);
                    return View(clienteEditVM);
                }

                TempData["SuccessMessage"] = "Cliente actualizado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (NotNumberPositiveException ex)//exeption personalizada q se lanza desde el servicio si el id es negativo.
            {
                //Se usa ModelState en vez de TempData porque el error ocurre en un form POST.
                //El usuario necesita ver el error EN el form para corregirlo y reintentar.
                //TempData redirige y pierde los datos del form.
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(clienteEditVM);
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio
            {
                //Se usa ModelState en vez de TempData porque el error ocurre en un form POST.
                //El usuario necesita ver el error EN el form para corregirlo y reintentar.
                //TempData redirige y pierde los datos del form.
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(clienteEditVM);
            }
            catch (EntityExistDbException ex)//exeption personalizada q se lanza desde el servicio si el cliente ya existe en la base de datos.
            {
                //Se usa ModelState en vez de TempData porque el error ocurre en un form POST.
                //El usuario necesita ver el error EN el form para corregirlo y reintentar.
                //TempData redirige y pierde los datos del form.
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(clienteEditVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";

                return View(clienteEditVM);
            }
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y devuelvo
                //la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!cliente.Success)
                {
                    TempData["ErrorMessage"] = cliente.Message;
                    return RedirectToAction(nameof(Index));
                }

                //paso de DTO a ViewModel para enviarlo a la vista
                var clienteDeleteVM = _mapper.Map<ClienteIndexViewModel>(cliente.Data);

                return View(clienteDeleteVM);
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

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync(int id)
        {
            try
            {
                var deletedCliente = await _clienteService.EliminarAsync(id);

                //si el servicio devuelve un error, agrego un mensaje de error al TempData
                //y redirijo a la vista Index para que se muestre la lista de productos actualizada
                if (!deletedCliente.Success)
                {
                    TempData["ErrorMessage"] = deletedCliente.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Cliente eliminada exitosamente.";

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
    }
}
