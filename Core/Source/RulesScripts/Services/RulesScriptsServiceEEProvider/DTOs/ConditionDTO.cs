using System;
using System.CodeDom;
using Sistran.Core.Framework.Data;


namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Summary description for ConditionDTO.
	/// </summary>
	[Serializable]
	public class ConditionDTO : DtoBase
	{
		private CodeConceptExpression _concept;
		private Operator	_comparisionOperator;
		private CodeExpression	_expression;

		
		public CodeConceptExpression Concept
		{
            get { return _concept; }
            set { _concept = value; }
		}
		
		public Operator ComparisionOperator
		{
			get { return _comparisionOperator; }
			set { _comparisionOperator = value; }
		}

		private System.Collections.DictionaryEntry	_valueType;
		public System.Collections.DictionaryEntry ValueType
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