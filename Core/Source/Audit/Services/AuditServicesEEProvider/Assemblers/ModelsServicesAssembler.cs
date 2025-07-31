using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Generic;
using System.Linq;
using AUDModel = Sistran.Core.Application.AuditServices.Models;
using AUDSM = Sistran.Core.Application.ModelServices.Models.Audit;
namespace Sistran.Core.Application.AuditServices.EEProvider.Assemblers
{
    public class ModelsServicesAssembler
    {
        #region Auditoria 


        /// <summary>
        /// Creates the audit.
        /// </summary>
        /// <param name="audit">The audit.</param>
        /// <returns></returns>
        public static AUDSM.AuditServiceModel CreateAudit(AUDModel.Audit audit)
        {
            AUDSM.AuditServiceModel auditServiceModel = new AUDSM.AuditServiceModel();

            var config = MapperCache.GetMapper<AUDModel.Audit, AUDSM.AuditServiceModel>(cfg =>
            {
                cfg.CreateMap<AUDModel.Audit, AUDSM.AuditServiceModel>();
            });

            var configChange = MapperCache.GetMapper<AUDModel.AuditChange, AUDSM.AuditChangeServiceModel>(cfg =>
            {
                cfg.CreateMap<AUDModel.AuditChange, AUDSM.AuditChangeServiceModel>();
            });

            auditServiceModel = config.Map<AUDModel.Audit, AUDSM.AuditServiceModel>(audit);
            auditServiceModel.StatusTypeService = StatusTypeService.Original;
            var auditServiceChanges = audit.Changes.Select(a => configChange.Map<AUDModel.AuditChange, AUDSM.AuditChangeServiceModel>(a)).ToList();
            auditServiceModel.Changes = auditServiceChanges;
            auditServiceModel.Changes.AsParallel().ForAll(x => x.StatusTypeService = StatusTypeService.Original);
            return auditServiceModel;
        }


        /// <summary>
        /// Creates the audits.
        /// </summary>
        /// <param name="audit">The audit.</param>
        /// <returns></returns>
        public static List<AUDSM.AuditServiceModel> CreateAudits(List<AUDModel.Audit> audit)
        {
            try
            {
                var result = new List<AUDSM.AuditServiceModel>();
                if (audit != null && audit.Count > 0)
                {
                    var config = MapperCache.GetMapper<AUDModel.Audit, AUDSM.AuditServiceModel>(cfg =>
                    {
                        cfg.CreateMap<AUDModel.AuditChange, AUDSM.AuditChangeServiceModel>();
                        cfg.CreateMap<AUDModel.Audit, AUDSM.AuditServiceModel>()
                        .ForMember(dest => dest.Changes, opt => opt.MapFrom(src => src.Changes));
                        cfg.CreateMap<AUDModel.User, AUDSM.UserServiceModel>();
                    });
                    result = config.Map<List<AUDModel.Audit>, List<AUDSM.AuditServiceModel>>(audit);
                }
                result.AsParallel().ForAll(x =>
                {
                    x.StatusTypeService = StatusTypeService.Original;
                    x.Changes.AsParallel().ForAll(y => y.StatusTypeService = StatusTypeService.Original);
                });
                return result;

            }
            catch (System.Exception ex)
            {

                throw;
            }

        }
        #endregion
        #region paquetes
        public static List<AUDSM.PackageServiceModel> CreatePackages(List<AUDModel.Package> packages)
        {
            var packageServiceModels = new List<AUDSM.PackageServiceModel>();
            if (packages != null && packages.Count > 0)
            {
                var config = MapperCache.GetMapper<AUDModel.Package, AUDSM.PackageServiceModel>(cfg =>
                {
                    cfg.CreateMap<AUDModel.Package, AUDSM.PackageServiceModel>();

                });
                packageServiceModels = config.Map(packages, packageServiceModels);
            }
            return packageServiceModels;
        }
        #endregion

        #region Entidades
        public static List<AUDSM.EntityServiceModel> CreateEntities(List<AUDModel.Entity> entities)
        {
            var entityServiceModels = new List<AUDSM.EntityServiceModel>();
            if (entities != null && entities.Count > 0)
            {
                var config = MapperCache.GetMapper<AUDModel.Entity, AUDSM.EntityServiceModel>(cfg =>
                {
                    cfg.CreateMap<AUDModel.Entity, AUDSM.EntityServiceModel>();
                });
                entityServiceModels = config.Map(entities, entityServiceModels);
            }
            return entityServiceModels;
        }
        #endregion
    }
}
