// -----------------------------------------------------------------------
// <copyright file="DeductibleUnitsServiceModel.cs" company="SISTRAN">
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
    /// Clase pública deductibleUnit
    /// </summary>
    [DataContract]
    public class DeductibleUnitsServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo Deductibleunit
        /// </summary>
        [DataMember]
        public List<DeductibleUnitServiceQueryModel> DeductibleUnitServiceModels { get; set; }
    }
}
