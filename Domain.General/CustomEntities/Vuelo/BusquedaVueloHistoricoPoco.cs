namespace Domain.General.CustomEntities.Vuelo
{
    public class BusquedaVueloHistoricoPoco
    {
        /// <summary>Código del vuelo.</summary>
        public string? VueloHistoricoCodigo { get; set; }

        /// <summary>Identificador del estado del vuelo.</summary>
        public int? EstadoVueloId { get; set; }

        /// <summary>Nombre del estado del vuelo.</summary>
        public string? EstadoVueloNombre { get; set; }

        /// <summary>Nombre del avión asociado al vuelo.</summary>
        public string? AvionNombre { get; set; }

        /// <summary>Nombre de la ciudad de origen.</summary>
        public string? CiudadOrigenNombre { get; set; }

        /// <summary>Nomenclatura de la ciudad de origen.</summary>
        public string? CiudadOrigenNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura de la ciudad de origen.</summary>
        public string? CiudadOrigenNombreNomenclatura { get; set; }

        /// <summary>Nombre de la ciudad de destino.</summary>
        public string? CiudadDestinoNombre { get; set; }

        /// <summary>Nomenclatura de la ciudad de destino.</summary>
        public string? CiudadDestinoNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura de la ciudad de destino.</summary>
        public string? CiudadDestinoNombreNomenclatura { get; set; }

        /// <summary>Precio del vuelo.</summary>
        public decimal? VueloHistoricoPrecio { get; set; }

        /// <summary>Descuento aplicado al vuelo.</summary>
        public decimal? VueloHistoricoDescuento { get; set; }

        /// <summary>Fecha y hora de salida del vuelo.</summary>
        public DateTime? VueloHistoricoFechaHoraSalida { get; set; }

        /// <summary>Fecha y hora de llegada del vuelo.</summary>
        public DateTime? VueloHistoricoFechaHoraLlegada { get; set; }

        /// <summary>Nombre completo del Usuario que realiza el cambio.</summary>
        public string UsuarioNombreCompleto { get; set; }

        /// <summary>Fecha y hora del registro del historico.</summary>
        public DateTime? VueloHistoricoFechaCreacion { get; set; }
    }
}