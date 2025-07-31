using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class IndividualRelationAppModelsView
    {
        public int IndividualRelationAppId { get; set; }

        public int ParentIndividualId { get; set; }

        public AgentModelsView ChildIndividual { get; set; }

        public int RelationTypeCd { get; set; }

        public AgencyModelsView Agency { get; set; }
    }
}