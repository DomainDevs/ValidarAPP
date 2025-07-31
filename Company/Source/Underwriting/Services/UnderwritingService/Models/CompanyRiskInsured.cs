using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Resumen de la Pliza
    /// </summary>
    [DataContract]
    public class CompanyRiskInsured
    {
        /// <summary>
        /// Obtiene o establece IndividualId de tomador del riesgo
        /// </summary>
        [DataMember]
        public CompanyIssuanceInsured Insured { get; set; }

        /// <summary>
        /// Obtiene o establece lista con individual id de beneficiarios
        /// </summary>
        [DataMember]
        public List<CompanyBeneficiary> Beneficiaries { get; set; }
    }
}
