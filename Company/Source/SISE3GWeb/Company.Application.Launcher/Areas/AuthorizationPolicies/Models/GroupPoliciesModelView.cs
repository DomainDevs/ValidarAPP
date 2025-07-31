using System;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Sistran.Company.Application.ModelServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class  GroupPoliciesModelView
    {

        public int IdGroupPolicies { set; get; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "OptionSelect")]
        public int IdModule { set; get; }

        public int IdSubModule { set; get; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "OptionSelect")]
        public int IdPackage { set; get; }

        [StringLength(30, ErrorMessage = "Maximo 30 Caracteres")]
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public string Description { set; get; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "OptionSelect")]
        public int IdPrefix { set; get; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "OptionSelect")]
        public int IdCoveredRisk { set; get; }

        public StatusTypeService StatusTypeService { set; get; }

        public Module Module { get; set; }
        public SubModule SubModule { get; set; }
        public _Package Package { get; set; }

        public string Key { get; set; }

        ///<summary>
        ///Se creó validación para el tipo de riesgo que sea mostrado cuando n ose selecciona un valor.
        ///</summary>
        ///<author>Diego Leon</author>
        ///<date>24/08/2018</date>
        ///<purpose>REQ_#079</purpose>
        ///<returns></returns>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "OptionSelect")]
        public int IdRisk { set; get; }

        

        internal static GroupPoliciesModelView CreateGroupPoliciesModelView(GroupPolicies group)
        {
            GroupPoliciesModelView model = new GroupPoliciesModelView();
            model.Description = group.Description;
            model.IdGroupPolicies = group.IdGroupPolicies;
            model.Module = group.Module;
            model.IdModule = group.Module.Id;
            model.SubModule = group.SubModule;
            model.IdSubModule = group.SubModule.Id;
            model.Package = group.Package;
            model.IdPackage = group.Package.PackageId;
            model.Key = group.Key;

            return model;
        }

        internal static GroupPolicies CreateGroupPolicies(GroupPoliciesModelView group)
        {
            GroupPolicies groupPolicies = new GroupPolicies();
            groupPolicies.Description = group.Description;
            groupPolicies.IdGroupPolicies = group.IdGroupPolicies;
            groupPolicies.Key = group.Key;
            groupPolicies.Module = new Module {Id = group.Module.Id , Description = group.Module.Description };
            groupPolicies.SubModule = new SubModule { Id = group.SubModule.Id , Description = group.SubModule.Description };
            groupPolicies.Package = new _Package { PackageId = group.Package.PackageId , Description = group.Package.Description };            
            return groupPolicies;
        }

        internal static List<GroupPolicies> CreateGroupPolicies(List<GroupPoliciesModelView> groupPolicies)
        {
            throw new NotImplementedException();
        }
    }
}