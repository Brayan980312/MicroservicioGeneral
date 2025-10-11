namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Realiza la inicialización de las propiedades de la entidad CreditoUsuario.</summary>
    public class CreditoUsuario : EntidadBase
    {
        /// <summary>Identificador único del registro de crédito del usuario.</summary>
        public int? CreditoUsuarioId { get; set; }

        /// <summary>Identificador único del usuario asociado al crédito.</summary>
        public int? UsuarioId { get; set; }

        /// <summary>Cantidad total de créditos asignados al usuario.</summary>
        public decimal? CreditoUsuarioCreditos { get; set; }
    }
}
