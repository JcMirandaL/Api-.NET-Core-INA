using AutoMapper;
using InaApp.Common.Exceptions;
using InaApp.Common.Interfaces;
using InaApp.Common.Response;
using InaApp.DTOs.FacturaDTOs;
using InaApp.Entities;
using InaApp.Repository;
using static InaApp.Common.Enums.Enumeradores;

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

            // Calcular los campos calculados de cada linea de detalle
            foreach (var f in facturaDTO.Detalles)
            {
                f.Subtotal = f.Cantidad * f.Precio;
                f.MontoImpuesto = (f.Subtotal * f.PorcentajeImpuesto) / 100m;
                f.DescuentoMonto = (f.Subtotal * f.DescuentoAplicado) / 100m;
                f.TotalLinea = (f.Subtotal + f.MontoImpuesto) - f.DescuentoMonto;
            }

            // Calcular subtotal general de la factura sumando los subtotales de cada detalle
            facturaDTO.Subtotal = facturaDTO.Detalles.Sum(x => x.Subtotal);

            // Calcular total de impuestos sumando el impuesto de cada linea
            facturaDTO.TotalImpuestos = facturaDTO.Detalles.Sum(x => x.MontoImpuesto);

            // Calcular descuento total sumando el descuento de cada linea
            facturaDTO.DescuentoTotal = facturaDTO.Detalles.Sum(x => x.DescuentoMonto);

            // Calcular total final: subtotal + impuestos - descuento
            facturaDTO.Total = facturaDTO.Subtotal + facturaDTO.TotalImpuestos - facturaDTO.DescuentoTotal;

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
                factura.TotalImpuestos = factura.Detalles.Sum(d => d.Cantidad * d.Precio * d.PorcentajeImpuesto / 100m);
                factura.DescuentoTotal = factura.Detalles.Sum(d => d.Cantidad * d.Precio * d.DescuentoAplicado / 100m);
                factura.Total = (factura.Subtotal + factura.TotalImpuestos) - factura.DescuentoTotal;
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

            //variables para acumular los totales de la factura
            decimal subtotalGeneral = 0;
            decimal totalImpuestos = 0;
            decimal totalDescuento = 0;

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

                //aplico cap: si el descuento ingresado supera el maximo del producto, se usa el maximo
                int descuentoFinal = detalle.DescuentoAplicado;
                if (descuentoFinal > productoExist.DescuentoMaximo)
                {
                    descuentoFinal = productoExist.DescuentoMaximo;
                }

                //calculo valores de la linea para ir acumulando los totales
                decimal subt = detalle.Cantidad * productoExist.Precio;
                subtotalGeneral += subt;
                totalImpuestos += (subt * productoExist.PorcentajeImpuesto) / 100m;
                totalDescuento += (subt * descuentoFinal) / 100m;

                //creo el detalle de la factura 
                var detalleFactura = new DetalleFactura
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    Precio = productoExist.Precio,
                    //guardo el descuento aplicado (ya con el cap aplicado)
                    DescuentoAplicado = descuentoFinal,
                    //asigno el producto navegable para que el mapper pueda acceder a PorcentajeImpuesto
                    Producto = productoExist
                };


                //agrego el detalle a la factura
                nuevaFactura.Detalles.Add(detalleFactura);

                //actualizo el stock 
                productoExist.Stock -= detalle.Cantidad;
            }


            //el descuento global se asigna a 0 (las nuevas facturas usan descuento por linea)
            nuevaFactura.descuento = 0;

            //crea la factura, sus detalles y actua productos un  solo saveChanges maneja transaccion internamente
            await _facturaRepository.CrearAsync(nuevaFactura);

            //actualizo cliente de la factura con el cliente existente
            //esto para que cargue el nombrte del cliente en el DTO de respuesta y no usar include en el repo(create-actualizar)
            nuevaFactura.Cliente = clienteExist;
            //Paso de entity a DTO para la respuesta
            var responseDTO = _mapper.Map<FacturaResponseDTO>(nuevaFactura);
            responseDTO.Subtotal = subtotalGeneral;
            responseDTO.TotalImpuestos = totalImpuestos;
            responseDTO.DescuentoTotal = totalDescuento;
            responseDTO.Total = subtotalGeneral + totalImpuestos - totalDescuento;

            //actualizo los campos calculados de cada detalle de la factura en el DTO de respuesta
            foreach (var detalle in responseDTO.Detalles)
            {
                detalle.Subtotal = detalle.Cantidad * detalle.Precio;
                detalle.MontoImpuesto = (detalle.Subtotal * detalle.PorcentajeImpuesto) / 100m;
                detalle.DescuentoMonto = (detalle.Subtotal * detalle.DescuentoAplicado) / 100m;
                detalle.TotalLinea = (detalle.Subtotal + detalle.MontoImpuesto) - detalle.DescuentoMonto;
            }

            return new Response<FacturaResponseDTO>
            {
                Message = "Factura creada exitosamente.",
                Data = responseDTO,
                Success = true
            };
       
        }



        public async Task<Response<FacturaResponseDTO>> CrearNotaCreditoAsync(FacturaCreateDTO entity)
        {
            if (entity.Detalles == null || entity.Detalles.Count == 0)
            {
                throw new NotFoundDbException("La Nota de Credito debe tener almenos un detalle de Factura.");
            }

            if (!entity.FacturaReferenciaId.HasValue)
            {
                throw new NotFoundDbException("La Nota de Credito debe tener un Id de factura de referencia.");
            }

            if (string.IsNullOrWhiteSpace(entity.MotivoNotaCredito))
            {
                throw new NotFoundDbException("La Nota de Credito debe tener un motivo.");
            }

            var clienteExist = await _clienteRepository.ObtenerPorIdAsync(entity.ClienteId);
            if (clienteExist == null)
            {
                throw new NotFoundDbException($"No se puede crear notas de credito con un cliente inexistente. Cliente ingresado {entity.ClienteId}.");
            }

            //.Value xq FacturaReferenciaId es nullable, pero ya se verifico que tenga valor
            var FacturaOriginal = await _facturaRepository.ObtenerPorIdAsync(entity.FacturaReferenciaId.Value);
            if (FacturaOriginal == null)
            {
                throw new NotFoundDbException($"No se puede crear notas de credito con una factura de referencia inexistente. Factura ingresada {entity.FacturaReferenciaId.Value}.");
            }

            if (FacturaOriginal.TipoDocumento != TipoDocumentoEnum.FacturaElectronica)
            {
                throw new NotFoundDbException($"No se puede crear notas de credito con una factura de referencia que no sea de tipo FacturaElectronica. Factura ingresada {entity.FacturaReferenciaId.Value}.");
            }


            Factura nuevaNotaCredito = _mapper.Map<Factura>(entity);
            nuevaNotaCredito.Detalles.Clear();
            nuevaNotaCredito.descuento = 0; // Descuento global en 0, ya que se usa descuento por línea
            decimal subtotalGeneral = 0;
            decimal totalImpuestos = 0;
            decimal totalDescuento = 0;

            foreach (var detalle in entity.Detalles)
            {
                var productoExist = await _productoRepository.ObtenerPorIdUpdateStockAsync(detalle.ProductoId);
                if (productoExist == null)
                {
                    throw new NotFoundDbException($"No se puede crear notas de credito con un producto inexistente. Producto ingresado {detalle.ProductoId}.");
                }
                

                var detalleOriginal = FacturaOriginal.Detalles.FirstOrDefault(d => d.ProductoId == detalle.ProductoId);
                if (detalleOriginal == null)
                {
                    throw new NotFoundDbException($"El producto con Id {detalle.ProductoId} no existe en la factura de referencia.");
                }

                if (detalle.Cantidad > detalleOriginal.Cantidad)
                {
                    throw new InsufficientStockException($"La cantidad de devolución para el producto {productoExist.Nombre} excede la cantidad original en la factura. Cantidad original: {detalleOriginal.Cantidad}, Cantidad solicitada: {detalle.Cantidad}.");
                }

                if (detalle.Cantidad < 0)
                {
                    throw new NotNumberPositiveException($"La cantidad de devolución para el producto {productoExist.Nombre} no puede ser menor a cero. Cantidad ingresada: {detalle.Cantidad}.");
                }

                if (detalle.Cantidad == 0)
                {
                    continue;// Si la cantidad es cero, no se realiza ninguna acción para este detalle se la salta
                }


                decimal precioOriginal = detalleOriginal.Precio;

                productoExist.Stock += detalle.Cantidad; // Devolver el stock del producto

                decimal subt = detalle.Cantidad * precioOriginal;
                subtotalGeneral += subt;
                totalImpuestos += (subt * productoExist.PorcentajeImpuesto) / 100m;
                int descuestoFinal = detalleOriginal.DescuentoAplicado;
                totalDescuento += (subt * descuestoFinal) / 100;

                var detalleNotaCredito = new DetalleFactura
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    Precio = precioOriginal,
                    DescuentoAplicado = descuestoFinal, 
                    Producto = productoExist
                };

                nuevaNotaCredito.Detalles.Add(detalleNotaCredito);
            }

            if (nuevaNotaCredito.Detalles.Count == 0)
            {
                throw new NotFoundDbException ($"La nota de crédito debe tener al menos un detalle con cantidad mayor a cero.");
            }

            await _facturaRepository.CrearAsync(nuevaNotaCredito);

            nuevaNotaCredito.Cliente = clienteExist;
            var responseDTO = _mapper.Map<FacturaResponseDTO>(nuevaNotaCredito);
            responseDTO.Subtotal = subtotalGeneral;
            responseDTO.TotalImpuestos = totalImpuestos;
            responseDTO.DescuentoTotal = totalDescuento;
            responseDTO.Total = (subtotalGeneral + totalImpuestos) - totalDescuento;

            foreach (var d in responseDTO.Detalles)
            {
                d.Subtotal = d.Cantidad * d.Precio;
                d.MontoImpuesto = (d.Subtotal * d.PorcentajeImpuesto) / 100m;
                d.DescuentoMonto = (d.Subtotal * d.DescuentoAplicado) / 100;
                d.TotalLinea = (d.Subtotal + d.MontoImpuesto) - d.DescuentoMonto;
            }

            return new Response<FacturaResponseDTO>
            {
                Message = "Nota de Crédito creada exitosamente.",
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
