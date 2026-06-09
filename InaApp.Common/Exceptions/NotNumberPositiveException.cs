using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Exceptions
{
    //hereda de la clase Exception que es la clase base para todas las excepciones en c#, se usa para crear una excepcion personalizada que se lanza cuando el id ingresado es menor o igual a 0
    public class NotNumberPositiveException : Exception
    {
        //constructor por defecto que llama al constructor de la clase padre Exception con el base, y se le pone msj predeterminado,
        public NotNumberPositiveException() : base("El id ingresado debe ser positivo") { }

        //este constructor permite pasar un mensaje personalizado al lanzar la excepcion, se le pasa el mensaje por parametro y
        //se llama al constructor de la clase padre Exception con el base       
        public NotNumberPositiveException(string message) : base(message) { }
    }
}
