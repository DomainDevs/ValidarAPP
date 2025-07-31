// -----------------------------------------------------------------------
// <copyright file="ParamProduct.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using System.Collections.Generic;

    /// <summary>
    /// Modelo de 
    /// </summary>
    public class ParamRiskType : BaseParamRiskType
    {
        public List<ParamGroupCoverage> GroupCoverages { get; set; }
    }
}
