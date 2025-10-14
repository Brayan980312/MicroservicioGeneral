namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;
    public interface IVueloService : ICreateService<ParamsCrearActualizarVuelo, Vuelo>, // OK
                                                                  IReadWithParamsService<ParamsConsultarVuelo, IEnumerable<Vuelo>>,  // OK
                                                                  IReadWithParamsService<ParamsConsultarVueloHistorico, IEnumerable<VueloHistorico>>, // OK
                                                                  ICreateService<ParamsCrearActualizarCompra, Compra>, // OK
                                                                  IReadWithParamsService<ParamsConsultarCompra, IEnumerable<Compra>>,  // OK
                                                                  IReadWithParamsService<ParamsConsultarCompraHistorico, IEnumerable<CompraHistorico>>, // OK
                                                                  IReadWithParamsService<ParamsConsultarCompraDetalle, IEnumerable<CompraDetalle>>,
                                                                  IReadWithParamsService<ParamsConsultarCompraDetalleHistorico, IEnumerable<CompraDetalleHistorico>>
    {
        /* Aquí se debe dejar el contrato personalizado */

        /// <summary>Actualiza el estado del vuelo (ej: disponible, cerrado, cancelado, en embarque).</summary>
        /// <param name="searchUsuario">Parametros con el que se actualizará el estado del vuelo.</param>
        /// <returns>Objeto del vuelo actualizado.</returns>
        Task<Vuelo> ActualizarEstadoVuelo(ParamsActualizarEstadoVuelo updateEstado);

        /// <summary>
        /// Reserva uno o varios asientos de un vuelo, aplicando validaciones de concurrencia.
        /// </summary>
        /// <param name="paramsReservar">Parámetros de la reserva (vuelo y lista de asientos).</param>
        /// <returns>Lista de asientos reservados exitosamente.</returns>
        Task<IEnumerable<VueloAsiento>> ReservarAsientosAsync(ParamsReservarAsientos paramsReservar);

        /// <summary>
        /// Realiza la compra de los asientos seleccionados para un vuelo, validando disponibilidad y concurrencia.
        /// </summary>
        /// <param name="paramsCompra">Parámetros con la información de compra y detalle de los asientos.</param>
        /// <returns>Lista de los detalles de compra generados.</returns>
        Task<Compra> ComprarAsientosAsync(ParamsCompraAsientos paramsCompra);
    }
}
