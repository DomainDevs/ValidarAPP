using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class CustomerKnowledgeDTO
    {
        [DataMember]
        public FinancialSarlaftDTO FinancialSarlaftDTO { get; set; }

        [DataMember]
        public SarlaftDTO SarlaftDTO { get; set; }

        [DataMember]
        public List<LegalRepresentativeDTO> LegalRepresentDTO { get; set; }

        [DataMember]
        public List<LinkDTO> LinksDTO { get; set; }

        [DataMember]
        public List<PartnersDTO> PartnerDTO { get; set; }

        [DataMember]
        public List<InternationalOperationDTO> InternationalOperationDTO { get; set; }

        [DataMember]
        public PepsDTO PepsDTO { get; set; }
        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int OperationId { get; set; }

        [DataMember]
        public CoSarlaftDTO CoSarlaftDTO { get; set; }

        [DataMember]
        public SarlaftExonerationtDTO SarlaftExonerationtDTO { get; set; }

        
    }
}
