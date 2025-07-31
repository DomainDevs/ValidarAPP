// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Param
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Enums;

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
