namespace Domain.General.CustomEntities.Vuelo
{
    /// <summary>
    /// Representa los parámetros para realizar la compra de uno o varios asientos en un vuelo.
    /// </summary>
    public class ParamsCompraAsientos
    {
        /// <summary>Identificador del usuario que realiza la compra.</summary>
        public int UsuarioId { get; set; }

        /// <summary>Identificador del vuelo en el cual se compran los asientos.</summary>
        public int VueloId { get; set; }

        /// <summary>Identificador del método de pago.</summary>
        public int MetodoPagoId { get; set; }

        /// <summary>Estado inicial de la compra (por ejemplo: 1 = Pagada, 2 = Pendiente, etc.).</summary>
        public int EstadoCompraId { get; set; }

        /// <summary>Monto total de la compra (suma de los precios de los asientos).</summary>
        public decimal CompraTotal { get; set; }

        /// <summary>Lista de los asientos que se van a comprar.</summary>
        public List<ParamsCompraDetalle> DetalleAsientos { get; set; } = new();
    }

    public class ParamsCompraDetalle
    {
        public int VueloAsientoId { get; set; }
        public string CompraDetalleNombrePasajero { get; set; }
        public string CompraDetalleIdentificacionPasajero { get; set; }
        public decimal CompraDetallePrecio { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
