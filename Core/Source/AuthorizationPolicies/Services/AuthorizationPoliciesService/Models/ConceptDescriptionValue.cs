using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class ConceptDescriptionValue : BaseConceptDescriptionValue
    {

        [DataMember]
        public _Concept Concept { set; get; }
       
    }
}
