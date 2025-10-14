namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Representa la información histórica del detalle de una compra de vuelo.</summary>
    public class CompraDetalleHistorico : EntidadBase
    {
        /// <summary>Identificador único del registro histórico del detalle de compra.</summary>
        public int? CompraDetalleHistoricoId { get; set; }

        /// <summary>Identificador del detalle de compra asociado.</summary>
        public int? CompraDetalleId { get; set; }

        /// <summary>Nombre del pasajero en el historial.</summary>
        public string? CompraDetalleHistoricoNombrePasajero { get; set; }

        /// <summary>Identificación del pasajero en el historial.</summary>
        public string? CompraDetalleHistoricoIdentificacionPasajero { get; set; }

        /// <summary>Fecha de registro del historial.</summary>
        public DateTime? CompraDetalleHistoricoFechaRegistro { get; set; }
    }
}
