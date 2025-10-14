namespace Domain.General.CustomEntities.Vuelo
{
    public class ParamsConsultarVueloHistorico
    {
        /// <summary>Identificador único del registro histórico del vuelo.</summary>
        public int? VueloHistoricoId { get; set; }

        /// <summary>Identificador del vuelo asociado.</summary>
        public int? VueloId { get; set; }

        /// <summary>Código histórico del vuelo.</summary>
        public string? VueloHistoricoCodigo { get; set; }

        /// <summary>Identificador del estado del vuelo en el historial.</summary>
        public int? EstadoVueloId { get; set; }

        /// <summary>Identificador del avión asociado al vuelo.</summary>
        public int? AvionId { get; set; }

        /// <summary>Identificador de la ciudad de origen.</summary>
        public int? CiudadOrigenId { get; set; }

        /// <summary>Identificador de la ciudad de destino.</summary>
        public int? CiudadDestinoId { get; set; }

        /// <summary>Precio del vuelo en el historial.</summary>
        public decimal? VueloHistoricoPrecio { get; set; }

        /// <summary>Descuento aplicado en el historial.</summary>
        public decimal? VueloHistoricoDescuento { get; set; }

        /// <summary>Fecha y hora de salida en el historial.</summary>
        public DateTime? VueloHistoricoFechaHoraSalida { get; set; }

        /// <summary>Fecha y hora de llegada en el historial.</summary>
        public DateTime? VueloHistoricoFechaHoraLlegada { get; set; }
    }
}
