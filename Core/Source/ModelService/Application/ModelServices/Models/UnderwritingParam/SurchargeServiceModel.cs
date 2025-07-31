// -----------------------------------------------------------------------
// <copyright file="DiscountServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
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
    public class SurchargeServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// propiedad Id
        /// </summary> 
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// propiedad RateType
        /// </summary> 
        [DataMember]
        public RateType Type { get; set; }

        /// <summary>
        /// propiedad Rate
        /// </summary> 
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// propiedad Description
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// propiedad Description
        /// </summary> 
        [DataMember]
        public string TinyDescription { get; set; }

    }
}
