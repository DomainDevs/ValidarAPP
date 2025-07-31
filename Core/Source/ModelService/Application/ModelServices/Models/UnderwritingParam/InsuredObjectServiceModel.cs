// -----------------------------------------------------------------------
// <copyright file="InsuredObjectServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para objeto del seguro
    /// </summary>
    [DataContract]
    public class InsuredObjectServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el id del objeto del seguro
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del objeto del seguro
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del objeto del seguro
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece si es declarativo 
        /// </summary>
        [DataMember]
        public bool IsDeclarative { get; set; }
    }
}
