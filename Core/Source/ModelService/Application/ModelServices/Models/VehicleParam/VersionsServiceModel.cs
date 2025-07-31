// -----------------------------------------------------------------------
// <copyright file="VersionsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camilo Ramirez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class VersionsServiceModel : ErrorServiceModel
    {
        [DataMember]
        public List<VersionServiceModel> ListVersionServiceModel { get; set; }
    }
}
