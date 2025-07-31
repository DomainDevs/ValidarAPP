// -----------------------------------------------------------------------
// <copyright file="CompanyParamBillingPeriod.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>@ETriana</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessService.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Clase raiz, para que hereden todas las Company
    /// </summary>
    /// [DataContract]
    public class CompanyGeneric
    {
        /// <summary>
        /// Gets or sets. llave primaria del negocio.
        /// </summary>
        [DataMember]
        public int id { get; set; }
        /// <summary>
        /// Gets or sets.  Obtiene o establece la descripción del negocio.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
