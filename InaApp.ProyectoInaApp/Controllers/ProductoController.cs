using AutoMapper;
using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.DTOs.CategoriaDTOs;
using InaApp.DTOs.Producto;
using InaApp.Entities;
using InaApp.ProyectoInaApp.Models.Producto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.Threading.Tasks;


namespace InaApp.ProyectoInaApp.Controllers
{
    public class ProductoController : Controller
    {
        //injecvion de dependencias
        private readonly IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;
        private readonly IMapper _mapper;

        public ProductoController(
            IGenericService<ProductoResponseDTO, 
            ProductoCreateDTO, 
            ProductoUpdateDTO> productoService,
            IGenericService<CategoriaResponseDTO, 
            CategoriaCreateDTO, 
            CategoriaUpdateDTO> categoriaService,
            IMapper mapper)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
            _mapper = mapper;
        }




        //CRUD + VIEWS
        // GET: ProductoController obetner todoss
        public async Task<ActionResult> Index()
        {
            try
            {
                //voy al service obtener todos
                var listProducts = await _productoService.ObtenerTodosAsync();

                //paso de DTO a ViewModel para poder mostrarlo en la vista y el .Data es xq los datos vienen encapsulados en un objeto de tipo ResponseDTO,
                //que contiene la propiedad Data que es la que contiene la lista de productos.
                var ListViewModel = _mapper.Map<List<ProductoIndexViewModel>>(listProducts.Data);

                //pasamos la lista ya mapeada(model) a la vista para que se pueda mostrar en la interfaz de usuario
                return View(ListViewModel);
            }
            catch (NotFoundDbException ex)
            {
                //el ViewBag permiten pasar datos desde el controlador a la vista. Como por ejemplo,
                //se puede utilizar ViewBag para pasar un mensaje de error a la vista en caso de que no se encuentren productos en la base de datos. Roles, userNmae, correo etc
                //ViewData sirve para pasar datos desde el controlador a la vista, pero a diferencia de ViewBag, ViewData es un diccionario
                //que permite almacenar y recuperar datos mediante claves.
                //model: pasar datos, dtos, entities, viewModels, listados, etc. a la vista.
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Error interno del servidor. Contacte con el administrador.";
                // Manejar la excepción según sea necesario
                return View();

            }

        }

        // GET: ProductoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                //voy al service obtener por id
                var producto = await _productoService.ObtenerPorIdAsync(id);

                //paso de DTO a ViewModel para poder mostrarlo en la vista y el .Data es xq los datos vienen encapsulados en un objeto de tipo ResponseDTO,
                var productoDetailsVM = _mapper.Map<ProductoIndexViewModel>(producto.Data);

