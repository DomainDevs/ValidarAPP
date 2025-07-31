using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ChangeAgentViewModel : EndorsementViewModel
    {
        public int Id { get; set; }

        public List<IssuanceAgency> Agencies { get; set; }

        public decimal Premium { get; set; }

        public string ChangeCoinsuranceFrom { get; set; }

        public int ProductId { get; set; }
    }
}