namespace Domain.General.Services.General
{
    using AutoMapper;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Avion;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Transactions;
    using Utilitarios.Constants;
    using Utilitarios.Extensions;
    using Utilitarios.Helpers;

    /// <summary>Implementación de reglas de negocio para el servicio de avion.</summary>
    public class AvionService : IAvionService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Instancia del Mapeador - Imapper.</summary>
        private readonly IMapper _iMapper;

        /// <summary>Inyeccion del servicio IConfiguracionService.</summary>
        private readonly IConfiguracionService _iConfiguracionService;

        /// <summary>Inyeccion del servicio IAdministracionService.</summary>
        private readonly IAdministracionService _iAdministracionService;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase AvionService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public AvionService(IUnitOfWork iUnitOfWork, IMapper iMapper, IConfiguracionService iConfiguracionService, IAdministracionService iAdministracionService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _iConfiguracionService = iConfiguracionService;
            _iAdministracionService = iAdministracionService;
        }

        #endregion

        #region Métodos

        /// <inheritdoc />
        public async Task<IEnumerable<Avion>> GetWithParamsAsync(ParamsConsultarAvion paramsSearch)
        {
            Avion entidad = _iMapper.Map<Avion>(paramsSearch);
            Expression<Func<Avion, bool>> filtro = entidad.ToFilterExpression<Avion>();
            return await _iUnitOfWork.Repository<Avion>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Avion>> CreateAsync(ParamsCrearActualizarAvion paramsCreateUpdate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCrearActualizarAvion(paramsCreateUpdate);
                IEnumerable<Avion> listaEntidad = new List<Avion>();
                Avion createUpdateEntidad = _iMapper.Map<Avion>(paramsCreateUpdate);

                if (paramsCreateUpdate.AvionId != null && paramsCreateUpdate.AvionId > 0)
                {
                    await _iUnitOfWork.Repository<Avion>().ActualizarAsync(createUpdateEntidad);
                }
                else
                {
                    // Crea y garantiza que el atributo primario vaya null
                    createUpdateEntidad.AvionId = null;
                    await _iUnitOfWork.Repository<Avion>().AdicionarAsync(createUpdateEntidad);
                }

                await _iUnitOfWork.SaveChangesAsync();

                // Llama el servicio ya creado para consultar la entidad completa
                ParamsConsultarAvion parametrosFiltrar = new ParamsConsultarAvion();
                listaEntidad = await GetWithParamsAsync(parametrosFiltrar);

                scope.Complete();
                return listaEntidad;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AsientoAvion>> GetWithParamsAsync(ParamsConsultarAsientoAvion paramsSearch)
        {
            AsientoAvion entidad = _iMapper.Map<AsientoAvion>(paramsSearch);
            Expression<Func<AsientoAvion, bool>> filtro = entidad.ToFilterExpression<AsientoAvion>();
            return await _iUnitOfWork.Repository<AsientoAvion>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AsientoAvion>> CreateAsync(ParamsCrearActualizarAsientoAvion paramsCreateUpdate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCrearActualizarAsientoAvion(paramsCreateUpdate);
                IEnumerable<AsientoAvion> listaEntidad = new List<AsientoAvion>();
                AsientoAvion createUpdateEntidad = _iMapper.Map<AsientoAvion>(paramsCreateUpdate);

                if (paramsCreateUpdate.AsientoAvionId != null && paramsCreateUpdate.AsientoAvionId > 0)
                {
                    await _iUnitOfWork.Repository<AsientoAvion>().ActualizarAsync(createUpdateEntidad);
                }
                else
                {
                    // Crea y garantiza que el atributo primario vaya null
                    createUpdateEntidad.AsientoAvionId = null;
                    await _iUnitOfWork.Repository<AsientoAvion>().AdicionarAsync(createUpdateEntidad);
                }

                await _iUnitOfWork.SaveChangesAsync();

                // Llama el servicio ya creado para consultar la entidad completa
                ParamsConsultarAsientoAvion parametrosFiltrar = new ParamsConsultarAsientoAvion();
                listaEntidad = await GetWithParamsAsync(parametrosFiltrar);

                scope.Complete();
                return listaEntidad;
            }
        }
        #endregion

        #region ValidacionCampos
        /// <summary>Valida los campos obligatorios para realizar la creacion o actualización de la entidad.</summary>
        /// <param name="paramsValidate">El objeto de tipo ParamsCrearActualizarAvion que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCrearActualizarAvion(ParamsCrearActualizarAvion paramsValidate)
        {
            string errores = string.Empty;
            IEnumerable<Avion> Resultado = new List<Avion>();
            ParamsConsultarAvion Existentes = new ParamsConsultarAvion();
            Resultado = await GetWithParamsAsync(Existentes);

            if (paramsValidate.AvionId != null && paramsValidate.AvionId > 0)
            {
                // Valida si el Id del avion existe
                if (Resultado.Where(x => x.AvionId == paramsValidate.AvionId).Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "El avion");
                }
            }

            if (string.IsNullOrEmpty(paramsValidate.AvionNombre))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "nombre");
            }
            else
            {
                // Valida si el nombre del avion ya existe en el sistema
                if (Resultado.Any(x => x.AvionNombre.Trim().ToUpper() == paramsValidate.AvionNombre.Trim().ToUpper() &&
                                                                    x.AvionId != paramsValidate.AvionId))
                {
                    errores += string.Format(DefaultMessages.AlreadyExistsData, $"el avion '{paramsValidate.AvionNombre.Trim()}'");
                }
            }

