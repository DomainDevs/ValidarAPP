// -----------------------------------------------------------------------
// <copyright file="Coverages2GServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Coverages2GServiceModel : ErrorServiceModel
    {
        [DataMember]
        public List<Coverage2GServiceModel> CoverageServiceModels { get; set; }
    }
}
