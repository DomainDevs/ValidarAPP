// -----------------------------------------------------------------------
// <copyright file="CptAlliancePrintFormatsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Printing
{    
    using System.Collections.Generic;
    using System.Runtime.Serialization;    

    /// <summary>
    /// Modelo de servicio de consulta de los Formatos de impresión de aliados.
    /// </summary>
    [DataContract]
    public class CptAlliancePrintFormatsServiceModel : Param.ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece la lista de los Formatos de impresión de aliados.
        /// </summary>
        [DataMember]
        public List<CptAlliancePrintFormatServiceModel> CptAlliancePrintFormatServiceModel { get; set; }
    }
}
