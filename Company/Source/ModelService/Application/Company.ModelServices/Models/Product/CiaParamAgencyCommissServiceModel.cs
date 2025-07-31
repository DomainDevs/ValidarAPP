// -----------------------------------------------------------------------
// <copyright file="CiaAgencyCommissServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.Product
{
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class CiaParamAgencyCommissServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int AgencyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal CommissPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? AdditionalCommissionPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? SchCommissPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? StDisCommissPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? AdditDisCommissPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string AgencyName { get; set; }
    }
}
