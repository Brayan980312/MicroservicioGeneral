namespace Domain.General.CustomEntities.Vuelo
{
    public class ParamsConsultarCompraDetalleHistorico
    {
        /// <summary>Identificador único del registro histórico del detalle de compra.</summary>
        public int? CompraDetalleHistoricoId { get; set; }

        /// <summary>Identificador del detalle de compra asociado.</summary>
        public int? CompraDetalleId { get; set; }

        /// <summary>Nombre del pasajero en el historial.</summary>
        public string? CompraDetalleHistoricoNombrePasajero { get; set; }

        /// <summary>Identificación del pasajero en el historial.</summary>
        public string? CompraDetalleHistoricoIdentificacionPasajero { get; set; }
    }
}
