namespace Domain.General.CustomEntities.Vuelo
{
    public class ParamsConsultarCompraHistorico
    {
        /// <summary>Identificador único del registro histórico de la compra.</summary>
        public int? CompraHistoricoId { get; set; }

        /// <summary>Identificador de la compra asociada.</summary>
        public int? CompraId { get; set; }

        /// <summary>Identificador del estado de compra asociado al registro histórico.</summary>
        public int? EstadoCompraId { get; set; }
    }
}
