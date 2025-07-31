using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models

{
    [DataContract]
    public class CompanyClause : BaseClause
    {
        /// <summary>
        /// Nivel De Condición
        /// </summary>
        [DataMember]
        public ConditionLevel ConditionLevel { get; set; }
    }
}
