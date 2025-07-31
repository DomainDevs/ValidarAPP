using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    [KnownType("AsociationLineModel")]
    public class AsociationLineModel
    {
        public int LineId { get; set; }
        public int AssociationLineId { get; set; }
        public int AssociationColumnId { get; set; }
        public int AssociationTypeId { get; set; }
        public int LineBusiness { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ByLineBusinessSubLineBusinessModel ByLineBusinessSubLineBusiness { get; set; }
        public ByOperationTypePrefixModel ByOperationTypePrefix { get; set; }
        public ByInsuredModel ByInsured { get; set; }
        public ByPolicyModel ByPolicy { get; set; }
        public ByFacultativeIssueModel ByFacultativeIssue { get; set; }
        public ByInsuredPrefixModel ByInsuredPrefix { get; set; }
        public ByLineBusinessSubLineBusinessRiskModel ByLineBusinessSubLineBusinessRisk { get; set; }
        public ByPolicyLineBusinessSubLineBusinessModel ByPolicyLineBusinessSubLineBusiness { get; set; }
        public ByLineBusinessSubLineBusinessCoverageModel ByLineBusinessSubLineBusinessCoverage { get; set; }
        public ByPrefixRiskModel ByPrefixRisk { get; set; }
        public ByPrefixProductModel ByPrefixProduct { get; set; }

    }

    //POR RAMO TECNICO y //POR RAMO / SUBRAMO TECNICO
    [KnownType("ByLineBusinessSubLineBusinessModel")]
    public class ByLineBusinessSubLineBusinessModel
    {
        public int LineBusinessId { get; set; }//cuando sea 0 será por lista de Ramos técnicos
                                               //cuando sea > 0 será por Ramos técnico y lista de subramos técnicos  
        public List<LineBusinessSubLineBusinessModel> LineBusinessSubLineBusiness { get; set; }

    }

    [KnownType("LineBusinessSubLineBusinessModel")]
    public class LineBusinessSubLineBusinessModel
    {
        public int Id { get; set; }
    }



    //POR RAMO/TIPO DE OPERACION
    [KnownType("ByOperationTypePrefixModel")]
    public class ByOperationTypePrefixModel
    {
        public int BusinessTypeId { get; set; }
        public List<PrefixModel> Prefixes { get; set; }
    }

    //POR RAMO
    [KnownType("PrefixModel")]
    public class PrefixModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

    }

    //POR ASEGURADO
    [KnownType("ByInsuredModel")]
    public class ByInsuredModel
    {
        public int IndividualId { get; set; }

    }

    //POR POLIZA
    [KnownType("ByPolicyModel")]
    public class ByPolicyModel
    {
        public int PolicyId { get; set; }

    }

    //POR FACULTATIVO
    [KnownType("ByFacultativeIssueModel")]
    public class ByFacultativeIssueModel
    {
        public List<PrefixModel> Prefixes { get; set; }
    }

    //POR FACULTATIVO
    [KnownType("ByInsuredPrefixModel")]
    public class ByInsuredPrefixModel
    {
        public int IndividualId { get; set; }
        public List<PrefixModel> Prefixes { get; set; }
    }

    //POR RAMO TÉCNICO / SUBRAMO TÉCNICO / RIESGO
    [KnownType("ByLineBusinessSubLineBusinessRiskModel")]
    public class ByLineBusinessSubLineBusinessRiskModel
    {
        public int LineBusinessId { get; set; }
        public int SubLineBusinessId { get; set; }
        public List<InsuredObjectModel> InsuredObject { get; set; }

    }

    //11. POR RAMO TÉCNICO / SUBRAMO TÉCNICO / COBERTURA
    [KnownType("ByLineBusinessSubLineBusinessCoverageModel")]
    public class ByLineBusinessSubLineBusinessCoverageModel
    {
        public int LineBusinessId { get; set; }
        public int SubLineBusinessId { get; set; }
        public List<CoverageModel> Coverage { get; set; }

    }

    [KnownType("CoverageModel")]
    public class CoverageModel
    {
        public int Id { get; set; }
    }
          
    //POR RAMO TÉCNICO / SUBRAMO TÉCNICO / POLIZA
    [KnownType("ByPolicyLineBusinessSubLineBusinessModel")]
    public class ByPolicyLineBusinessSubLineBusinessModel
    {
        public int LineBusinessId { get; set; }
        public int SubLineBusinessId { get; set; }
        public int PolicyId { get; set; }
    }

    //POR RAMO / RIESGO
    [KnownType("ByPrefixRiskModel")]
    public class ByPrefixRiskModel
    {
        public int PrefixId { get; set; }
        public List<InsuredObjectModel> InsuredObject { get; set; }

    }

    [KnownType("InsuredObjectModel")]
    public class InsuredObjectModel
    {
        public int Id { get; set; }
    }



    //POR RAMO COMERCIAL / PRODUCTO
    [KnownType("ByPrefixProductModel")]
    public class ByPrefixProductModel
    {
        public int PrefixId { get; set; }
        public int ProductId { get; set; }

    }
}
