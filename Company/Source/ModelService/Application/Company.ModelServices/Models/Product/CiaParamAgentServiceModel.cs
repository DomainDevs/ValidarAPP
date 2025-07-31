// -----------------------------------------------------------------------
// <copyright file="CiaAgentServiceModel.cs" company="SISTRAN">
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
    public class CiaParamAgentServiceModel : ParametricServiceModel
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
        public int ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int LockerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CiaParamAgencyCommissServiceModel> AgencyComiss { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[DataMember]
        //public List<CiaParamIncentiveServiceModel> Incentives { get; set; }
    }
}
