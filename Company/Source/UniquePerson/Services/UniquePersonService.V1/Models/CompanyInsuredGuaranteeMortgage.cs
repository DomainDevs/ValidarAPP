using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyInsuredGuaranteeMortgage : BaseInsuredGuarantee
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
        public DateTime? AppraisalDate { get; set; }

        /// <summary>
        /// ExpertName
        /// </summary>
        [DataMember]
        public string ExpertName { get; set; }

        [DataMember]
        public CompanyAssetType AssetType { get; set; }

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
        public CompanyMeasurementType MeasurementType { get; set; }

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

    }
}
