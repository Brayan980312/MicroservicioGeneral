namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Representa el historial de cambios de estado de una compra dentro del sistema de vuelos.</summary>
    public class CompraHistorico : EntidadBase
    {
        /// <summary>Identificador único del registro histórico de la compra.</summary>
        public int? CompraHistoricoId { get; set; }

        /// <summary>Identificador de la compra asociada.</summary>
        public int? CompraId { get; set; }

        /// <summary>Identificador del estado de compra asociado al registro histórico.</summary>
        public int? EstadoCompraId { get; set; }

        /// <summary>Fecha y hora en que se registró el cambio de estado de la compra.</summary>
        public DateTime? CompraHistoricoFechaRegistro { get; set; }
    }
}
