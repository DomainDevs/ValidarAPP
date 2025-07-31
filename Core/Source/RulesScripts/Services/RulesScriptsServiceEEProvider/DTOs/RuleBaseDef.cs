using Sistran.Core.Application.RulesScriptsServices.Enums;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Summary description for RuleBaseDef.
    /// </summary>
    public class RuleBaseDef
    {
        private string _name;
        private RuleBaseType _type;
        private RuleDefCollection _ruleSet;

        public RuleBaseDef()
        {
            _name = null;
            _ruleSet = new RuleDefCollection();
            _type = RuleBaseType.Sequence;
        }

        public RuleBaseDef(string name, RuleBaseType type)
        {
            _name = name;
            _ruleSet = new RuleDefCollection();
            _type = type;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public RuleBaseType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public RuleDefCollection RuleSet
        {
            get
            {
                return _ruleSet;
            }
            set
            {
                _ruleSet = value;
            }
        }
    }
}