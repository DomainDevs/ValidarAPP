// -----------------------------------------------------------------------
// <copyright file="FasecoldasServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camilo Ramirez</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.VehicleParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo para lista de modelo FasecoldasServiceModel
    /// </summary>
    [DataContract]
    public class FasecoldasServiceModel : ErrorServiceModel
    {
        [DataMember]
        public List<FasecoldaServiceModel> ListFasecoldaModelService { get; set; }
    }
}
