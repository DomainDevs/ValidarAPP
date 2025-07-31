// -----------------------------------------------------------------------
// <copyright file="DiscountsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública deductiblesubject
    /// </summary>
    [DataContract]
    public class SurchargesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo Deductiblesubject
        /// </summary>
        [DataMember]
        public List<SurchargeServiceModel> SurchargeServiceModel { get; set; }
    }
}
