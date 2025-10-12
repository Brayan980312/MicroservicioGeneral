namespace Api.General.Controllers
{
    using AutoMapper;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Avion;
    using Domain.General.DTOs.Administracion;
    using Domain.General.DTOs.Avion;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class AvionController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase VueloController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public AvionController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con toda la entidad del sistema.</returns>
        [HttpPost]
        [Route("CrearActualizarAvion")]
        public async Task<ActionResult<List<AvionDto>>> CrearActualizarAvion(ParamsCrearActualizarAvion parametrosCrearActualizarEntidad)
        {
            IAvionService Service = _iServiceUnitOfWork.GetService<IAvionService>();

            IEnumerable<Avion> listadoEntidad = await Service.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<List<AvionDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarAvion")]
        public async Task<ActionResult<List<AvionDto>>> ConsultarAvion([FromQuery] ParamsConsultarAvion parametrosConsultarEntidad)
        {
            IAvionService Service = _iServiceUnitOfWork.GetService<IAvionService>();

            IEnumerable<Avion> listadoEntidad = await Service.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<AvionDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con toda la entidad del sistema.</returns>
        [HttpPost]
        [Route("CrearActualizarAsientoAvion")]
        public async Task<ActionResult<List<AsientoAvionDto>>> CrearActualizarAsientoAvion(ParamsCrearActualizarAsientoAvion parametrosCrearActualizarEntidad)
        {
            IAvionService Service = _iServiceUnitOfWork.GetService<IAvionService>();

            IEnumerable<AsientoAvion> listadoEntidad = await Service.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<List<AsientoAvionDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarAsientoAvion")]
        public async Task<ActionResult<List<AsientoAvionDto>>> ConsultarAsientoAvion([FromQuery] ParamsConsultarAsientoAvion parametrosConsultarEntidad)
        {
            IAvionService Service = _iServiceUnitOfWork.GetService<IAvionService>();

            IEnumerable<AsientoAvion> listadoEntidad = await Service.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<AsientoAvionDto>>(listadoEntidad));
        }
    }
}