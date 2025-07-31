// -----------------------------------------------------------------------
// <copyright file="DetailServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Enums;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública tipo detalle
    /// </summary>
    [DataContract]
    public class DetailServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de detalle
        /// </summary>
        [DataMember]
        public DetailTypeServiceQueryModel DetailType { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si está habilitado
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de tasa
        /// </summary>
        [DataMember]
        public RateType? RateType { get; set; }

        /// <summary>
        /// Obtiene o establece la tasa
        /// </summary>
        [DataMember]
        public decimal? Rate { get; set; }

        /// <summary>
        /// Obtiene o establece el sublimite
        /// </summary>
        [DataMember]
        public decimal? SublimitAmt { get; set; }
    }
}
