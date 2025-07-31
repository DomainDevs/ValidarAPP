using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class CoHierarchyAssociationModelsView
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool ExclusionayInd { get; set; }
        public bool EnabledInd { get; set; }
        public decimal? LimitInsuredAmt { get; set; }
        public ModuleModelsView Module { get; set; }
        public SubModuleModelsView SubModule { get; set; }
    }
}