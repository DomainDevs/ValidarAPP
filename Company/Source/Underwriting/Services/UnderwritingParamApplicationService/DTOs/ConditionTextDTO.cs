// -----------------------------------------------------------------------
// <copyright file="ConditionTextDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>camilo Jimenéz</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    using System;
    using System.Runtime.Serialization;
    using Sistran.Company.Application.Utilities.DTO;

    /// <summary>
    /// ConditionTextDTO. Modelo DTO ConditionText.
    /// </summary>
    [DataContract]
    public class ConditionTextDTO
    {
        /// <summary>
        /// Get or sets Id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Get or sets Title.
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// Get or sets Body.
        /// </summary>
        [DataMember]
        public String Body { get; set; }

        /// <summary>
        /// Get or sets Objeto ConditionTextLevel.
        /// </summary>
        [DataMember]
        public ConditionTextLevelDTO ConditionTextLevel { get; set; }

        /// <summary>
        /// Get or sets Objeto ConditionTextLevel.
        /// </summary>
        [DataMember]
        public ConditionTextLevelTypeDTO ConditionTextLevelType { get; set; }

        /// <summary>
        /// Get or sets Objeto Error DTO.
        /// </summary>
        [DataMember]
        public ErrorDTO ErrorDto { get; set; }
    }
}
