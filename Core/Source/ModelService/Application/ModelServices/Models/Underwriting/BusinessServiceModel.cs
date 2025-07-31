// -----------------------------------------------------------------------
// <copyright file="BusinessServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de negocio.
    /// </summary>
    [DataContract]
    public class BusinessServiceModel : Param.ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id del negocio.
        /// </summary>
        [DataMember]
        public int BusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del negocio.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el negocio está habilitado.
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Obtiene o establece el ramo comercial asociado.
        /// </summary>
        [DataMember]
        public PrefixServiceQueryModel PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece la configuración asociada al negocio.
        /// </summary>
        [DataMember]
        public List<BusinessConfigurationServiceModel> BusinessConfiguration { get; set; }
    }
}
