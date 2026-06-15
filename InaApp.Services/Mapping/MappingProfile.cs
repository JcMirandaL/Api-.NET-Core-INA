using AutoMapper;
using InaApp.DTOs.ClienteDTOs;
using InaApp.DTOs.Producto;
using InaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            //DTO UPDATE A ENTITY
            CreateMap<ProductoUpdateDTO, Producto>();
            CreateMap<ClienteUpdateDTO, Cliente>();
            

            //ENTITY A DTO RESPONSE
            CreateMap<Producto, ProductoResponseDTO>();
            CreateMap<Cliente, ClienteResponseDTO>();
        }


    }
}
