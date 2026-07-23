using InaApp.Entities;
using Microsoft.EntityFrameworkCore;



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

        public DbSet<Categoria> Categorias { get; set; }
         
        public DbSet<Factura> Factura { get; set; }

        public DbSet<DetalleFactura> DetalleFactura { get; set; }



        //el fluet api es una forma de configurar el modelo de datos utilizando el metodo OnModelCreating,
        //que se ejecuta cuando se crea el modelo de datos, y permite configurar las entidades, las relaciones, las restricciones(check, unique, constraint, etc)
        //aquello que no se puede definir con el dataAnnotations se puede configurar con el fluent api, por ejemplo
        //la configuracion de un indice unico en el campo 'Nombre' de la entidad Producto, o FK, etc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //llama al metodo base para que se ejecute la configuracion por defecto del modelo de datos,
            base.OnModelCreating(modelBuilder);


            //---------------------------PRODUCTO/CATEGORIA-----------------------------//
            //relacion de 1 : N, 1 categoria : muchos productos
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)//1 producto tiene 1 categoria
                .WithMany(c => c.Productos)//1 categoria tiene muchos productos
                .HasForeignKey(p => p.CategoriaId);//la FK es el campo CategoriaId de la entidad Producto


            //---------------------------FACTURA/CLIENTE-----------------------------//
            //relacion de 1 : N, 1 cliente muchas facturas
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)//1 factura tiene 1 cliente
                .WithMany(c => c.Facturas)//1 cliente tiene muchass facturas
                .HasForeignKey(f => f.ClienteId);//FK


            //-------------------DETALLES FACTURA/PRODUCTO Y FACTURTA-----------------------------//
            //PK COMPUESTA
            modelBuilder.Entity<DetalleFactura>()
                .HasKey(d => new { d.FacturaId, d.ProductoId });//Pk COMPUESTA

            //relacion de 1 : N, 1 factura muchos detalles
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(d => d.Factura)
                .WithMany(f => f.Detalles)
                .HasForeignKey(d => d.FacturaId);

            //relacion de 1 : N, 1 producto muchos detalles
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.ProductoId);

        }

    }
}
