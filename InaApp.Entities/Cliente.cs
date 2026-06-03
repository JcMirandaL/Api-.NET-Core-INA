using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaApp.Entities
{
    public class Cliente
    {

        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }

        public DateOnly FechaNacimiento { get; set; }

        public bool Estado { get; set; } = false;

    }
}
