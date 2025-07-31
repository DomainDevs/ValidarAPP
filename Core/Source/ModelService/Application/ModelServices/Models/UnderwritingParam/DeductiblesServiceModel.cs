// -----------------------------------------------------------------------
// <copyright file="DeductiblesServiceModel.cs" company="SISTRAN">
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
    /// Clase pública deductiblesubject
    /// </summary>
    [DataContract]
    public class DeductiblesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo Deductiblesubject
        /// </summary>
        [DataMember]
        public List<DeductibleServiceModel> DeductibleServiceModel { get; set; }
    }
}
