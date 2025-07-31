using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using System;
using System.Collections;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities
{
    public class RuleEditorData
    {
        public bool IsNew;
        public int RuleSetId;
        public string Description;
        public DateTime CurrentFrom;
        public int Version;
        public int PackageId;
        public int LevelId;
        public IList Rules;
        public IList Conditions;
        public IList Actions;

        public int ConditionEditingItem;
        public int ActionEditingItem;
        public RuleDTO EditingRule;
    }
}
