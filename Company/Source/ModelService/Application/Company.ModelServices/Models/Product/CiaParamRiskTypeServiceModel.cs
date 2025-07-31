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
    public class CiaParamRiskTypeServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int MaxRiskQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? RuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? PreRuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? ScriptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? CoveredRiskType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? SubCoveredRiskType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CiaParamCoverageServiceModel> GroupCoverages { get; set; }
    }
}
