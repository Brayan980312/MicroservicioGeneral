namespace Domain.General.Services.General
{
    using AutoMapper;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Pais;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Transactions;
    using Utilitarios.Constants;
    using Utilitarios.Extensions;
    using Utilitarios.Helpers;

    /// <summary>Implementación de reglas de negocio para el servicio de administración.</summary>
    public class AdministracionService : IAdministracionService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Instancia del Mapeador - Imapper.</summary>
        private readonly IMapper _iMapper;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase AdministracionService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public AdministracionService(IUnitOfWork iUnitOfWork, IMapper iMapper)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
        }

        #endregion

        #region Métodos

        /// <inheritdoc />
        public async Task<IEnumerable<Pais>> GetWithParamsAsync(ParamsConsultarPais paramsSearch)
        {
            Pais entidad = _iMapper.Map<Pais>(paramsSearch);
            Expression<Func<Pais, bool>> filtro = entidad.ToFilterExpression<Pais>();
            return await _iUnitOfWork.Repository<Pais>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Pais>> UpdateAsync(ParamsCrearActualizarPais paramsCreateUpdate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCrearActualizarPais(paramsCreateUpdate);
                IEnumerable<Pais> listaEntidad = new List<Pais>();
                Pais createUpdateEntidad = _iMapper.Map<Pais>(paramsCreateUpdate);

                if(paramsCreateUpdate.PaisId != null && paramsCreateUpdate.PaisId > 0)
                {
                    await _iUnitOfWork.Repository<Pais>().ActualizarAsync(createUpdateEntidad);
                }                    
                else
                {
                    // Crea y garantiza que el atributo primario vaya null
                    createUpdateEntidad.PaisId = null;
                    await _iUnitOfWork.Repository<Pais>().AdicionarAsync(createUpdateEntidad);
                }

                await _iUnitOfWork.SaveChangesAsync();

                // Llama el servicio ya creado para consultar la entidad completa
                ParamsConsultarPais parametrosFiltrar = new ParamsConsultarPais();
                listaEntidad = await GetWithParamsAsync(parametrosFiltrar);

                scope.Complete();
                return listaEntidad;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Ciudad>> GetWithParamsAsync(ParamsConsultarCiudad paramsSearch)
        {
            Ciudad entidad = _iMapper.Map<Ciudad>(paramsSearch);
            Expression<Func<Ciudad, bool>> filtro = entidad.ToFilterExpression<Ciudad>();
            return await _iUnitOfWork.Repository<Ciudad>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Ciudad>> UpdateAsync(ParamsCrearActualizarCiudad paramsCreateUpdate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCrearActualizarCiudad(paramsCreateUpdate);
                IEnumerable<Ciudad> listaEntidad = new List<Ciudad>();
                Ciudad createUpdateEntidad = _iMapper.Map<Ciudad>(paramsCreateUpdate);

                if (paramsCreateUpdate.CiudadId != null && paramsCreateUpdate.CiudadId > 0)
                {
                    await _iUnitOfWork.Repository<Ciudad>().ActualizarAsync(createUpdateEntidad);
                }
                else
                {
                    // Crea y garantiza que el atributo primario vaya null
                    createUpdateEntidad.CiudadId = null;
                    await _iUnitOfWork.Repository<Ciudad>().AdicionarAsync(createUpdateEntidad);
                }

                await _iUnitOfWork.SaveChangesAsync();

                // Llama el servicio ya creado para consultar la entidad completa
                ParamsConsultarCiudad parametrosFiltrar = new ParamsConsultarCiudad();
                listaEntidad = await GetWithParamsAsync(parametrosFiltrar);

                scope.Complete();
                return listaEntidad;
            }
        }

        #endregion

        #region ValidacionCampos
        /// <summary>Valida los campos obligatorios para realizar la creacion o actualización del pais.</summary>
        /// <param name="parametrosCrearActualizarPais">El objeto de tipo ParamsCrearActualizarPais que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCrearActualizarPais(ParamsCrearActualizarPais parametrosCrearActualizarPais)
        {
            string errores = string.Empty;
            IEnumerable<Pais> PaisResultado = new List<Pais>();
            ParamsConsultarPais PaisExistentes = new ParamsConsultarPais();
            PaisResultado = await GetWithParamsAsync(PaisExistentes);

            if (parametrosCrearActualizarPais.PaisId != null && parametrosCrearActualizarPais.PaisId > 0)
            {
                // Valida si el Id del pais existe
                if (PaisResultado.Where(x => x.PaisId == parametrosCrearActualizarPais.PaisId).Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "El pais");
                }
            }

            if (string.IsNullOrEmpty(parametrosCrearActualizarPais.PaisNombre))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "nombre");
            }
            else
            {
                // Valida si el nombre del pais ya existe en el sistema
                if (PaisResultado.Any(x => x.PaisNombre.Trim().ToUpper() == parametrosCrearActualizarPais.PaisNombre.Trim().ToUpper() &&
                                                                    x.PaisId != parametrosCrearActualizarPais.PaisId))
                {
                    errores += string.Format(DefaultMessages.AlreadyExistsData, $"el país '{parametrosCrearActualizarPais.PaisNombre.Trim()}'");
                }
            }

            if (string.IsNullOrEmpty(parametrosCrearActualizarPais.PaisNomenclatura))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "nomenclatura");
            }
            else
            {
                // Valida si la nomenclatura del pais ya existe en el sistema
                if (PaisResultado.Any(x => x.PaisNomenclatura.Trim().ToUpper() == parametrosCrearActualizarPais.PaisNomenclatura.Trim().ToUpper() &&
                                                                    x.PaisId != parametrosCrearActualizarPais.PaisId))
                {
                    errores += string.Format(DefaultMessages.AlreadyExistsData,
                        $"la nomenclatura '{parametrosCrearActualizarPais.PaisNomenclatura.Trim()}'");
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }

        /// <summary>Valida los campos obligatorios para realizar la creacion o actualización de la ciudad.</summary>
        /// <param name="parametrosCrearActualizarCiudad">El objeto de tipo ParamsCrearActualizarCiudad que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCrearActualizarCiudad(ParamsCrearActualizarCiudad parametrosCrearActualizarCiudad)
        {
            string errores = string.Empty;
            IEnumerable<Ciudad> Resultado = new List<Ciudad>();
            ParamsConsultarCiudad Existentes = new ParamsConsultarCiudad();
            Resultado = await GetWithParamsAsync(Existentes);

            if (parametrosCrearActualizarCiudad.CiudadId != null && parametrosCrearActualizarCiudad.CiudadId > 0)
            {
                // Valida si el Id del pais existe
                if (Resultado.Where(x => x.CiudadId == parametrosCrearActualizarCiudad.CiudadId).Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "La ciudad");
                }
            }

            if (string.IsNullOrEmpty(parametrosCrearActualizarCiudad.CiudadNombre))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "nombre");
            }
            else
            {
                // Valida si el nombre del pais ya existe en el sistema
                if (Resultado.Any(x => x.CiudadNombre.Trim().ToUpper() == parametrosCrearActualizarCiudad.CiudadNombre.Trim().ToUpper() &&
                                                                    x.CiudadId != parametrosCrearActualizarCiudad.CiudadId))
                {
                    errores += string.Format(DefaultMessages.AlreadyExistsData, $"el país '{parametrosCrearActualizarCiudad.CiudadNombre.Trim()}'");
                }
            }

            if (string.IsNullOrEmpty(parametrosCrearActualizarCiudad.CiudadNomenclatura))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "nomenclatura");
            }
            else
            {
                // Valida si la nomenclatura del pais ya existe en el sistema
                if (Resultado.Any(x => x.CiudadNomenclatura.Trim().ToUpper() == parametrosCrearActualizarCiudad.CiudadNomenclatura.Trim().ToUpper() &&
                                                                    x.CiudadId != parametrosCrearActualizarCiudad.CiudadId))
                {
                    errores += string.Format(DefaultMessages.AlreadyExistsData,
                        $"la nomenclatura '{parametrosCrearActualizarCiudad.CiudadNomenclatura.Trim()}'");
                }
            }

            if (parametrosCrearActualizarCiudad.PaisId == null || parametrosCrearActualizarCiudad.PaisId == 0)
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "país");
            }
            else
            {
                IEnumerable<Pais> ResultadoPais = new List<Pais>();
                ParamsConsultarPais ExistentesPais = new ParamsConsultarPais();
                ExistentesPais.PaisId = parametrosCrearActualizarCiudad.PaisId;
                ResultadoPais = await GetWithParamsAsync(ExistentesPais);

                if (ResultadoPais.Count() == 0)
                {
                    errores += string.Format(DefaultMessages.DataNotFound, "El país");
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
