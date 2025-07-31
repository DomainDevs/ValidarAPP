using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class PersonDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Names { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string SecondSurname { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
        [DataMember]
        public string Document { get; set; }
        [DataMember]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public string BirthPlace { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public int MaritalStatusId { get; set; }
        [DataMember]
        public int? ExonerationTypeCode { get; set; }
        [DataMember]
        public List<AddressDTO> Addresses { get; set; }
        [DataMember]
        public List<EmailDTO> Emails { get; set; }
        [DataMember]
        public List<PhoneDTO> Phones { get; set; }
        [DataMember]
        public int EconomicActivityId { get; set; }
        [DataMember]
        public string EconomicActivityDescription { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public List<IndividualSarlaftDTO> Sarlaft { get; set; }

        [DataMember]
        public string CheckPayable { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public  List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }
        [DataMember]
        public bool DataProtection { get; set; }

        /// <summary>
        /// DocumentNumber
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// FullName
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public IdentificationDocumentDTO IdentificationDocument { get; set; }

        [DataMember]
        public bool ElectronicBiller { get; set; }

        

    }
}
