// -----------------------------------------------------------------------
// <copyright file="BusinessConfigurationServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de acuerdo de negocio.
    /// </summary>
    [DataContract]
    public class BusinessConfigurationServiceModel : Param.ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id del acuerdo de negocio.
        /// </summary>
        [DataMember]
        public int BusinessConfigurationId { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del negocio.
        /// </summary>
        [DataMember]
        public int BusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece la solicitud agrupadora.
        /// </summary>
        [DataMember]
        public RequestEndorsementServiceQueryModel Request { get; set; }

        /// <summary>
        /// Obtiene o establece el producto.
        /// </summary>
        [DataMember]
        public ProductServiceQueryModel Product { get; set; }

        /// <summary>
        /// Obtiene o establece el grupo de coberturas.
        /// </summary>
        [DataMember]
        public GroupCoverageServiceQueryModel GroupCoverage { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de asistencia.
        /// </summary>
        [DataMember]
        public AssistanceTypeServiceQueryModel Assistance { get; set; }

        /// <summary>
        /// Obtiene o establece el id producto respuesta.
        /// </summary>
        [DataMember]
        public string ProductIdResponse { get; set; }
    }
}
