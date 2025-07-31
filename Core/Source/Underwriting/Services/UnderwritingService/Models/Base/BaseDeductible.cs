using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class BaseDeductible
    {
        /// <summary>
        /// Atributo para la propiedad Id
        /// </summary> 
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Atributo para la propiedad Rate
        /// </summary> 
        [DataMember]
        public decimal? Rate { get; set; }

        /// <summary>
        /// Atributo para la propiedad DeductPremiumAmount
        /// </summary> 
        [DataMember]
        public decimal DeductPremiumAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad AccDeductAmt
        /// </summary> 
        [DataMember]
        public decimal AccDeductAmt { get; set; }

        /// <summary>
        /// Atributo para la propiedad DeductValue
        /// </summary> 
        [DataMember]
        public decimal DeductValue { get; set; }

        /// <summary>
        /// Atributo para la propiedad MaxDeductValue
        /// </summary> 
        [DataMember]
        public decimal? MaxDeductValue { get; set; }

        /// <summary>
        /// Atributo para la propiedad MinDeductValue
        /// </summary> 
        [DataMember]
        public decimal? MinDeductValue { get; set; }

        /// <summary>
        /// Atributo para la propiedad Description
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad IsDefault
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Atributo para la propiedad RateType
        /// </summary> 
        [DataMember]
        public RateType RateType { get; set; }
    }
}
