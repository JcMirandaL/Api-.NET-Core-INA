using InaApp.Common.Interfaces;
using InaApp.Data;
using InaApp.DTOs.CategoriaDTOs;
using InaApp.DTOs.ClienteDTOs;
using InaApp.DTOs.Producto;
using InaApp.ProyectoInaApp.Mapping;
using InaApp.Repository;
using InaApp.Services;
using InaApp.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace InaApp.ProyectoInaApp.Extensions
{
    public static class DependecyInjection
    {
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


            //refrencia al autoMapper(injeccion de dependencia)
            services.AddAutoMapper(fg => { }, typeof(MappingProfile), typeof(WebMappingProfile));


            //inyecciones de dependencia de services
            //defino las inyeccion de dependencias
            services.AddScoped<IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>, ProductoService>();
            services.AddScoped<IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>, ClienteService>();
            services.AddScoped<IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO>, CategoriaSerrvice>();



            //inyecciones de dependencia de repository
            //noi uso l Igeneric xq en el service no la uso en el const4ructor
            //xq el repo de producto tiene metodos propios, entonces no puedo usar el tipado de IGenericRepository
            services.AddScoped<ProductoRepository>();
            services.AddScoped<ClienteRepository>();
            services.AddScoped<CategoriaRepository>();


            return services;
        }
    }
}
