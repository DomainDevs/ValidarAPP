using System;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Summary description for CodeDynamicConceptExpression.
	/// </summary>
	[Serializable]
	public class CodeDynamicConceptExpression :	CodeConceptExpression
	{
		public CodeDynamicConceptExpression()
		{
		}

		public CodeDynamicConceptExpression(PrimaryKey key) : base(key)
		{
		}
	}
}
