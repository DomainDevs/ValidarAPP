// -----------------------------------------------------------------------
// <copyright file="CiaAssistanceTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.Product
{
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class CiaParamAssistanceTypeServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el id de la asistencia
        /// </summary>
        [DataMember]
        public int AssistanceId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del tipo de asistencia
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
