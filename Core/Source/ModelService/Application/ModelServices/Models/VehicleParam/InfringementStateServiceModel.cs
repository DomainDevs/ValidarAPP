// -----------------------------------------------------------------------
// <copyright file="InfringementStateServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using System.Runtime.Serialization;

    [DataContract]
    public class InfringementStateServiceModel : ParametricServiceModel
    {
        [DataMember]
        public int InfringementStateCode { get; set; }

        [DataMember]
        public string InfringementStateDescription { get; set; }
    }
}