// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.Param
{
    using Sistran.Company.Application.ModelServices.Enums;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
 
    /// <summary>
    /// Modelo general de Error para cada item
    /// </summary>
    [DataContract]
    public class ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el tipo de error del modelo de servicio.
        /// </summary>
        [DataMember]
        public ErrorTypeService ErrorTypeService { get; set; }

        /// <summary>
        /// Obtiene o establece el listado de errores a retornar en los modelos de servicio.
        /// </summary>
        [DataMember]
        public List<string> ErrorDescription { get; set; }
    }
}
