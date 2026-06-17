using AutoMapper;
using InaApp.Common.Interfaces;
using InaApp.Common.Response;
using InaApp.Repository;
using InaApp.DTOs.CategoriaDTOs;
using InaApp.Common.Exceptions;
using InaApp.Entities;

namespace InaApp.Services
{
    public class CategoriaSerrvice : IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO>
    {
        private readonly CategoriaRepository _categoriaRepository;
        //variable para inyectar mapper
        private readonly IMapper _mapper;

        public CategoriaSerrvice(CategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }




        public async Task<Response<CategoriaResponseDTO>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotNumberPositiveException($"El Id de la categoría debe ser mayor a cero. Id Ingresado: {id}.");
            }

            var categoria = await _categoriaRepository.ObtenerPorIdAsync(id);
            if (categoria == null)
            {
                throw new NotFoundDbException($"La categoría con el Id '{id}' no existe.");
            }

            return new Response<CategoriaResponseDTO>
            {
                Message = "Categoría obtenida exitosamente: ",
                Data = _mapper.Map<CategoriaResponseDTO>(categoria),
                Success = true
            };
        }
        
        
        public async Task<Response<List<CategoriaResponseDTO>>> ObtenerTodosAsync()
        {
            var listaCategorias = await _categoriaRepository.ObtenerTodosAsync();
            if (listaCategorias is null || listaCategorias.Count == 0)
            {
                throw new NotFoundDbException("No se encontraron categorías en la base de datos.");
            }


            return new Response<List<CategoriaResponseDTO>>
            {
                Message = "Categorías obtenidas exitosamente: ",
                Data = _mapper.Map<List<CategoriaResponseDTO>>(listaCategorias),
                Success = true
            };
        }


        public async Task<Response<CategoriaResponseDTO>> CrearAsync(CategoriaCreateDTO entity)
        {
            var categoriaExistente = await _categoriaRepository.ObtenerPorNombreAsync(entity.Nombre);
            if (categoriaExistente != null)
            {
                throw new EntityExistDbException($"Ya existe una categoría con el nombre '{entity.Nombre}'.");
            }

            Categoria nuevaCategoria = _mapper.Map<Categoria>(entity);

            nuevaCategoria = await _categoriaRepository.CrearAsync(nuevaCategoria);

            return new Response<CategoriaResponseDTO>
            {
                Message = "Categoría creada exitosamente: ",
                Data = _mapper.Map<CategoriaResponseDTO>(nuevaCategoria),
                Success = true
            };
        }

        public async Task<Response<CategoriaResponseDTO>> ActualizarAsync(CategoriaUpdateDTO entity)
        {
            if (entity.Id <= 0)
            {
                throw new NotNumberPositiveException($"El Id de la categoría debe ser mayor a cero. Id Ingresado: {entity.Id}.");
            }

            var categoriaExistente = await _categoriaRepository.ObtenerPorIdAsync(entity.Id);
            if (categoriaExistente == null)
            {
                throw new NotFoundDbException($"La categoría con el Id '{entity.Id}' no existe.");
            }

            var nombreExistente = await _categoriaRepository.ObtenerPorNombreAsync(entity.Nombre);
            if (nombreExistente != null && nombreExistente.Id != entity.Id)
            {
                throw new EntityExistDbException($"Ya existe una categoría con el nombre '{entity.Nombre}'.");
            }

            Categoria categoriaActualizar = _mapper.Map<Categoria>(entity);

            categoriaActualizar = await _categoriaRepository.ActualizarAsync(categoriaActualizar);

            return new Response<CategoriaResponseDTO>
            {
                Message = "Categoría actualizada exitosamente: ",
                Data = _mapper.Map<CategoriaResponseDTO>(categoriaActualizar),
                Success = true
            };
        }


        public async Task<Response<bool>> EliminarAsync(int id)
        {

            if (id <= 0)
            {
                throw new NotNumberPositiveException("El id debe ser un número positivo.");
            }

            var categoria = _categoriaRepository.ObtenerPorIdAsync(id).Result;
            if (categoria == null)
            {
                throw new NotFoundDbException($"La categoría con id {id} no existe.");
            }

            categoria.Estado = false;

            await _categoriaRepository.ActualizarAsync(categoria);

            return new Response<bool>
            {
                Message = "Categoría eliminada exitosamente.",
                Data = true,
                Success = true
            };
        }
    

    }
}
