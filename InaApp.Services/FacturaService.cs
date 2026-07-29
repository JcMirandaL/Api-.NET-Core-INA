using AutoMapper;
using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.Common.Response;
using InaApp.Data;
using InaApp.DTOs.FacturaDTOs;
using InaApp.Entities;
using InaApp.Repository;

namespace InaApp.Services
{
    public class FacturaService : IGenericService<FacturaResponseDTO, FacturaCreateDTO, FacturaUpdateDTO>
    {
        //variables para el repo y mapper de inyeccion
        private readonly FacturaRepository _facturaRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly ProductoRepository _productoRepository;
        private readonly IMapper _mapper;

        //constructor para inicializar variables de inyeccion
        public FacturaService(
            FacturaRepository facturaRepository, 
            ClienteRepository clienteRepository,
            ProductoRepository productoRepository,
            IMapper mapper)
        {
            _facturaRepository = facturaRepository;
            _clienteRepository = clienteRepository;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }




        //CRUD
        public async Task<Response<FacturaResponseDTO>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
            {
                //msj perzonalizado 
                throw new NotNumberPositiveException($"El Id del producto debe ser mayor a cero. Id Ingresado: {id}.");
            }

            var factura = await _facturaRepository.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                throw new NotFoundDbException($"El Id {id} de la factura no existe o esta inactivo");
            }

            //paso de entity a DTO mapoeado a la lista
            var facturaDTO = _mapper.Map<FacturaResponseDTO>(factura);

            // Calcular el subtotal de cada linea de detalle
            foreach (var f in facturaDTO.Detalles)
            {
                f.Subtotal = f.Cantidad * f.Precio;
            }

            // Calcular subtotal general de la factura sumando los subtotales de cada detalle
            facturaDTO.Subtotal = facturaDTO.Detalles.Sum(x => x.Subtotal);

            // Calcular total
            facturaDTO.Total = facturaDTO.Subtotal - (facturaDTO.Subtotal * facturaDTO.descuento / 100);

            return new Response<FacturaResponseDTO>
            {
                Message = "Factura encontrada exitosamente: ",
                Data = facturaDTO,
                Success = true
            };
        }


        public async Task<Response<List<FacturaResponseDTO>>> ObtenerTodosAsync()
        {
            var listaFacturas = await _facturaRepository.ObtenerTodosAsync();
            if (listaFacturas == null || listaFacturas.Count == 0)
            {
                throw new NotFoundDbException("No se encontraron facturas activas en la base de datos.");
            }

            //paso de entity a DTO mapoeado a la lista
            var listaFacturasDTO = _mapper.Map<List<FacturaResponseDTO>>(listaFacturas);

            foreach (var factura in listaFacturasDTO)
            {
                factura.Subtotal = factura.Detalles.Sum(d => d.Cantidad * d.Precio);
                factura.Total = factura.Subtotal - (factura.Subtotal * factura.descuento / 100);
            }

            return new Response<List<FacturaResponseDTO>>
            {
                Message = "Clientes obtenidos exitosamente.",
                Data = listaFacturasDTO,
                Success = true
            };
        }


        public async Task<Response<FacturaResponseDTO>> CrearAsync(FacturaCreateDTO entity)
        {
            //que venga al menos un detalle de factura
            if (entity.Detalles == null || entity.Detalles.Count == 0)
            {
                throw new NotFoundDbException("La Factura debe tener almenos un detalle de Factura.");
            }

            var clienteExist = await _clienteRepository.ObtenerPorIdAsync(entity.ClienteId);
            if (clienteExist == null)
            {
                throw new NotFoundDbException($"No se puede crear facturas con un cliente inexistente. Cliente ingresado {entity.ClienteId}.");
            }


            //paso de DTO a entity para luego guardar la entidad en DB
            Factura nuevaFactura = _mapper.Map<Factura>(entity);
            //limpio los detalles xq se van a llenar en el foreach y asi no se duplican los detalles de la factura al crearla
            nuevaFactura.Detalles.Clear();

            //inicializo la lista de detalles de la factura
            foreach (var detalle in entity.Detalles)
            {
                //metodo obtener po id solo para eso xq este no tiene asnotrackin y asi puede trackear el producto
                //y con el saveChanges de factura darle seguimiento y actualizar el stock
                var productoExist = await _productoRepository.ObtenerPorIdUpdateStockAsync(detalle.ProductoId);
                if (productoExist == null)
                {
                    throw new NotFoundDbException($"No se puede crear facturas con un producto inexistente. Producto ingresado {detalle.ProductoId}.");
                }

                if (productoExist.Stock < detalle.Cantidad)
                {
                    throw new InsufficientStockException($"No hay suficiente stock del producto {productoExist.Nombre}. Stock disponible: {productoExist.Stock}, Cantidad solicitada: {detalle.Cantidad}.");
                }

                //creo el detalle de la factura 
                var detalleFactura = new DetalleFactura
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    Precio = productoExist.Precio
                };


                //agrego el detalle a la factura
                nuevaFactura.Detalles.Add(detalleFactura);

                //actualizo el stock 
                productoExist.Stock -= detalle.Cantidad;
            }


            //campos calculados solo eicsten en el dto
            var subtotal = nuevaFactura.Detalles.Sum(d => d.Cantidad * d.Precio);
            if (entity.descuento > subtotal)
            {
                throw new DiscountOutRange("El descuento no puede superar el Subtotal.");
            }

            var total = subtotal - (subtotal * nuevaFactura.descuento / 100);
            if (total < 0)
            {
                throw new TotalOutRange("El total no puede ser negativo.");
            }

            //crea la factura, sus detalles y actua productos un  solo saveChanges maneja transaccion internamente
            await _facturaRepository.CrearAsync(nuevaFactura);

            //actualizo cliente de la factura con el cliente existente
            //esto para que cargue el nombrte del cliente en el DTO de respuesta y no usar include en el repo(create-actualizar)
            nuevaFactura.Cliente = clienteExist;
            //Paso de entity a DTO para la respuesta
            var responseDTO = _mapper.Map<FacturaResponseDTO>(nuevaFactura);
            responseDTO.Subtotal = subtotal;
            responseDTO.Total = total;

            //actualizo el subtotal de cada detalle de la factura en el DTO de respuesta
            foreach (var detalle in responseDTO.Detalles)
            {
                detalle.Subtotal = detalle.Cantidad * detalle.Precio;
            }

            return new Response<FacturaResponseDTO>
            {
                Message = "Factura creada exitosamente.",
                Data = responseDTO,
                Success = true
            };
       
        }


        public async Task<Response<bool>> EliminarAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotNumberPositiveException($"El Id '{id}' de la factura debe ser un número positivo.");
            }

            var facturaExistente = await _facturaRepository.ObtenerPorIdAsync(id);
            if (facturaExistente == null)
            {
                throw new NotFoundDbException($"El cliente con Id '{id}' no existe o esta inactivo en la base de datos.");
            }

            foreach (var detalle in facturaExistente.Detalles)
            {
               
                detalle.Producto.Stock += detalle.Cantidad; // Devolver el stock del producto
            }

            facturaExistente.Estado = false;

            await _facturaRepository.ActualizarAsync(facturaExistente);

            return new Response<bool>
            {
                Message = "Factura anulada exitosamente.",
                Data = true,
                Success = true
            };
        }


        
        //se implementa para cumplir con interfaz pero no se usa
        public Task<Response<FacturaResponseDTO>> ActualizarAsync(FacturaUpdateDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
