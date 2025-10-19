namespace Api.General.Controllers
{
    using AutoMapper;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Metricas;
    using Domain.General.DTOs.Administracion;
    using Domain.General.DTOs.Metricas;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Domain.General.Services.General;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controlador que maneja los endpoints relacionados con las metricas del sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class MetricasController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        /// <summary>
        /// Obtiene una instancia del servicio de metricas a través de la unidad de trabajo de servicios.
        /// </summary>
        private IMetricasService MetricasService => _iServiceUnitOfWork.GetService<IMetricasService>();

        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase MetricasController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public MetricasController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        #region EndPoints

        /// <summary>Endpoint para consultar los vuelos más buscados en el sistema.</summary>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarVuelosMasBuscados")]
        public async Task<ActionResult<List<VuelosMasBuscadosDto>>> ConsultarVuelosMasBuscados()
        {
            IEnumerable<VuelosMasBuscadosPoco> listadoEntidad = await MetricasService.ConsultarVuelosMasBuscados();
            return Ok(_iMapper.Map<List<VuelosMasBuscadosDto>>(listadoEntidad));
        }

        #endregion
    }
}
