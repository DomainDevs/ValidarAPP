// -----------------------------------------------------------------------
// <copyright file="ParamClauseInsuredObject.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------

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
    /// Modelo para objeto del seguro
    /// </summary>
    [DataContract]
    public class BaseParamClauseInsuredObject: Extension
    { 
      /// <summary>
      /// Obtiene o establece Identificador
      /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Atributo para la propiedad Description
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece Atributo para la propiedad Small Description
        /// </summary> 
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
