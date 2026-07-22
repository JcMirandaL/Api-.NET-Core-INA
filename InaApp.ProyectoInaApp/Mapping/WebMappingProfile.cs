using AutoMapper;
using InaApp.DTOs.CategoriaDTOs;
using InaApp.DTOs.ClienteDTOs;
using InaApp.DTOs.Producto;
using InaApp.ProyectoInaApp.Models.Categoria;
using InaApp.ProyectoInaApp.Models.Cliente;
using InaApp.ProyectoInaApp.Models.Producto;

namespace InaApp.ProyectoInaApp.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            //DE DTO A VIEWMODEL
            //PRDUCTO
            CreateMap<ProductoResponseDTO, ProductoIndexViewModel>();
            CreateMap<ProductoResponseDTO, ProductoEditViewModel>();
            //CATEGORIA
            CreateMap<CategoriaResponseDTO, CategoriaIndexViewModel>();
            CreateMap<CategoriaResponseDTO, CategoriaEditViewModel>();
            //CLIENTE
            CreateMap<ClienteResponseDTO, ClienteIndexViewModel>();
            CreateMap<ClienteResponseDTO, ClienteEditViewModel>();


            //DE VIEW MODEL A DTO
            //PRDUCTO
            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
            CreateMap<ProductoCreateViewModel, ProductoCreateDTO>();
            CreateMap<ProductoEditViewModel, ProductoUpdateDTO>();
            //CATEGORIA
            CreateMap<CategoriaIndexViewModel, CategoriaResponseDTO>();
            CreateMap<CategoriaCreateViewModel, CategoriaCreateDTO>();
            CreateMap<CategoriaEditViewModel, CategoriaUpdateDTO>();
            //CLIENTE
            CreateMap<ClienteIndexViewModel, ClienteResponseDTO>();
            CreateMap<ClienteCreateViewModel, ClienteCreateDTO>();
            CreateMap<ClienteEditViewModel, ClienteUpdateDTO>();

        }

    }
}
