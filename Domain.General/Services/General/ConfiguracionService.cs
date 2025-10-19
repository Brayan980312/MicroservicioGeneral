namespace Domain.General.Services.General
{
    using AutoMapper;
    using Domain.General.CustomEntities.Params;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Transactions;
    using Utilitarios.Constants;
    using Utilitarios.Extensions;
    using Utilitarios.Helpers;

    /// <summary>Implementación de reglas de negocio para el servicio de configuracion.</summary>
    public class ConfiguracionService : IConfiguracionService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Instancia del Mapeador - Imapper.</summary>
        private readonly IMapper _iMapper;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase ConfiguracionService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public ConfiguracionService(IUnitOfWork iUnitOfWork, IMapper iMapper)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
        }

        #endregion

        #region Métodos

        /// <inheritdoc />
        public async Task<IEnumerable<Parametros>> GetWithParamsAsync(ParamsConsultarParametros paramsSearch, int? userId = null)
        {
            Parametros parametros = _iMapper.Map<Parametros>(paramsSearch);
            Expression<Func<Parametros, bool>> filtro = parametros.ToFilterExpression<Parametros>();
            return await _iUnitOfWork.Repository<Parametros>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Parametros>> UpdateAsync(ParamsActualizarParametros paramsUpdate, int? userId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposActualizarParametro(paramsUpdate);
                IEnumerable<Parametros> listaParametros = new List<Parametros>();
                Parametros updateParametro = _iMapper.Map<Parametros>(paramsUpdate);
                updateParametro.ParametrosEstado = true;
                await _iUnitOfWork.Repository<Parametros>().ActualizarAsync(updateParametro);
                await _iUnitOfWork.SaveChangesAsync();

                // Llama el servicio ya creado para consultar los parametros
                ParamsConsultarParametros parametrosFiltrar = new ParamsConsultarParametros();
                listaParametros = await GetWithParamsAsync(parametrosFiltrar);

                scope.Complete();
                return listaParametros;

            }
                
        }

        #endregion

        #region ValidacionCampos
        /// <summary>Valida los campos obligatorios para realizar la actualización del parametro.</summary>
        /// <param name="parametrosActualizarParametro">El objeto de tipo ParametrosActualizarParametro que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposActualizarParametro(ParamsActualizarParametros parametrosActualizarParametro)
        {
            string errores = string.Empty;

            if (parametrosActualizarParametro.ParametrosId == 0 || parametrosActualizarParametro.ParametrosId == null)
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName,"Id");
            }
            else
            {
                // Valida si el Id del parametro existe
                IEnumerable<Parametros> parametrosResultado = new List<Parametros>();
                ParamsConsultarParametros parametrosExistentes = new ParamsConsultarParametros();
                parametrosExistentes.ParametrosId = parametrosActualizarParametro.ParametrosId;
                parametrosResultado = await GetWithParamsAsync(parametrosExistentes);

                if (parametrosResultado.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "El parametro");
                }
            }

            // Validacion del valor del parametro
            if (string.IsNullOrEmpty(errores))
            {
                if (string.IsNullOrEmpty(parametrosActualizarParametro.ParametrosValor))
                {
                    errores += string.Format(DefaultMessages.FieldRequiredWithName, "valor");
                }
                else if (!RegexHelper.EsNumero(parametrosActualizarParametro.ParametrosValor))
                {
                    errores += string.Format(DefaultMessages.InvalidNumberWithName, "valor");
                }
                else if (!RegexHelper.EsEnteroPositivo(parametrosActualizarParametro.ParametrosValor))
                {
                    errores += string.Format(DefaultMessages.InvalidNumberMin, "valor", "0");
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }
        #endregion
    }
}
