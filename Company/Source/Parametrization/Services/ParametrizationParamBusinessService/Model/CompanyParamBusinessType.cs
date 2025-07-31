// -----------------------------------------------------------------------
// <copyright file="CompanyParamBusinessType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>@ETriana</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessService.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Runtime.Serialization;

    /// <summary>
    /// CompanyParamBusinessType. Modelo Company.
    /// </summary>
    [DataContract]
    public class CompanyParamBusinessType : CompanyGeneric
    {
        /// <summary>
        /// Gets or sets. llave primaria del negocio.
        /// </summary>
        [DataMember]
        public int BUSINESS_TYPE_CD { get; set; }


        /// <summary>
        /// Gets or sets.  Obtiene o establece la descripción del negocio.
        /// </summary>
        [DataMember]
        public string SMALL_DESCRIPTION { get; set; }
    }
}
