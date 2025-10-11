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

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class CreditosController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

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

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto Dto con toda la entidad del sistema.</returns>
        [HttpPost]
        [Route("CrearActualizarCreditoUsuario")]
        public async Task<ActionResult<CreditoUsuarioDto>> CrearActualizarCreditoUsuario(ParamsCrearActualizarCreditoUsuario parametrosCrearActualizarEntidad)
        {
            ICreditosService Service = _iServiceUnitOfWork.GetService<ICreditosService>();

            CreditoUsuario listadoEntidad = await Service.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<CreditoUsuarioDto>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarCreditoUsuario")]
        public async Task<ActionResult<CreditoUsuarioDto>> ConsultarCreditoUsuario([FromQuery] ParamsConsultarCreditoUsuario parametrosConsultarEntidad)
        {
            ICreditosService Service = _iServiceUnitOfWork.GetService<ICreditosService>();

            CreditoUsuario listadoEntidad = await Service.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<CreditoUsuarioDto>(listadoEntidad));
        }
    }
}