using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredGuaranteeMortgageDTO
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Address { get; set; }

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

        [DataMember]
        public bool ClosedInd { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

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
        public DateTime? AppraisalDate { get; set; }

        /// <summary>
        /// ExpertName
        /// </summary>
        [DataMember]
        public string ExpertName { get; set; }

        [DataMember]
        public SelectDTO AssetType { get; set; }

        /// <summary>
        /// numero de escritura
        /// </summary>
        [DataMember]
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Valor Avalúo
        /// </summary>
        [DataMember]
        public decimal InsuranceValueAmount { get; set; }

        /// <summary>
        /// Área Medida
        /// </summary>
        [DataMember]
        public decimal MeasureAreaQuantity { get; set; }

        /// <summary>
        /// Área construida
        /// </summary>
        [DataMember]
        public decimal BuiltAreaQuantity { get; set; }

        /// <summary>
        /// Tipo de medidas
        /// </summary>
        [DataMember]
        public SelectDTO MeasurementType { get; set; }

        /// <summary>
        /// Compañia aseguradora
        /// </summary>
        [DataMember]
        public SelectDTO InsuranceCompany { get; set; }
        /// <summary>
        /// Nro.Póliza
        /// </summary>
        [DataMember]
        public string PolicyNumber { get; set; }


    }
}
