namespace Domain.General.CustomEntities.Avion
{
    public class ParamsCrearActualizarAsientoAvion
    {
        /// <summary>Identificador único del asiento.</summary>
        public int? AsientoAvionId { get; set; }

        /// <summary>Identificador del avión al que pertenece el asiento.</summary>
        public int? AvionId { get; set; }

        /// <summary>Nombre o código del asiento (por ejemplo, '1A', '2B').</summary>
        public string? AsientoAvionNombre { get; set; }

        /// <summary>Indica si el asiento es de tipo VIP. True si es VIP, False si no lo es.</summary>
        public bool? AsientoAvionVIP { get; set; }

        /// <summary>Porcentaje adicional aplicado a un asiento VIP.</summary>
        public decimal? AsientoAvionVIPPorcentaje { get; set; }

        /// <summary>Estado del asiento. True si está activo, False si está inactivo.</summary>
        public bool? AsientoAvionEstado { get; set; }
    }
}
