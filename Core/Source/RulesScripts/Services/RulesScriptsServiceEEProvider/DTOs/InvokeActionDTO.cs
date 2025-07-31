using System;
using System.Collections;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Summary description for InvokeActionDTO.
	/// </summary>
	[Serializable]
	public class InvokeActionDTO : ActionDTO	
	{		
		private DictionaryEntry	_function;
		public DictionaryEntry Function
		{
			get { return _function; }
			set { _function = value; }
		}

		private string	_message;
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}
	}
}