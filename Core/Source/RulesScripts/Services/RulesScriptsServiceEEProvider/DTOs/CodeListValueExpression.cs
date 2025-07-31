using System;
using System.CodeDom;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Summary description for CodeListValueExpression.
	/// </summary>
	[Serializable]
	public class CodeListValueExpression : CodeExpression	
	{
		private Object	_key;
		private Object	_value;

		public CodeListValueExpression()
		{}
		public CodeListValueExpression(object key, object value)
		{
			_key = key;
			_value = value;
		}
		
		public Object Key
		{
			get { return _key; }
			set { _key = value; }
		}
		
		public Object Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}
}