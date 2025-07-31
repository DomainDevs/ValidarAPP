//using Sistran.Company.Application.Locations.Models;
using Sistran.Company.Application.Locations.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.Location.PropertyServices.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.Models
{
    /// <summary>
    /// Riesgo hogar
    /// </summary>
    [DataContract]
    public class CompanyPropertyRisk : BasePropertyRisk
    {
        [DataMember]
        public CompanyRisk Risk { get; set; }

        [DataMember]
        public CompanyRiskActivity RiskActivity { get; set; }

        [DataMember]
        public List<CompanyInsuredObject> InsuredObjects { get; set; }

        [DataMember]
        public CompanyIssuanceInsured MainInsured { get; set; }
        /// <summary>
        /// Obtener o setear Tipo de nomenclatura
        /// </summary>
        [DataMember]
        public CompanyNomenclatureAddress NomenclatureAddress { get; set; }

        /// <summary>
        /// Obtener o setear Ciudad
        /// </summary>
        [DataMember]
        public City City { get; set; }

        /// <summary>
        /// Tipo de construccion
        /// </summary>
        [DataMember]
        public CompanyConstructionType ConstructionType { get; set; }

        /// <summary>
        /// Tipo de riesgo
        /// </summary>
        [DataMember]
        public RiskType RiskType { get; set; }

        /// <summary>
        /// Obtener o setear Uso del riesgo
        /// </summary>
        [DataMember]
        //public CompanyRiskUse RiskUse { get; set; }
        public CompanyRiskUse RiskUse { get; set; }


        /// <summary>
        /// Localidad
        /// </summary>
        [DataMember]
        public CompanyLocation CompanyLocation { get; set; }

        /// <summary>
        /// uso vivienda
        /// </summary>
        [DataMember]
        public CompanyUseHouse CompanyUseHouse { get; set; }

        /// <summary>
        /// tipo vivienda
        /// </summary>
        [DataMember]
        public CompanyHouseType CompanyHouseType { get; set; }

        [DataMember]
        public CompanyAssuranceMode AssuranceMode { get; set; }

        [DataMember]
        public CompanyRiskSubActivity RiskSubActivity { get; set; }
        
        [DataMember]
        public bool PrincipalRisk { get; set; }

        [DataMember]
        public int? HomeType { get; set; }

        [DataMember]
        public int? HomeUse { get; set; }

        [DataMember]
        public int BillingPeriodDepositPremium { get; set; }

        [DataMember]
        public int DeclarationPeriodCode { get; set; }

        /// <summary>
        /// Periodo de Declaración
        /// </summary>
        [DataMember]
        public DeclarationPeriod DeclarationPeriod { get; set; }
        /// <summary>
        /// Periodo de Ajuste 
        /// </summary>
        [DataMember]
        public AdjustPeriod AdjustPeriod { get; set; }
    }
}
