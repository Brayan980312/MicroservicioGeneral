namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Representa el detalle de una compra de vuelo.</summary>
    public class CompraDetalle : EntidadBase
    {
        /// <summary>Identificador único del detalle de la compra.</summary>
        public int? CompraDetalleId { get; set; }

        /// <summary>Identificador de la compra asociada.</summary>
        public int? CompraId { get; set; }

        /// <summary>Identificador del asiento del vuelo adquirido.</summary>
        public int? VueloAsientoId { get; set; }

        /// <summary>Nombre del pasajero.</summary>
        public string? CompraDetalleNombrePasajero { get; set; }

        /// <summary>Identificación del pasajero.</summary>
        public string? CompraDetalleIdentificacionPasajero { get; set; }

        /// <summary>Precio del asiento o pasaje.</summary>
        public decimal? CompraDetallePrecio { get; set; }

        /// <summary>Navegación inversa hacia el asiento del vuelo (uno a uno).</summary>
        public VueloAsiento VueloAsiento { get; set; } = null!;
    }
}
