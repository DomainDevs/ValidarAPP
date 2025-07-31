// -----------------------------------------------------------------------
// <copyright file="SalePointsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Param;

    /// <summary>
    /// Clase pública Sale Points Service Model
    /// </summary>
    [DataContract]
    public class SalePointsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo puntos de venta
        /// </summary>
        [DataMember]
        public List<SalePointServiceModel> SalePointServiceModel { get; set; }
    }
}