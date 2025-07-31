using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ModelServices.Models.UnderwritingParam
{
    [DataContract]
    public class ComponentRelationsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo ComponentRelationServiceModel
        /// </summary>
        [DataMember]
        public List<ComponentRelationServiceModel> ComponentRelationServiceModels { get; set; }
    }
}
