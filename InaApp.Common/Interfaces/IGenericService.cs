using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Interfaces
{
    //parametrizar la interfaz para puede ser usada x cualquier entidad
    public interface IGenericService<E>
    {
        //definir los metodos que se van a implementar en las clases q usen esta I generica
        //metodo para obtener todos los productos task(asincrono) xq debe esperar la respuesta de la base datos y list xq devuelve una lista
        Task<List<E>> ObtenerTodosAsync();

        //E xq es el nombre q le di al objeto x en cuestion y task(asincrono) xq debe esperar la respuesta
        //de la base datos, el Async solo para q se sepa q ese metod es asincrono
        Task<E> ObtenerPorIdAsync(int id);

        Task<E> CrearAsync(E entity);

        Task<E> ActualizarAsync(E entity);

        //este en vez de E va bool para que debuelva un boleano si se elimino o no el producto
        Task<bool> EliminarAsync(int id);
    }
}
