// -----------------------------------------------------------------------
// <copyright file="VersionVehicleFasecoldasModelService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camilo Ramirez</author>
// -----------------------------------------------------------------------


namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo para lista de modelo VersionVehicleFasecoldaModelService
    /// </summary>
    [DataContract]
    public class VersionVehicleFasecoldasServiceModel : ErrorServiceModel
    {
        [DataMember]
       public  List<VersionVehicleFasecoldaServiceModel> ListVersionVehicleFasecoldaModelService { get; set; }

    }

}
