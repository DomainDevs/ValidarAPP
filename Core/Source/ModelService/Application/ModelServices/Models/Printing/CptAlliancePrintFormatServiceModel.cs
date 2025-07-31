// -----------------------------------------------------------------------
// <copyright file="CptAlliancePrintFormatServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Printing
{
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;


    /// <summary>
    /// Modelo de servicio de los Formatos de impresión de aliados.
    /// </summary>
    [DataContract]
    public class CptAlliancePrintFormatServiceModel
    {
        /// <summary>
        /// Obtiene o establece el identificador del formato de impresión.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el código del ramo.
        /// </summary>
        [DataMember]
        public PrefixServiceQueryModel PrefixServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece el código tipo de endoso.
        /// </summary>
        [DataMember]
        public EndorsementTypeServiceQueryModel EndorsementTypeServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece el Nombre formato de impresión.
        /// </summary>
        [DataMember]
        public string Format { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si se encuentra habilitado el formato de impresión.
        /// </summary>
        [DataMember]
        public bool Enable { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del registro.
        /// </summary>
        [DataMember]
        public ParametricServiceModel ParametricServiceModel { get; set; }
    }
}
