// -----------------------------------------------------------------------
// <copyright file="ClauseServiceRelationModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    [DataContract]
    public class ClauseServiceRelationModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el id de clausula
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece si la clausula es obligatoria
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
