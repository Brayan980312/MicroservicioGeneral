namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Representa la información de una compra de vuelo.</summary>
    public class Compra : EntidadBase
    {
        /// <summary>Identificador único de la compra.</summary>
        public int? CompraId { get; set; }

        /// <summary>Identificador del usuario que realiza la compra.</summary>
        public int? UsuarioId { get; set; }

        /// <summary>Identificador del vuelo adquirido.</summary>
        public int? VueloId { get; set; }

        /// <summary>Identificador del estado de la compra.</summary>
        public int? EstadoCompraId { get; set; }        

        /// <summary>Fecha de la compra.</summary>
        public DateTime? CompraFecha { get; set; }

        /// <summary>Identificador del método de pago utilizado.</summary>
        public int? MetodoPagoId { get; set; }

        /// <summary>Total de la compra.</summary>
        public decimal? CompraTotal { get; set; }
    }
}
