namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Compras;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;
    public interface IComprasService :  IReadWithParamsService<ParamsConsultarCompra, IEnumerable<Compra>>,
                                                                         IReadWithParamsService<ParamsConsultarCompraHistorico, IEnumerable<CompraHistorico>>,
                                                                         IReadWithParamsService<ParamsConsultarCompraDetalle, IEnumerable<CompraDetalle>>,
                                                                         IReadWithParamsService<ParamsConsultarCompraDetalleHistorico, IEnumerable<CompraDetalleHistorico>>
    {
        /// <summary>Reserva uno o varios asientos de un vuelo, aplicando validaciones de concurrencia.</summary>
        /// <param name="paramsReservar">Parámetros de la reserva (vuelo y lista de asientos).</param>
        /// <returns>Lista de asientos reservados exitosamente.</returns>
        Task<IEnumerable<VueloAsiento>> ReservarAsientosAsync(ParamsReservarAsientos paramsReservar);

        /// <summary>Realiza la compra de los asientos seleccionados para un vuelo, validando disponibilidad y concurrencia.</summary>
        /// <param name="paramsCompra">Parámetros con la información de compra y detalle de los asientos.</param>
        /// <returns>Lista de los detalles de compra generados.</returns>
        Task<Compra> ComprarAsientosAsync(ParamsCompraAsientos paramsCompra, int userId);
    }
}
