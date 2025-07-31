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
    public class CiaParamCoveragesServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public CiaParamGroupCoverageServiceModel Coverage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CiaParamGroupCoverageServiceModel> CoverageAllied { get; set; }
    }
}
