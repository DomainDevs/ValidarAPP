// -----------------------------------------------------------------------
// <copyright file="ConditionLevelServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------   ------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo para los niveles 
    /// </summary>
    [DataContract]
    public class ConditionLevelServiceModel
    {
        /// <summary>
        /// Obtiene o establece Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
