using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class Company 
    {
        /// <summary>
        /// role
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// EconomicActivity
        /// </summary>
        public EconomicActivity EconomicActivity { get; set; }

        /// <summary>
        /// IdentificationDocument
        /// </summary>
        public IdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// CompanyType
        /// </summary>
        public CompanyType CompanyType { get; set; }

        /// <summary>
        /// AssociationType
        /// </summary>
        public AssociationType AssociationType { get; set; }

        /// <summary>
        /// Insured
        /// </summary>
        public Insured Insured { get; set; }

        /// <summary>
        /// Consortiums
        /// </summary>
        public List<Consortium> Consortiums { get; set; }

        /// <summary>
        /// CountryId
        /// </summary>
        [DataMember]
        public int CountryId { get; set; }

        /// <summary>
        /// VerifyDigit
        /// </summary>
        public int? VerifyDigit { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Tipo de individuo
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }


        /// <summary>
        /// Tipo Cliente
        /// </summary>
        [DataMember]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Tipo Cliente
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// CustomerTypeDescription
        /// </summary>
        [DataMember]
        public string CustomerTypeDescription { get; set; }

    }
}