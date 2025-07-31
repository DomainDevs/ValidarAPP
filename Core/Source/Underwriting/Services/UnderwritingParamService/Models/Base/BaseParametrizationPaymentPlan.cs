// -----------------------------------------------------------------------
// <copyright file="ParametrizationPaymentPlan.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Collections.Generic;
    using System.Runtime.Serialization;    

    /// <summary>
    /// Plan de pago (Modelo del negocio)
    /// </summary>
    [DataContract]
    public class BaseParametrizationPaymentPlan: Extension
    {
        /// <summary>
        /// Obtiene o establece el ID de plan de pago
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Obtiene o establece la descripcion del plan de pago
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// Obtiene o establece la descripcion abreviada
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de Cuotas
        /// </summary>
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de días hasta el vencimiento de la primera cuota
        /// </summary>
        [DataMember]
        public int FirstPayQuantity { get; set; }
        
        /// <summary>
        /// Obtiene o establece la mínima cantidad de días entre el vencimiento de la última cuota y la fecha de fin de vigencia
        /// </summary>
        [DataMember]
        public int? LastPayQuantity { get; set; }
        
        /// <summary>
        /// Obtiene o establece un valor que indica si las cuotas son apartir de la fecha de emision
        /// </summary>
        [DataMember]
        public bool IsIssueDate { get; set; }
        
        /// <summary>
        /// Obtiene o establece un valor que indica si las cuotas son apartir de la mayor de ambas
        /// </summary>
        [DataMember]
        public bool IsGreaterDate { get; set; }
        
        /// <summary>
        /// Obtiene o establece la unidad de tiempo
        /// </summary>
        [DataMember]
        public int? GapUnit { get; set; }
        
        /// <summary>
        /// Obtiene o establece el tiempo entre cuotas
        /// </summary>
        [DataMember]
        public int? GapQuantity { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es plan de pago con financiacion.
        /// </summary>
        [DataMember]
        public bool Financing { get; set; }


    }
}