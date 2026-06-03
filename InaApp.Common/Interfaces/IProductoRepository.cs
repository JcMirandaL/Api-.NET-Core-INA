using InaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Interfaces
{
    public interface IProductoRepository
    {
        //definir los metodos que se van a implementar en la clase ProductoService
        //metodo para obtener todos los productos task(asincrono) xq debe esperar la respuesta de la base datos y list xq devuelve una lista
        Task<List<Producto>> ObtenerTodosAsync();

        //Producto xq es el objeto en cuestion y task(asincrono) xq debe esperar la respuesta de la base datos, el Async solo para q se sepa q ese metod es asincrono
        Task<Producto> ObtenerPorIdAsync(int id);

        Task<Producto> CrearAsync(Producto producto);

        Task<Producto> ActualizarAsync(Producto producto);

        //este en vez de Producto va bool para que debuelva un boleano si se elimino o no el producto
        Task<bool> EliminarAsync(int id);
    }
}