            if (paramsValidate.CiudadId == null || paramsValidate.CiudadId == 0)
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "ciudad");
            }
            else
            {
                // Valida si la ciudad ingresada existe
                IEnumerable<Ciudad> ciudades = new List<Ciudad>();
                ParamsConsultarCiudad paramsConsultarCiudad = new ParamsConsultarCiudad();
                paramsConsultarCiudad.CiudadId = paramsValidate.CiudadId;
                ciudades = await _iAdministracionService.GetWithParamsAsync(paramsConsultarCiudad);

                if (ciudades.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "La ciudad");
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }

        /// <summary>Valida los campos obligatorios para realizar la creacion o actualización de la entidad.</summary>
        /// <param name="paramsValidate">El objeto de tipo ParamsCrearActualizarAsientoAvion que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCrearActualizarAsientoAvion(ParamsCrearActualizarAsientoAvion paramsValidate)
        {
            string errores = string.Empty;
            IEnumerable<AsientoAvion> Resultado = new List<AsientoAvion>();
            ParamsConsultarAsientoAvion Existentes = new ParamsConsultarAsientoAvion();
            Resultado = await GetWithParamsAsync(Existentes);

            if (paramsValidate.AsientoAvionId != null && paramsValidate.AsientoAvionId > 0)
            {
                // Valida si el Id del asiento del avion existe
                if (Resultado.Where(x => x.AsientoAvionId == paramsValidate.AsientoAvionId).Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "El asiento");
                }
            }

            if (paramsValidate.AvionId == null || paramsValidate.AvionId == 0)
            {
                errores += string.Format(DefaultMessages.DataNotFound, "El avión");
            }
            else
            {
                // Valida que el avión si exista en el sistema
                IEnumerable<Avion> aviones = new List<Avion>();
                ParamsConsultarAvion paramsConsultarAvion = new ParamsConsultarAvion();
                paramsConsultarAvion.AvionId = paramsValidate.AvionId;
                aviones = await GetWithParamsAsync(paramsConsultarAvion);

                if (aviones.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "El avión");
                }
            }

            if (string.IsNullOrEmpty(paramsValidate.AsientoAvionNombre))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "nombre");
            }
            else
            {
                // Valida si el nombre del asiento ya lo tiene el avión al que se le va a asociar
                if (Resultado.Any(x => x.AsientoAvionNombre.Trim().ToUpper() == paramsValidate.AsientoAvionNombre.Trim().ToUpper() &&
                                                                    x.AsientoAvionId != paramsValidate.AsientoAvionId &&
                                                                    x.AvionId == paramsValidate.AvionId))
                {
                    errores += string.Format(DefaultMessages.AlreadyExistsData, $"el asiento '{paramsValidate.AsientoAvionNombre.Trim()}' asociado al avión");
                }
            }

            if (paramsValidate.AsientoAvionVIP == true && (paramsValidate.AsientoAvionVIPPorcentaje == null || paramsValidate.AsientoAvionVIPPorcentaje == 0))
            {
                errores += "Si el asiento es VIP, debe asignar un porcentaje adicional.";
            }
            else if (paramsValidate.AsientoAvionVIP == true && (paramsValidate.AsientoAvionVIPPorcentaje != null && paramsValidate.AsientoAvionVIPPorcentaje > 0))
            {
                // Valida que el porcentaje del asiento VIP esté comprendiendo entre ciertos valores
                if (paramsValidate.AsientoAvionVIPPorcentaje > 100 || paramsValidate.AsientoAvionVIPPorcentaje <= 0)
                {
                    errores += string.Format(DefaultMessages.RangeError, "el porcentaje","0","100");
                    
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
