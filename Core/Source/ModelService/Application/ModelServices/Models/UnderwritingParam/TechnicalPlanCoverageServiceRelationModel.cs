using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    public class TechnicalPlanCoverageServiceRelationModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Objeto del Seguro
        /// </summary>
        [DataMember]
        public InsuredObjectServiceQueryModel InsuredObject { get; set; }
        /// <summary>
        /// Obtiene o establece la Cobertura
        /// </summary>
        [DataMember]
        public CoverageServiceQueryModel Coverage { get; set; }
        /// <summary>
        /// Obtiene o establece la Cobertura Principal
        /// </summary>
        [DataMember]
        public CoverageServiceQueryModel PrincipalCoverage { get; set; }
        /// <summary>
        /// Obtiene o establece el Porcentaje de Suma
        /// </summary>
        [DataMember]
        public decimal? CoveragePercentage { get; set; }
        /// <summary>
        /// Obtiene o establece la Lista de Coberturas Aliadas
        /// </summary>
        public List<AllyCoverageServiceModel> AlliedCoverages { get; set; }
    }
}
