using Sistran.Core.Application.ModelServices.Models.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies
{
    [DataContract]
    public class ModuleSubmoduleServicesQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de modulos y subModulos asociados
        /// </summary>
        [DataMember]
        public List<ModuleSubmoduleServiceQueryModel> ModuleSubModuleQueryModel { get; set; }
    }
}
