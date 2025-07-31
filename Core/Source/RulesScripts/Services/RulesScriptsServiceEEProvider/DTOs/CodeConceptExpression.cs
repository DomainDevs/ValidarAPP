using System;
using System.CodeDom;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Summary description for ListEntityDTO.
	/// </summary>
	[Serializable]
	public class CodeConceptExpression : CodeExpression
	{
		private PrimaryKey	_concept;

		public CodeConceptExpression()
		{}
		
		public CodeConceptExpression(PrimaryKey concept)
		{
			_concept = concept;
		}

		public PrimaryKey ConceptKey
		{
			get { return _concept; }
			set { _concept = value; }
		}
	}
}