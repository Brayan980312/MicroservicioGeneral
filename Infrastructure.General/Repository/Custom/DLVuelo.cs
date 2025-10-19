namespace Infrastructure.General.Repository.Custom
{
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.Repository;
    using Infrastructure.General.Persistence.Context;
    using Microsoft.EntityFrameworkCore;
    using Utilitarios.Data;
    using static Domain.General.Enums.Enum;

    public class DLVuelo : CrudSqlRepositorio<Vuelo>, IDLVuelo
    {
        #region Variables

        /// <summary>Implementacion contexto.</summary>
        private readonly GeneralContext _GeneralContext;
        #endregion

        #region Constructor

        /// <summary>Inicializa la clase DLVuelo.</summary>
        /// <param name="GeneralContext">Contexto bd.</param>
        public DLVuelo(GeneralContext GeneralContext) : base(GeneralContext)
        {
            this._GeneralContext = GeneralContext;
        }

        #endregion

        /// <summary>Consulta personalizada que obtiene los vuelos del sistema con su correspondientes relaciones (EstadoVuelo, Avion, Ciudades).</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsConsultarVuelo que contiene los criterios de búsqueda para consultar los vuelos del sistema.</param>
        /// <returns>Lista BusquedaVueloPoco que cumplen con los filtros de búsqueda.</returns>
        public async Task<IEnumerable<BusquedaVueloPoco>> ConsultarVuelos(ParamsConsultarVuelo objSearch)
        {
            var vuelos = await _GeneralContext.Vuelo
                 .Include(estadoVuelo => estadoVuelo.EstadoVuelo)
                 .Include(avion => avion.Avion)
                 .Include(ciudadOrigen => ciudadOrigen.CiudadOrigen)
                    .ThenInclude(paisOrigen => paisOrigen.Pais)
                 .Include(ciudadDestino => ciudadDestino.CiudadDestino)
                    .ThenInclude(paisDestino => paisDestino.Pais)
                 .Where(vuelo => (vuelo.VueloId == objSearch.VueloId || objSearch.VueloId == null || objSearch.VueloId == 0) &&
                                                  (vuelo.VueloCodigo == objSearch.VueloCodigo || objSearch.VueloCodigo == null || objSearch.VueloCodigo == "") &&
                                                  (vuelo.EstadoVueloId == objSearch.EstadoVueloId || objSearch.EstadoVueloId == null || objSearch.EstadoVueloId == 0) &&
                                                  (vuelo.AvionId == objSearch.AvionId || objSearch.AvionId == null || objSearch.AvionId == 0) &&
                                                  (vuelo.CiudadOrigenId == objSearch.CiudadOrigenId || objSearch.CiudadOrigenId == null || objSearch.CiudadOrigenId == 0) &&
                                                  (vuelo.CiudadDestinoId == objSearch.CiudadDestinoId || objSearch.CiudadDestinoId == null || objSearch.CiudadDestinoId == 0) &&
                                                  (
                                                        objSearch.VueloFechaHoraSalida == null ||
                                                        vuelo.VueloFechaHoraSalida.Value.Date == objSearch.VueloFechaHoraSalida.Value.Date
                                                    ) &&
                                                    (
                                                        objSearch.VueloFechaHoraLlegada == null ||
                                                        vuelo.VueloFechaHoraLlegada.Value.Date == objSearch.VueloFechaHoraLlegada.Value.Date
                                                    )) 
                 .Select(vuelos => new BusquedaVueloPoco
                    {
                        VueloId = vuelos.AvionId,
                        VueloCodigo = vuelos.VueloCodigo,
                        EstadoVueloId = vuelos.EstadoVueloId,
                        EstadoVueloNombre = vuelos.EstadoVuelo.EstadoVueloNombre,
                        AvionId = vuelos.AvionId,
                        AvionNombre = vuelos.Avion.AvionNombre,
                        CiudadOrigenId = vuelos.CiudadOrigenId,
                        CiudadOrigenNombre = vuelos.CiudadOrigen.CiudadNombre,
                        CiudadOrigenNomenclatura = vuelos.CiudadOrigen.CiudadNomenclatura,
                        CiudadOrigenNombreNomenclatura = vuelos.CiudadOrigen.CiudadNombre + " (" + vuelos.CiudadOrigen.CiudadNomenclatura + ")",
                        PaisOrigenId = vuelos.CiudadOrigen.Pais.PaisId,
                        PaisOrigenNombre = vuelos.CiudadOrigen.Pais.PaisNombre,
                        PaisOrigenNomenclatura = vuelos.CiudadOrigen.Pais.PaisNomenclatura,
                        PaisOrigenNombreNomenclatura = vuelos.CiudadOrigen.Pais.PaisNombre + " (" + vuelos.CiudadOrigen.Pais.PaisNomenclatura + ")",
                        CiudadDestinoId = vuelos.CiudadDestinoId,
                        CiudadDestinoNombre = vuelos.CiudadDestino.CiudadNombre,
                        CiudadDestinoNomenclatura = vuelos.CiudadDestino.CiudadNomenclatura,
                        CiudadDestinoNombreNomenclatura = vuelos.CiudadDestino.CiudadNombre + " (" + vuelos.CiudadDestino.CiudadNomenclatura + ")",
                        PaisDestinoId = vuelos.CiudadDestino.Pais.PaisId,
                        PaisDestinoNombre = vuelos.CiudadDestino.Pais.PaisNombre,
                        PaisDestinoNomenclatura = vuelos.CiudadDestino.Pais.PaisNomenclatura,
                        PaisDestinoNombreNomenclatura = vuelos.CiudadDestino.Pais.PaisNombre + " (" + vuelos.CiudadDestino.Pais.PaisNomenclatura +")",
                        VueloPrecio= vuelos.VueloPrecio,
                        VueloDescuento = vuelos.VueloDescuento,
                        VueloFechaHoraSalida = vuelos.VueloFechaHoraSalida,
                        VueloFechaHoraLlegada = vuelos.VueloFechaHoraLlegada
                    }
                 )
                 .AsNoTracking()
                 .ToListAsync();
            return vuelos;
        }

        /// <summary>Consulta personalizada que obtiene los vuelos disponibles del sistema.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsBusquedaVuelosDisponibles que contiene los criterios de búsqueda para consultar los vuelos disponibles del sistema.</param>
        /// <returns>Lista BusquedaVuelosDisponiblesPoco que cumplen con los filtros de búsqueda.</returns>
        public async Task<IEnumerable<BusquedaVuelosDisponiblesPoco>> ConsultarVuelosDisponibles(ParamsBusquedaVuelosDisponibles objSearch)
        {
            var vuelos = await _GeneralContext.Vuelo
                 .Include(ciudadOrigen => ciudadOrigen.CiudadOrigen)
                    .ThenInclude(paisOrigen => paisOrigen.Pais)
                 .Include(ciudadDestino => ciudadDestino.CiudadDestino)
                    .ThenInclude(paisDestino => paisDestino.Pais)
                 .Where(vuelo => vuelo.EstadoVueloId == (int)Domain.General.Enums.Enum.EstadoVuelo.Disponible &&
                                                  (vuelo.CiudadOrigenId == objSearch.CiudadOrigenId || objSearch.CiudadOrigenId == null || objSearch.CiudadOrigenId == 0) &&
                                                  (vuelo.CiudadDestinoId == objSearch.CiudadDestinoId || objSearch.CiudadDestinoId == null || objSearch.CiudadDestinoId == 0) &&
                                                  vuelo.VueloFechaHoraSalida.Value.Date == objSearch.VueloFechaSalida.Date)
                 .Select(vuelos => new BusquedaVuelosDisponiblesPoco
                 {
                     VueloId = vuelos.AvionId,
                     CiudadOrigenNombre = vuelos.CiudadOrigen.CiudadNombre,
                     CiudadOrigenNomenclatura = vuelos.CiudadOrigen.CiudadNomenclatura,
                     CiudadOrigenNombreNomenclatura = vuelos.CiudadOrigen.CiudadNombre + " (" + vuelos.CiudadOrigen.CiudadNomenclatura + ")",
                     PaisOrigenNombre = vuelos.CiudadOrigen.Pais.PaisNombre,
                     PaisOrigenNomenclatura = vuelos.CiudadOrigen.Pais.PaisNomenclatura,
                     PaisOrigenNombreNomenclatura = vuelos.CiudadOrigen.Pais.PaisNombre + " (" + vuelos.CiudadOrigen.Pais.PaisNomenclatura + ")",
                     CiudadDestinoNombre = vuelos.CiudadDestino.CiudadNombre,
                     CiudadDestinoNomenclatura = vuelos.CiudadDestino.CiudadNomenclatura,
                     CiudadDestinoNombreNomenclatura = vuelos.CiudadDestino.CiudadNombre + " (" + vuelos.CiudadDestino.CiudadNomenclatura + ")",
                     PaisDestinoNombre = vuelos.CiudadDestino.Pais.PaisNombre,
                     PaisDestinoNomenclatura = vuelos.CiudadDestino.Pais.PaisNomenclatura,
                     PaisDestinoNombreNomenclatura = vuelos.CiudadDestino.Pais.PaisNombre + " (" + vuelos.CiudadDestino.Pais.PaisNomenclatura + ")",
                     VueloPrecio = vuelos.VueloPrecio,
                     VueloDescuento = vuelos.VueloDescuento,
                     VueloFechaHoraSalida = vuelos.VueloFechaHoraSalida,
                     VueloFechaHoraLlegada = vuelos.VueloFechaHoraLlegada
                 }
                 )
                 .AsNoTracking()
                 .ToListAsync();
            return vuelos;
        }

        /// <summary>Consulta personalizada que obtiene los asientos que tiene un vuelo para validar en que estado están.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsBusquedaAsientoVuelo que contiene los criterios de búsqueda para obtener los asientos de un vuelo.</param>
        /// <returns>Lista AsientosVueloPoco que cumplen con los filtros de búsqueda.</returns>
        public async Task<IEnumerable<AsientosVueloPoco>> ConsultarVuelosAsientos(ParamsBusquedaAsientoVuelo objSearch)
        {
            var asientosVuelo = await _GeneralContext.VueloAsiento
     .Include(v => v.AsientoAvion)
     .Include(v => v.CompraDetalle)
     .Where(v => objSearch.VueloId == null || objSearch.VueloId == 0 || v.VueloId == objSearch.VueloId)
     .Select(v => new AsientosVueloPoco
     {
         VueloAsientoId = v.VueloAsientoId,
         AsientoAvionId = v.AsientoAvionId,

         VueloAsientoEstado =
             (!(v.VueloAsientoReservado ?? false) && !(v.VueloAsientoComprado ?? false))
                 ? "Disponible"
                 : (v.VueloAsientoComprado ?? false)
                     ? "Comprado"
                     : (v.VueloAsientoReservado ?? false)
                         ? "Reservado"
                         : "",

         AsientoAvionNombre = v.AsientoAvion.AsientoAvionNombre,
         AsientoAvionVIP = (v.AsientoAvion.AsientoAvionVIP ?? false) ? "VIP" : "",
         CompraDetalleNombrePasajero = v.CompraDetalle != null ? v.CompraDetalle.CompraDetalleNombrePasajero : "",
         CompraDetalleIdentificacionPasajero = v.CompraDetalle != null ? v.CompraDetalle.CompraDetalleIdentificacionPasajero : ""
     })
     .AsNoTracking()
     .ToListAsync();

            return asientosVuelo;
        }

        /// <summary>Consulta personalizada que obtiene el historico de un vuelo con su correspondientes relaciones (EstadoVuelo, Avion, Ciudades).</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsConsultarVueloHistorico que contiene los criterios de búsqueda para consultar el historico de un vuelo.</param>
        /// <returns>Lista BusquedaVueloHistoricoPoco que cumplen con los filtros de búsqueda.</returns>
        public async Task<IEnumerable<BusquedaVueloHistoricoPoco>> ConsultarVueloHistorico(ParamsConsultarVueloHistorico objSearch)
        {
            var vuelos = await _GeneralContext.VueloHistorico
                 .Include(estadoVuelo => estadoVuelo.EstadoVuelo)
                 .Include(avion => avion.Avion)
                 .Include(ciudadOrigen => ciudadOrigen.CiudadOrigen)
                 .Include(ciudadDestino => ciudadDestino.CiudadDestino)
                 .Include(ciudadDestino => ciudadDestino.Usuario)
                 .Where(vuelo => vuelo.VueloId == objSearch.VueloId || objSearch.VueloId == null || objSearch.VueloId == 0)
                 .Select(vuelos => new BusquedaVueloHistoricoPoco
                 {
                     VueloHistoricoCodigo = vuelos.VueloHistoricoCodigo,
                     EstadoVueloId = vuelos.EstadoVueloId,
                     EstadoVueloNombre = vuelos.EstadoVuelo.EstadoVueloNombre,
                     AvionNombre = vuelos.Avion.AvionNombre,
                     CiudadOrigenNombre = vuelos.CiudadOrigen.CiudadNombre,
                     CiudadOrigenNomenclatura = vuelos.CiudadOrigen.CiudadNomenclatura,
                     CiudadOrigenNombreNomenclatura = vuelos.CiudadOrigen.CiudadNombre + " (" + vuelos.CiudadOrigen.CiudadNomenclatura + ")",
                     CiudadDestinoNombre = vuelos.CiudadDestino.CiudadNombre,
                     CiudadDestinoNomenclatura = vuelos.CiudadDestino.CiudadNomenclatura,
                     CiudadDestinoNombreNomenclatura = vuelos.CiudadDestino.CiudadNombre + " (" + vuelos.CiudadDestino.CiudadNomenclatura + ")",
                     VueloHistoricoPrecio = vuelos.VueloHistoricoPrecio,
                     VueloHistoricoDescuento = vuelos.VueloHistoricoDescuento,
                     VueloHistoricoFechaHoraSalida = vuelos.VueloHistoricoFechaHoraSalida,
                     VueloHistoricoFechaHoraLlegada = vuelos.VueloHistoricoFechaHoraLlegada,
                     UsuarioNombreCompleto = vuelos.Usuario.UsuarioNombreCompleto,
                     VueloHistoricoFechaCreacion = vuelos.VueloHistoricoFechaCreacion
                 }
                 )
                 .AsNoTracking()
                 .ToListAsync();
            return vuelos;
        }
    }
}
