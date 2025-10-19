namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;
    public interface IVueloService : ICreateService<ParamsCrearVuelo, Vuelo>,
                                                                  IUpdateService<ParamsActualizarVuelo, Vuelo>,
                                                                  IReadWithParamsService<ParamsConsultarVuelo, IEnumerable<Vuelo>>,
                                                                  IReadWithParamsService<ParamsConsultarVueloHistorico, IEnumerable<BusquedaVueloHistoricoPoco>>
                                                                  
    {
        /* Aquí se debe dejar el contrato personalizado */

        /// <summary>Actualiza el estado del vuelo (ej: disponible, cerrado, cancelado, en embarque).</summary>
        /// <param name="updateEstado">Parametros con el que se actualizará el estado del vuelo.</param>
        /// <returns>Objeto del vuelo actualizado.</returns>
        Task<Vuelo> ActualizarEstadoVuelo(ParamsActualizarEstadoVuelo updateEstado, int userId);

        /// <summary>Contrato personalizado para la consulta de vuelos.</summary>
        /// <param name="searchFlight">Parametros con el que se buscara los vuelos bajo ciertos criterios.</param>
        /// <returns>Lista BusquedaVueloPoco con la información de los vuelos buscados.</returns>
        Task<IEnumerable<BusquedaVueloPoco>> ConsultarVueloPersonalizado(ParamsConsultarVuelo searchFlight);

        /// <summary>Contrato personalizado para la consulta de los asientos de un vuelo.</summary>
        /// <param name="searchSeatFlight">Parametros con el que se buscara los asientos de un vuelo bajo ciertos criterios.</param>
        /// <returns>Lista AsientosVueloPoco con la información de los asientos del vuelo buscados.</returns>
        Task<IEnumerable<AsientosVueloPoco>> ConsultarVueloAsientoPersonalizado(ParamsBusquedaAsientoVuelo searchSeatFlight);

        

        /// <summary>Contrato personalizado para la consulta de vuelos disponibles en el sistema.</summary>
        /// <param name="searchFlight">Parametros con el que se buscara los vuelos disponibles bajo ciertos criterios.</param>
        /// <returns>Lista BusquedaVuelosDisponiblesPoco con la información de los vuelos disponibles buscados.</returns>
        Task<IEnumerable<BusquedaVuelosDisponiblesPoco>> ConsultarVuelosDisponiblesPersonalizado(ParamsBusquedaVuelosDisponibles searchFlightAvailable);
    }
}
