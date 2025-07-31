
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    using Sistran.Core.Application.ModelServices.Enums;
    using System.Collections.Generic;

    public class IncentivesForAgentsSaveModelsView
    {
        public int AgentAgencyId { get; set; }
        public List<IncentivesAgent> Incentives { get; set; }
        public int IndividualId { get; set; }
        public int ProductId { get; set; }
        public StatusTypeService Status { get; set; }   
    }

    public class IncentivesAgent
    {
        public int Id { get; set; }
        public decimal Incentive_Amt { get; set; }
    }
}