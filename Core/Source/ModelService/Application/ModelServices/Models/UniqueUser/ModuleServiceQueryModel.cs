// -----------------------------------------------------------------------
// <copyright file="ModuleServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UniqueUser
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ModuleServiceQueryModel" />
    /// </summary>
    [DataContract]
    public class ModuleServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }
    }
}
