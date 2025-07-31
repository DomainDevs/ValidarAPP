// -----------------------------------------------------------------------
// <copyright file="EndorsementTypeServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de ramo comercial.
    /// </summary>
    [DataContract]
    public class EndorsementTypeServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id del ramo comercial.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del ramo comrcial.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
