// -----------------------------------------------------------------------
// <copyright file="InsurancesObjectsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Objetos del seguro 
    /// </summary>
    [DataContract]
    public class InsurancesObjectsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece los objetos del seguro (Modelo del servicio)
        /// </summary>
        [DataMember]
        public List<InsuredObjectServiceModel> InsuredObjectServiceModel { get; set; }
    }
}
