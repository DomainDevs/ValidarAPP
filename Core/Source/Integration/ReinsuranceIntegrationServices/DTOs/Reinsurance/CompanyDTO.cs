using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class CompanyDTO
    {
        /// <summary>
        /// Identificador del individuo
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

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
