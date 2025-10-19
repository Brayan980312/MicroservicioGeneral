namespace Domain.General.DTOs.Vuelo
{
    public class VuelosDisponiblesDto
    {
        /// <summary>Identificador único del vuelo.</summary>
        public int? VueloId { get; set; }

        /// <summary>Nombre de la ciudad de origen.</summary>
        public string? CiudadOrigenNombre { get; set; }

        /// <summary>Nomenclatura de la ciudad de origen.</summary>
        public string? CiudadOrigenNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura de la ciudad de origen.</summary>
        public string? CiudadOrigenNombreNomenclatura { get; set; }

        /// <summary>Nombre del país al que pertenece la ciudad de origen del vuelo.</summary>
        public string? PaisOrigenNombre { get; set; }

        /// <summary>Nomenclatura del país al que pertenece la ciudad de origen del vuelo.</summary>
        public string? PaisOrigenNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura del país al que pertenece la ciudad de origen del vuelo.</summary>
        public string? PaisOrigenNombreNomenclatura { get; set; }

        /// <summary>Nombre de la ciudad de destino.</summary>
        public string? CiudadDestinoNombre { get; set; }

        /// <summary>Nomenclatura de la ciudad de destino.</summary>
        public string? CiudadDestinoNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura de la ciudad de destino.</summary>
        public string? CiudadDestinoNombreNomenclatura { get; set; }

        /// <summary>Nombre del país al que pertenece la ciudad de destino del vuelo.</summary>
        public string? PaisDestinoNombre { get; set; }

        /// <summary>Nomenclatura del país al que pertenece la ciudad de destino del vuelo.</summary>
        public string? PaisDestinoNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura del país al que pertenece la ciudad de destino del vuelo.</summary>
        public string? PaisDestinoNombreNomenclatura { get; set; }

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
