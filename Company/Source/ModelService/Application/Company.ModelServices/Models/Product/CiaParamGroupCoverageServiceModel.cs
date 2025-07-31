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
    public class CiaParamGroupCoverageServiceModel : ParametricServiceModel
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
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsSelected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsSublimit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? RuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? PosRuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal SublimitPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? MainCoverageId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? MainCoveragePercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? ScriptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SubLineBusinessDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string LineBusinessDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int LineBusinessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int InsuredObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string InsuredObjectIdDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsPremiumMin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool NoCalculate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamDeductiblesCoverageServiceModel> DeductiblesCoverage { get; set; }
    }
}
