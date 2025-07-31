using Sistran.Core.Application.RulesScriptsServices.Models;
using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models
{
    public class RuleCompositeViewModel
    {
        public List<Concept> Actions { get; set; }
        
        public string Change { get; set; }

        public List<Concept> Conditions { get; set; }
        
        public List<Parameter> Parameters { get; set; }
        
        public int RuleId { get; set; }
        
        public string RuleName { get; set; }
    }
}


