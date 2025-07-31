using System;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using System.Collections;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Summary description for DecisionTableDTO.
    /// </summary>
    [Serializable]
    public class DecisionTableDTO
    {
        private RuleBase _table;
        private IList _conditions;
        private IList _actions;
        private IList _rules;

        public DecisionTableDTO()
        {
        }

        public DecisionTableDTO(RuleBase table)
        {
            _table = table;
        }

        public DecisionTableDTO(RuleBase table, IList conditions, IList actions, IList rules)
        {
            _table = table;
            _conditions = conditions;
            _actions = actions;
            _rules = rules;
        }

        public RuleBase Table
        {
            get
            {
                return _table;
            }
        }

        public IList Conditions
        {
            get
            {
                return _conditions;
            }
        }

        public IList Actions
        {
            get
            {
                return _actions;
            }
        }

        public IList Rules
        {
            get
            {
                return _rules;
            }
        }
    }
}