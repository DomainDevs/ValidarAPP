// -----------------------------------------------------------------------
// <copyright file="AssistanceTypeQueryServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de tipo de assitencia.
    /// </summary>
    [DataContract]
    public class AssistanceTypeServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id del tipo de assitencia.
        /// </summary>
        [DataMember]
        public int AssistanceCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del tipo de assitencia.
        /// </summary>
        [DataMember]
        public string AssistanceDescription { get; set; }
    }
}
