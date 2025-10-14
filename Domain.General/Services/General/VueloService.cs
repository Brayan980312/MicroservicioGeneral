namespace Domain.General.Services.General
{
    using AutoMapper;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Avion;
    using Domain.General.CustomEntities.Params;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;
    using Utilitarios.Constants;
    using Utilitarios.Extensions;
    using Utilitarios.Helpers;

    /// <summary>Implementación de reglas de negocio para el servicio de vuelo.</summary>
    public class VueloService : IVueloService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Instancia del Mapeador - Imapper.</summary>
        private readonly IMapper _iMapper;

        /// <summary>Inyeccion del servicio IConfiguracionService.</summary>
        private readonly IConfiguracionService _iConfiguracionService;

        /// <summary>Inyeccion del servicio IAdministracionService.</summary>
        private readonly IAdministracionService _iAdministracionService;

        /// <summary>Inyeccion del servicio IAvionService.</summary>
        private readonly IAvionService _iAvionService;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase VueloService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public VueloService(IUnitOfWork iUnitOfWork, IMapper iMapper, IAdministracionService iAdministracionService, IAvionService iAvionService, IConfiguracionService iConfiguracionService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _iAdministracionService = iAdministracionService;
            _iAvionService = iAvionService;
            _iConfiguracionService = iConfiguracionService;
        }

        #endregion

        #region Métodos Implementados

        /// <inheritdoc />
        public async Task<IEnumerable<Vuelo>> GetWithParamsAsync(ParamsConsultarVuelo paramsSearch, int? userId = null)
        {
            Vuelo entidad = _iMapper.Map<Vuelo>(paramsSearch);
            Expression<Func<Vuelo, bool>> filtro = entidad.ToFilterExpression<Vuelo>();
            return await _iUnitOfWork.Repository<Vuelo>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<Vuelo> CreateAsync(ParamsCrearActualizarVuelo paramsCreateUpdate, int? userId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCrearActualizarVuelo(paramsCreateUpdate);
                IEnumerable<Vuelo> listaEntidad = new List<Vuelo>();
                Vuelo createUpdateEntidad = _iMapper.Map<Vuelo>(paramsCreateUpdate);

                createUpdateEntidad.UsuarioCreacionId = userId;
                createUpdateEntidad.VueloFechaCreacion = DateTime.Now;

                if (paramsCreateUpdate.VueloId != null && paramsCreateUpdate.VueloId > 0)
                {
                    await _iUnitOfWork.Repository<Vuelo>().ActualizarAsync(createUpdateEntidad);
                }
                else
                {
                    // Crea y garantiza que el atributo primario vaya null
                    createUpdateEntidad.VueloId = null;
                    createUpdateEntidad.EstadoVueloId = (int?)Domain.General.Enums.Enum.EstadoVuelo.Programado;
                    await _iUnitOfWork.Repository<Vuelo>().AdicionarAsync(createUpdateEntidad);
                }
                await _iUnitOfWork.SaveChangesAsync();
                
                // Guarda inmediatamente el historico del vuelo
                await CreateVueloHistorico(createUpdateEntidad);

                scope.Complete();
                return createUpdateEntidad;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Compra>> GetWithParamsAsync(ParamsConsultarCompra paramsSearch, int? userId = null)
        {
            Compra entidad = _iMapper.Map<Compra>(paramsSearch);
            entidad.UsuarioId = userId;
            Expression<Func<Compra, bool>> filtro = entidad.ToFilterExpression<Compra>();
            return await _iUnitOfWork.Repository<Compra>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<Compra> CreateAsync(ParamsCrearActualizarCompra paramsCreateUpdate, int? userId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCrearActualizarCompra(paramsCreateUpdate);
                IEnumerable<Compra> listaEntidad = new List<Compra>();
                Compra createUpdateEntidad = _iMapper.Map<Compra>(paramsCreateUpdate);
                createUpdateEntidad.UsuarioId = userId;
                createUpdateEntidad.CompraFecha = DateTime.Now;

                if (paramsCreateUpdate.CompraId != null && paramsCreateUpdate.CompraId > 0)
                {
                    await _iUnitOfWork.Repository<Compra>().ActualizarAsync(createUpdateEntidad);
                }
                else
                {
                    // Crea y garantiza que el atributo primario vaya null
                    createUpdateEntidad.CompraId = null;
                    createUpdateEntidad.EstadoCompraId = (int?)Domain.General.Enums.Enum.EstadoCompra.Comprado;
                    await _iUnitOfWork.Repository<Compra>().AdicionarAsync(createUpdateEntidad);
                }
                await _iUnitOfWork.SaveChangesAsync();

                // Guarda inmediatamente el historico de la compra
                await CreateCompraHistorico(createUpdateEntidad);

                scope.Complete();
                return createUpdateEntidad;
            }
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
        public async Task<IEnumerable<VueloHistorico>> GetWithParamsAsync(ParamsConsultarVueloHistorico paramsSearch, int? userId = null)
        {
            VueloHistorico entidad = _iMapper.Map<VueloHistorico>(paramsSearch);
            Expression<Func<VueloHistorico, bool>> filtro = entidad.ToFilterExpression<VueloHistorico>();
            return await _iUnitOfWork.Repository<VueloHistorico>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CompraDetalleHistorico>> GetWithParamsAsync(ParamsConsultarCompraDetalleHistorico paramsSearch, int? userId = null)
        {
            CompraDetalleHistorico entidad = _iMapper.Map<CompraDetalleHistorico>(paramsSearch);
            Expression<Func<CompraDetalleHistorico, bool>> filtro = entidad.ToFilterExpression<CompraDetalleHistorico>();
            return await _iUnitOfWork.Repository<CompraDetalleHistorico>().ConsultarListaAsync(filtro);
        }

        public async Task<Vuelo> ActualizarEstadoVuelo(ParamsActualizarEstadoVuelo updateEstado) 
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                // Validaciones de cambio de estado de vuelo
                await ValidarCamposActualizarEstadoVuelo(updateEstado);
                Vuelo objetoEntidad = new Vuelo();
                IEnumerable<Vuelo> ListadoEntidad = new List<Vuelo>();
                ParamsConsultarVuelo paramsConsultarVuelo = new ParamsConsultarVuelo();
                paramsConsultarVuelo.VueloId = updateEstado.VueloId;
                ListadoEntidad = await GetWithParamsAsync(paramsConsultarVuelo);
                objetoEntidad = ListadoEntidad.FirstOrDefault();
                objetoEntidad.EstadoVueloId = updateEstado.EstadoVueloId;

                await _iUnitOfWork.Repository<Vuelo>().ActualizarAsync(objetoEntidad);
                await _iUnitOfWork.SaveChangesAsync();

                // Guarda inmediatamente el historico del vuelo con los cambios realizados
                await CreateVueloHistorico(objetoEntidad);

                scope.Complete();
                return objetoEntidad;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<VueloAsiento>> ReservarAsientosAsync(ParamsReservarAsientos paramsReservar)
        {
            if (paramsReservar == null || paramsReservar.ListaAsientoVuelo == null || !paramsReservar.ListaAsientoVuelo.Any())
                throw new ArgumentException("Debe especificar al menos un asiento para reservar.");

            using (var scope = new TransactionScope(TransactionScopeOption.Required,new TimeSpan(0, 5, 0),TransactionScopeAsyncFlowOption.Enabled))
            {
                var listaIds = paramsReservar.ListaAsientoVuelo.Select(a => a.VueloAsientoId).ToList();

                VueloAsiento entidad = new VueloAsiento();
                entidad.VueloId = paramsReservar.VueloId;
                Expression<Func<VueloAsiento, bool>> filtro = entidad.ToFilterExpression<VueloAsiento>();
                IEnumerable<VueloAsiento> asientos = await _iUnitOfWork.Repository<VueloAsiento>().ConsultarListaAsync(filtro);

                // Validar que todos los asientos solicitados existen en ese vuelo
                var idsAsientosVuelo = asientos.Select(a => a.VueloAsientoId).ToHashSet();
                var idsNoEncontrados = listaIds.Where(id => !idsAsientosVuelo.Contains(id)).ToList();

                if (idsNoEncontrados.Any())
                    throw new InvalidOperationException($"Uno o más asientos no pertenecen al vuelo (IDs: {string.Join(", ", idsNoEncontrados)}).");

                var ahora = DateTime.Now;
                var bloqueados = new List<VueloAsiento>();

                foreach (var asiento in asientos)
                {
                    // 2️ Validar concurrencia
                    var paramAsiento = paramsReservar.ListaAsientoVuelo.First(x => x.VueloAsientoId == asiento.VueloAsientoId);
                    if (paramAsiento.RowVersion != null && !asiento.RowVersion.SequenceEqual(paramAsiento.RowVersion))
                        throw new DbUpdateConcurrencyException($"El asiento {asiento.VueloAsientoId} fue modificado por otro proceso.");

                    // 3️ Validar si ya está bloqueado
                    if (asiento.VueloAsientoBloqueadoHasta.HasValue && asiento.VueloAsientoBloqueadoHasta.Value > ahora)
                        throw new InvalidOperationException($"El asiento {asiento.VueloAsientoId} ya está bloqueado temporalmente.");

                    // 4️ Marcar el bloqueo
                    asiento.VueloAsientoBloqueadoHasta = ahora.AddMinutes(5); // bloqueado por 5 minutos

                    // Actualiza en contexto
                    await _iUnitOfWork.Repository<VueloAsiento>().ActualizarAsync(asiento);
                    bloqueados.Add(asiento);
                }

                await _iUnitOfWork.SaveChangesAsync();

                scope.Complete();
                return bloqueados;
            }
        }

        public async Task<Compra> ComprarAsientosAsync(ParamsCompraAsientos paramsCompra)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                if (paramsCompra == null || paramsCompra.DetalleAsientos == null || !paramsCompra.DetalleAsientos.Any())
                    throw new ArgumentException("Debe especificar al menos un asiento para la compra.");

                var listaIds = paramsCompra.DetalleAsientos.Select(a => a.VueloAsientoId).ToList();

                // 1️ Consultar los asientos asociados al vuelo
                VueloAsiento entidadFiltro = new() { VueloId = paramsCompra.VueloId };
                Expression<Func<VueloAsiento, bool>> filtro = entidadFiltro.ToFilterExpression<VueloAsiento>();
                IEnumerable<VueloAsiento> asientosVuelo = await _iUnitOfWork.Repository<VueloAsiento>().ConsultarListaAsync(filtro);

                // 2️ Validar que los asientos pertenezcan al vuelo
                var idsAsientosVuelo = asientosVuelo.Select(a => a.VueloAsientoId).ToHashSet();
                var idsNoEncontrados = listaIds.Where(id => !idsAsientosVuelo.Contains(id)).ToList();
                if (idsNoEncontrados.Any())
                    throw new InvalidOperationException($"Uno o más asientos no pertenecen al vuelo (IDs: {string.Join(", ", idsNoEncontrados)}).");

                // 3️ Obtener solo los asientos que se van a comprar
                var asientosAComprar = asientosVuelo.Where(a => listaIds.Contains(a.VueloAsientoId)).ToList();

                // 4️ Validar concurrencia y disponibilidad
                foreach (var asientoParam in paramsCompra.DetalleAsientos)
                {
                    var asiento = asientosAComprar.First(a => a.VueloAsientoId == asientoParam.VueloAsientoId);

                    if (asientoParam.RowVersion != null && !asiento.RowVersion.SequenceEqual(asientoParam.RowVersion))
                        throw new InvalidOperationException($"El asiento {asiento.VueloAsientoId} fue modificado por otro usuario.");

                    if (asiento.VueloAsientoComprado)
                        throw new InvalidOperationException($"El asiento {asiento.VueloAsientoId} ya fue comprado.");

                    if (asiento.VueloAsientoBloqueadoHasta.HasValue && asiento.VueloAsientoBloqueadoHasta > DateTime.Now)
                        throw new InvalidOperationException($"El asiento {asiento.VueloAsientoId} está bloqueado temporalmente.");
                }

                // 5️ Crear la compra principal
                Compra nuevaCompra = new Compra
                {
                    UsuarioId = paramsCompra.UsuarioId,
                    VueloId = paramsCompra.VueloId,
                    EstadoCompraId = paramsCompra.EstadoCompraId,
                    MetodoPagoId = paramsCompra.MetodoPagoId,
                    CompraFecha = DateTime.Now,
                    CompraTotal = paramsCompra.CompraTotal
                };

                await _iUnitOfWork.Repository<Compra>().AdicionarAsync(nuevaCompra);
                await _iUnitOfWork.SaveChangesAsync(); // Guarda para obtener CompraId

                // 6️⃣ Actualizar los asientos como comprados
                foreach (var asiento in asientosAComprar)
                {
                    asiento.VueloAsientoReservado = false;
                    asiento.VueloAsientoComprado = true;
                    asiento.VueloAsientoBloqueadoHasta = null;

                    await _iUnitOfWork.Repository<VueloAsiento>().ActualizarAsync(asiento);
                }

                // 7️⃣ Crear los detalles de compra
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
                }

                await _iUnitOfWork.SaveChangesAsync();
                scope.Complete();

                return nuevaCompra;
            }
        }

        #endregion

        #region MetodosPropios
        private async Task CreateVueloHistorico(Vuelo createHistorico)
        {
            VueloHistorico vueloHistorico = new VueloHistorico();
            vueloHistorico.VueloId = createHistorico.VueloId;
            vueloHistorico.VueloHistoricoCodigo = createHistorico.VueloCodigo;
            vueloHistorico.EstadoVueloId = createHistorico.EstadoVueloId;
            vueloHistorico.AvionId = createHistorico.AvionId;
            vueloHistorico.CiudadOrigenId = createHistorico.CiudadOrigenId;
            vueloHistorico.CiudadDestinoId = createHistorico.CiudadDestinoId;
            vueloHistorico.VueloHistoricoPrecio = createHistorico.VueloPrecio;
            vueloHistorico.VueloHistoricoDescuento = createHistorico.VueloDescuento;
            vueloHistorico.VueloHistoricoFechaHoraSalida = createHistorico.VueloFechaHoraSalida;
            vueloHistorico.VueloHistoricoFechaHoraLlegada = createHistorico.VueloFechaHoraLlegada;
            vueloHistorico.UsuarioCreacionId = createHistorico.UsuarioCreacionId;
            vueloHistorico.VueloHistoricoFechaCreacion = DateTime.Now;
            await _iUnitOfWork.Repository<VueloHistorico>().AdicionarAsync(vueloHistorico);
            await _iUnitOfWork.SaveChangesAsync();
        }

        private async Task CreateCompraHistorico(Compra createHistorico)
        {
            CompraHistorico compraHistorico = new CompraHistorico();
            compraHistorico.CompraId = createHistorico.CompraId;
            compraHistorico.EstadoCompraId = createHistorico.EstadoCompraId;
            compraHistorico.CompraHistoricoFechaRegistro = DateTime.Now;
            await _iUnitOfWork.Repository<CompraHistorico>().AdicionarAsync(compraHistorico);
            await _iUnitOfWork.SaveChangesAsync();
        }

        private async Task CreateCompraDetalleHistorico(CompraDetalle createHistorico)
        {
            CompraDetalleHistorico compraDetalleHistorico = new CompraDetalleHistorico();
            compraDetalleHistorico.CompraDetalleId = createHistorico.CompraDetalleId;
            compraDetalleHistorico.CompraDetalleHistoricoNombrePasajero = createHistorico.CompraDetalleNombrePasajero;
            compraDetalleHistorico.CompraDetalleHistoricoIdentificacionPasajero = createHistorico.CompraDetalleIdentificacionPasajero;
            compraDetalleHistorico.CompraDetalleHistoricoFechaRegistro = DateTime.Now;
            await _iUnitOfWork.Repository<CompraDetalleHistorico>().AdicionarAsync(compraDetalleHistorico);
            await _iUnitOfWork.SaveChangesAsync();
        }

        private async Task<IEnumerable<EstadoVuelo>> ConsultaEstadoVuelo()
        {
            return await _iUnitOfWork.Repository<EstadoVuelo>().ConsultarTodosAsync();
        }

        private async Task<IEnumerable<EstadoCompra>> ConsultaEstadoCompra()
        {
            return await _iUnitOfWork.Repository<EstadoCompra>().ConsultarTodosAsync();
        }

        #endregion

        #region ValidacionCampos
        /// <summary>Valida los campos obligatorios para realizar la creacion o actualización del vuelo.</summary>
        /// <param name="parametrosCrearActualizarVuelo">El objeto de tipo ParamsCrearActualizarVuelo que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCrearActualizarVuelo(ParamsCrearActualizarVuelo parametrosCrearActualizarVuelo)
        {
            string errores = string.Empty;
            IEnumerable<Vuelo> Resultado = new List<Vuelo>();
            ParamsConsultarVuelo Existentes = new ParamsConsultarVuelo();
            Resultado = await GetWithParamsAsync(Existentes);
            IEnumerable<EstadoVuelo> ResultadoEstadoVuelos = await ConsultaEstadoVuelo();

            if (parametrosCrearActualizarVuelo.VueloId != null && parametrosCrearActualizarVuelo.VueloId > 0)
            {
                // Valida si el Id del vuelo existe
                if (Resultado.Where(x => x.VueloId == parametrosCrearActualizarVuelo.VueloId).Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "El vuelo");
                }
            }

            // Validar el codigo del vuelo
            if (string.IsNullOrEmpty(errores))
            {
                if (string.IsNullOrEmpty(parametrosCrearActualizarVuelo.VueloCodigo))
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "codigo");
                }
                else
                {
                    // Valida si el codigo del vuelo ya existe en el sistema
                    if (Resultado.Any(x => x.VueloCodigo.Trim().ToUpper() == parametrosCrearActualizarVuelo.VueloCodigo.Trim().ToUpper() &&
                                                                        x.VueloId != parametrosCrearActualizarVuelo.VueloId))
                    {
                        errores += string.Format(DefaultMessages.AlreadyExistsData, $"el codigo de vuelo '{parametrosCrearActualizarVuelo.VueloCodigo.Trim()}'");
                    }
                }
            }

            // Validar el estado de vuelo
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.EstadoVueloId == null || parametrosCrearActualizarVuelo.EstadoVueloId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "estado vuelo");
                }
                else
                {
                    // Valida si el estado de vuelo existe
                    if (!ResultadoEstadoVuelos.Any(x => x.EstadoVueloId == parametrosCrearActualizarVuelo.EstadoVueloId))
                    {
                        errores += string.Format(DefaultMessages.DataNotFound, "El estado de vuelo");
                    }
                }
            }

            // Valida que el estado del vuelo sea el correcto para la creación del mismo
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.EstadoVueloId != (int?)Domain.General.Enums.Enum.EstadoVuelo.Programado && (parametrosCrearActualizarVuelo.VueloId == null || parametrosCrearActualizarVuelo.VueloId == 0))
                {
                    errores += "El estado de vuelo no es el correcto para la creación del vuelo.";
                }
            }

            // Validar el avion
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.AvionId == null || parametrosCrearActualizarVuelo.AvionId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "avión");
                }
                else
                {
                    // Consulta los aviones para validar si existe o no
                    IEnumerable<Avion> aviones = new List<Avion>();
                    ParamsConsultarAvion paramsConsultarAvion = new ParamsConsultarAvion();
                    paramsConsultarAvion.AvionId = parametrosCrearActualizarVuelo.AvionId;
                    aviones = await _iAvionService.GetWithParamsAsync(paramsConsultarAvion);

                    if (aviones.Count() == 0)
                    {
                        errores += string.Format(DefaultMessages.DataNotFound, "El avión");
                    }
                }
            }

            // Validar que el avión esté activo en el sistema
            if (string.IsNullOrEmpty(errores))
            {
                // Consulta los aviones para validar si existe o no
                IEnumerable<Avion> aviones = new List<Avion>();
                ParamsConsultarAvion paramsConsultarAvion = new ParamsConsultarAvion();
                paramsConsultarAvion.AvionId = parametrosCrearActualizarVuelo.AvionId;
                paramsConsultarAvion.AvionEstado = true;
                aviones = await _iAvionService.GetWithParamsAsync(paramsConsultarAvion);

                if (aviones.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.NotActiveData, "El avión");
                }
            }

            // Validar la ciudad de origen
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.CiudadOrigenId == null || parametrosCrearActualizarVuelo.CiudadOrigenId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "ciudad de origen");
                }
                else
                {
                    // Valida si la ciudad ingresada existe
                    IEnumerable<Ciudad> ciudades = new List<Ciudad>();
                    ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                    paramsConsultarCiudad.CiudadId = parametrosCrearActualizarVuelo.CiudadOrigenId;
                    ciudades = await _iAdministracionService.GetWithParamsAsync(paramsConsultarCiudad);

                    if (ciudades.Count() == 0)
                    {
                        errores += string.Format(DefaultMessages.DataNotFound, "La ciudad de origen");
                    }
                }
            }

            // Validar que la ciudad de origen esté activa
            if (string.IsNullOrEmpty(errores))
            {
                // Valida si la ciudad ingresada existe
                IEnumerable<Ciudad> ciudades = new List<Ciudad>();
                ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                paramsConsultarCiudad.CiudadId = parametrosCrearActualizarVuelo.CiudadOrigenId;
                paramsConsultarCiudad.CiudadEstado = true;
                ciudades = await _iAdministracionService.GetWithParamsAsync(paramsConsultarCiudad);

                if (ciudades.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.NotActiveData, "La ciudad de origen");
                }
            }

            // Validar que el avion esté en la ciudad de origen
            if (string.IsNullOrEmpty(errores))
            {
                // Consulta los aviones para validar si existe o no
                IEnumerable<Avion> aviones = new List<Avion>();
                ParamsConsultarAvion paramsConsultarAvion = new ParamsConsultarAvion();
                paramsConsultarAvion.AvionId = parametrosCrearActualizarVuelo.AvionId;
                paramsConsultarAvion.CiudadId = parametrosCrearActualizarVuelo.CiudadOrigenId;
                aviones = await _iAvionService.GetWithParamsAsync(paramsConsultarAvion);

                if (aviones.Count() == 0)
                {
                    errores += "El avión no se encuentra en la ciudad de origen.";
                }
            }

            // Validar la ciudad de destino
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.CiudadDestinoId == null || parametrosCrearActualizarVuelo.CiudadDestinoId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "ciudad de destino");
                }
                else
                {
                    // Valida si la ciudad ingresada existe
                    IEnumerable<Ciudad> ciudades = new List<Ciudad>();
                    ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                    paramsConsultarCiudad.CiudadId = parametrosCrearActualizarVuelo.CiudadDestinoId;
                    ciudades = await _iAdministracionService.GetWithParamsAsync(paramsConsultarCiudad);

                    if (ciudades.Count() == 0)
                    {
                        errores += string.Format(DefaultMessages.DataNotFound, "La ciudad de destino");
                    }
                }
            }

            // Validar que la ciudad de destino esté activa
            if (string.IsNullOrEmpty(errores))
            {
                // Valida si la ciudad ingresada existe
                IEnumerable<Ciudad> ciudades = new List<Ciudad>();
                ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                paramsConsultarCiudad.CiudadId = parametrosCrearActualizarVuelo.CiudadDestinoId;
                paramsConsultarCiudad.CiudadEstado = true;
                ciudades = await _iAdministracionService.GetWithParamsAsync(paramsConsultarCiudad);

                if (ciudades.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.NotActiveData, "La ciudad de destino");
                }
            }

            // Validar que la ciudad origen y destino no sea la misma
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.CiudadOrigenId == parametrosCrearActualizarVuelo.CiudadDestinoId)
                {
                    errores += "La ciudad de origen y ciudad destino deben ser diferentes";
                }
            }

            // Validar el precio del vuelo por asiento
            if (string.IsNullOrEmpty(errores)) 
            {
                if (parametrosCrearActualizarVuelo.VueloPrecio == null || parametrosCrearActualizarVuelo.VueloPrecio == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "precio");
                }
                else
                {
                    if (parametrosCrearActualizarVuelo.VueloPrecio <= 0)
                    {
                        errores += string.Format(DefaultMessages.InvalidNumberMin, "El precio", "0");
                    }
                }

            }
            
            // Validar el precio de descuento (Si viene que sea mayor a 0.01...)
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.VueloDescuento != null)
                {
                    if (parametrosCrearActualizarVuelo.VueloDescuento < 0 || parametrosCrearActualizarVuelo.VueloDescuento > 100)
                    {
                        errores += string.Format(DefaultMessages.RangeError, "El porcentaje de descuento", "0", "100");
                    }
                }
            }

            // Validar fecha de salida 
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.VueloFechaHoraSalida == null)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "fecha salida");
                }
                else
                {
                    if (parametrosCrearActualizarVuelo.VueloFechaHoraSalida < DateTime.Now)
                    {
                        errores += "La fecha de salida no debe ser menor a la hora actual";
                    }
                }
            }

            // Validar fecha de llegada 
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarVuelo.VueloFechaHoraLlegada == null)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "fecha llegada");
                }
                else
                {
                    if (parametrosCrearActualizarVuelo.VueloFechaHoraLlegada < parametrosCrearActualizarVuelo.VueloFechaHoraSalida)
                    {
                        errores += "La fecha de llegada no debe ser menor a la fecha de salida";
                    }
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }

        /// <summary>Valida los campos obligatorios para realizar la actualización del estado de un vuelo.</summary>
        /// <param name="parametrosActualizarEstadoVuelo">El objeto de tipo ParamsActualizarEstadoVuelo que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposActualizarEstadoVuelo(ParamsActualizarEstadoVuelo parametrosActualizarEstadoVuelo)
        {
            string errores = string.Empty;
            if (parametrosActualizarEstadoVuelo.VueloId == null || parametrosActualizarEstadoVuelo.VueloId == 0)
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "vuelo");
            }
            else
            {
                // Valida que el vuelo exista en el sistema
                IEnumerable<Vuelo> vuelos = new List<Vuelo>();
                ParamsConsultarVuelo paramsConsultaVuelo = new ParamsConsultarVuelo();
                paramsConsultaVuelo.VueloId = parametrosActualizarEstadoVuelo.VueloId;
                vuelos = await GetWithParamsAsync(paramsConsultaVuelo);

                if (vuelos.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "El vuelo");
                }
            }

            // Valida que el estado se envie y que exista en el sistema
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosActualizarEstadoVuelo.EstadoVueloId == null || parametrosActualizarEstadoVuelo.EstadoVueloId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "estado vuelo");
                }
                else
                {
                    // Valida si el estado de vuelo existe
                    IEnumerable<EstadoVuelo> ResultadoEstadoVuelos = await ConsultaEstadoVuelo();
                    if (!ResultadoEstadoVuelos.Any(x => x.EstadoVueloId == parametrosActualizarEstadoVuelo.EstadoVueloId))
                    {
                        errores += string.Format(DefaultMessages.DataNotFound, "El estado de vuelo");
                    }
                }
            }

            // Valida si el cambio de estado es permitido
            if (string.IsNullOrEmpty(errores))
            {
                // Obtiene el estado actual del vuelo y el que se desea actualizar
                int estadoActual = 0;
                int estadoCambio = 0;

                IEnumerable<Vuelo> vuelos = new List<Vuelo>();
                ParamsConsultarVuelo paramsConsultaVuelo = new ParamsConsultarVuelo();
                paramsConsultaVuelo.VueloId = parametrosActualizarEstadoVuelo.VueloId;
                vuelos = await GetWithParamsAsync(paramsConsultaVuelo);

                estadoActual = Convert.ToInt32(vuelos.FirstOrDefault().EstadoVueloId);
                estadoCambio = Convert.ToInt32(parametrosActualizarEstadoVuelo.EstadoVueloId);

                if (estadoActual == estadoCambio)
                {
                    errores += "Señor usuario, el estado es el mismo que el actual en el vuelo.";
                }
                else
                {
                    // Diccionario con las reglas de transición prohibidas
                    Dictionary<int, List<int>> transicionesInvalidas = new Dictionary<int, List<int>>
                    {
                        { (int)Enums.Enum.EstadoVuelo.Programado, new List<int> { (int)Enums.Enum.EstadoVuelo.Cerrado, (int)Enums.Enum.EstadoVuelo.EnEmbarque, (int)Enums.Enum.EstadoVuelo.Despegado, (int)Enums.Enum.EstadoVuelo.Aterrizado, (int)Enums.Enum.EstadoVuelo.Finalizado } },
                        { (int)Enums.Enum.EstadoVuelo.Disponible, new List<int> { (int)Enums.Enum.EstadoVuelo.Programado, (int)Enums.Enum.EstadoVuelo.Despegado, (int)Enums.Enum.EstadoVuelo.Aterrizado, (int)Enums.Enum.EstadoVuelo.Finalizado } },
                        { (int)Enums.Enum.EstadoVuelo.Cerrado, new List<int> { (int)Enums.Enum.EstadoVuelo.Programado, (int)Enums.Enum.EstadoVuelo.Despegado, (int)Enums.Enum.EstadoVuelo.Aterrizado, (int)Enums.Enum.EstadoVuelo.Finalizado } },
                        { (int)Enums.Enum.EstadoVuelo.EnEmbarque, new List<int> { (int)Enums.Enum.EstadoVuelo.Programado, (int)Enums.Enum.EstadoVuelo.Disponible, (int)Enums.Enum.EstadoVuelo.Cerrado, (int)Enums.Enum.EstadoVuelo.Aterrizado, (int)Enums.Enum.EstadoVuelo.Finalizado, (int)Enums.Enum.EstadoVuelo.Cancelado } },
                        { (int)Enums.Enum.EstadoVuelo.Despegado, new List<int> { (int)Enums.Enum.EstadoVuelo.Programado, (int)Enums.Enum.EstadoVuelo.Disponible, (int)Enums.Enum.EstadoVuelo.Cerrado, (int)Enums.Enum.EstadoVuelo.EnEmbarque, (int)Enums.Enum.EstadoVuelo.Finalizado, (int)Enums.Enum.EstadoVuelo.Cancelado } },
                        { (int)Enums.Enum.EstadoVuelo.Aterrizado, new List<int> { (int)Enums.Enum.EstadoVuelo.Programado, (int)Enums.Enum.EstadoVuelo.Disponible, (int)Enums.Enum.EstadoVuelo.Cerrado, (int)Enums.Enum.EstadoVuelo.EnEmbarque, (int)Enums.Enum.EstadoVuelo.Despegado, (int)Enums.Enum.EstadoVuelo.Cancelado } },
                        { (int)Enums.Enum.EstadoVuelo.Finalizado, new List<int> { (int)Enums.Enum.EstadoVuelo.Programado, (int)Enums.Enum.EstadoVuelo.Disponible, (int)Enums.Enum.EstadoVuelo.Cerrado, (int)Enums.Enum.EstadoVuelo.EnEmbarque, (int)Enums.Enum.EstadoVuelo.Despegado, (int)Enums.Enum.EstadoVuelo.Aterrizado, (int)Enums.Enum.EstadoVuelo.Cancelado } },
                        { (int)Enums.Enum.EstadoVuelo.Cancelado, new List<int> { (int)Enums.Enum.EstadoVuelo.Programado, (int)Enums.Enum.EstadoVuelo.Disponible, (int)Enums.Enum.EstadoVuelo.Cerrado, (int)Enums.Enum.EstadoVuelo.EnEmbarque, (int)Enums.Enum.EstadoVuelo.Despegado, (int)Enums.Enum.EstadoVuelo.Aterrizado, (int)Enums.Enum.EstadoVuelo.Finalizado } },
                    };

                    // Validar si el cambio es inválido
                    if (transicionesInvalidas.ContainsKey(estadoActual) && transicionesInvalidas[estadoActual].Contains(estadoCambio))
                    {
                        string nombreEstadoActual = Enum.GetName(typeof(Enums.Enum.EstadoVuelo), estadoActual);
                        string nombreEstadoCambio = Enum.GetName(typeof(Enums.Enum.EstadoVuelo), estadoCambio);

                        throw new InvalidOperationException(
                            $"No se puede cambiar el estado del vuelo de '{nombreEstadoActual}' a '{nombreEstadoCambio}'."
                        );
                    }
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }

        /// <summary>Valida los campos obligatorios para realizar la creacion o actualización de la compra de un vuelo.</summary>
        /// <param name="parametrosCrearActualizarCompra">El objeto de tipo ParamsCrearActualizarCompra que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCrearActualizarCompra(ParamsCrearActualizarCompra parametrosCrearActualizarCompra)
        {
            string errores = string.Empty;
            IEnumerable<Compra> Resultado = new List<Compra>();
            ParamsConsultarCompra Existentes = new ParamsConsultarCompra();
            Resultado = await GetWithParamsAsync(Existentes);

            if (parametrosCrearActualizarCompra.CompraId != null && parametrosCrearActualizarCompra.CompraId > 0)
            {
                // Valida si el Id de la compra existe
                if (Resultado.Where(x => x.CompraId == parametrosCrearActualizarCompra.CompraId).Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "La compra");
                }
            }

            // Valida el vuelo
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarCompra.VueloId == null || parametrosCrearActualizarCompra.VueloId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "vuelo");
                }
                else {
                    // Valida que el vuelo exista en el sistema
                    IEnumerable<Vuelo> vuelos = new List<Vuelo>();
                    ParamsConsultarVuelo paramsConsultaVuelo = new ParamsConsultarVuelo();
                    paramsConsultaVuelo.VueloId = parametrosCrearActualizarCompra.VueloId;
                    vuelos = await GetWithParamsAsync(paramsConsultaVuelo);

                    if (vuelos.Count() == 0)
                    {
                        errores += string.Format(DefaultMessages.DataNotFound, "El vuelo");
                    }
                }
            }

            // Valida que el vuelo tenga un estado correcto para realizar la compra
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarCompra.CompraId == null || parametrosCrearActualizarCompra.CompraId == 0)
                {
                    IEnumerable<Vuelo> vuelos = new List<Vuelo>();
                    ParamsConsultarVuelo paramsConsultaVuelo = new ParamsConsultarVuelo();
                    paramsConsultaVuelo.VueloId = parametrosCrearActualizarCompra.VueloId;
                    paramsConsultaVuelo.EstadoVueloId = (int?)Domain.General.Enums.Enum.EstadoVuelo.Disponible;
                    vuelos = await GetWithParamsAsync(paramsConsultaVuelo);

                    if (vuelos.Count() == 0)
                    {
                        errores += "No se permite comprar un vuelo con un estado diferente de disponible.";
                    }
                }
            }

            // Si la compra ya existe entonces se debe validar que el vuelo si esté asociado a la compra
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearActualizarCompra.CompraId != null && parametrosCrearActualizarCompra.CompraId > 0)
                {
                    IEnumerable<Compra> compras = new List<Compra>();
                    ParamsConsultarCompra paramsConsultaCompra = new ParamsConsultarCompra();
                    paramsConsultaCompra.CompraId = parametrosCrearActualizarCompra.CompraId;
                    paramsConsultaCompra.VueloId = parametrosCrearActualizarCompra.VueloId;
                    compras = await GetWithParamsAsync(paramsConsultaCompra);

                    if (compras.Count() == 0)
                    {
                        errores += "El vuelo no se encuentra asociado a la compra.";
                    }
                }
            }

            // Valida estado de compra
            if (string.IsNullOrEmpty(errores)) 
            {
                if (parametrosCrearActualizarCompra.EstadoCompraId == null || parametrosCrearActualizarCompra.EstadoCompraId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "estado compra");
                }
                else
                {
                    // Valida si el estado de compra existe en el sistema
                    IEnumerable<EstadoCompra> estadosCompra = new List<EstadoCompra>();
                    estadosCompra = await ConsultaEstadoCompra();

                    if (!estadosCompra.Any(x => x.EstadoCompraId == parametrosCrearActualizarCompra.EstadoCompraId))
                    {
                        errores += string.Format(DefaultMessages.DataNotFound, "El estado de compra");
                    }
                }
            }

            // Si es una compra inicial debe validar que el estado de la compra sea "Comprado=1"
            if (string.IsNullOrEmpty(errores)) {
                 if (parametrosCrearActualizarCompra.CompraId == null || parametrosCrearActualizarCompra.CompraId == 0)
                {
                    if (parametrosCrearActualizarCompra.EstadoCompraId != (int?)Domain.General.Enums.Enum.EstadoCompra.Comprado)
                    {
                        errores += "El estado de compra no es valido";
                    }
                }
            }

            // Valida si la fecha de compra cumple con el parametro antes de la salida del vuelo
            if (string.IsNullOrEmpty(errores)) 
            {

                //Obtiene el vuelo para obtener su fecha y hora de salida para saber si está permitido comprar el vuelo
                IEnumerable<Vuelo> vuelos = new List<Vuelo>();
                ParamsConsultarVuelo paramsConsultaVuelo = new ParamsConsultarVuelo();
                paramsConsultaVuelo.VueloId = parametrosCrearActualizarCompra.VueloId;
                vuelos = await GetWithParamsAsync(paramsConsultaVuelo);

                // Obtiene parametros del sistema
                ParamsConsultarParametros paramsConsultarParametros = new ParamsConsultarParametros();
                paramsConsultarParametros.ParametrosNombre = "TiempoCompraVueloAntesSalida";
                IEnumerable<Parametros> parametrosCompraAntesSalida = await _iConfiguracionService.GetWithParamsAsync(paramsConsultarParametros);

                DateTime fechaHoraActual = DateTime.Now;
                DateTime fechaHoraSalidaVuelo = (DateTime)vuelos.FirstOrDefault().VueloFechaHoraSalida;

                if (parametrosCompraAntesSalida.Count() > 0)
                {
                    // Le suma la cantidad de segundos parametrizados a la hora actual
                    fechaHoraActual.AddSeconds(Convert.ToInt32(parametrosCompraAntesSalida.FirstOrDefault().ParametrosValor));
                }

                if (fechaHoraActual >= fechaHoraSalidaVuelo)
                {
                    errores += "Señor usuario, según el tiempo estimado de compra, ya no se puede realizar.";
                }
            }

            // Valida el metodo de pago
            if (string.IsNullOrEmpty(errores)) {
                if (parametrosCrearActualizarCompra.MetodoPagoId == null || parametrosCrearActualizarCompra.MetodoPagoId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "metodo de pago");
                }
                else
                {
                    // Valida si el metodo de pago existe
                    ParamsConsultarMetodoPago paramsConsultarMetodoPago = new ParamsConsultarMetodoPago();
                    paramsConsultarMetodoPago.MetodoPagoId = parametrosCrearActualizarCompra.MetodoPagoId;
                    IEnumerable<MetodoPago> metodoPagos = await _iAdministracionService.GetWithParamsAsync(paramsConsultarMetodoPago);

                    if (metodoPagos.Count() == 0)
                    {
                        errores += string.Format(DefaultMessages.DataNotFound, "El metodo de pago");
                    }
                }
            }

            // Valida el costo de la compra
            if (string.IsNullOrEmpty(errores)) {
                if (parametrosCrearActualizarCompra.CompraTotal == null || parametrosCrearActualizarCompra.CompraTotal == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "compra total");
                }
                else
                {
                    if (parametrosCrearActualizarCompra.CompraTotal <= 0)
                    {
                        errores += string.Format(DefaultMessages.InvalidNumberMin, "La compra total", "0");
                    }
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }
        #endregion
    }
}
