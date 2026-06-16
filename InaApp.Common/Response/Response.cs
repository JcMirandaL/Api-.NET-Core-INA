using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Response
{
    //esta clase es para estandarizar las respuestas de los servicios al cliente, para que todas las respuestas
    //tengan la misma estructura, y se pueda manejar de forma uniforme, como por ejemplo en el controller,
    //se puede retornar una respuesta con el mismo formato, y el cliente siempre sabra que esperar ese formato,
    //y no tendra que manejar diferentes formatos de respuesta para cada servicio
    public class Response <T>
    {
        public string Message { get; set; } = string.Empty;
        
        //el ? es para indicar que el tipo de dato puede ser nulo, xq en algunos casos no se va a devolver data,
        //como por ejemplo en el metodo eliminar, donde solo se devuelve un booleano
        public T? Data { get; set; }


        public bool Success { get; set; }
    }
}

