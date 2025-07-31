// -----------------------------------------------------------------------
// <copyright file="InfringementGroupTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using System.Runtime.Serialization;

    public class InfringementGroupTypeServiceModel : ParametricServiceModel
    {
        [DataMember]
        public int InfringementGroupCode { get; set; }

        [DataMember]
        public string InfringementGroupDescription { get; set; }
    }
}