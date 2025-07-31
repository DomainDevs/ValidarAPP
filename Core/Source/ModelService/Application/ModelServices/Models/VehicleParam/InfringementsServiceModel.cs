// -----------------------------------------------------------------------
// <copyright file="InfringementsServiceModel.cs" company="SISTRAN">
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
    public class InfringementsServiceModel : ErrorServiceModel
    {
        [DataMember]
        public List<InfringementServiceModel> InfringementServiceModel { get; set; }
    }
}