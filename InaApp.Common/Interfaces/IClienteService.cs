using InaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Interfaces
{
    public interface IClienteService
    {
        //definir los metodos que se van a implementar en la clase ProductoService
        //metodo para obtener todos los productos task(asincrono) xq debe esperar la respuesta de la base datos y list xq devuelve una lista
        Task<List<Cliente>> ObtenerTodosAsync();

        //Cliente xq es el objeto en cuestion y task(asincrono) xq debe esperar la respuesta de la base datos, el Async solo para q se sepa q ese metod es asincrono
        Task<Cliente> ObtenerPorIdAsync(int id);

        Task<Cliente> CrearAsync(Cliente cliente); 

        Task<Cliente> ActualizarAsync(Cliente cliente);

        //este en vez de Cliente va bool para que debuelva un boleano si se elimino o no el producto
        Task<bool> EliminarAsync(int id);
    }

}
