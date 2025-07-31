// -----------------------------------------------------------------------
// <copyright file="CiaCommercialClassServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.Product
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class CiaParamCopyProductServiceModel
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del Producto
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Nombre Corto del Producto
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los grupos de coberturas
        /// </summary>
        [DataMember]
        public bool CopyGroupCoverages { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los planes de pago
        /// </summary>
        [DataMember]
        public bool CopyPaymentPlan { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los paquetes de reglas
        /// </summary>
        [DataMember]
        public bool CopyRuleSet { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia las formas de impresion
        /// </summary>
        [DataMember]
        public bool CopyPrintingFormes { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los intermediarios
        /// </summary>
        [DataMember]
        public bool CopyAgent { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los Limites RC
        /// </summary>
        [DataMember]
        public bool CopyLimitRC { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los Guiones
        /// </summary>
        [DataMember]
        public bool CopyScript { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia las actividades del riesgo
        /// </summary>
        [DataMember]
        public bool CopyActivityRisk { get; set; }
    }
}
