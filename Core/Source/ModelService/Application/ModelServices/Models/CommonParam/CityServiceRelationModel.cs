// -----------------------------------------------------------------------
// <copyright file="CityServiceRelationModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de Ciudad
    /// </summary>
    [DataContract]
    public class CityServiceRelationModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la Ciudad
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de la Ciudad
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el estado al que pertenece la ciudad
        /// </summary>
        [DataMember]
        public StateServiceQueryModel State { get; set; }
    }
}