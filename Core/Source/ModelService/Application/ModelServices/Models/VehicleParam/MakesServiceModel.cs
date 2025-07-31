// -----------------------------------------------------------------------
// <copyright file="MakesServiceModel.cs" company="SISTRAN">
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
    public class MakesServiceModel : ErrorServiceModel
    {
        [DataMember]
        public List<MakeServiceModel> ListMakesServiceModel { get; set; }
    }
}
