namespace Domain.General.CustomEntities.Vuelo
{
    public class AsientosVueloPoco
    {
        /// <summary>Identificador único del registro del asiento asignado a un vuelo.</summary>
        public int? VueloAsientoId { get; set; }

        /// <summary>Identificador del asiento físico dentro del avión.</summary>
        public int? AsientoAvionId { get; set; }

        /// <summary>Indica el estado en el que se encuentra actualmente el asiento asociado al vuelo.</summary>
        public string? VueloAsientoEstado { get; set; }

        /// <summary>Nombre o código del asiento (por ejemplo, '1A', '2B').</summary>
        public string? AsientoAvionNombre { get; set; }

        /// <summary>Indica si el asiento es de tipo VIP. True si es VIP, False si no lo es.</summary>
        public string? AsientoAvionVIP { get; set; }

        /// <summary>Nombre del pasajero.</summary>
        public string? CompraDetalleNombrePasajero { get; set; }

        /// <summary>Identificación del pasajero.</summary>
        public string? CompraDetalleIdentificacionPasajero { get; set; }
    }
}
