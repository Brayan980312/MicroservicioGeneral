namespace Domain.General.Services.General
{
    using AutoMapper;
    using Domain.General.CustomEntities.Compras;
    using Domain.General.CustomEntities.Params;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.External;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;
    using Utilitarios.Constants;
    using Utilitarios.Extensions;

    /// <summary>Implementación de reglas de negocio para el servicio de compras.</summary>
    public class ComprasService : IComprasService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Instancia del Mapeador - Imapper.</summary>
        private readonly IMapper _iMapper;

        /// <summary>Inyeccion del servicio IConfiguracionService.</summary>
        private readonly IConfiguracionService _iConfiguracionService;

        /// <summary>Inyeccion del servicio Vuelos.</summary>
        private readonly IVueloService _iVueloService;

        /// <summary>Inyeccion del servicio Redis Cache.</summary>
        private readonly ICacheService _iCacheService;
        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase VueloService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public ComprasService(IUnitOfWork iUnitOfWork, IMapper iMapper, IConfiguracionService iConfiguracionService, IVueloService iVueloService, ICacheService iCacheService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _iConfiguracionService = iConfiguracionService;
            _iVueloService = iVueloService;
            _iCacheService = iCacheService;
        }

        #endregion

        #region Metodos Implementados
        /// <inheritdoc />
        public async Task<IEnumerable<Compra>> GetWithParamsAsync(ParamsConsultarCompra paramsSearch, int? userId = null)
        {
            Compra entidad = _iMapper.Map<Compra>(paramsSearch);
            entidad.UsuarioId = userId;
            Expression<Func<Compra, bool>> filtro = entidad.ToFilterExpression<Compra>();
            return await _iUnitOfWork.Repository<Compra>().ConsultarListaAsync(filtro);
        }


        /// <inheritdoc />
        public async Task<IEnumerable<CompraHistorico>> GetWithParamsAsync(ParamsConsultarCompraHistorico paramsSearch, int? userId = null)
        {
            CompraHistorico entidad = _iMapper.Map<CompraHistorico>(paramsSearch);
            Expression<Func<CompraHistorico, bool>> filtro = entidad.ToFilterExpression<CompraHistorico>();
            return await _iUnitOfWork.Repository<CompraHistorico>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CompraDetalle>> GetWithParamsAsync(ParamsConsultarCompraDetalle paramsSearch, int? userId = null)
        {
            CompraDetalle entidad = _iMapper.Map<CompraDetalle>(paramsSearch);
            Expression<Func<CompraDetalle, bool>> filtro = entidad.ToFilterExpression<CompraDetalle>();
            return await _iUnitOfWork.Repository<CompraDetalle>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CompraDetalleHistorico>> GetWithParamsAsync(ParamsConsultarCompraDetalleHistorico paramsSearch, int? userId = null)
        {
            CompraDetalleHistorico entidad = _iMapper.Map<CompraDetalleHistorico>(paramsSearch);
            Expression<Func<CompraDetalleHistorico, bool>> filtro = entidad.ToFilterExpression<CompraDetalleHistorico>();
            return await _iUnitOfWork.Repository<CompraDetalleHistorico>().ConsultarListaAsync(filtro);
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<VueloAsiento>> ReservarAsientosAsync(ParamsReservarAsientos paramsReservar)
        {
            
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarReservaAsientosAsync(paramsReservar);
                var listaIds = paramsReservar.ListaAsientoVuelo.Select(a => a.VueloAsientoId).ToList();

                VueloAsiento filtroAsiento = new VueloAsiento { VueloId = paramsReservar.VueloId };
                Expression<Func<VueloAsiento, bool>> filtro = filtroAsiento.ToFilterExpression<VueloAsiento>();
                IEnumerable<VueloAsiento> asientos = await _iUnitOfWork.Repository<VueloAsiento>().ConsultarListaAsync(filtro);

                // Obtiene la cantidad de segundos que estará bloqueado para la reserva de asientos y la compra de los mismos
                int cantidadSegundosReservaAsientos = 0;
                IEnumerable<Parametros> TiempoCompraVuelo;
                ParamsConsultarParametros parametrosBusqueda = new ParamsConsultarParametros();
                parametrosBusqueda.ParametrosNombre = "TiempoCompraVuelo";
                TiempoCompraVuelo = await _iConfiguracionService.GetWithParamsAsync(parametrosBusqueda);                

                if (TiempoCompraVuelo.Count() > 0)
                {
                    cantidadSegundosReservaAsientos = Convert.ToInt32(TiempoCompraVuelo.FirstOrDefault()!.ParametrosValor);
                }

                var ahora = DateTime.Now;
                var bloqueados = new List<VueloAsiento>();

                foreach (var asiento in asientos.Where(a => listaIds.Contains((int)a.VueloAsientoId)))
                {
                    asiento.VueloAsientoBloqueadoHasta = ahora.AddSeconds(cantidadSegundosReservaAsientos);
                    await _iUnitOfWork.Repository<VueloAsiento>().ActualizarAsync(asiento);
                    bloqueados.Add(asiento);
                }

                await _iUnitOfWork.SaveChangesAsync();
                scope.Complete();
                return bloqueados;
            }
        }

        public async Task<Compra> ComprarAsientosAsync(ParamsCompraAsientos paramsCompra, int userId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCompraAsientosAsync(paramsCompra);
                var listaIds = paramsCompra.DetalleAsientos.Select(a => a.VueloAsientoId).ToList();

                // Consultar los asientos del vuelo
                VueloAsiento filtroAsiento = new() { VueloId = paramsCompra.VueloId };
                Expression<Func<VueloAsiento, bool>> filtro = filtroAsiento.ToFilterExpression<VueloAsiento>();
                IEnumerable<VueloAsiento> asientosVuelo = await _iUnitOfWork.Repository<VueloAsiento>().ConsultarListaAsync(filtro);

                var asientosAComprar = asientosVuelo.Where(a => listaIds.Contains((int)a.VueloAsientoId)).ToList();

                // Crear la compra principal
                Compra nuevaCompra = new Compra
                {
                    UsuarioId = userId,
                    VueloId = paramsCompra.VueloId,
                    EstadoCompraId = (int)Domain.General.Enums.Enum.EstadoCompra.Comprado,
                    MetodoPagoId = (int)Domain.General.Enums.Enum.MetodoPago.Creditos,
                    CompraFecha = DateTime.Now,
                    CompraTotal = paramsCompra.CompraTotal
                };

                await _iUnitOfWork.Repository<Compra>().AdicionarAsync(nuevaCompra);
                await _iUnitOfWork.SaveChangesAsync();

                // Genera el historico de la compra
                await CreateCompraHistorico(nuevaCompra);

                // Actualizar los asientos comprados
                foreach (var asiento in asientosAComprar)
                {
                    asiento.VueloAsientoReservado = false;
                    asiento.VueloAsientoComprado = true;
                    asiento.VueloAsientoBloqueadoHasta = null;

                    await _iUnitOfWork.Repository<VueloAsiento>().ActualizarAsync(asiento);
                }

                // Crear los detalles de compra
                foreach (var item in paramsCompra.DetalleAsientos)
                {
                    var detalle = new CompraDetalle
                    {
                        CompraId = nuevaCompra.CompraId,
                        VueloAsientoId = item.VueloAsientoId,
                        CompraDetalleNombrePasajero = item.CompraDetalleNombrePasajero,
                        CompraDetalleIdentificacionPasajero = item.CompraDetalleIdentificacionPasajero,
                        CompraDetallePrecio = item.CompraDetallePrecio
                    };

                    await _iUnitOfWork.Repository<CompraDetalle>().AdicionarAsync(detalle);
                    await _iUnitOfWork.SaveChangesAsync();

                    await CreateCompraDetalleHistorico(detalle);
                }

                // Consulta el vuelo comprado para borrar el cache redis
                ParamsConsultarVuelo paramsConsultaVuelo = new ParamsConsultarVuelo { VueloId = paramsCompra.VueloId };
                Vuelo vueloComprado = new Vuelo();
                IEnumerable<Vuelo> vuelos = await _iVueloService.GetWithParamsAsync(paramsConsultaVuelo);
                vueloComprado = vuelos.FirstOrDefault();

                string cacheKey = $"vuelo:{vueloComprado.CiudadOrigenId}:{vueloComprado.CiudadDestinoId}:{vueloComprado.VueloFechaHoraSalida:yyyyMMdd}";
                await _iCacheService.RemoveAsync(cacheKey);

                scope.Complete();                

                return nuevaCompra;
            }
        }

        public async Task<IEnumerable<ComprasRealizadasUsuarioPoco>> BuscarComprasUsuarioAsycn(int userId)
        {
            return await _iUnitOfWork.DLComprasPersonalizado.BuscarComprasUsuarioAsycn(userId); ;

        }
        #endregion

        #region Metodos Propios
        private async Task CreateCompraHistorico(Compra createHistorico)
        {
            CompraHistorico vueloHistorico = new CompraHistorico();
            vueloHistorico.CompraHistoricoId = null;
            vueloHistorico.CompraId = createHistorico.CompraId;
            vueloHistorico.EstadoCompraId = createHistorico.EstadoCompraId;
            vueloHistorico.CompraHistoricoFechaRegistro = DateTime.Now;

            await _iUnitOfWork.Repository<CompraHistorico>().AdicionarAsync(vueloHistorico);
            await _iUnitOfWork.SaveChangesAsync();
        }

        private async Task CreateCompraDetalleHistorico(CompraDetalle createHistorico)
        {
            CompraDetalleHistorico vueloHistorico = new CompraDetalleHistorico();
            vueloHistorico.CompraDetalleHistoricoId = null;
            vueloHistorico.CompraDetalleId = createHistorico.CompraDetalleId;
            vueloHistorico.CompraDetalleHistoricoNombrePasajero = createHistorico.CompraDetalleNombrePasajero;
            vueloHistorico.CompraDetalleHistoricoIdentificacionPasajero = createHistorico.CompraDetalleIdentificacionPasajero;
            vueloHistorico.CompraDetalleHistoricoFechaRegistro = DateTime.Now;
            await _iUnitOfWork.Repository<CompraDetalleHistorico>().AdicionarAsync(vueloHistorico);
            await _iUnitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Validacion Campos
        /// <summary>Valida las reglas de negocio antes de realizar la reserva de asientos.</summary>
        /// <param name="paramsReservar">Parámetros que contienen el vuelo y los asientos a reservar.</param>
        /// <exception cref="ValidationException">Se lanza si hay algún error de validación.</exception>
        private async Task ValidarReservaAsientosAsync(ParamsReservarAsientos paramsReservar)
        {
            // Validar parámetros requeridos
            if (paramsReservar == null)
            {
                throw new ValidationException("No se encontró información en la petición");
            }
            else if (paramsReservar.VueloId == null || paramsReservar.VueloId == 0)
            {
                throw new ValidationException(string.Format(DefaultMessages.FieldRequiredWithName, "vuelo"));                
            }

            if (paramsReservar?.ListaAsientoVuelo == null || !paramsReservar.ListaAsientoVuelo.Any())
            {
                throw new ValidationException("Debe especificar al menos un asiento para reservar.");
            }

            // Validar que el vuelo exista
            ParamsConsultarVuelo paramsConsultaVuelo = new ParamsConsultarVuelo { VueloId = paramsReservar.VueloId };
            IEnumerable<Vuelo> vuelos = await _iVueloService.GetWithParamsAsync(paramsConsultaVuelo);

            if (vuelos == null || !vuelos.Any())
            {
                throw new ValidationException(string.Format(DefaultMessages.DataNotFound, "El vuelo"));
            }
            else if (vuelos.Any(x => x.EstadoVueloId != 2))
            {
                // Valida que si tenga un estado optimo para realizar la reserva de asientos para la compra
                throw new ValidationException("El vuelo no tiene un estado valido para reservar asientos.");
            }

            // Validar que los asientos existan en el vuelo
            VueloAsiento filtroAsiento = new VueloAsiento { VueloId = paramsReservar.VueloId };
            Expression<Func<VueloAsiento, bool>> filtro = filtroAsiento.ToFilterExpression<VueloAsiento>();
            IEnumerable<VueloAsiento> asientosVuelo = await _iUnitOfWork.Repository<VueloAsiento>().ConsultarListaAsync(filtro);

            var idsAsientosVuelo = asientosVuelo.Select(a => a.VueloAsientoId).ToHashSet();
            var idsSolicitados = paramsReservar.ListaAsientoVuelo.Select(a => a.VueloAsientoId).ToList();
            var idsNoEncontrados = idsSolicitados.Where(id => !idsAsientosVuelo.Contains(id)).ToList();

            if (idsNoEncontrados.Any())
            {
                throw new ValidationException("Uno o más asientos no pertenecen al vuelo.");
            }

            // Validar que los asientos no estén bloqueados ni modificados
            var ahora = DateTime.Now;
            foreach (var asiento in asientosVuelo)
            {
                var paramAsiento = paramsReservar.ListaAsientoVuelo.FirstOrDefault(x => x.VueloAsientoId == asiento.VueloAsientoId);
                if (paramAsiento == null) continue;

                // Concurrencia
                if (paramAsiento.RowVersion != null && !asiento.RowVersion.SequenceEqual(paramAsiento.RowVersion))
                {
                    throw new ValidationException("Uno o más asientos fueron modificados por otro proceso");
                }

                // Bloqueo
                if (asiento.VueloAsientoBloqueadoHasta.HasValue && asiento.VueloAsientoBloqueadoHasta.Value > ahora)
                {
                    throw new ValidationException("Uno o más asientos están boqueados temporalmente");
                }

                // Comprado
                if (asiento.VueloAsientoComprado.HasValue && asiento.VueloAsientoComprado == true)
                {
                    throw new ValidationException("Uno o más asientos ya fueron comprados");
                }
            }
        }

        /// <summary>Valida las reglas de negocio antes de realizar la compra de asientos.</summary>
        /// <param name="paramsCompra">Parámetros con la información de la compra.</param>
        /// <exception cref="ValidationException">Se lanza si ocurre algún error de validación.</exception>
        private async Task ValidarCompraAsientosAsync(ParamsCompraAsientos paramsCompra)
        {
            string errores = string.Empty;

            // Validar datos mínimos requeridos
            if (paramsCompra == null)
            {
                throw new ValidationException("Los parámetros de compra no pueden ser nulos.");
            }
            else
            {
                if (paramsCompra.VueloId == null || paramsCompra.VueloId == 0)
                    throw new ValidationException(string.Format(DefaultMessages.FieldRequiredWithName, "vuelo"));

                if (paramsCompra.DetalleAsientos == null || !paramsCompra.DetalleAsientos.Any())
                    throw new ValidationException("Debe especificar al menos un asiento para la compra.");
            }

            // Validar que el vuelo exista
            ParamsConsultarVuelo paramsConsultaVuelo = new ParamsConsultarVuelo { VueloId = paramsCompra.VueloId };
            IEnumerable<Vuelo> vuelos = await _iVueloService.GetWithParamsAsync(paramsConsultaVuelo);

            if (vuelos == null || !vuelos.Any())
            {
                throw new ValidationException(string.Format(DefaultMessages.DataNotFound, "El vuelo"));
            }
            else if (vuelos.Any(x => x.EstadoVueloId != 2))
            {
                // Valida que si tenga un estado optimo para realizar la reserva de asientos para la compra
                throw new ValidationException("El vuelo no tiene un estado valido para comprar asientos.");
            }

            // Consultar los asientos asociados al vuelo
            VueloAsiento filtroAsiento = new() { VueloId = paramsCompra.VueloId };
            Expression<Func<VueloAsiento, bool>> filtro = filtroAsiento.ToFilterExpression<VueloAsiento>();
            IEnumerable<VueloAsiento> asientosVuelo = await _iUnitOfWork.Repository<VueloAsiento>().ConsultarListaAsync(filtro);

            var idsAsientosVuelo = asientosVuelo.Select(a => a.VueloAsientoId).ToHashSet();
            var listaIds = paramsCompra.DetalleAsientos.Select(a => a.VueloAsientoId).ToList();

            // Validar que los asientos pertenezcan al vuelo
            var idsNoEncontrados = listaIds.Where(id => !idsAsientosVuelo.Contains(id)).ToList();
            if (idsNoEncontrados.Any())
                throw new ValidationException("Uno o más asientos no pertenecen al vuelo");

            // Validar concurrencia y disponibilidad
            var asientosAComprar = asientosVuelo.Where(a => listaIds.Contains((int)a.VueloAsientoId)).ToList();
            foreach (var asientoParam in paramsCompra.DetalleAsientos)
            {
                var asiento = asientosAComprar.FirstOrDefault(a => a.VueloAsientoId == asientoParam.VueloAsientoId);
                if (asiento == null) continue;

                // Concurrencia
                if (asientoParam.RowVersion != null && !asiento.RowVersion.SequenceEqual(asientoParam.RowVersion))
                    throw new ValidationException("Uno o más asientos fueron modificados por otro usuario.");

                // Ya comprado
                if (asiento.VueloAsientoComprado == true)
                    throw new ValidationException("Uno o más asientos ya fueron comprados.");
            }
        }

        #endregion
    }
}
