// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies
{
    using Sistran.Company.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;
    /// <summary>
    /// Modelo general de motivos de rechazo
    /// </summary>
    [DataContract]
    public class RejectionCauseServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el id de motivo de rechazo
        /// </summary>
        [DataMember]
        public int id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de motivo de rechazo 
        /// </summary>
        [DataMember]
        public string description { get; set; }


        /// <summary>
        /// Obtiene o establece la descripcion corta de motivo de rechazo 
        /// </summary>
        [DataMember]
        public  GenericModelServicesQueryModel GroupPolicies { get; set; }
     }
}
