using InaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//la capa data es para manejar todo lo relacionado con la base de datos, como las entidades,
//los repositorios, el contexto de la base de datos, etc
namespace InaApp.Data
{
    //hereda de la clase DbContext que es la clase base para trabajar con Entity Framework Core, proporciona metodos
    //y propiedades para manejar la conexion a la base de datos, las entidades, etc
    public class ApplicationDbContex : DbContext
    {

        //este es el constructor de la clase, se usa xq es propio del entity framework core,
        //y se le pasa el parametro options que es de tipo DbContextOptions<ApplicationDbContex>
        //que es una clase que contiene la configuracion para la conexion a la base de datos,
        //como el proveedor de la base de datos, la cadena de conexion, etc
        public ApplicationDbContex(DbContextOptions<ApplicationDbContex> options) : base(options)
        {
        }


        //entidades que va a migrar a tabals de base datos, cada DbSet representa una tabla en la base de datos,
        //el nombre del DbSet es el nombre de la tabla y el tipo de dato es la entidad que se va a mapear a esa tabla
        public DbSet<Producto> Productos { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
