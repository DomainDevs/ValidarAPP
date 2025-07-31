using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Componentes pagadores
    /// </summary>
    [DataContract]
    public class CompanyPayerComponent : BasePayerComponent
    {
        [DataMember]
        public CompanyComponent Component { get; set; }
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }
        [DataMember]
        public CompanyCoverage Coverage { get; set; }
    }
}
