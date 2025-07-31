using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using System.Runtime.Serialization;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class ConceptDescription : BaseConceptDescription
    {
        [DataMember]
        public PoliciesAut Policies { set; get; }

        [DataMember]
        public MRules._Concept Concept { set; get; }

        [DataMember]
        public ConceptDescriptionValue ConceptDescriptionValue { set; get; }
    }
}
