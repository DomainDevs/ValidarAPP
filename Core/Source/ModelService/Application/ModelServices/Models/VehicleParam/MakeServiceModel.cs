// -----------------------------------------------------------------------
// <copyright file="MakeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camilo Ramirez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para Marcas
    /// </summary>
    [DataContract]
    public class MakeServiceModel : ParametricServiceModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }


    }
}