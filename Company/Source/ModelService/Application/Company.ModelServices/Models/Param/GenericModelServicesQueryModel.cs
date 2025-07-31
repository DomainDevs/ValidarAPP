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
    public class GenericModelServicesQueryModel 
    {
        /// <summary>
        /// Obtiene o establece el id 
        /// </summary>
        [DataMember] 
        public int id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [DataMember]
        public string description { get; set; }

        /// <summary>
        /// Obtiene o establece el descripcion corta
        /// </summary>
        [DataMember]
        public string smallDescription { get; set; }
    }
}
