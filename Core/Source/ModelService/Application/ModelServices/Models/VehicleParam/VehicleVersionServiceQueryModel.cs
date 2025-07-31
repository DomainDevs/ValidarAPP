// -----------------------------------------------------------------------
// <copyright file="VehicleVersionServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Tipo de version 
    /// </summary>
    [DataContract]
    public class VehicleVersionServiceQueryModel
    {
        /// <summary>
        /// se obtiene identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// se obtiene descripcion 
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
