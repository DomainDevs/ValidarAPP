// -----------------------------------------------------------------------
// <copyright file="PoliciesServicesModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.Reports
{
    using System.Collections.Generic;
    using Sistran.Company.Application.ModelServices.Models.Param;
    public class PoliciesServicesModel : ErrorServiceModel
    {
        public List<PoliciesServiceModel> policiesServiceModel { get; set; }
    }
}
