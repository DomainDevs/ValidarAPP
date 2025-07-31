// -----------------------------------------------------------------------
// <copyright file="InsuredObjectServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// MOD-S Objeto del seguro
    /// </summary>
    [DataContract]
    public class InsuredObjectServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el id del objeto del seguro 
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la descipcion del objeto del seguro
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
