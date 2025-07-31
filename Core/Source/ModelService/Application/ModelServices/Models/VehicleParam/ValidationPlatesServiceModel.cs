// -----------------------------------------------------------------------
// <copyright file="ValidationPlatesModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Michael Tapiero</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    /// <summary>
    /// Modelo de servicio para Validation Plate
    /// </summary>
    [DataContract]
    public class ValidationPlatesServiceModel : Param.ErrorServiceModel
    {
        [DataMember]
        public List<ValidationPlateServiceModel> ValidationPlateModel { get; set; }
    }
}
