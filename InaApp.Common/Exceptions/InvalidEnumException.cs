using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Exceptions
{
    public class InvalidEnumException : Exception
    {
        //constructor por defecto que llama al constructor de la clase padre Exception con el base, y se le pone msj predeterminado,
        public InvalidEnumException() : base("El tipo de cedula es incorrecto.") { }

        //este constructor permite pasar un mensaje personalizado al lanzar la excepcion, se le pasa el mensaje por parametro y
        //se llama al constructor de la clase padre Exception con el base       
        public InvalidEnumException(string message) : base(message) { }
    }
}
