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
            CreateMap<Producto, ProductoResponseDTO>();
            CreateMap<Cliente, ClienteResponseDTO>();
            CreateMap<Categoria, CategoriaResponseDTO>();
        }


    }
}
