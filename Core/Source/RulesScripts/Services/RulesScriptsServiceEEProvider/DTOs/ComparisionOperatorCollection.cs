using System.CodeDom;
using System.Collections.Specialized;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Colección de operadores de comparación.
	/// </summary>
	public sealed class ComparisonOperatorCollection : OperatorCollection  	
	{	
		#region Constants
		public static readonly Operator Equal= new 
			Operator(CodeBinaryOperatorType.IdentityEquality,
			"EQUAL","EQUAL_SMALLDESC");
		
		public static readonly Operator Greater=new 
			Operator(CodeBinaryOperatorType.GreaterThan ,
			"GREATER","GREATER_SMALLDESC");
		
		public static readonly Operator Less=new 
			Operator(CodeBinaryOperatorType.LessThan,
			"LESS","LESS_SMALLDESC");
		
		public static readonly Operator GreaterOrEqual=new 
			Operator(CodeBinaryOperatorType.GreaterThanOrEqual,
			"GREATER_OR_EQUAL","GREATER_OR_EQUAL_SMALLDESC");
		
		public static readonly Operator LessOrEqual=new 
			Operator(CodeBinaryOperatorType.LessThanOrEqual,
			"LESS_OR_EQUAL","LESS_OR_EQUAL_SMALLDESC");
		
		public static readonly Operator NotEqual=new 
			Operator(CodeBinaryOperatorType.IdentityInequality,
			"NOT_EQUAL","NOT_EQUAL_SMALLDESC");
		#endregion Constants

		public ComparisonOperatorCollection()
		{
			_innerList = new ListDictionary();
			_innerList.Add(Equal.Code, Equal);
			_innerList.Add(Greater.Code, Greater);
			_innerList.Add(Less.Code, Less);
			_innerList.Add(GreaterOrEqual.Code, GreaterOrEqual);
			_innerList.Add(LessOrEqual.Code, LessOrEqual);
			_innerList.Add(NotEqual.Code, NotEqual);
		}
	}
}
