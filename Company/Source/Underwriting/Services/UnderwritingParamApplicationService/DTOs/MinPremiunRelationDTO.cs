// -----------------------------------------------------------------------
// <copyright file="MiniumPremiunRelationDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Wilfrido Heredia Carrera</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    using Sistran.Company.Application.Utilities.DTO;
    using System.Runtime.Serialization;
    /// <summary>
    /// MinPremiunRelationDTO
    /// </summary>
    [DataContract]
    public class MinPremiunRelationDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public EndorsementTypeDTO EndorsementType { get; set; }
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }
        [DataMember]
        public ProductDTO Product { get; set; }
        [DataMember]
        public GroupCoverageDTO GroupCoverage { get; set; }
        [DataMember]
        public MinPremiunRangeDTO MinPremiunRange { get; set; }
        [DataMember]
        public decimal SubMinPremiun { get; set; }
        [DataMember]
        public decimal RiskMinPremiun { get; set; }
        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
