namespace Domain.General.DTOs.Vuelo
{
    public class CompraDetalleHistoricoDto
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
