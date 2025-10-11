namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio de administracion.
    /// <para>Este servicio permite operaciones de creacion y lectura de la entidad pais, ciudad y metodo pago, utilizando los contratos genéricos <see cref="ICreateService{TParam, TEntity}"/> y <see cref="IReadWithParamsService{TParam, TEntity}"/>.</para>
    /// </summary>
    public interface IAdministracionService : ICreateService<ParamsCrearActualizarPais, IEnumerable<Pais>>, 
                                                                                    IReadWithParamsService<ParamsConsultarPais, IEnumerable<Pais>>,
                                                                                    ICreateService<ParamsCrearActualizarCiudad, IEnumerable<Ciudad>>, 
                                                                                    IReadWithParamsService<ParamsConsultarCiudad, IEnumerable<Ciudad>>,
                                                                                    ICreateService<ParamsCrearActualizarMetodoPago, IEnumerable<MetodoPago>>,
                                                                                    IReadWithParamsService<ParamsConsultarMetodoPago, IEnumerable<MetodoPago>>
    {
        /* Aquí se debe dejar el contrato personalizado */
    }
}
