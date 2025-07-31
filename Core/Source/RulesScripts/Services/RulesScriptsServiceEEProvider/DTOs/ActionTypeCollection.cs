using System;
using System.Collections;
using System.Collections.Specialized;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Summary description for Operators.
	/// </summary>
	[Serializable]
	public class ActionTypeCollection : IEnumerable	
	{	
		private static IDictionary _innerList;
		
		public static readonly ActionType AssignAction=new ActionType(0,"LBL_ASSIGNACTION");
		public static readonly ActionType InvokeAction=new ActionType(1,"LBL_INVOKE");
        public static readonly ActionType TemporaryStoreAction = new ActionType(2, "LBL_TEMPORARYASSIGNACTION");

		public ActionTypeCollection()
		{
			if(_innerList==null)
			{
				_innerList =new ListDictionary();
				_innerList.Add(AssignAction.Key, AssignAction.Description);
				_innerList.Add(InvokeAction.Key, InvokeAction.Description);
                _innerList.Add(TemporaryStoreAction.Key, TemporaryStoreAction.Description);
            }
		}

		public IEnumerator GetEnumerator()
		{
			return _innerList.GetEnumerator();
		}

		public Object this[int key]
		{
			get{return _innerList[key];}
		}
	}

	[Serializable]
	public class ActionType
	{

		public ActionType(){}
		public ActionType(int key, string description)
		{
			_key=key;
			_description=description;
		}

		private int	_key;
		public int Key
		{
			get { return _key; }
			set { _key = value; }
		}

		private string	_description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

	}
}
