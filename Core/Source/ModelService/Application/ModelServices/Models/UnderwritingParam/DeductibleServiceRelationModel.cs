// -----------------------------------------------------------------------
// <copyright file="ClauseServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    [DataContract]
    public class DeductibleServiceRelationModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el id del deducible
        /// </summary>
        [DataMember]
        public int Id { get; set; }

      
        /// <summary>
        /// Obtiene o establece si el deducible es obligatorio
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
