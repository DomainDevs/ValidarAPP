using Sistran.Core.Application.ModelServices.Models.Param;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    [DataContract]
    public class FirstPayComponentServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>
        [DataMember]
        public int ComponentId { get; set; }

        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>
        [DataMember]
        public int FinancialPlanId { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
