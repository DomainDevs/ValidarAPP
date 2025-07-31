using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    public class AutoMapperInitialize
    {
        static AutoMapperInitialize()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                /*Underwriting*/
                cfg.CreateMap<CompanySummary, ComponentValueDTO>();

                /*Unique Users*/
                cfg.CreateMap<IndividualRelationAppModelsView, Sistran.Core.Application.UniquePersonService.V1.Models.IndividualRelationApp>();
                cfg.CreateMap<Agency, AgencyModelsView>();
                cfg.CreateMap<Agent, AgentModelsView>();
                cfg.CreateMap<List<Application.UniquePersonService.V1.Models.IndividualRelationApp>, List<IndividualRelationAppModelsView>>()
               .ConvertUsing(ss => ss.Select(bs => AutoMapper.Mapper.Map<Sistran.Core.Application.UniquePersonService.V1.Models.IndividualRelationApp, IndividualRelationAppModelsView>(bs)).ToList());

                cfg.CreateMap<CoHierarchyAssociation, CoHierarchyAssociationModelsView>();
                cfg.CreateMap<Module, ModuleModelsView>();
                cfg.CreateMap<SubModule, SubModuleModelsView>();
                cfg.CreateMap<List<CoHierarchyAssociation>, List<CoHierarchyAssociationModelsView>>()
               .ConvertUsing(ss => ss.Select(bs => AutoMapper.Mapper.Map<CoHierarchyAssociation, CoHierarchyAssociationModelsView>(bs)).ToList());

                cfg.CreateMap<Profile, ProfileModelsView>();
                cfg.CreateMap<AccessProfile, ProfileAccessView>();
                cfg.CreateMap<List<Profile>, List<ProfileModelsView>>()
              .ConvertUsing(ss => ss.Select(bs => AutoMapper.Mapper.Map<Profile, ProfileModelsView>(bs)).ToList());
                
                /* Any other module */
            });
        }
        public static void Run()
        { }
    }
}