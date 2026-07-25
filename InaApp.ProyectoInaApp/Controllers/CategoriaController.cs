using AutoMapper;
using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.CategoriaDTOs;
using InaApp.ProyectoInaApp.Models.Categoria;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.ProyectoInaApp.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(IGenericService<
            CategoriaResponseDTO, 
            CategoriaCreateDTO, 
            CategoriaUpdateDTO> categoriaService, 
            IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }




        // GET: CategoriaController
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var categoryList = await _categoriaService.ObtenerTodosAsync();

                //paso de DTO a ViewModel   
                var categoryVM = _mapper.Map<List<CategoriaIndexViewModel>>(categoryList.Data);

                return View(categoryVM);
            }
            catch (NotFoundDbException ex)
            {
                //temData para almacenart el msj temporalmente y mostrarlo en la vista, luego se borra cuando hay una nueva solicitud
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }



        // GET: CategoriaController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var category = await _categoriaService.ObtenerPorIdAsync(id);

                //paso de DTO a ViewModel
                var categoryVM = _mapper.Map<CategoriaIndexViewModel>(category.Data);

                return View(categoryVM);
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
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }



        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CategoriaCreateViewModel categoriaCreateVM) 
        {
            try
            {
                //para validar que los datos ingresados en el formulario cumplan con las reglas de validación definidas en el modelo de vista(DataAnotations)
                if (!ModelState.IsValid)
                {
                    //si no es valido, devuelve la vista con los datos ingresados para que el usuario pueda corregirlos
                    return View(categoriaCreateVM);
                }

                //paso de ViewModel a DTO para poder enviarlo al servicio y persistirlo en la base de datos
                var categoryCreateDTO = _mapper.Map<CategoriaCreateDTO>(categoriaCreateVM);

                //llamo serive le paso el dto ya mapeado
                var newCategory = await _categoriaService.CrearAsync(categoryCreateDTO);

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y devuelvo la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!newCategory.Success)
                {
                    ModelState.AddModelError(string.Empty, newCategory.Message);
                    return View(categoriaCreateVM);
                }

                TempData["SuccessMessage"] = "Categoria creada exitosamente.";

                //retorna al index para mostrar la lista de categorias acxtualizada
                return RedirectToAction(nameof(Index));

            }
            catch (EntityExistDbException ex)
            {
                //Se usa ModelState en vez de TempData porque el error ocurre en un form POST.
                //El usuario necesita ver el error EN el form para corregirlo y reintentar.
                //TempData redirige y pierde los datos del form.
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoriaCreateVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View(categoriaCreateVM);
            }
        }



        // GET: CategoriaController/Edit/5
        [HttpGet]
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var category = await _categoriaService.ObtenerPorIdAsync(id);

                if (!category.Success)
                {
                    TempData["ErrorMessage"] = category.Message;
                    return RedirectToAction(nameof(Index)); 
                }

                //paso de responseDTO a ViewModel para poder mostrarlo en la vista y el
                //.Data es xq los datos vienen encapsulados en un objeto de tipo ResponseDTO,
                var categoryEditVM = _mapper.Map<CategoriaEditViewModel>(category.Data);

                //pasamos el producto ya mapeada(model) a la vista para mostrar en la interfaz de usuario
                return View(categoryEditVM);
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
            catch (EntityExistDbException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            { 
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        //este decorador es para vistas sirve para proteger la aplicación contra ataques de falsificación de solicitudes entre sitios (CSRF).
        //genera un tokken automatyico para evitar ataques
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(CategoriaEditViewModel categoriaEditVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(categoriaEditVM);
                }

                //paso de ViewModel a DTO para poder enviarlo al servicio y persistirlo en la base de dato
                var categoryUpdateDTO = _mapper.Map<CategoriaUpdateDTO>(categoriaEditVM);

                //llamo al servicio y le paso el dto ya mapeado
                var updatedCategory = await _categoriaService.ActualizarAsync(categoryUpdateDTO);

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y
                //devuelvo la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!updatedCategory.Success)
                {
                    ModelState.AddModelError(string.Empty, updatedCategory.Message);
                    return View(categoriaEditVM);
                }

                TempData["SuccessMessage"] = "Categoria actualizada exitosamente.";

                //retorna al index para mostrar la lista de categorias acxtualizada
                return RedirectToAction(nameof(Index));

            }
            catch (NotNumberPositiveException ex)
            {
                //Se usa ModelState en vez de TempData porque el error ocurre en un form POST.
                //El usuario necesita ver el error EN el form para corregirlo y reintentar.
                //TempData redirige y pierde los datos del form.
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoriaEditVM);
            }
            catch (NotFoundDbException ex)
            {
                //Se usa ModelState en vez de TempData porque el error ocurre en un form POST.
                //El usuario necesita ver el error EN el form para corregirlo y reintentar.
                //TempData redirige y pierde los datos del form.
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoriaEditVM);
            }
            catch (EntityExistDbException ex)
            {
                //Se usa ModelState en vez de TempData porque el error ocurre en un form POST.
                //El usuario necesita ver el error EN el form para corregirlo y reintentar.
                //TempData redirige y pierde los datos del form.
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoriaEditVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View(categoriaEditVM);
            }
        }

        
        
        // GET: CategoriaController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var category = await _categoriaService.ObtenerPorIdAsync(id);

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y devuelvo
                //la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!category.Success)
                {
                    TempData["ErrorMessage"] = category.Message;
                    return RedirectToAction(nameof(Index));
                }

                //paso de ResponseDTO a ViewModel para poder mostrarlo en la vista y el
                //Data es xq los datos vienen encapsulados en un objeto de tipo ResponseDTO,
                var categoryDeleteVM = _mapper.Map<CategoriaIndexViewModel>(category.Data);

                return View(categoryDeleteVM);
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
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //el nombre de los metodos delete son diferentes xq ambos tienen la misma firma i parametros, hay q diferenciarlos
        public async Task<ActionResult> DeleteConfirmedAsync(int id)
        { 
            try
            {
                var categoryDeleted = await _categoriaService.EliminarAsync(id);

                //si el servicio devuelve un error, agrego un mensaje de error al TempData
                //y redirijo a la vista Index para que se muestre la lista de productos actualizada
                if (!categoryDeleted.Success)
                {
                    TempData["ErrorMessage"] = categoryDeleted.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Categoria eliminada exitosamente.";

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
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
