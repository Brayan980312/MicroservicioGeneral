namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Realiza la inicialización de las propiedades de la entidad Método de Pago.</summary>
    public class MetodoPago : EntidadBase
    {
        /// <summary>Identificador único del método de pago.</summary>
        public int? MetodoPagoId { get; set; }

        /// <summary>Nombre del método de pago (por ejemplo, 'Efectivo', 'Transferencia', 'Tarjeta', 'Creditos').</summary>
        public string? MetodoPagoNombre { get; set; }

        /// <summary>Descripción adicional del método de pago.</summary>
        public string? MetodoPagoDescripcion { get; set; }

        /// <summary>Estado del método de pago. True si está activo, False si está inactivo.</summary>
        public bool? MetodoPagoEstado { get; set; }
    }
}
