// -----------------------------------------------------------------------
// <copyright file="ParametrizationQuota.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;    

    /// <summary>
    /// Distribucion de cuota de plan de pago
    /// </summary>
    [DataContract]
    public class BaseParametrizationQuota: Extension
    {
        /// <summary>
        /// Obtiene o establece el Identificador de la cuota de plan de pago
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Obtiene o establece el numero que corresponde a la cuota
        /// </summary>
        [DataMember]
        public int Number { get; set; }
        
        /// <summary>
        ///  Obtiene o establece el Porcentaje
        /// </summary>        
        [DataMember]
        public decimal Percentage { get; set; }
        
        /// <summary>
        /// Obtiene o establece el tiempo entre cuotas
        /// </summary>
        [DataMember]
        public int? GapQuantity { get; set; }
    }
}
