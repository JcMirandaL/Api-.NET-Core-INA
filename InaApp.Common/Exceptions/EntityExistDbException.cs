using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Exceptions
{
    public class EntityExistDbException : Exception
    {

        //constructor para poder pasarle el mensaje de la excepcion
        //el base es para llamar al constructor de la clase Exception
        public EntityExistDbException() : base("La entidad ya existe en la base de datos") { }

        //constructor que recibe un mensaje personalizado, viene de la clase padre Exception
        public EntityExistDbException(string message) : base(message) { }

    }
}
