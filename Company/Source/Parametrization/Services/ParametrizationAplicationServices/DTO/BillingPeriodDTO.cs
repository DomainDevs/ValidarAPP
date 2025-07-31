// -----------------------------------------------------------------------
// <copyright file="BillingPeriodDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>ETriana</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationAplicationServices.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// BillingPeriodDTO. Modelo DTO.
    /// </summary>
    [DataContract]
    public class BillingPeriodDTO
    {
        /// <summary>
        /// Gets or sets. llave primaria del negocio.
        /// </summary>
        [DataMember]
        public int BILLING_PERIOD_CD { get; set; }

        /// <summary>
        /// Gets or sets. Descripción del negocio.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
