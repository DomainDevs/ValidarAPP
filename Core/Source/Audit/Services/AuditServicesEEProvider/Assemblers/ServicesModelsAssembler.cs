using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AUDModel = Sistran.Core.Application.AuditServices.Models;
using AUDSM = Sistran.Core.Application.ModelServices.Models.Audit;
namespace Sistran.Core.Application.AuditServices.EEProvider.Assemblers
{
    public class ServicesModelsAssembler
    {
        #region Auditoria 


        /// <summary>
        /// Creates the audit.
        /// </summary>
        /// <param name="auditServiceModel">The audit service model.</param>
        /// <returns></returns>
        public static AUDModel.Audit CreateAudit(AUDSM.AuditServiceModel auditServiceModel)
        {
            AUDModel.Audit audit = new AUDModel.Audit();
            var config = MapperCache.GetMapper<AUDSM.AuditServiceModel, AUDModel.Audit>(cfg =>
            {
                cfg.CreateMap<AUDSM.AuditServiceModel, AUDModel.Audit>();
                cfg.CreateMap<AUDSM.UserServiceModel, AUDModel.User>();
                cfg.CreateMap<AUDSM.PackageServiceModel, AUDModel.Package>();
                cfg.CreateMap<ConcurrentBag<AUDSM.AuditChangeServiceModel>, ConcurrentBag<AUDModel.AuditChange>>();
            });
            audit = config.Map<AUDSM.AuditServiceModel, AUDModel.Audit>(auditServiceModel);
            return audit;
        }


        /// <summary>
        /// Creates the audits.
        /// </summary>
        /// <param name="auditServiceModel">The audit service model.</param>
        /// <returns></returns>
        public static List<AUDModel.Audit> CreateAudits(List<AUDSM.AuditServiceModel> auditServiceModel)
        {
            List<AUDModel.Audit> audits = new List<AUDModel.Audit>();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AUDSM.AuditServiceModel, AUDModel.Audit>().
                ForMember(x => x.Changes, opts => opts.MapFrom(y => y.Changes));
                cfg.CreateMap<AUDSM.AuditChangeServiceModel, AUDModel.AuditChange>();
                cfg.CreateMap<List<AUDSM.AuditChangeServiceModel>, List<AUDModel.AuditChange>>()
                .ConvertUsing(ss => ss.Select(bs => Mapper.Map<AUDSM.AuditChangeServiceModel, AUDModel.AuditChange>(bs)).ToList());
                cfg.CreateMap<AUDModel.User, AUDSM.UserServiceModel>();

            });
            audits = Mapper.Map<List<AUDSM.AuditServiceModel>, List<AUDModel.Audit>>(auditServiceModel);
            return audits;
        }
        #endregion
        #region Paquetes 


        /// <summary>
        /// Creates the package.
        /// </summary>
        /// <param name="packageServiceModel">The package service model.</param>
        /// <returns></returns>
        public static AUDModel.Package CreatePackage(AUDSM.PackageServiceModel packageServiceModel)
        {
            AUDModel.Package package = new AUDModel.Package();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AUDSM.PackageServiceModel, AUDModel.Package>();
            });
            package = Mapper.Map<AUDSM.PackageServiceModel, AUDModel.Package>(packageServiceModel);
            return package;
        }

        /// <summary>
        /// Creates the packages.
        /// </summary>
        /// <param name="packageServiceModels">The package service models.</param>
        /// <returns></returns>
        public static List<AUDModel.Package> CreatePackages(List<AUDSM.PackageServiceModel> packageServiceModels)
        {
            List<AUDModel.Package> packages = new List<AUDModel.Package>();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AUDSM.PackageServiceModel, AUDModel.Package>();
                cfg.CreateMap<List<AUDSM.PackageServiceModel>, List<AUDModel.Package>>()
                .ConvertUsing(ss => ss.Select(bs => Mapper.Map<AUDSM.PackageServiceModel, AUDModel.Package>(bs)).ToList());
            });
            packages = Mapper.Map<List<AUDSM.PackageServiceModel>, List<AUDModel.Package>>(packageServiceModels);
            return packages;
        }
        #endregion
    }
}
