// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.Param
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo generico para retornar el resultado de las operaciones de los ABM
    /// </summary>
    [DataContract]
    public class ParametrizationResult
    {
        /// <summary>
        /// Obtiene o establece el total de item agregados
        /// </summary>
        [DataMember]
        public int TotalAdded { get; set; }

        /// <summary>
        /// Obtiene o establece el Total de item modificados
        /// </summary>
        [DataMember]
        public int TotalModified { get; set; }

        /// <summary>
        /// Obtiene o establece el Totat de item eliminados
        /// </summary>
        [DataMember]
        public int TotalDeleted { get; set; }

        /// <summary>
        /// Obtiene o establece el mensaje de error o adicional
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
