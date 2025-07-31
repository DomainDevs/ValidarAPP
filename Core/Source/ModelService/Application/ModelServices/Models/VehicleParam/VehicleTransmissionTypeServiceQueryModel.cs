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
    /// Tipo de transmision
    /// </summary>
    [DataContract]
    public class VehicleTransmissionTypeServiceQueryModel
    {
        /// <summary>
        /// Se obtiene identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Se obtiene descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}