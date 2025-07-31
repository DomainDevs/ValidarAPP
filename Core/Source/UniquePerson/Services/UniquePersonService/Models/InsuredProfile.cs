// -----------------------------------------------------------------------
// <copyright file="InsuredProfile.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de las propiedades del Perfil de Asegurado.
    /// </summary>    
    [DataContract]
    public class InsuredProfile
    {
        /// <summary>
        /// Obtiene o establece el Id del perfil de asegurado
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción larga la  del perfil de asegurado
        /// </summary>
        [DataMember]
        public string LongDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción corta del perfil de asegurado.
        /// </summary>
        [DataMember]
        public string ShortDescription { get; set; }     
    }
}