using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Exceptions
{
    public class NotFoundDbException : Exception
    {
        //constructor para poder pasarle el mensaje de la excepcion
        //el base es para llamar al constructor de la clase padre Exception
        public NotFoundDbException() : base("La entidad NO existe en la base de datos") { }

        //constructor que recibe un mensaje personalizado, viene de la clase padre Exception
        //y se le pasa el msj x parametro 
        public NotFoundDbException(string message) : base(message) { }
    }
}
