namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Auditoria;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio de auditoría.
    /// <para>Este servicio permite operaciones de creación utilizando los contratos genéricos <see cref="ICreateService{TParam, TEntity}"/>.</para>
    /// </summary>
    public interface IAuditoriaService : ICreateService<ParamsAuditoria, Auditoria>
    {
        /* Aquí se debe dejar el contrato personalizado */
    }
}

