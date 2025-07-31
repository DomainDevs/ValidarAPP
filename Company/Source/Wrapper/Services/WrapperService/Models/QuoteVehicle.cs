
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteVehicle
    {
        #region RiskCore          
        [DataMember]
        public decimal AmountInsured { get; set; }

        [DataMember]
        public List<Beneficiary> Beneficiaries { get; set; }

        [DataMember]
        public List<Clause> Clauses { get; set; }

        [DataMember]
        public List<QuoteCoverage> Coverages { get; set; }

        [DataMember]
        public CoveredRiskType? CoveredRiskType { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<Core.Application.RulesScriptsServices.Models.DynamicConcept> DynamicProperties { get; set; }

        [DataMember]
        public QuoteGroupCoverage GroupCoverage { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public bool IsInsuredPayer { get; set; }

        [DataMember]
        public LimitRc LimitRc { get; set; }

        [DataMember]
        public Core.Application.UniquePersonService.V1.Models.Insured MainInsured { get; set; }

        [DataMember]
        public Core.Application.UnderwritingServices.Enums.RiskStatusType? OriginalStatus { get; set; }

        [DataMember]
        public List<QuotePayerComponent> PayerComponents { get; set; }

        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public RatingZone RatingZone { get; set; }

        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public Sistran.Core.Application.UnderwritingServices.Enums.RiskStatusType? Status { get; set; }

        [DataMember]
        public Text Text { get; set; }

        #endregion

        #region VehicleCore
        [DataMember]
        public List<Accessory> Accesories { get; set; }

        [DataMember]
        public Body Body { get; set; }

        [DataMember]
        public string ChassisSerial { get; set; }

        [DataMember]
        public Color Color { get; set; }

        [DataMember]
        public string EngineSerial { get; set; }

        [DataMember]
        public bool IsImported { get; set; }

        [DataMember]
        public bool IsNew { get; set; }

        [DataMember]
        public bool IsTruck { get; set; }

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public int LoadTypeCode { get; set; }

        [DataMember]
        public Make Make { get; set; }

        [DataMember]
        public Model Model { get; set; }

        [DataMember]
        public decimal NewPrice { get; set; }

        [DataMember]
        public decimal OriginalPrice { get; set; }

        [DataMember]
        public int PassengerQuantity { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public decimal PriceAccesories { get; set; }

        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public decimal SingleRate { get; set; }

        [DataMember]
        public decimal StandardVehiclePrice { get; set; }

        [DataMember]
        public int TrailersQuantity { get; set; }

        [DataMember]
        public Use Use { get; set; }

        [DataMember]
        public Version Version { get; set; }

        [DataMember]
        public int Year { get; set; }

        #endregion

        #region VehicleCompany
        [DataMember]
        public Fasecolda CompanyFasecolda { get; set; }

        [DataMember]
        public ServiceType ServiceType { get; set; }

        #endregion
    }
}
