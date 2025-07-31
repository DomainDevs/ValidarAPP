// -----------------------------------------------------------------------
// <copyright file="ParamPaymentMethod.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Metodo de pago
    /// </summary>
    [DataContract]
    public class BaseParamPaymentMethod: Extension
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece dato para validar si se puede crear con ese plan de pago
        /// </summary>
        [DataMember]
        public bool IsInCome { get; set; }


    }
}
