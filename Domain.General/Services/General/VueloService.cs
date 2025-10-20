namespace Domain.General.Services.General
{
    using AutoMapper;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Avion;
    using Domain.General.CustomEntities.Compras;
    using Domain.General.CustomEntities.Metricas;
    using Domain.General.CustomEntities.Params;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;
    using Utilitarios.Constants;
    using Utilitarios.Extensions;

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

        /// <summary>Inyeccion del servicio Metricas.</summary>
        private readonly IMetricasService _iMetricasService;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase VueloService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public VueloService(IUnitOfWork iUnitOfWork, IMapper iMapper, IAdministracionService iAdministracionService, IAvionService iAvionService, IConfiguracionService iConfiguracionService, IMetricasService iMetricasService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _iAdministracionService = iAdministracionService;
            _iAvionService = iAvionService;
            _iConfiguracionService = iConfiguracionService;
            _iMetricasService = iMetricasService;
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

        /// <summary>Servicio personalizado para la consulta de vuelos.</summary>
        /// <param name="searchFlight">Parametros con el que se buscara los vuelos bajo ciertos criterios.</param>
        /// <returns>Lista BusquedaVueloPoco con la información de los vuelos buscados.</returns>
        public async Task<IEnumerable<BusquedaVueloPoco>> ConsultarVueloPersonalizado(ParamsConsultarVuelo paramsSearch) 
        {
            return await _iUnitOfWork.DLVueloPersonalizado.ConsultarVuelos(paramsSearch);
        }

        /// <summary>Contrato personalizado para la consulta de vuelos disponibles en el sistema.</summary>
        /// <param name="paramsSearch">Parametros con el que se buscara los vuelos disponibles bajo ciertos criterios.</param>
        /// <returns>Lista BusquedaVuelosDisponiblesPoco con la información de los vuelos disponibles buscados.</returns>
        public async Task<IEnumerable<BusquedaVuelosDisponiblesPoco>> ConsultarVuelosDisponiblesPersonalizado(ParamsBusquedaVuelosDisponibles paramsSearch)
        {
            // Realiza la actualización de la metrica de vuelos más buscados
            ParamsCrearActualizarVuelosMasBuscados metricaVuelosMasBuscados = new ParamsCrearActualizarVuelosMasBuscados();
            metricaVuelosMasBuscados.CiudadOrigenId = paramsSearch.CiudadOrigenId;
            metricaVuelosMasBuscados.CiudadDestinoId = paramsSearch.CiudadDestinoId;
            await _iMetricasService.CreateAsync(metricaVuelosMasBuscados);

            IEnumerable<BusquedaVuelosDisponiblesPoco> vuelosDisponibles = await _iUnitOfWork.DLVueloPersonalizado.ConsultarVuelosDisponibles(paramsSearch);
            List<BusquedaVuelosDisponiblesPoco> vuelosDisponiblesFinales = new List<BusquedaVuelosDisponiblesPoco>();

            // Recorre cada uno de los vuelos disponibles para consultar los asientos del vuelo y validar si están disponibles la cantidad de pasajeros que solicita el usuario
            foreach (BusquedaVuelosDisponiblesPoco item in vuelosDisponibles)
            {
                IEnumerable<VueloAsiento> asientosVuelo = await ConsultaVueloAsientos((int)item.VueloId);
                int cantidadDisponibles = asientosVuelo.Count(a => (a.VueloAsientoReservado ?? false) == false && (a.VueloAsientoComprado ?? false) == false);

                if (paramsSearch.CantidadPasajeros <= cantidadDisponibles)
                {
                    vuelosDisponiblesFinales.Add(item);
                } 
            }

            IEnumerable<BusquedaVuelosDisponiblesPoco> resultadoFinal = vuelosDisponiblesFinales;

            return resultadoFinal;
        }

        /// <inheritdoc />
        public async Task<Vuelo> CreateAsync(ParamsCrearVuelo paramsCreate, int? userId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCrearVuelo(paramsCreate);
                IEnumerable<Vuelo> listaEntidad = new List<Vuelo>();
                Vuelo createEntidad = _iMapper.Map<Vuelo>(paramsCreate);

                // Crea y garantiza que el atributo primario vaya null
                createEntidad.VueloId = null;
                createEntidad.EstadoVueloId = (int?)Domain.General.Enums.Enum.EstadoVuelo.Programado;
                createEntidad.UsuarioCreacionId = userId;
                createEntidad.VueloFechaCreacion = DateTime.Now;
                await _iUnitOfWork.Repository<Vuelo>().AdicionarAsync(createEntidad);
                await _iUnitOfWork.SaveChangesAsync();
                
                // Guarda inmediatamente el historico del vuelo
                await CreateVueloHistorico(createEntidad);

                // Obtiene los asientos del avión guardado en el vuelo y los guarda en la entidad VueloAsiento (Solo se crea con el vuelo, no es para actualizar en esta petición)
                IEnumerable<AsientoAvion> asientosAvion = new List<AsientoAvion>();
                ParamsConsultarAsientoAvion paramsConsultarAsientoAvion = new ParamsConsultarAsientoAvion();
                paramsConsultarAsientoAvion.AvionId = createEntidad.AvionId;
                paramsConsultarAsientoAvion.AsientoAvionEstado = true;
                asientosAvion = await _iAvionService.GetWithParamsAsync(paramsConsultarAsientoAvion);

                // Recorre cada asiento y lo crea
                List<VueloAsiento> listadoVueloAsientosAgregar = new List<VueloAsiento>();
                foreach (AsientoAvion asientoActual in asientosAvion)
                {
                    VueloAsiento vueloAsiento = new VueloAsiento();
                    vueloAsiento.VueloAsientoId = null;
                    vueloAsiento.VueloId = (int)createEntidad.VueloId!;
                    vueloAsiento.AsientoAvionId = (int)asientoActual.AsientoAvionId!;
                    vueloAsiento.VueloAsientoReservado = false;
                    vueloAsiento.VueloAsientoComprado = false;
                    vueloAsiento.VueloAsientoBloqueadoHasta = null;

                    listadoVueloAsientosAgregar.Add(vueloAsiento);
                }
                await _iUnitOfWork.Repository<VueloAsiento>().AdicionarMasivoAsync(listadoVueloAsientosAgregar);
                await _iUnitOfWork.SaveChangesAsync();

                scope.Complete();
                return createEntidad;
            }
        }

        public async Task<Vuelo> UpdateAsync(ParamsActualizarVuelo paramsUpdate, int? userId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposActualizarVuelo(paramsUpdate);

                // Obtiene el objeto del vuelo que se desea consultar
                Vuelo vueloActualizar = new Vuelo();
                vueloActualizar.VueloId = paramsUpdate.VueloId;
                Expression<Func<Vuelo, bool>> filtro = vueloActualizar.ToFilterExpression<Vuelo>();
                vueloActualizar = await _iUnitOfWork.Repository<Vuelo>().ConsultarUnoAsync(filtro);
                vueloActualizar.VueloCodigo = paramsUpdate.VueloCodigo;
                vueloActualizar.CiudadOrigenId = paramsUpdate.CiudadOrigenId;
                vueloActualizar.CiudadDestinoId = paramsUpdate.CiudadDestinoId;
                vueloActualizar.VueloPrecio = paramsUpdate.VueloPrecio;
                vueloActualizar.VueloDescuento = paramsUpdate.VueloDescuento;
                vueloActualizar.VueloFechaHoraSalida = paramsUpdate.VueloFechaHoraSalida;
                vueloActualizar.VueloFechaHoraLlegada = paramsUpdate.VueloFechaHoraLlegada;

                await _iUnitOfWork.Repository<Vuelo>().ActualizarAsync(vueloActualizar);
                await _iUnitOfWork.SaveChangesAsync();

                // Guarda inmediatamente el historico del vuelo
                vueloActualizar.UsuarioCreacionId = userId;
                await CreateVueloHistorico(vueloActualizar);

                scope.Complete();
                return vueloActualizar;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<BusquedaVueloHistoricoPoco>> GetWithParamsAsync(ParamsConsultarVueloHistorico paramsSearch, int? userId = null)
        {
            return await _iUnitOfWork.DLVueloPersonalizado.ConsultarVueloHistorico(paramsSearch);
        }

        public async Task<Vuelo> ActualizarEstadoVuelo(ParamsActualizarEstadoVuelo updateEstado, int userId) 
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

                objetoEntidad.UsuarioCreacionId = userId;
                objetoEntidad.VueloFechaCreacion = DateTime.Now;

                // Guarda inmediatamente el historico del vuelo con los cambios realizados
                await CreateVueloHistorico(objetoEntidad);

                scope.Complete();
                return objetoEntidad;
            }
        }

        /// <summary>Contrato personalizado para la consulta de los asientos de un vuelo.</summary>
        /// <param name="searchSeatFlight">Parametros con el que se buscara los asientos de un vuelo bajo ciertos criterios.</param>
        /// <returns>Lista AsientosVueloPoco con la información de los asientos del vuelo buscados.</returns>
        public async Task<IEnumerable<AsientosVueloPoco>> ConsultarVueloAsientoPersonalizado(ParamsBusquedaAsientoVuelo searchSeatFlight)
        {
            return await _iUnitOfWork.DLVueloPersonalizado.ConsultarVuelosAsientos(searchSeatFlight);
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

        private async Task<IEnumerable<EstadoVuelo>> ConsultaEstadoVuelo()
        {
            return await _iUnitOfWork.Repository<EstadoVuelo>().ConsultarTodosAsync();
        }

        private async Task<IEnumerable<VueloAsiento>> ConsultaVueloAsientos(int vueloId)
        {
            VueloAsiento entidad = new VueloAsiento();
            entidad.VueloId = vueloId;
            entidad.AsientoAvionId = null;
            entidad.VueloAsientoReservado = null;
            entidad.VueloAsientoComprado = null;
            Expression<Func<VueloAsiento, bool>> filtro = entidad.ToFilterExpression<VueloAsiento>();
            return await _iUnitOfWork.Repository<VueloAsiento>().ConsultarListaAsync(filtro);
        }

        #endregion

        #region ValidacionCampos
        /// <summary>Valida los campos obligatorios para realizar la creacion del vuelo.</summary>
        /// <param name="parametrosCrearVuelo">El objeto de tipo ParamsCrearVuelo que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCrearVuelo(ParamsCrearVuelo parametrosCrearVuelo)
        {
            string errores = string.Empty;
            IEnumerable<Vuelo> Resultado = new List<Vuelo>();
            ParamsConsultarVuelo Existentes = new ParamsConsultarVuelo();
            Resultado = await GetWithParamsAsync(Existentes);
            IEnumerable<EstadoVuelo> ResultadoEstadoVuelos = await ConsultaEstadoVuelo();

            // Validar el codigo del vuelo
            if (string.IsNullOrEmpty(errores))
            {
                if (string.IsNullOrEmpty(parametrosCrearVuelo.VueloCodigo))
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "codigo");
                }
                else
                {
                    // Valida si el codigo del vuelo ya existe en el sistema
                    if (Resultado.Any(x => x.VueloCodigo.Trim().ToUpper() == parametrosCrearVuelo.VueloCodigo.Trim().ToUpper()))
                    {
                        errores += string.Format(DefaultMessages.AlreadyExistsData, $"el codigo de vuelo '{parametrosCrearVuelo.VueloCodigo.Trim()}'");
                    }
                }
            }

            // Validar el avion
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearVuelo.AvionId == null || parametrosCrearVuelo.AvionId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "avión");
                }
                else
                {
                    // Consulta los aviones para validar si existe o no
                    IEnumerable<BusquedaAvionPoco> aviones = new List<BusquedaAvionPoco>();
                    ParamsConsultarAvion paramsConsultarAvion = new ParamsConsultarAvion();
                    paramsConsultarAvion.AvionId = parametrosCrearVuelo.AvionId;
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
                IEnumerable<BusquedaAvionPoco> aviones = new List<BusquedaAvionPoco>();
                ParamsConsultarAvion paramsConsultarAvion = new ParamsConsultarAvion();
                paramsConsultarAvion.AvionId = parametrosCrearVuelo.AvionId;
                paramsConsultarAvion.AvionEstado = true;
                aviones = await _iAvionService.GetWithParamsAsync(paramsConsultarAvion);

                if (aviones.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.NotActiveData, "El avión");
                }
            }

            // Validar que el avión por lo menos tenga x cantidad de asientos disponibles
            if (string.IsNullOrEmpty(errores))
            {
                // Obtiene el parametro que define cuantos asientos disponibles son el minimo que debe tener el avión para poderse usar en un vuelo
                IEnumerable<Parametros> MinimoAsientosAvionVuelo;
                ParamsConsultarParametros parametrosBusqueda = new ParamsConsultarParametros();
                parametrosBusqueda.ParametrosNombre = "MinimoAsientosAvionVuelo";
                MinimoAsientosAvionVuelo = await _iConfiguracionService.GetWithParamsAsync(parametrosBusqueda);

                if (MinimoAsientosAvionVuelo.Count() > 0)
                {
                    // Consulta la cantidad de asientos disponibles que tiene el avión asociado al vuelo
                    IEnumerable<AsientoAvion> asientosAvion = new List<AsientoAvion>();
                    ParamsConsultarAsientoAvion paramsConsultarAsientoAvion = new ParamsConsultarAsientoAvion();
                    paramsConsultarAsientoAvion.AvionId = parametrosCrearVuelo.AvionId;
                    paramsConsultarAsientoAvion.AsientoAvionEstado = true;
                    asientosAvion = await _iAvionService.GetWithParamsAsync(paramsConsultarAsientoAvion);
                    int? valorParametro = Convert.ToInt32(MinimoAsientosAvionVuelo.FirstOrDefault()!.ParametrosValor);

                    if (asientosAvion.Count() < valorParametro)
                    {
                        errores += "La cantidad minima de asientos disponibles del avión asociado al vuelo debe ser: "+ valorParametro.ToString();
                    }
                }
            }

            // Validar la ciudad de origen
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearVuelo.CiudadOrigenId == null || parametrosCrearVuelo.CiudadOrigenId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "ciudad de origen");
                }
                else
                {
                    // Valida si la ciudad ingresada existe
                    IEnumerable<BusquedaCiudadPoco> ciudades = new List<BusquedaCiudadPoco>();
                    ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                    paramsConsultarCiudad.CiudadId = parametrosCrearVuelo.CiudadOrigenId;
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
                IEnumerable<BusquedaCiudadPoco> ciudades = new List<BusquedaCiudadPoco>();
                ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                paramsConsultarCiudad.CiudadId = parametrosCrearVuelo.CiudadOrigenId;
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
                IEnumerable<BusquedaAvionPoco> aviones = new List<BusquedaAvionPoco>();
                ParamsConsultarAvion paramsConsultarAvion = new ParamsConsultarAvion();
                paramsConsultarAvion.AvionId = parametrosCrearVuelo.AvionId;
                paramsConsultarAvion.CiudadId = parametrosCrearVuelo.CiudadOrigenId;
                aviones = await _iAvionService.GetWithParamsAsync(paramsConsultarAvion);

                if (aviones.Count() == 0)
                {
                    errores += "El avión no se encuentra en la ciudad de origen.";
                }
            }

            // Validar la ciudad de destino
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearVuelo.CiudadDestinoId == null || parametrosCrearVuelo.CiudadDestinoId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "ciudad de destino");
                }
                else
                {
                    // Valida si la ciudad ingresada existe
                    IEnumerable<BusquedaCiudadPoco> ciudades = new List<BusquedaCiudadPoco>();
                    ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                    paramsConsultarCiudad.CiudadId = parametrosCrearVuelo.CiudadDestinoId;
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
                IEnumerable<BusquedaCiudadPoco> ciudades = new List<BusquedaCiudadPoco>();
                ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                paramsConsultarCiudad.CiudadId = parametrosCrearVuelo.CiudadDestinoId;
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
                if (parametrosCrearVuelo.CiudadOrigenId == parametrosCrearVuelo.CiudadDestinoId)
                {
                    errores += "La ciudad de origen y ciudad destino deben ser diferentes";
                }
            }

            // Validar el precio del vuelo por asiento
            if (string.IsNullOrEmpty(errores)) 
            {
                if (parametrosCrearVuelo.VueloPrecio == null || parametrosCrearVuelo.VueloPrecio == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "precio");
                }
                else
                {
                    if (parametrosCrearVuelo.VueloPrecio <= 0)
                    {
                        errores += string.Format(DefaultMessages.InvalidNumberMin, "El precio", "0");
                    }
                }

            }
            
            // Validar el precio de descuento (Si viene que sea mayor a 0.01...)
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearVuelo.VueloDescuento != null)
                {
                    if (parametrosCrearVuelo.VueloDescuento < 0 || parametrosCrearVuelo.VueloDescuento > 100)
                    {
                        errores += string.Format(DefaultMessages.RangeError, "El porcentaje de descuento", "0", "100");
                    }
                }
            }

            // Validar fecha de salida 
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearVuelo.VueloFechaHoraSalida == null)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "fecha salida");
                }
                else
                {
                    if (parametrosCrearVuelo.VueloFechaHoraSalida <= DateTime.Now)
                    {
                        errores += "La fecha de salida no debe ser menor o igual a la hora actual";
                    }
                }
            }

            // Validar fecha de llegada 
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosCrearVuelo.VueloFechaHoraLlegada == null)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "fecha llegada");
                }
                else
                {
                    if (parametrosCrearVuelo.VueloFechaHoraLlegada <= parametrosCrearVuelo.VueloFechaHoraSalida)
                    {
                        errores += "La fecha de llegada no debe ser menor o igual a la fecha de salida";
                    }
                }
            }

            // Validar solapamiento de vuelo por avión
            if (string.IsNullOrEmpty(errores))
            {
                IEnumerable<Vuelo> vuelosMismoAvion = Resultado
                    .Where(v => v.AvionId == parametrosCrearVuelo.AvionId && v.EstadoVueloId != (int)Domain.General.Enums.Enum.EstadoVuelo.Cancelado);

                foreach (Vuelo vuelo in vuelosMismoAvion)
                {
                    bool solapado =
                        (parametrosCrearVuelo.VueloFechaHoraSalida < vuelo.VueloFechaHoraLlegada &&
                         parametrosCrearVuelo.VueloFechaHoraLlegada > vuelo.VueloFechaHoraSalida);

                    if (solapado)
                    {
                        errores += $"El avión asignado ya tiene un vuelo programado que se solapa con el rango {vuelo.VueloFechaHoraSalida} - {vuelo.VueloFechaHoraLlegada}. ";
                        break;
                    }
                }
            }

            // Validar vuelos duplicados entre mismas ciudades en la misma fecha
            if (string.IsNullOrEmpty(errores))
            {
                // Obtiene el parametro que define diferencia de segundos entre vuelos con la misma ciudad de origen y destino
                int cantidadSegundosDiferenciaVuelosMismoOrigenDestino = 0;
                IEnumerable<Parametros> TiempoMaximoEntreVueloMismoOrigenDestino;
                ParamsConsultarParametros parametrosBusqueda = new ParamsConsultarParametros();
                parametrosBusqueda.ParametrosNombre = "TiempoMaximoEntreVueloMismoOrigenDestino";
                TiempoMaximoEntreVueloMismoOrigenDestino = await _iConfiguracionService.GetWithParamsAsync(parametrosBusqueda);

                if (TiempoMaximoEntreVueloMismoOrigenDestino.Count() > 0)
                {
                    cantidadSegundosDiferenciaVuelosMismoOrigenDestino = Convert.ToInt32(TiempoMaximoEntreVueloMismoOrigenDestino.FirstOrDefault()!.ParametrosValor);
                }

                IEnumerable<Vuelo> vuelosMismoTrayecto = Resultado.Where(v => v.CiudadOrigenId == parametrosCrearVuelo.CiudadOrigenId &&
                                                                                                                  v.CiudadDestinoId == parametrosCrearVuelo.CiudadDestinoId &&
                                                                                                                  v.EstadoVueloId != (int)Domain.General.Enums.Enum.EstadoVuelo.Cancelado);

                foreach (Vuelo vuelo in vuelosMismoTrayecto)
                {
                    // Calcula la diferencia en segundos
                    var diferenciaSegundos = Math.Abs((parametrosCrearVuelo.VueloFechaHoraSalida - vuelo.VueloFechaHoraSalida)?.TotalSeconds ?? 0);

                    if (diferenciaSegundos < cantidadSegundosDiferenciaVuelosMismoOrigenDestino)
                    {
                        var diferenciaMinutos = Math.Round(cantidadSegundosDiferenciaVuelosMismoOrigenDestino / 60.0, 1);
                        errores += $"Ya existe un vuelo programado para el mismo trayecto con menos de {diferenciaMinutos} minutos de diferencia en la fecha de salida. ";
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }

        /// <summary>Valida los campos obligatorios para realizar la actualización del vuelo.</summary>
        /// <param name="parametrosActualizarrVuelo">El objeto de tipo ParamsActualizarVuelo que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposActualizarVuelo(ParamsActualizarVuelo parametrosActualizarVuelo)
        {
            string errores = string.Empty;
            // Obtiene los vuelos del sistema
            IEnumerable<Vuelo> Resultado = new List<Vuelo>();
            ParamsConsultarVuelo Existentes = new ParamsConsultarVuelo();
            Resultado = await GetWithParamsAsync(Existentes);

            // Valida si existe el vuelo en el sistema
            if (Resultado.Where(x => x.VueloId == parametrosActualizarVuelo.VueloId).Count() == 0)
            {
                errores += string.Format(DefaultMessages.DataNotFound, "El vuelo");
            }

            // Si el vuelo tiene un estado diferente de programado, no se podrá modificar codigoVuelo, Ciudades o Fechas
            if (string.IsNullOrEmpty(errores))
            {
                // Obtiene el vuelo
                Vuelo vueloActualizar = new Vuelo();
                vueloActualizar = Resultado.Where(x => x.VueloId == parametrosActualizarVuelo.VueloId).FirstOrDefault();

                bool codigo = vueloActualizar.VueloCodigo == parametrosActualizarVuelo.VueloCodigo ? true : false;
                bool ciudadOrigen = vueloActualizar.CiudadOrigenId == parametrosActualizarVuelo.CiudadOrigenId ? true : false;
                bool ciudadDestino = vueloActualizar.CiudadDestinoId == parametrosActualizarVuelo.CiudadDestinoId ? true : false;
                bool fechaHoraSalida = vueloActualizar.VueloFechaHoraSalida == parametrosActualizarVuelo.VueloFechaHoraSalida ? true : false;
                bool fechaHoraLlegada = vueloActualizar.VueloFechaHoraLlegada == parametrosActualizarVuelo.VueloFechaHoraLlegada ? true : false;

                bool precio = vueloActualizar.VueloPrecio == parametrosActualizarVuelo.VueloPrecio ? true : false;
                bool descuento = vueloActualizar.VueloDescuento == parametrosActualizarVuelo.VueloDescuento ? true : false;

                // Si tratan de cambiar el codigo de vuelo, ciudades o fechas se debe validar que no esté diferente de programado 
                if (!codigo || !ciudadOrigen || !ciudadDestino || !fechaHoraSalida || !fechaHoraLlegada)
                {
                    if (vueloActualizar.EstadoVueloId != (int)Domain.General.Enums.Enum.EstadoVuelo.Programado)
                    {
                        errores += "No se permite actualizar el vuelo, ya que se encuentra en un estado diferente de programado";
                    }
                }
                else if (!precio || !descuento)
                {
                    // Si tratan de cambiar el precio o descuento se debe valir que sea diferente de programado, disponible o cerrado
                    if (vueloActualizar.EstadoVueloId != (int)Domain.General.Enums.Enum.EstadoVuelo.Programado &&
                        vueloActualizar.EstadoVueloId != (int)Domain.General.Enums.Enum.EstadoVuelo.Disponible &&
                        vueloActualizar.EstadoVueloId != (int)Domain.General.Enums.Enum.EstadoVuelo.Cerrado)
                    {
                        errores += "No se permite actualizar el vuelo, ya que se encuentra en un estado diferente de programado, disponible o cerrado";
                    }
                }
            }

            // Validar el codigo del vuelo
            if (string.IsNullOrEmpty(errores))
            {
                if (string.IsNullOrEmpty(parametrosActualizarVuelo.VueloCodigo))
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "codigo");
                }
                else
                {
                    // Valida si el codigo del vuelo ya existe en el sistema
                    if (Resultado.Any(x => x.VueloCodigo.Trim().ToUpper() == parametrosActualizarVuelo.VueloCodigo.Trim().ToUpper() &&
                                                                        x.VueloId != parametrosActualizarVuelo.VueloId))
                    {
                        errores += string.Format(DefaultMessages.AlreadyExistsData, $"el codigo de vuelo '{parametrosActualizarVuelo.VueloCodigo.Trim()}'");
                    }
                }
            }

            // Validar la ciudad de origen
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosActualizarVuelo.CiudadOrigenId == null || parametrosActualizarVuelo.CiudadOrigenId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "ciudad de origen");
                }
                else
                {
                    // Valida si la ciudad ingresada existe
                    IEnumerable<BusquedaCiudadPoco> ciudades = new List<BusquedaCiudadPoco>();
                    ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                    paramsConsultarCiudad.CiudadId = parametrosActualizarVuelo.CiudadOrigenId;
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
                IEnumerable<BusquedaCiudadPoco> ciudades = new List<BusquedaCiudadPoco>();
                ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                paramsConsultarCiudad.CiudadId = parametrosActualizarVuelo.CiudadOrigenId;
                paramsConsultarCiudad.CiudadEstado = true;
                ciudades = await _iAdministracionService.GetWithParamsAsync(paramsConsultarCiudad);

                if (ciudades.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.NotActiveData, "La ciudad de origen");
                }
            }

            // Validar la ciudad de destino
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosActualizarVuelo.CiudadDestinoId == null || parametrosActualizarVuelo.CiudadDestinoId == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "ciudad de destino");
                }
                else
                {
                    // Valida si la ciudad ingresada existe
                    IEnumerable<BusquedaCiudadPoco> ciudades = new List<BusquedaCiudadPoco>();
                    ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                    paramsConsultarCiudad.CiudadId = parametrosActualizarVuelo.CiudadDestinoId;
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
                IEnumerable<BusquedaCiudadPoco> ciudades = new List<BusquedaCiudadPoco>();
                ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                paramsConsultarCiudad.CiudadId = parametrosActualizarVuelo.CiudadDestinoId;
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
                if (parametrosActualizarVuelo.CiudadOrigenId == parametrosActualizarVuelo.CiudadDestinoId)
                {
                    errores += "La ciudad de origen y ciudad destino deben ser diferentes";
                }
            }

            // Validar el precio del vuelo por asiento
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosActualizarVuelo.VueloPrecio == null || parametrosActualizarVuelo.VueloPrecio == 0)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "precio");
                }
                else
                {
                    if (parametrosActualizarVuelo.VueloPrecio <= 0)
                    {
                        errores += string.Format(DefaultMessages.InvalidNumberMin, "El precio", "0");
                    }
                }

            }

            // Validar el precio de descuento (Si viene que sea mayor a 0.01...)
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosActualizarVuelo.VueloDescuento != null)
                {
                    if (parametrosActualizarVuelo.VueloDescuento < 0 || parametrosActualizarVuelo.VueloDescuento > 100)
                    {
                        errores += string.Format(DefaultMessages.RangeError, "El porcentaje de descuento", "0", "100");
                    }
                }
            }

            // Validar fecha de salida 
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosActualizarVuelo.VueloFechaHoraSalida == null)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "fecha salida");
                }
                else
                {
                    if (parametrosActualizarVuelo.VueloFechaHoraSalida <= DateTime.Now)
                    {
                        errores += "La fecha de salida no debe ser menor o igual a la hora actual";
                    }
                }
            }

            // Validar fecha de llegada 
            if (string.IsNullOrEmpty(errores))
            {
                if (parametrosActualizarVuelo.VueloFechaHoraLlegada == null)
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "fecha llegada");
                }
                else
                {
                    if (parametrosActualizarVuelo.VueloFechaHoraLlegada <= parametrosActualizarVuelo.VueloFechaHoraSalida)
                    {
                        errores += "La fecha de llegada no debe ser menor o igual a la fecha de salida";
                    }
                }
            }

            // Validar vuelos duplicados entre mismas ciudades en la misma fecha
            if (string.IsNullOrEmpty(errores))
            {
                // Obtiene el parametro que define diferencia de segundos entre vuelos con la misma ciudad de origen y destino
                int TiempoMaximoEntreVueloMismoOrigenDestino = 0;
                IEnumerable<Parametros> TiempoMaximoVueloEntreVueloMismoOrigenDestino;
                ParamsConsultarParametros parametrosBusqueda = new ParamsConsultarParametros();
                parametrosBusqueda.ParametrosNombre = "TiempoMaximoEntreVueloMismoOrigenDestino";
                TiempoMaximoVueloEntreVueloMismoOrigenDestino = await _iConfiguracionService.GetWithParamsAsync(parametrosBusqueda);

                if (TiempoMaximoVueloEntreVueloMismoOrigenDestino.Count() > 0)
                {
                    TiempoMaximoEntreVueloMismoOrigenDestino = Convert.ToInt32(TiempoMaximoVueloEntreVueloMismoOrigenDestino.FirstOrDefault()!.ParametrosValor);
                }

                IEnumerable<Vuelo> vuelosMismoTrayecto = Resultado.Where(v => v.CiudadOrigenId == parametrosActualizarVuelo.CiudadOrigenId &&
                                                                                                                  v.CiudadDestinoId == parametrosActualizarVuelo.CiudadDestinoId &&
                                                                                                                  v.VueloId != parametrosActualizarVuelo.VueloId &&
                                                                                                                  v.EstadoVueloId != (int)Domain.General.Enums.Enum.EstadoVuelo.Cancelado);

                foreach (Vuelo vuelo in vuelosMismoTrayecto)
                {
                    // Calcula la diferencia en segundos
                    var diferenciaSegundos = Math.Abs((parametrosActualizarVuelo.VueloFechaHoraSalida - vuelo.VueloFechaHoraSalida)?.TotalSeconds ?? 0);

                    if (diferenciaSegundos < TiempoMaximoEntreVueloMismoOrigenDestino)
                    {
                        var diferenciaMinutos = Math.Round(TiempoMaximoEntreVueloMismoOrigenDestino / 60.0, 1);
                        errores += $"Ya existe un vuelo programado para el mismo trayecto con menos de {diferenciaMinutos} minutos de diferencia en la fecha de salida. ";
                        break;
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

                        errores += $"No se puede cambiar el estado del vuelo de '{nombreEstadoActual}' a '{nombreEstadoCambio}'.";
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
