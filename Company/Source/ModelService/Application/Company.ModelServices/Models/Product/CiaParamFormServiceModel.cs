// -----------------------------------------------------------------------
// <copyright file="CiaFormServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.Product
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class CiaParamFormServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string StrCurrentFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string FormNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int FormId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int CoverGroupId { get; set; }

        [DataMember]
        public int ProductId { get; set; }
    }
}
