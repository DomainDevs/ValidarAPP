using System;
using System.Collections;
using Sistran.Core.Framework.Data;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Summary description for RuleDTO.
	/// </summary>
	[Serializable]
	public class RuleDTO : DtoBase	
	{
		private string	_name;
		private IList	_conditions;
		private IList	_actions;

		public RuleDTO()
		{
			_conditions=new ArrayList();
			_actions=new ArrayList();
		}
		
		private int	_id;
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		
		public IList Conditions
		{
			get { return _conditions; }
			set { _conditions = value; }
		}
		
		public IList Actions
		{
			get { return _actions; }
			set { _actions = value; }
		}	
	}
}