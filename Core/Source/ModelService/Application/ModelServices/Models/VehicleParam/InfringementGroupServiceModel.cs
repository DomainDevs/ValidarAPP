// -----------------------------------------------------------------------
// <copyright file="InfringementGroupServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para infraccciones
    /// </summary>
    [DataContract]
    public class InfringementGroupServiceModel : ParametricServiceModel
    {
        [DataMember]
        public int InfringementGroupCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int InfrigementOneYear { get; set; }

        [DataMember]
        public int InfrigementThreeYear { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}