                //pasamos el producto ya mapeada(model) a la vista para mostrar en la interfaz de usuario
                return View(productoDetailsVM);
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
                return View();
            }
        }



        // GET: ProductoController/Create
        [HttpGet]//devuleve la vista para crear un producto
        public async Task<ActionResult> CreateAsync()
        {
            //obtengo todas la categorias
            var categorias = await _categoriaService.ObtenerTodosAsync();

            //asigno las categorias a la propiedad del viewModel para que se puedan mostrar en la vista en el select
            var viewModel = new ProductoCreateViewModel
            {
                //categorias propiedad del ProductoCrteateViewModel
                Categorias = new SelectList(categorias.Data, "Id", "Nombre")
            };

            return View(viewModel);
        }

        // POST: ProductoController/Create
        [HttpPost]
        //este decorador es para vistas sirve para proteger la aplicación contra ataques de falsificación de solicitudes entre sitios (CSRF).
        //genera un tokken automatyico para evitar ataques
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> CreateAsync(ProductoCreateViewModel productoVM)
        {
            try
            {
                //para validar que los datos ingresados en el formulario cumplan con las reglas de validación definidas en el modelo de vista (ViewModel).DataAnotations
                if (!ModelState.IsValid)
                {
                    //cargoi todas las categorias para que el select de la vista se pueda mostrar correctamente
                    var categorias = await _categoriaService.ObtenerTodosAsync();
                    //asigno las categopria al viewModel para que se pueda mostrar en la vista(select)
                    productoVM.Categorias = new SelectList(categorias.Data, "Id", "Nombre");

                    //si no es valido devuelva la vista con los datos ingresados para que el usuario pueda corregirlos
                    return View(productoVM);
                }

                //MAPEO DE VIEWMODEL A DTO PARA ENVIARLO AL SERVICIO
                var productoCreateDTO = _mapper.Map<ProductoCreateDTO>(productoVM);

                //llamo service crear y le paso el dto mapeado desde el viewModel
                var response = await _productoService.CrearAsync(productoCreateDTO);


                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y devuelvo la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!response.Success)
                {
                    //cargoi todas las categorias para que el select de la vista se pueda mostrar correctamente
                    var categorias = await _categoriaService.ObtenerTodosAsync();
                    //asigno las categopria al viewModel para que se pueda mostrar en la vista(select)
                    productoVM.Categorias = new SelectList(categorias.Data, "Id", "Nombre");

                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(productoVM);
                }

                //si todo sale bien, guardo un mensaje de éxito en TempData
                //cuando hay otra peticion se pierde el mensaje
                TempData["SuccessMessage"] = "Producto creado exitosamente.";

                //una vez guardado el producto, redirijo a la vista Index para que se muestre la lista de productos actualizada
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                //si hay un error, devuelvo la vista para que el usuario pueda intentar crear el producto nuevamente
                return View();
            }
        }

        

        // GET: ProductoController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var product = await _productoService.ObtenerPorIdAsync(id);

                if (!product.Success)
                {
                    TempData["ErrorMessage"] = product.Message;
                    return RedirectToAction(nameof(Index));
                }

                //paso de responseDTO a ViewModel para poder mostrarlo en la vista y el
                //.Data es xq los datos vienen encapsulados en un objeto de tipo ResponseDTO,
                var productoEditVM = _mapper.Map<ProductoEditViewModel>(product.Data);

                //pasamos el producto ya mapeada(model) a la vista para mostrar en la interfaz de usuario
                return View(productoEditVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ProductoController/Edit/5
        //aqui se envia post xq para los form de html solo utiliza get y post, no put ni delete 
        [HttpPost]
        //este decorador es para vistas sirve para proteger la aplicación contra ataques de falsificación de solicitudes entre sitios (CSRF).
        //genera un tokken automatyico para evitar ataques
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ProductoEditViewModel productoEditVM)
        {
            try
            {

                //para validar que los datos ingresados en el formulario cumplan con las reglas de validación definidas en el modelo de vista (ViewModel).DataAnotations
                if (!ModelState.IsValid)
                {
                    //si no devuelva la vista con los datos ingresados para que el usuario pueda corregirlos
                    return View(productoEditVM);
                }

                //MAPEO DE VIEWMODEL A DTO PARA ENVIARLO AL SERVICIO
                var productoUpdateDTO = _mapper.Map<ProductoUpdateDTO>(productoEditVM);

                //llamo service crear y le paso el dto mapeado desde el viewModel
                var response = await _productoService.ActualizarAsync(productoUpdateDTO);


                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y devuelvo la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(productoEditVM);
                }

                //si todo sale bien, guardo un mensaje de éxito en TempData
                //cuando hay otra peticion se pierde el mensaje
                TempData["SuccessMessage"] = "Producto modificado exitosamente.";

                //una vez guardado el producto, redirijo a la vista Index para que se muestre la lista de productos actualizada
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        
        // GET: ProductoController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var response = await _productoService.ObtenerPorIdAsync(id);

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            //paso de ResponseDTO a ViewModel para poder mostrarlo en la vista y el
            //Data es xq los datos vienen encapsulados en un objeto de tipo ResponseDTO,
            var productoDeleteVM = _mapper.Map<ProductoIndexViewModel>(response.Data);

            return View(productoDeleteVM);
        }

        // POST: ProductoController/Delete/5
        //post xq los form de html solo utiliza get y post, no put ni delete
        [HttpPost]
        //este decorador es para vistas sirve para proteger la aplicación contra ataques de falsificación de solicitudes entre sitios (CSRF).
        //genera un tokken automatyico para evitar ataques
        [ValidateAntiForgeryToken]
        //el nombre de los metodos delete son diferentes xq ambos tienen la misma firma i parametros, hay q diferenciarlos
        public async Task<ActionResult> DeleteConfirmedAsync(int id)
        {
            try
            {
                //llamo service eliminar y le paso el id del producto a eliminar
                var response = await _productoService.EliminarAsync(id);

                //si el servicio devuelve un error, agrego un mensaje de error al TempData
                //y redirijo a la vista Index para que se muestre la lista de productos actualizada
                if (!response.Success)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                //si todo sale bien, guardo un mensaje de éxito en TempData
                //cuando hay otra peticion se pierde el mensaje
                TempData["SuccessMessage"] = "Producto eliminado exitosamente.";

                //redirecciono a la vista Index para que se muestre la lista de productos actualizada
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
                
        }

        

    }
}
