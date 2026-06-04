using InaApp.Common.Interfaces;
using InaApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.Api.Controllers
{
    //la clase controller es la clase base para todos los controladores en ASP.NET Core MVC, proporciona metodos y propiedades
    //para manejar las solicitudes HTTP y generar respuestas HTTP

    [ApiController]//indica que esta clase es un controlador de API, lo que significa que se espera que maneje solicitudes HTTP y genere respuestas HTTP
    //cuando pongo [controller] toma solo el nombre del controller en minuscula en este caso producto omitiendo la parte de Controller del nombre. ruta= https://localhost:5001/api/producto
    [Route("api/[controller]")]//define la ruta pricipal para las solicitudes HTTP, htps://localhost:5001/api/producto
    public class ProductoController : Controller//hereda de la clase Controller para poder usar sus metodos y propiedades
    {

        //variable para guardar esa instancia, se tipa de tipo interface y el nombre con _ para indicar que es una variable privada
        private readonly IGenericService<Producto> _productoService;//solo de lectura xq no se va a modidficar


        //inyecto al service en el constructor, el constructor es un metodo especial que se ejecuta cuando se crea una instancia de la clase,
        //se usa para inicializar las variables o propiedades de la clase
        //para inyectar hay que definir la inyaccion en el program.cs, en este caso se inyecta el service con su interface para que se pueda usar en el controlador, y se le asigna la instancia que me llega por el parametro del constructor a la variable privada para poder usarla en los metodos del controlador
        public ProductoController(IGenericService<Producto> productoService)
        {
            //asigno la instancia que me llego por el param, detro del constructor a la variable privada para poder usarla en los metodos del controlador
            _productoService = productoService;
        }




        // CRUD BASICO 
        [HttpGet]//decorador para decir que la ruta es de un get
        //el ActionResult es para indicar que devuelce un status code 
        public ActionResult Index()//index es obtener todos
        {
            //llamo al metodo del service
            _productoService.ObtenerTodosAsync();

            //200 = ok
            return StatusCode(200, "correcto, real");
        }


        [HttpGet( "{id}" )]
        // GET: htps://localhost:5001/api/producto/5
        public ActionResult Details(int id)//details es obtener por id o x detalle ese parametro de la ruta debe llamrse igaul al parametro del metodo
        {
            return Ok("hola"); 
        }


        // GET: ProductoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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


        // GET: ProductoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductoController/Edit/5
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

        
        // GET: ProductoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
