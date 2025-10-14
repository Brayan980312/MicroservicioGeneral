namespace Api.General.Controllers
{
    using AutoMapper;
    using Domain.General.CustomEntities.Creditos;
    using Domain.General.DTOs.Creditos;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]

    public class CreditosController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        /// <summary>
        /// Obtiene una instancia del servicio de creditos a través de la unidad de trabajo de servicios.
        /// </summary>
        private ICreditosService CreditosService => _iServiceUnitOfWork.GetService<ICreditosService>();

        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase CreditosController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public CreditosController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        #region EndPoints
        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("CrearActualizarCreditoUsuario")]
        public async Task<ActionResult<CreditoUsuarioDto>> CrearActualizarCreditoUsuario(ParamsCrearActualizarCreditoUsuario parametrosCrearActualizarEntidad)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");
            CreditoUsuario listadoEntidad = await CreditosService.CreateAsync(parametrosCrearActualizarEntidad, Convert.ToInt32(usuarioIdClaim.Value));
            return Ok(_iMapper.Map<CreditoUsuarioDto>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet("ConsultarCreditoUsuario")]
        public async Task<ActionResult<CreditoUsuarioDto>> ConsultarCreditoUsuario([FromQuery] ParamsConsultarCreditoUsuario parametrosConsultarEntidad)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");
            CreditoUsuario listadoEntidad = await CreditosService.GetWithParamsAsync(parametrosConsultarEntidad, Convert.ToInt32(usuarioIdClaim.Value));
            return Ok(_iMapper.Map<CreditoUsuarioDto>(listadoEntidad));
        }
        #endregion

    }
}