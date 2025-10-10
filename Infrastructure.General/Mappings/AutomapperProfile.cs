namespace Infrastructure.General.Mappings
{
    using AutoMapper;
    using Domain.General.CustomEntities.Params;
    using Domain.General.Entities;
    using Domain.General.DTOs.Auditoria;
    using Domain.General.DTOs.Parametros;


    /// <summary>Configura los mapeos entre las entidades de dominio general y los objetos de transferencia de datos (DTOs/Params) mediante AutoMapper.
    /// Permite la conversión bidireccional entre los tipos configurados.
    /// </summary>
    public class AutomapperProfile : Profile
    {

        /// <summary>Inicializa una nueva instancia de <see cref="AutomapperProfile"/> y configura los mapeos entre entidades y DTOs/Params.</summary>
        public AutomapperProfile()
        {
            /// <summary>Mapea la entidad <see cref="Auditoria"/> a <see cref="ParamsAuditoria"/> y viceversa.</summary>
            CreateMap<Auditoria, ParamsAuditoria>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Auditoria"/> a <see cref="AuditoriaDto"/> y viceversa.</summary>
            CreateMap<Auditoria, AuditoriaDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Parametros"/> a <see cref="ParametrosDto"/> y viceversa.</summary>
            CreateMap<Parametros, ParametrosDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Parametros"/> a <see cref="ParamsActualizarParametros"/> y viceversa.</summary>
            CreateMap<Parametros, ParamsActualizarParametros>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Parametros"/> a <see cref="ParamsConsultarParametros"/> y viceversa.</summary>
            CreateMap<Parametros, ParamsConsultarParametros>().ReverseMap();
        }
    }
}
