namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Avion;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio de avion.
    /// <para>Este servicio permite operaciones de creacion y lectura de la entidad avion y asientoAvion , utilizando los contratos genéricos <see cref="ICreateService{TParam, TEntity}"/> y <see cref="IReadWithParamsService{TParam, TEntity}"/>.</para>
    /// </summary>
    public interface IAvionService : ICreateService<ParamsCrearActualizarAvion, IEnumerable<BusquedaAvionPoco>>,
                                                                  IReadWithParamsService<ParamsConsultarAvion, IEnumerable<BusquedaAvionPoco>>,
                                                                  ICreateService<ParamsCrearActualizarAsientoAvion, IEnumerable<AsientoAvion>>,
                                                                  IReadWithParamsService<ParamsConsultarAsientoAvion, IEnumerable<AsientoAvion>>
    {
        /* Aquí se debe dejar el contrato personalizado */
    }
}
