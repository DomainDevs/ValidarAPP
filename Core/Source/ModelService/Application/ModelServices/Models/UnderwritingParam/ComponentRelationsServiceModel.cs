using Sistran.Core.Application.ModelServices.Models.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
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
