// -----------------------------------------------------------------------
// <copyright file="VersionServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gomez Hernandez</author>
// -----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{    /// <summary>
     /// Modelo de vehiculo
     /// </summary>
    [DataContract]
    public class VehicleModelServiceQueryModel
    {    /// <summary>
         /// se obtiene identificador del modelo
         /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// se obtiene la descripcion del modelo
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}