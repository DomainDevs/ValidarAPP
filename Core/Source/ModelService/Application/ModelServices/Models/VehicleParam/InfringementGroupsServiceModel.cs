// -----------------------------------------------------------------------
// <copyright file="InfringementGroupsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class InfringementGroupsServiceModel : ErrorServiceModel
    {
        [DataMember]
        public List<InfringementGroupServiceModel> InfringementGroupServiceModel { get; set; }
    }
}