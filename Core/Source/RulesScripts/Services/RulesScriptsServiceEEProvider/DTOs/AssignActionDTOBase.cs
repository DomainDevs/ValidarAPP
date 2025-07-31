using System;
using System.Collections;
using System.CodeDom;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    [Serializable]
    public abstract class AssignActionDTOBase : ActionDTO
    {
        private Operator _arithmeticOperator;
        private DictionaryEntry _valueType;
        private CodeExpression _expression;

        public Operator ArithmeticOperator
        {
            get { return _arithmeticOperator; }
            set { _arithmeticOperator = value; }
        }

        public DictionaryEntry ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        public CodeExpression Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }
    }
}
