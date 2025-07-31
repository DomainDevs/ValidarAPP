using AutoMapper;
using Sistran.Core.Application.AuditServices.Enums;
using Sistran.Core.Application.AuditServices.Models;
using Sistran.Core.Application.ModelServices.Models.Audit;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Core.Framework.UIF.Web.Areas.Audit.Models
{
    internal class ModelAssembler
    {
		public static AuditServiceModel CreateAudit(AuditModelView auditModelView)
        {
            var immap = CreateMappAudit();
            var p = immap.Map<AuditModelView, AuditServiceModel>(auditModelView);
            AuditServiceModel AuditServiceModel = new AuditServiceModel();
            AuditServiceModel = p;
            return AuditServiceModel;
        }
        public static IMapper CreateMappAudit()
        {
            var config = MapperCache.GetMapper<AuditModelView, AuditServiceModel>(cfg =>
            {
                cfg.CreateMap<AuditModelView, AuditServiceModel>()
                .ForMember(x => x.User, o => o.MapFrom(s => new UserServiceModel { Id = s.UserId }))
                .ForMember(x => x.RegisterDate, o => o.MapFrom(s => s.CurrentFrom))
                .ForMember(x => x.CurrentTo, o => o.MapFrom(s => s.CurrentTo))
                .ForMember(x => x.ActionType, o => o.MapFrom(s => (AudictTypeService?)s.TransaccionId))
                .ForMember(x => x.Package, o => o.MapFrom(s => new PackageServiceModel { Id = s.SchemaId.Value }));
            });
            return config;
        }
    }
}