using System.Collections;
using System.Collections.Specialized;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Clase base para las colecciones de operadores.
	/// </summary>
	public abstract class OperatorCollection: IEnumerable
	{
		protected IDictionary _innerList;

		public IEnumerator GetEnumerator()
		{
			return _innerList.GetEnumerator();
		}

		public Operator this[int key]
		{
			get{return (Operator)_innerList[key];}
		}

		internal virtual void SetItems()
		{
			_innerList=new ListDictionary();
		}

	}
}
