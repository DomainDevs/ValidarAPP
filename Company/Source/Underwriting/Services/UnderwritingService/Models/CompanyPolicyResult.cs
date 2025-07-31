using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Resultado Grabado Poliza
    /// </summary>
    [DataContract]
    public class CompanyPolicyResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsError { get; set; }

        /// <summary>
        /// Gets or sets the document number.
        /// </summary>
        /// <value>
        /// The document number.
        /// </value>
        [DataMember]
        public decimal DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [DataMember]
        public List<ErrorBase> Errors { get; set; }

        /// <summary>
        /// Gets or sets the infringe policies.
        /// </summary>
        /// <value>
        /// The infringe policies.
        /// </value>
        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int TemporalId { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }
        
        [DataMember]
        public int EndorsementNumber { get; set; }

        [DataMember]
        public int PromissoryNoteNumCode { get; set; }
        [DataMember]
        public bool IsReinsured { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
    }
}