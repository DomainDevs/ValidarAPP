using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.MassiveServices.Models
{
    [DataContract]
    public class FilterIndividual
    {
        /// <summary>
        /// tipo de individuo 
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }

        /// <summary>
        /// Codigo asegurado
        /// </summary>
        [DataMember]
        public int? InsuredCode { get; set; }

        /// <summary>
        /// Modelo persona
        /// </summary>
        [DataMember]
        public MassivePerson Person { get; set; }
       
        /// <summary>
        /// Modelo compañia
        /// </summary>
        [DataMember]
        public MassiveCompany Company { get; set; }
        
        /// <summary>
        ///  Es asegurado
        /// </summary>
        [DataMember]
        public bool IsInsured { get; set; }
        
        /// <summary>
        ///  Mensaje de error
        /// </summary>
        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public int? AgentCode { get; set; }

        [DataMember]
        public int? AgentTypeCode { get; set; }

        public string GetHash()
        {
            if (InsuredCode.HasValue)
            {
                return "IC" + InsuredCode;
            }
            else if (Person != null)
            {
                return "P" + Person.DocumentTypeId + Person.DocumentNumber;
            }
            else if (Company != null)
            {
                return "C" + Company.DocumentTypeId + Company.DocumentNumber;
            }
            else
            {
                return "Error";
            }
        }

        /// <summary>
        ///  es lista clinton
        /// </summary>
        [DataMember]
        public bool? IsCLintonList { get; set; }

        /// <summary>
        /// Almacena error por sarlaft
        /// </summary>
        [DataMember]
        public string SarlaftError { get; set; }

        /// <summary>
        /// Fecha de baja del beneficiario
        /// </summary>
        [DataMember]
        public System.DateTime? DeclinedDate { get; set; }
    }
}