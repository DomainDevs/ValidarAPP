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
    public class CiaParamCoverageServiceModel : ParametricServiceModel
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
        public int? RiskTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? PrefixId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CiaParamInsuredObjectServiceModel> InsuredObjects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CiaParamFormServiceModel> Form { get; set; }
    }
}
