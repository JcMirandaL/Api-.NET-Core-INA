using AutoMapper;
using InaApp.DTOs.CategoriaDTOs;
using InaApp.DTOs.ClienteDTOs;
using InaApp.DTOs.Producto;
using InaApp.Entities;


namespace InaApp.Services.Mapping
{
    //hereda de profile que es la clase base de AutoMapper para definir las configuraciones de mapeo entre objetos
    public class MappingProfile : Profile
    {
        //constructor para coinfuguracion de mapeos
        public MappingProfile()
        {
            //DTO CREATE A ENTITY
            CreateMap<ProductoCreateDTO, Producto>();
            CreateMap<ClienteCreateDTO, Cliente>();
            CreateMap<CategoriaCreateDTO, Categoria>();


            //DTO UPDATE A ENTITY
            CreateMap<ProductoUpdateDTO, Producto>();
            CreateMap<ClienteUpdateDTO, Cliente>();
            CreateMap<CategoriaUpdateDTO, Categoria>();


            //ENTITY A DTO RESPONSE
            CreateMap<Producto, ProductoResponseDTO>()
                //el formember se utiliza para mapeo de una propiedad específica, en este caso
                //se mapea la propiedad CategoriaNombre del DTO ProductoResponseDTO con el nombre de la
                //categoria que esta dentro de la entidad producto, xq en el getById y GetAll no da problemas
                //pero en create y update si manda CategoriaNombre null xq en el repo no tienen el include de categoria
                .ForMember(destiny => destiny.CategoriaNombre, //expresion lambda
                options => options.MapFrom(origin => origin.Categoria.Nombre));
            CreateMap<Cliente, ClienteResponseDTO>();
            CreateMap<Categoria, CategoriaResponseDTO>();
        }


    }
}
