namespace Infrastructure.General.Mappings
{
    using AutoMapper;
    using Domain.General.CustomEntities.Params;
    using Domain.General.Entities;
    using Domain.General.DTOs.Auditoria;
    using Domain.General.DTOs.Parametros;
    using Domain.General.CustomEntities.Auditoria;
    using Domain.General.DTOs.Pais;
    using Domain.General.CustomEntities.Pais;
    using Domain.General.DTOs.Administracion;
    using Domain.General.CustomEntities.Administracion;


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

            /// <summary>Mapea la entidad <see cref="Pais"/> a <see cref="PaisDto"/> y viceversa.</summary>
            CreateMap<Pais, PaisDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Pais"/> a <see cref="ParamsCrearActualizarPais"/> y viceversa.</summary>
            CreateMap<Pais, ParamsCrearActualizarPais>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Pais"/> a <see cref="ParamsConsultarPais"/> y viceversa.</summary>
            CreateMap<Pais, ParamsConsultarPais>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="Ciudad"/> a <see cref="CiudadDto"/> y viceversa.</summary>
            CreateMap<Ciudad, CiudadDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Ciudad"/> a <see cref="ParamsCrearActualizarCiudad"/> y viceversa.</summary>
            CreateMap<Ciudad, ParamsCrearActualizarCiudad>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Ciudad"/> a <see cref="ParamsConsultarCiudad"/> y viceversa.</summary>
            CreateMap<Ciudad, ParamsConsultarCiudad>().ReverseMap();
        }
    }
}
