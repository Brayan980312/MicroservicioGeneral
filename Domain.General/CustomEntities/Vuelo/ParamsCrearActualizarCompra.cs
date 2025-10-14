namespace Domain.General.CustomEntities.Vuelo
{
    public class ParamsCrearActualizarCompra
    {
        /// <summary>Identificador único de la compra.</summary>
        public int? CompraId { get; set; }

        /// <summary>Identificador del vuelo adquirido.</summary>
        public int? VueloId { get; set; }

        /// <summary>Identificador del estado de la compra.</summary>
        public int? EstadoCompraId { get; set; }

        /// <summary>Identificador del método de pago utilizado.</summary>
        public int? MetodoPagoId { get; set; }

        /// <summary>Total de la compra.</summary>
        public decimal? CompraTotal { get; set; }
    }
}
