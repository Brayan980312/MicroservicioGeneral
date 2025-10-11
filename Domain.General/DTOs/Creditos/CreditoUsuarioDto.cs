namespace Domain.General.DTOs.Creditos
{
    public class CreditoUsuarioDto
    {
        /// <summary>Identificador único del registro de crédito del usuario.</summary>
        public int? CreditoUsuarioId { get; set; }

        /// <summary>Identificador único del usuario asociado al crédito.</summary>
        public int? UsuarioId { get; set; }

        /// <summary>Cantidad total de créditos asignados al usuario.</summary>
        public decimal? CreditoUsuarioCreditos { get; set; }
    }
}
