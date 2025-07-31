using System.Runtime.Serialization;

namespace Sistran.Core.Integration.PropertyServices.DTOs
{
    [DataContract]
    public class RiskLocationDTO
    {
        /// <summary>
        /// Obtener o setear Años de construccion
        /// </summary>
        [DataMember]
        public int ConstructionYear { get; set; }

        /// <summary>
        /// Numero de pisos
        /// </summary>
        [DataMember]
        public int FloorNumber { get; set; }

        /// <summary>
        /// Obtener o setear Tiene nomenclatura
        /// </summary>
        [DataMember]
        public bool HasNomenclature { get; set; }

        /// <summary>
        /// Obtener o setear Latitud
        /// </summary>
        [DataMember]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Longitud
        /// </summary>
        [DataMember]
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Obtener o setear Direccion
        /// </summary>
        [DataMember]
        public string FullAddress { get; set; }

        /// <summary>
        /// Obtener o setear Es declarado
        /// </summary>
        [DataMember]
        public bool IsDeclarative { get; set; }

        /// <summary>
        /// Obtener o setear PML
        /// </summary>
        [DataMember]
        public decimal? PML { get; set; }

        /// <summary>
        /// Obtener o setear Calle
        /// </summary>
        [DataMember]
        public string Square { get; set; }

        /// <summary>
        /// Obtener o Setear Edad riesgo  
        /// </summary>
        [DataMember]
        public int RiskAge { get; set; }

        /// <summary>
        /// Id del Riesgo
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }

        /// <summary>
        /// Monto Asegurado
        /// </summary>
        [DataMember]
        public decimal AmountInsured { get; set; }

        [DataMember]
        public int CoveredRiskType { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public int? CountryId { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public int? StateId { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public int? CityId { get; set; }

        [DataMember]
        public decimal? DocumentNum { get; set; }

        [DataMember]
        public int? PolicyId { get; set; }

        [DataMember]
        public int? EndorsementId { get; set; }

        [DataMember]
        public int InsuredId { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }
    }
}
