using System.CodeDom;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Summary description for RuleDef.
    /// </summary>
    public class RuleDef
    {
        private string _name = null;
        private CodeParameterDeclarationExpressionCollection _parameters = new CodeParameterDeclarationExpressionCollection();
        private CodeExpressionCollection _conditions = new CodeExpressionCollection();
        private CodeStatementCollection _consequence = new CodeStatementCollection();

        public RuleDef(string name)
        {
            _name = name;
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

        public CodeParameterDeclarationExpressionCollection Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public CodeExpressionCollection Conditions
        {
            get
            {
                return _conditions;
            }
        }

        public CodeStatementCollection Consequence
        {
            get
            {
                return _consequence;
            }
        }
    }
}