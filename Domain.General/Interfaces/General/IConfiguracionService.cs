namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Params;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio de parametros.
    /// <para>Este servicio permite operaciones de actualización y lectura de parametros, utilizando los contratos genéricos <see cref="IUpdateService{TParam, TEntity}"/> y <see cref="IReadWithParamsService{TParam, TEntity}"/>.</para>
    /// </summary>
    public interface IConfiguracionService : IUpdateService<ParamsActualizarParametros, IEnumerable<Parametros>>, IReadWithParamsService<ParamsConsultarParametros,Parametros>
    {
        /* Aquí se debe dejar el contrato personalizado */
    }
}
