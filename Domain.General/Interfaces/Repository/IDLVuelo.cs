namespace Domain.General.Interfaces.Repository
{
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Utilitarios.Contracts;
    public interface IDLVuelo : ICrudSqlRepositorio<Vuelo>
    {
        /// <summary>Consulta personalizada que obtiene los vuelos del sistema con su correspondientes relaciones (EstadoVuelo, Avion, Ciudades).</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsConsultarVuelo que contiene los criterios de búsqueda para consultar los vuelos del sistema.</param>
        /// <returns>Lista BusquedaVueloPoco que cumplen con los filtros de búsqueda.</returns>
        Task<IEnumerable<BusquedaVueloPoco>> ConsultarVuelos(ParamsConsultarVuelo objSearch);

        /// <summary>Consulta personalizada que obtiene los vuelos disponibles del sistema.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsBusquedaVuelosDisponibles que contiene los criterios de búsqueda para consultar los vuelos disponibles del sistema.</param>
        /// <returns>Lista BusquedaVuelosDisponiblesPoco que cumplen con los filtros de búsqueda.</returns>
        Task<IEnumerable<BusquedaVuelosDisponiblesPoco>> ConsultarVuelosDisponibles(ParamsBusquedaVuelosDisponibles objSearch);

        /// <summary>Consulta personalizada que obtiene los asientos que tiene un vuelo para validar en que estado están.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsBusquedaAsientoVuelo que contiene los criterios de búsqueda para obtener los asientos de un vuelo.</param>
        /// <returns>Lista AsientosVueloPoco que cumplen con los filtros de búsqueda.</returns>
        Task<IEnumerable<AsientosVueloPoco>> ConsultarVuelosAsientos(ParamsBusquedaAsientoVuelo objSearch);

        /// <summary>Consulta personalizada que obtiene el historico de un vuelo.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsConsultarVueloHistorico que contiene los criterios de búsqueda para consultar el historico de un vuelo.</param>
        /// <returns>Lista BusquedaVueloHistoricoPoco que cumplen con los filtros de búsqueda.</returns>
        Task<IEnumerable<BusquedaVueloHistoricoPoco>> ConsultarVueloHistorico(ParamsConsultarVueloHistorico objSearch);
    }
}
