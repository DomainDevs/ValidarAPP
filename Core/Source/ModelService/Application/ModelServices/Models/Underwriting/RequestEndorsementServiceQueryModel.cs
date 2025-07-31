// -----------------------------------------------------------------------
// <copyright file="RequestEndorsementQueryServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de solicitud agrupadora con endoso.
    /// </summary>
    [DataContract]
    public class RequestEndorsementServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la solicitud agrupadora con endoso.
        /// </summary>
        [DataMember]
        public int RequestEndorsementId { get; set; }

        /// <summary>
        /// Obtiene o establece la solicitud agrupadora.
        /// </summary>
        [DataMember]
        public int RequestId { get; set; }

        /// <summary>
        /// Obtiene o establece el id producto de la solicitud agrupadora.
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo comercial de la solicitud agrupadora.
        /// </summary>
        [DataMember]
        public int PrefixCode { get; set; }
    }
}
