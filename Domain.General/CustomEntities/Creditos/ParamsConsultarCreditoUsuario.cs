namespace Domain.General.CustomEntities.Creditos
{
    public class ParamsConsultarCreditoUsuario
    {
        /// <summary>Identificador único del registro de crédito del usuario.</summary>
        public int? CreditoUsuarioId { get; set; }

        /// <summary>Cantidad total de créditos asignados al usuario.</summary>
        public decimal? CreditoUsuarioCreditos { get; set; }
    }
}
