namespace Domain.General.CustomEntities.Vuelo
{
    public class ParamsConsultarVuelo
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
    }
}
