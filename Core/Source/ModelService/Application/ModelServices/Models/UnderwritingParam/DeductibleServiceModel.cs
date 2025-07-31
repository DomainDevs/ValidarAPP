// -----------------------------------------------------------------------
// <copyright file="DeductibleServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de moneda
    /// </summary>
    [DataContract]
    public class DeductibleServiceModel : ParametricServiceModel
    {
       /// <summary>
        /// Atributo para la propiedad Id
        /// </summary> 
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Atributo para la propiedad RateType
        /// </summary> 
        [DataMember]
        public RateType RateType { get; set; }

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
        /// Atributo para la propiedad DeductibleSubject
        /// </summary> 
        [DataMember]
        public DeductibleSubjectServiceQueryModel DeductibleSubject { get; set; }

        /// <summary>
        /// Atributo para la propiedad DeductibleUnit
        /// </summary> 
        [DataMember]
        public DeductibleUnitServiceQueryModel DeductibleUnit { get; set; }

        /// <summary>
        /// Atributo para la propiedad DeductValue
        /// </summary> 
        [DataMember]
        public decimal DeductValue { get; set; }

        /// <summary>
        /// Atributo para la propiedad MaxDeductibleSubject
        /// </summary> 
        [DataMember]
        public DeductibleSubjectServiceQueryModel MaxDeductibleSubject { get; set; }

        /// <summary>
        /// Atributo para la propiedad MaxDeductibleUnit
        /// </summary> 
        [DataMember]
        public DeductibleUnitServiceQueryModel MaxDeductibleUnit { get; set; }

        /// <summary>
        /// Atributo para la propiedad MaxDeductValue
        /// </summary> 
        [DataMember]
        public decimal? MaxDeductValue { get; set; }

        /// <summary>
        /// Atributo para la propiedad MinDeductibleSubject
        /// </summary> 
        [DataMember]
        public DeductibleSubjectServiceQueryModel MinDeductibleSubject { get; set; }

        /// <summary>
        /// Atributo para la propiedad MinDeductibleUnit
        /// </summary> 
        [DataMember]
        public DeductibleUnitServiceQueryModel MinDeductibleUnit { get; set; }

        /// <summary>
        /// Atributo para la propiedad MinDeductValue
        /// </summary> 
        [DataMember]
        public decimal? MinDeductValue { get; set; }

        /// <summary>
        /// Atributo para la propiedad Currency
        /// </summary> 
        [DataMember]
        public CurrencyServiceQueryModel Currency { get; set; }

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

        [DataMember]
        public LineBusinessServiceQueryModel LineBusiness { get; set; }
    }
}
