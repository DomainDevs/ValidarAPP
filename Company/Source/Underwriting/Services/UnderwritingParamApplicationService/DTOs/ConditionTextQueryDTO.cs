// -----------------------------------------------------------------------
// <copyright file="ConditionTextQueryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camilo jimenéz</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Company.Application.Utilities.DTO;
    /// <summary>
    /// ConditionTextQueryDTO. Modelo DTO ConditionTextQueryDTO.
    /// </summary>
    [DataContract]
    public class ConditionTextQueryDTO
    {
        /// <summary>
        /// Get or sets ConditionText.
        /// </summary>
        [DataMember]
        public List<ConditionTextDTO> ConditionText { get; set; }

        /// <summary>
        /// Get or sets ErrorDto.
        /// </summary>
        [DataMember]
        public ErrorDTO ErrorDto { get; set; }
    }
}
