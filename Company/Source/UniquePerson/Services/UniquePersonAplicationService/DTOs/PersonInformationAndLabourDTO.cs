using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    using Core.Application.AuthorizationPoliciesServices.Models;

    [DataContract]
    public class PersonInformationAndLabourDTO
    {        
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// IndividualID Person
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int TypePerson { get; set; }
        [DataMember]
        public int? EducativeLevel { get; set; }
        [DataMember]
        public int? Children { get; set; }
        [DataMember]
        public int? HouseType { get; set; }
        [DataMember]
        public int? SocialLayer { get; set; }
        [DataMember]
        public string SpouseName { get; set; }
        [DataMember]
        public int? Occupation { get; set; }
        [DataMember]
        public int? Speciality { get; set; }
        [DataMember]
        public int? OtherOccupation { get; set; }
        [DataMember]
        public int? IncomeLevel { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string JobSector { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public int? CompanyPhone { get; set; }
        [DataMember]
        public int? PersonType { get; set; }

        [DataMember]
        public int BirthCountryId { get; set; }

        [DataMember]
        public List<PersonInterestGroupDTO> PersonInterestGroup { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }
    }
}
