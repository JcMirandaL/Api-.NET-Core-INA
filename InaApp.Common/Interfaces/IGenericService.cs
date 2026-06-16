
using InaApp.Common.Response;

namespace InaApp.Common.Interfaces
{
    //parametrizar la interfaz para puede ser usada x cualquier entidad
    //puede usar los DTOs de response, create o ,update, x eso se le pasan 3 parametros genericos
    public interface IGenericService<TResponse, TCreate, TUpdate>
    {
        //definir los metodos que se van a implementar en las clases q usen esta I generica
        //metodo para obtener todos los productos task(asincrono) xq debe esperar la respuesta de la base datos y list xq devuelve una lista
        Task <Response<List<TResponse>>> ObtenerTodosAsync();

        //E xq es el nombre q le di al objeto x en cuestion y task(asincrono) xq debe esperar la respuesta
        //de la base datos, el Async solo para q se sepa q ese metod es asincrono
        Task<Response<TResponse>> ObtenerPorIdAsync(int id);

        Task<Response<TResponse>> CrearAsync(TCreate entity);

        Task<Response<TResponse>> ActualizarAsync(TUpdate entity);

        //este en vez de E va bool para que debuelva un boleano si se elimino o no el producto
        Task<Response<bool>> EliminarAsync(int id);
    }
}
