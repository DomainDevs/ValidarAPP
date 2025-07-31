// -----------------------------------------------------------------------
// <copyright file="CurrenciesServiceQueryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Moreno</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.UnderwritingParam
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública CurrenciesServiceModel
    /// </summary>
    [DataContract]
    public class CurrenciesServiceQueryDTO : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo Deductiblesubject
        /// </summary>
        [DataMember]
        public List<CurrencyServiceQueryDTO> CurrencyServiceModel { get; set; }
    }
}
