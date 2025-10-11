namespace Domain.General.CustomEntities.Creditos
{
    public class ParamsCrearActualizarCreditoUsuario
    {
        /// <summary>Identificador único del registro de crédito del usuario.</summary>
        public int? CreditoUsuarioId { get; set; }

        /// <summary>Identificador único del usuario asociado al crédito.</summary>
        public int? UsuarioId { get; set; }

        /// <summary>Cantidad total de créditos asignados al usuario.</summary>
        public decimal? CreditoUsuarioCreditos { get; set; }

        /// <summary>Valida si se debe sumar o restar creditos (1=Sumar 2=Restar).</summary>
        public int? Accion { get; set; }
    }
}
