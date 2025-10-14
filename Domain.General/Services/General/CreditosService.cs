namespace Domain.General.Services.General
{
    using AutoMapper;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Creditos;
    using Domain.General.CustomEntities.Params;
    using Domain.General.Entities;
    using Domain.General.Interfaces.External;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Transactions;
    using Utilitarios.Constants;
    using Utilitarios.Entities;
    using Utilitarios.Extensions;
    using Utilitarios.Helpers;

    public class CreditosService : ICreditosService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Instancia del Mapeador - Imapper.</summary>
        private readonly IMapper _iMapper;

        /// <summary>Inyeccion del servicio IConfiguracionService.</summary>
        private readonly IConfiguracionService _iConfiguracionService;

        /// <summary>Inyeccion del servicio ISeguridadService.</summary>
        private readonly ISeguridadService _iSeguridadService;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase CreditosService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public CreditosService(IUnitOfWork iUnitOfWork, IMapper iMapper, IConfiguracionService iCo, IConfiguracionService iConfiguracionService, ISeguridadService iSeguridadService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _iConfiguracionService = iConfiguracionService;
            _iSeguridadService = iSeguridadService;
        }

        #endregion

        #region Métodos

        /// <inheritdoc />
        public async Task<CreditoUsuario> GetWithParamsAsync(ParamsConsultarCreditoUsuario paramsSearch, int? userId = null)
        {
            CreditoUsuario creditoUsuario = new CreditoUsuario();
            CreditoUsuario creditoUsuarioBuscado = new CreditoUsuario();

            CreditoUsuario entidad = _iMapper.Map<CreditoUsuario>(paramsSearch);
            entidad.UsuarioId = userId;
            Expression<Func<CreditoUsuario, bool>> filtro = entidad.ToFilterExpression<CreditoUsuario>();
            creditoUsuarioBuscado = await _iUnitOfWork.Repository<CreditoUsuario>().ConsultarUnoAsync(filtro);
            return creditoUsuarioBuscado == null ? creditoUsuario : creditoUsuarioBuscado;
        }

        /// <inheritdoc />
        public async Task<CreditoUsuario> CreateAsync(ParamsCrearActualizarCreditoUsuario paramsCreateUpdate, int? userId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCrearActualizarCreditoUsuario(paramsCreateUpdate);
                CreditoUsuario listaEntidad = new CreditoUsuario();
                CreditoUsuario createUpdateEntidad = _iMapper.Map<CreditoUsuario>(paramsCreateUpdate);
                createUpdateEntidad.UsuarioId = userId;

                if (paramsCreateUpdate.CreditoUsuarioId != null && paramsCreateUpdate.CreditoUsuarioId > 0)
                {
                    if (paramsCreateUpdate.Accion == 1)
                    {
                        await ValidarMontoMaximoSistema(paramsCreateUpdate);
                        CreditoUsuario Resultado = new CreditoUsuario();
                        ParamsConsultarCreditoUsuario Existente = new ParamsConsultarCreditoUsuario();
                        Existente.CreditoUsuarioId = paramsCreateUpdate.CreditoUsuarioId;
                        Resultado = await GetWithParamsAsync(Existente);
                        createUpdateEntidad.CreditoUsuarioCreditos = createUpdateEntidad.CreditoUsuarioCreditos + Resultado.CreditoUsuarioCreditos;
                    }
                    await _iUnitOfWork.Repository<CreditoUsuario>().ActualizarAsync(createUpdateEntidad);
                }
                else
                {
                    // Crea y garantiza que el atributo primario vaya null
                    createUpdateEntidad.CreditoUsuarioId = null;
                    await _iUnitOfWork.Repository<CreditoUsuario>().AdicionarAsync(createUpdateEntidad);
                }

                await _iUnitOfWork.SaveChangesAsync();

                scope.Complete();
                return createUpdateEntidad;
            }
        }


        #endregion

        #region ValidacionCampos
        /// <summary>Valida los campos obligatorios para realizar la creacion o actualización de los creditos del usuario.</summary>
        /// <param name="parametrosCrearActualizarCreditoUsuario">El objeto de tipo ParamsCrearActualizarCreditoUsuario que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCrearActualizarCreditoUsuario(ParamsCrearActualizarCreditoUsuario parametrosCrearActualizarCreditoUsuario)
        {
            string errores = string.Empty;
            CreditoUsuario Resultado = new CreditoUsuario();
            ParamsConsultarCreditoUsuario Existente = new ParamsConsultarCreditoUsuario();

            if (parametrosCrearActualizarCreditoUsuario.CreditoUsuarioId != null && parametrosCrearActualizarCreditoUsuario.CreditoUsuarioId > 0)
            {
                // Valida si la cuenta a actualizar si existe o no
                Existente.CreditoUsuarioId = parametrosCrearActualizarCreditoUsuario.CreditoUsuarioId;
                Resultado = await GetWithParamsAsync(Existente);
                if (Resultado == null || Resultado.CreditoUsuarioId != parametrosCrearActualizarCreditoUsuario.CreditoUsuarioId)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "La cuenta ingresada");
                }
            }

            if (parametrosCrearActualizarCreditoUsuario.CreditoUsuarioCreditos == null || parametrosCrearActualizarCreditoUsuario.CreditoUsuarioCreditos == 0)
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "créditos");
            }
            else if (!RegexHelper.EsNumero(parametrosCrearActualizarCreditoUsuario.CreditoUsuarioCreditos.ToString()))
            {
                errores += string.Format(DefaultMessages.InvalidNumberWithName, "créditos");
            }
            else if (!RegexHelper.EsEnteroPositivo(parametrosCrearActualizarCreditoUsuario.CreditoUsuarioCreditos.ToString()))
            {
                errores += string.Format(DefaultMessages.InvalidNumberMin, "créditos", "0");
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }

        /// <summary>Si se le suma creditos a la cuenta se debe validar bajo un parametro que no exceda el limite.</summary>
        /// <param name="parametrosCrearActualizarCreditoUsuario">El objeto de tipo ParamsCrearActualizarCreditoUsuario que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si el monto maximo ya se pasó.</exception>
        private async Task ValidarMontoMaximoSistema(ParamsCrearActualizarCreditoUsuario parametrosCrearActualizarCreditoUsuario) 
        {
            string errores = string.Empty;

            IEnumerable<Parametros> ValorMaximoCreditos;
            ParamsConsultarParametros parametrosBusqueda = new ParamsConsultarParametros();
            parametrosBusqueda.ParametrosNombre = "CreditosMaximosSistema";
            ValorMaximoCreditos = await _iConfiguracionService.GetWithParamsAsync(parametrosBusqueda);

            if (ValorMaximoCreditos.Count() > 0)
            {
                CreditoUsuario Resultado = new CreditoUsuario();
                ParamsConsultarCreditoUsuario Existente = new ParamsConsultarCreditoUsuario();
                Existente.CreditoUsuarioId = parametrosCrearActualizarCreditoUsuario.CreditoUsuarioId;
                Resultado = await GetWithParamsAsync(Existente);
                int? valorParametro = Convert.ToInt32(ValorMaximoCreditos.FirstOrDefault().ParametrosValor);
                if ((Resultado.CreditoUsuarioCreditos + parametrosCrearActualizarCreditoUsuario.CreditoUsuarioCreditos) > valorParametro)
                {
                    errores += "Señor usuario, no puede superar el limite de creditos en el sistema. Limite: " + valorParametro.ToString() + ".";
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
