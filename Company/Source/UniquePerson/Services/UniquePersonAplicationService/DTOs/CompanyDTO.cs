using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class CompanyDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string Document { get; set; }

        [DataMember]
        public string BusinessName { get; set; }

        [DataMember]
        public int AssociationTypeId { get; set; }

        [DataMember]
        public int CompanyTypeId { get; set; }

        [DataMember]
        public int CountryOriginId { get; set; }

        [DataMember]
        public string CheckPayable { get; set; }

        [DataMember]
        public int EconomicActivityId { get; set; }

        [DataMember]
        public string EconomicActivityDescription { get; set; }

        [DataMember]
        public List<AddressDTO> Addresses { get; set; }

        [DataMember]
        public List<EmailDTO> Emails { get; set; }

        [DataMember]
        public List<PhoneDTO> Phones { get; set; }

        [DataMember]
        public int? VerifyDigit { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int? ExonerationTypeCode { get; set; }

        [DataMember]
        public List<IndividualSarlaftDTO> Sarlaft { get; set; }
        [DataMember]
        public List<ConsorciatedDTO> ConsortiumMembers { get; set; }

        [DataMember]
        public InsuredDTO Insured { get; set; }

        [DataMember]
        public string NitAssociationType { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }
        
        [DataMember]
        public ConsortiumEventDTO ConsortiumeventDTO { get; set; }

        [DataMember]
        public List<ConsortiumEventDTO> ConsortiumEventDTOs { get; set; }

    }
}
