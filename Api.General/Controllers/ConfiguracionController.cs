namespace Api.General.Controllers
{
    using AutoMapper;
    using Domain.General.CustomEntities.Params;
    using Domain.General.DTOs.Parametros;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]

    public class ConfiguracionController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        /// <summary>
        /// Obtiene una instancia del servicio de configuración a través de la unidad de trabajo de servicios.
        /// </summary>
        private IConfiguracionService ConfiguracionService => _iServiceUnitOfWork.GetService<IConfiguracionService>();

        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase ParametrosController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public ConfiguracionController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        #region EndPoints
        /// <summary>Endpoint para actualizar un parametro del sistema.</summary>
        /// <param name="parametrosActualizarParametro">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de ParametrosDto con todos los parametros del sistema.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("ActualizarParametro")]
        public async Task<ActionResult<List<ParametrosDto>>> ActualizarParametro(ParamsActualizarParametros parametrosActualizarParametro)
        {
            IEnumerable<Parametros> listadoParametros = await ConfiguracionService.UpdateAsync(parametrosActualizarParametro);
            return Ok(_iMapper.Map<List<ParametrosDto>>(listadoParametros));
        }

        /// <summary>Endpoint para consultar los parametros del sistema.</summary>
        /// <param name="parametrosConsultarParametros">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de ParametrosDto con todos los parametros del sistema.</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("ConsultarParametros")]
        public async Task<ActionResult<List<ParametrosDto>>> ConsultarParametros([FromQuery] ParamsConsultarParametros parametrosConsultarParametros)
        {
            IEnumerable<Parametros> listadoParametros = await ConfiguracionService.GetWithParamsAsync(parametrosConsultarParametros);
            return Ok(_iMapper.Map<List<ParametrosDto>>(listadoParametros));
        }
        #endregion

    }
}