// -----------------------------------------------------------------------
// <copyright file="VersionServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gomez Hernandez</author>
// -----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    /// <summary>
    /// Tipo de combustible del vehiculo
    /// </summary>
    [DataContract]
    public class VehicleFuelServiceQueryModel
    {
        /// <summary>
        /// se obtiene identificador del tipo de combustible
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// se obtiene descripcion del tipo de combustible
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}