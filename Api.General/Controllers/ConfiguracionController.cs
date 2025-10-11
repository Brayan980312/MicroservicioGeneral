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

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ConfiguracionController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

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

        /// <summary>Endpoint para actualizar un parametro del sistema.</summary>
        /// <param name="parametrosActualizarParametro">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de ParametrosDto con todos los parametros del sistema.</returns>
        [HttpPost]
        [Route("ActualizarParametro")]
        public async Task<ActionResult<List<ParametrosDto>>> ActualizarParametro(ParamsActualizarParametros parametrosActualizarParametro)
        {
            IConfiguracionService parametrosService = _iServiceUnitOfWork.GetService<IConfiguracionService>();

            IEnumerable<Parametros> listadoParametros = await parametrosService.UpdateAsync(parametrosActualizarParametro);
            return Ok(_iMapper.Map<List<ParametrosDto>>(listadoParametros));
        }

        /// <summary>Endpoint para consultar los parametros del sistema.</summary>
        /// <param name="parametrosConsultarParametros">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de ParametrosDto con todos los parametros del sistema.</returns>
        [HttpGet("ConsultarParametros")]
        public async Task<ActionResult<List<ParametrosDto>>> ConsultarParametros([FromQuery] ParamsConsultarParametros parametrosConsultarParametros)
        {
            IConfiguracionService parametrosService = _iServiceUnitOfWork.GetService<IConfiguracionService>();

            IEnumerable<Parametros> listadoParametros = await parametrosService.GetWithParamsAsync(parametrosConsultarParametros);
            return Ok(_iMapper.Map<List<ParametrosDto>>(listadoParametros));
        }
    }
}