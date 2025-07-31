// -----------------------------------------------------------------------
// <copyright file="CurrenciesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública CurrenciesServiceModel
    /// </summary>
    [DataContract]
    public class CurrenciesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo Deductiblesubject
        /// </summary>
        [DataMember]
        public List<CurrencyServiceQueryModel> CurrencyServiceModel { get; set; }
    }
}
