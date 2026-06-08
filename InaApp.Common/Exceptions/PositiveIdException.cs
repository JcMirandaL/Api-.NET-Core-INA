using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Common.Exceptions
{
    public class PositiveIdException : Exception
    {
        public PositiveIdException() : base("El id ingresado debe ser positivo") { }

        //constructor que recibe un mensaje personalizado, viene de la clase padre Exception
        public PositiveIdException(string message) : base(message) { }
    }
}
