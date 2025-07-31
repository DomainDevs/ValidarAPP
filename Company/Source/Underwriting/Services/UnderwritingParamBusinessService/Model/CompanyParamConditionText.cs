using Sistran.Core.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    /// <summary>
    /// CompanyParamConditionText. Condition Text Model Company.
    /// </summary>
    [DataContract]
    public class CompanyParamConditionText: ParamConditionText
    {
        /// <summary>
        /// Get or sets Objeto ConditionTextLevel.
        /// </summary>
        [DataMember]
        public CompanyParamConditionTextlevelType companyConditionTextLevelType { get; set; }

        /// <summary>
        /// Get or sets Objeto ConditionTextLevel.
        /// </summary>
        [DataMember]
        public CompanyParamConditionTextlevel companyConditionTextLevel { get; set; }
    }
}
