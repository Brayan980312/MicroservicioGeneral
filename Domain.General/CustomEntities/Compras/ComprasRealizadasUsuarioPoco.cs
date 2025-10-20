namespace Domain.General.CustomEntities.Compras
{
    /// <summary>Representa la información general de una compra de vuelo, incluyendo los datos del vuelo, el pago y los pasajeros asociados.</summary>
    public class ComprasRealizadasUsuarioPoco
    {
        /// <summary>Fecha y hora en que se realizó la compra.</summary>
        public DateTime CompraFecha { get; set; }

        /// <summary>Nombre del método de pago utilizado.</summary>
        public string MetodoPagoNombre { get; set; }

        /// <summary>Valor total pagado por la compra.</summary>
        public decimal CompraTotal { get; set; }

        /// <summary>Código identificador del vuelo asociado a la compra.</summary>
        public string VueloCodigo { get; set; }

        /// <summary>Nombre del avión utilizado para el vuelo.</summary>
        public string AvionNombre { get; set; }

        /// <summary>Estado actual del vuelo.</summary>
        public string EstadoVueloNombre { get; set; }

        /// <summary>Identificador del estado del vuelo.</summary>
        public int EstadoVueloId { get; set; }

        /// <summary>Nombre y nomenclatura de la ciudad de origen.</summary>
        public string CiudadOrigenNombreNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura (código IATA) de la ciudad de destino.</summary>
        public string CiudadDestinoNombreNomenclatura { get; set; }

        /// <summary>Fecha y hora de salida del vuelo.</summary>
        public DateTime VueloFechaHoraSalida { get; set; }

        /// <summary>Fecha y hora estimada de llegada del vuelo.</summary>
        public DateTime VueloFechaHoraLlegada { get; set; }

        /// <summary>Lista de detalles de la compra, que incluyen la información de cada pasajero y su asiento asignado.</summary>
        public List<AsientosAsociadosACompraPoco> CompraDetalle { get; set; }
    }

    /// <summary>Representa el detalle individual de una compra, incluyendo el asiento asignado, el pasajero y el precio correspondiente.</summary>
    public class AsientosAsociadosACompraPoco 
    {
        /// <summary>Nombre o código del asiento dentro del avión.</summary>
        public string AsientoAvionNombre { get; set; }

        /// <summary>Indica si el asiento es de tipo VIP. Si está vacío, el asiento es estándar.</summary>
        public string AsientoAvionVIP { get; set; }

        /// <summary>Precio asociado al asiento para este pasajero.</summary>
        public decimal CompraDetallePrecio { get; set; }

        /// <summary>Nombre completo del pasajero que ocupa el asiento.</summary>
        public string CompraDetalleNombrePasajero { get; set; }

        /// <summary>Número de identificación o documento del pasajero.</summary>
        public string CompraDetalleIdentificacionPasajero { get; set; }
    }
}
