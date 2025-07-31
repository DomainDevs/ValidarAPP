// -----------------------------------------------------------------------
// <copyright file="InsuredObjectServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System.Runtime.Serialization;
    /// <summary>
    /// Modelo objeto del seguro
    /// </summary>
    [DataContract]
    public class InsuredObjectServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
