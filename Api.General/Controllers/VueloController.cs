namespace Api.General.Controllers
{
    using AutoMapper;
    using Domain.General.CustomEntities.Compras;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.DTOs.Compras;
    using Domain.General.DTOs.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controlador que maneja los endpoints relacionados con los vuelos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class VueloController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        /// <summary>
        /// Obtiene una instancia del servicio de vuelos a través de la unidad de trabajo de servicios.
        /// </summary>
        private IVueloService VueloService => _iServiceUnitOfWork.GetService<IVueloService>();

        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase VueloController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public VueloController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        #region EndPoints
        /// <summary>Endpoint para crear un nuevo vuelo.</summary>
        /// <param name="parametrosCrearEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto Dto del vuelo creado.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("CrearVuelo")]
        public async Task<ActionResult<VueloDto>> CrearVuelo(ParamsCrearVuelo parametrosCrearEntidad)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            Vuelo Entidad = await VueloService.CreateAsync(parametrosCrearEntidad, Convert.ToInt32(usuarioIdClaim!.Value));
            return Ok(_iMapper.Map<VueloDto>(Entidad));
        }

        /// <summary>Endpoint para actualizar un vuelo.</summary>
        /// <param name="parametrosActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto Dto del vuelo actualizado.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("ActualizarVuelo")]
        public async Task<ActionResult<VueloDto>> ActualizarVuelo(ParamsActualizarVuelo parametrosActualizarEntidad)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            Vuelo Entidad = await VueloService.UpdateAsync(parametrosActualizarEntidad, Convert.ToInt32(usuarioIdClaim!.Value));
            return Ok(_iMapper.Map<VueloDto>(Entidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ConsultarVuelo")]
        public async Task<ActionResult<List<VueloDto>>> ConsultarVuelo([FromQuery] ParamsConsultarVuelo parametrosConsultarEntidad)
        {
            IEnumerable<Vuelo> listadoEntidad = await VueloService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<VueloDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar los vuelos bajo un servicio personalizado.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ConsultarVueloPersonalizado")]
        public async Task<ActionResult<List<VueloDto>>> ConsultarVueloPersonalizado([FromQuery] ParamsConsultarVuelo parametrosConsultarEntidad)
        {
            IEnumerable<BusquedaVueloPoco> listadoEntidad = await VueloService.ConsultarVueloPersonalizado(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<VueloDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar los vuelos bajo un servicio personalizado.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarVueloDisponibles")]
        public async Task<ActionResult<List<VuelosDisponiblesDto>>> ConsultarVueloDisponibles([FromQuery] ParamsBusquedaVuelosDisponibles parametrosConsultarEntidad)
        {
            IEnumerable<BusquedaVuelosDisponiblesPoco> listadoEntidad = await VueloService.ConsultarVuelosDisponiblesPersonalizado(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<VuelosDisponiblesDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarVueloAsientos")]
        public async Task<ActionResult<List<AsientosVueloDto>>> ConsultarVueloAsientos([FromQuery] ParamsBusquedaAsientoVuelo parametrosConsultarEntidad)
        {
            IEnumerable<AsientosVueloPoco> listadoEntidad = await VueloService.ConsultarVueloAsientoPersonalizado(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<AsientosVueloDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para actualizar el estado de un vuelo.</summary>
        /// <param name="parametrosActualizarEstadoVuelo">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto Dto con la entidad del sistema actualizada.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("ActualizarEstadoVuelo")]
        public async Task<ActionResult<VueloDto>> ActualizarEstadoVuelo(ParamsActualizarEstadoVuelo parametrosActualizarEstadoVuelo)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            Vuelo Entidad = await VueloService.ActualizarEstadoVuelo(parametrosActualizarEstadoVuelo, Convert.ToInt32(usuarioIdClaim!.Value));
            return Ok(_iMapper.Map<VueloDto>(Entidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ConsultarVueloHistorico")]
        public async Task<ActionResult<List<VueloHistoricoDto>>> ConsultarVueloHistorico([FromQuery] ParamsConsultarVueloHistorico parametrosConsultarEntidad)
        {
            IEnumerable<BusquedaVueloHistoricoPoco> listadoEntidad = await VueloService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<VueloHistoricoDto>>(listadoEntidad));
        }
        #endregion

    }
}