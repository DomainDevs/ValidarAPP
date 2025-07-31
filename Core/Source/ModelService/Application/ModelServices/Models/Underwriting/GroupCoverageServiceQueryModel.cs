// -----------------------------------------------------------------------
// <copyright file="GroupCoverageQueryServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo del grupo de coberturas.
    /// </summary>
    [DataContract]
    public class GroupCoverageServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id del grupo de coberturas.
        /// </summary>
        [DataMember]
        public int GroupCoverageId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del grupo de coberturas.
        /// </summary>
        [DataMember]
        public string GroupCoverageSmallDescription { get; set; }
    }
}
