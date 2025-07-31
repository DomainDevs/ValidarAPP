using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyInsuredGuaranteePledge : BaseInsuredGuarantee
    {
        /// <summary>
        /// valor de avaluo
        /// </summary>
        [DataMember]
        public decimal AppraisalAmount { get; set; }

        /// <summary>
        /// Fecha de avalúo
        /// </summary>
        [DataMember]
        public DateTime AppraisalDate { get; set; }

        /// <summary>
        /// Placa
        /// </summary>
        [DataMember]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Numero de motor
        /// </summary>
        [DataMember]
        public string EngineNumer { get; set; }

        /// <summary>
        /// Numero de chassis
        /// </summary>
        [DataMember]
        public string ChassisNumer { get; set; }

        /// <summary>
        /// Compañía Aseguradora Id
        /// </summary>
        [DataMember]
        public decimal InsuranceCompanyId { get; set; }

        /// <summary>
        /// Compañía Aseguradora
        /// </summary>
        [DataMember]
        public string InsuranceCompany { get; set; }

        /// <summary>
        /// Nro.Póliza
        /// </summary>
        [DataMember]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Valor Asegurado
        /// </summary>
        [DataMember]
        public decimal InsuranceValueAmount { get; set; }

    }
}
