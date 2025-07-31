// -----------------------------------------------------------------------
// <copyright file="InfringementServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using System.Runtime.Serialization;

    [DataContract]
    public class InfringementServiceModel : ParametricServiceModel
    {
        [DataMember]
        public string InfringementCode { get; set; }

        [DataMember]
        public int? InfringementPreviousCode { get; set; }

        [DataMember]
        public string InfringementDescription { get; set; }

        [DataMember]
        public int? InfringementGroupCode { get; set; }

        [DataMember]
        public string InfringementGroupDescription { get; set; }
    }
}
