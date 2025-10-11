namespace Domain.General.CustomEntities.Administracion
{
    public class ParamsCrearActualizarMetodoPago
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
