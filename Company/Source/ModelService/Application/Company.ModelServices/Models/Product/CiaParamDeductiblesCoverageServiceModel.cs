// -----------------------------------------------------------------------
// <copyright file="CiaCommercialClassServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.Product
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class CiaParamDeductiblesCoverageServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int GroupCoverId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int CoverageId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int BeneficiaryTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsSelected { get; set; }
    }
}
