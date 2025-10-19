namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Representa la información de un vuelo.</summary>
    public class Vuelo : EntidadBase
    {
        /// <summary>Identificador único del vuelo.</summary>
        public int? VueloId { get; set; }

        /// <summary>Código del vuelo.</summary>
        public string? VueloCodigo { get; set; }

        /// <summary>Identificador del estado del vuelo.</summary>
        public int? EstadoVueloId { get; set; }

        /// <summary>Identificador del avión asociado al vuelo.</summary>
        public int? AvionId { get; set; }

        /// <summary>Identificador de la ciudad de origen.</summary>
        public int? CiudadOrigenId { get; set; }

        /// <summary>Identificador de la ciudad de destino.</summary>
        public int? CiudadDestinoId { get; set; }

        /// <summary>Precio del vuelo.</summary>
        public decimal? VueloPrecio { get; set; }

        /// <summary>Descuento aplicado al vuelo.</summary>
        public decimal? VueloDescuento { get; set; }

        /// <summary>Fecha y hora de salida del vuelo.</summary>
        public DateTime? VueloFechaHoraSalida { get; set; }

        /// <summary>Fecha y hora de llegada del vuelo.</summary>
        public DateTime? VueloFechaHoraLlegada { get; set; }

        /// <summary>Identificador del usuario que creó el vuelo.</summary>
        public int? UsuarioCreacionId { get; set; }

        /// <summary>Fecha de creación del vuelo.</summary>
        public DateTime? VueloFechaCreacion { get; set; }

        /// <summary>Entidad de navegación hacia el estado en el que se encuentra un vuelo.</summary>
        public EstadoVuelo? EstadoVuelo { get; set; }

        /// <summary>Entidad de navegación hacia el avión que está asociado al vuelo.</summary>
        public Avion? Avion { get; set; }

        /// <summary>Entidad de navegación hacia la ciudad de donde despega el avión.</summary>
        public Ciudad? CiudadOrigen { get; set; }

        /// <summary>Entidad de navegación hacia la ciudad de donde aterriza el avión.</summary>
        public Ciudad? CiudadDestino { get; set; }
    }
}
