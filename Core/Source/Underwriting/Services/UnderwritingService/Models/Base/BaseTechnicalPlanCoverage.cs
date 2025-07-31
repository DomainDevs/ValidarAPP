using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Coberturas por Planes Tecnicos
    /// </summary>
    [DataContract]
    public class BaseTechnicalPlanCoverage : Extension
    {
        /// <summary>
        /// Atributo Codigo para Technical Plan Id
        /// </summary>
        [DataMember]
        public int TechnicalPlanId { get; set; }

        /// <summary>
        /// Atributo Codigo para Coverage Id
        /// </summary>
        [DataMember]
        public int CoverageId { get; set; }

        /// <summary>
        /// Atributo Is Sublimit
        /// </summary>
        [DataMember]
        public bool IsSublimit { get; set; }

        /// <summary>
        /// Atributo Codigo para Main CoverageId
        /// </summary>
        [DataMember]
        public int? MainCoverageId { get; set; }

        /// <summary>
        /// Atributo Valor MOneda MainCoveragePercentage
        /// </summary> 
        [DataMember]
        public decimal? MainCoveragePercentage { get; set; }
    }
}
