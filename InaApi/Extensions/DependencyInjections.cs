using InaApp.Common.Interfaces;
using InaApp.Data;
using InaApp.Entities;
using InaApp.Repository;
using InaApp.Services;
using Microsoft.EntityFrameworkCore;

namespace InaApp.Api.Extensions
{
    public static class DependencyInjections
    {
        //metod statico xq se va a llamar sin necesidad de crear una instancia de la clase, y devuelve un IServiceCollection para
        //poder encadenar las llamadas a este metodo en el program.cs
        //los parametros son el IServiceCollection para poder agregar los servicios en forma de copleccion y el IConfiguration para poder acceder
        //a la configuracion de la aplicacion, como la cadena de conexion a la base de datos
        public static IServiceCollection AddAplicationServices(
            this IServiceCollection services, 
            IConfiguration configuration
        )
        {
            //base datos dbContext
            //el applicationDbContex es el contexto de la base de datos que se va a usar
            //para acceder a la base de datos, y se le pasa la configuracion del string de conexion
            services.AddDbContext<ApplicationDbContex>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });



            //inyecciones de dependencia de services
            //defino las inyeccion de dependencias
            services.AddScoped<IGenericService<Producto>, ProductoService>();
            services.AddScoped<IGenericService<Cliente>, ClienteService>();



            //inyecciones de dependencia de repository
            services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();


            return services;
        }
    }
}
