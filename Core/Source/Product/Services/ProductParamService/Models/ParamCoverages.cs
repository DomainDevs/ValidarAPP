// -----------------------------------------------------------------------
// <copyright file="ParamProduct.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
using System.Collections.Generic;

namespace Sistran.Core.Application.ProductParamService.Models
{
    /// <summary>
    /// Modelo de
    /// </summary>
    public class ParamCoverages
    {
        /// <summary>
        /// 
        /// </summary>
        public ParamGroupCoverage Coverage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ParamGroupCoverage> CoverageAllied { get; set; }
    }
}
