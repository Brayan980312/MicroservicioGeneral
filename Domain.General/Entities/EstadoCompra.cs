namespace Domain.General.Entities
{
    using Utilitarios.Entities;
    /// <summary>Representa el estado actual de una compra dentro del sistema de vuelos.</summary>
    public class EstadoCompra : EntidadBase
    {
        /// <summary>Identificador único del estado de compra.</summary>
        public int? EstadoCompraId { get; set; }

        /// <summary>Nombre del estado de compra (por ejemplo, 'Pendiente', 'Pagada', 'Cancelada').</summary>
        public string? EstadoCompraNombre { get; set; }

        /// <summary>Descripción detallada del estado de compra.</summary>
        public string? EstadoCompraDescripcion { get; set; }

        /// <summary>Indica si el estado de compra está activo o inactivo. True si está activo, False si no lo está.</summary>
        public bool? EstadoCompraEstado { get; set; }
    }
}
