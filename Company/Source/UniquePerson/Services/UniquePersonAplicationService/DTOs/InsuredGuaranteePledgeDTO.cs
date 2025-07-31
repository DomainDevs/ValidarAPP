using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredGuaranteePledgeDTO
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public GuaranteeDTO Guarantee { get; set; }

        [DataMember]
        public SelectDTO Branch { get; set; }

        [DataMember]
        public SelectDTO City { get; set; }

        [DataMember]
        public SelectDTO Country { get; set; }

        [DataMember]
        public SelectDTO State { get; set; }


        [DataMember]
        public SelectDTO Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool ClosedInd { get; set; }

        /// <summary>
        /// fecha registro
        /// </summary>
        [DataMember]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// fecha ultima actualizacion
        /// </summary>
        [DataMember]
        public DateTime LastChangeDate { get; set; }

        [DataMember]
        public SelectDTO Status { get; set; }


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
        /// Compañía Aseguradora
        /// </summary>
        [DataMember]
        public SelectDTO InsuranceCompany { get; set; }

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
