// -----------------------------------------------------------------------
// <copyright file="ConditionTextDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>camilo Jimenéz</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Modelo DTO ConditionTextLevel
    /// </summary>
    [DataContract]
    public class ConditionTextLevelDTO
    {
        /// <summary>
        /// Get or sets Id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Get or sets Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
