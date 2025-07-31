namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// MOD-S Tipos de riesgo cubiertos
    /// </summary>
    [DataContract]
    public class CoveredRiskTypesQueryServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece los tipos de riesgo cubiertos
        /// </summary>
        [DataMember]
        public List<CoveredRiskTypeQueryServiceModel> CoveredRiskTypeQueryServiceModels { get; set; }
    }
}
