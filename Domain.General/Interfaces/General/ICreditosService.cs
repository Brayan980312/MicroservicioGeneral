namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Creditos;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio de creditos.
    /// <para>Este servicio permite operaciones de actualización y lectura de creditos asociados al usuario, utilizando los contratos genéricos <see cref="ICreateService{TParam, TEntity}"/> y <see cref="IReadWithParamsService{TParam, TEntity}"/>.</para>
    /// </summary>
    public interface ICreditosService : ICreateService<ParamsCrearActualizarCreditoUsuario, CreditoUsuario>, IReadWithParamsService<ParamsConsultarCreditoUsuario,CreditoUsuario>
    {
        /* Aquí se debe dejar el contrato personalizado */
    }
}