// -----------------------------------------------------------------------
// <copyright file="ParamClause.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.UnderwritingServices.Models;

    /// <summary>
    /// Clausulas (modelo)
    /// </summary>
    public class BaseParamClause: Extension
    {
        /// <summary>
        /// Obtiene o establece Fecha Inicio
        /// </summary>
        [DataMember]
        public DateTime InputStartDate { get; set; }
        
        /// <summary>
        /// Obtiene o establece Fecha Vencimiento
        /// </summary>
        [DataMember]
        public DateTime? DueDate { get; set; }

       
    }
}
