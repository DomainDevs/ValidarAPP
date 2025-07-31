// -----------------------------------------------------------------------
// <copyright file="PrefixQueryServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de ramo comercial.
    /// </summary>
    [DataContract]
    public class PrefixServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id del ramo comercial.
        /// </summary>
        [DataMember]
        public int PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del ramo comrcial.
        /// </summary>
        [DataMember]
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del ramo comercial.
        /// </summary>
        [DataMember]
        public string PrefixSmallDescription { get; set; }
    }
}
