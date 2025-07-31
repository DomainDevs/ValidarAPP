using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class AGENT_AGENCY
	{

		#region InnerClass
		public enum AGENT_AGENCYFields
		{
			INDIVIDUAL_ID,
			AGENT_AGENCY_ID,
			DESCRIPTION,
			BRANCH_CD,
			ANNOTATIONS,
			AGENT_CD,
			AGENCY_GROUP_CD,
			DECLINED_DATE,
			AGENT_DECLINED_TYPE_CD,
			AGENT_TYPE_CD
		}
		#endregion

		#region Data Members

			int _individual_id;
			int _agent_agency_id;
			string _description;
			int _branch_cd;
			string _annotations;
			int _agent_cd;
			string _agency_group_cd;
			string _declined_date;
			string _agent_declined_type_cd;
			int _agent_type_cd;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
			 set {_individual_id = value;}
		}

		[DataMember]
		public int  AGENT_AGENCY_ID
		{
			 get { return _agent_agency_id; }
			 set {_agent_agency_id = value;}
		}

		[DataMember]
		public string  DESCRIPTION
		{
			 get { return _description; }
			 set {_description = value;}
		}

		[DataMember]
		public int  BRANCH_CD
		{
			 get { return _branch_cd; }
			 set {_branch_cd = value;}
		}

		[DataMember]
		public string  ANNOTATIONS
		{
			 get { return _annotations; }
			 set {_annotations = value;}
		}

		[DataMember]
		public int  AGENT_CD
		{
			 get { return _agent_cd; }
			 set {_agent_cd = value;}
		}

		[DataMember]
		public string  AGENCY_GROUP_CD
		{
			 get { return _agency_group_cd; }
			 set {_agency_group_cd = value;}
		}

		[DataMember]
		public string  DECLINED_DATE
		{
			 get { return _declined_date; }
			 set {_declined_date = value;}
		}

		[DataMember]
		public string  AGENT_DECLINED_TYPE_CD
		{
			 get { return _agent_declined_type_cd; }
			 set {_agent_declined_type_cd = value;}
		}

		[DataMember]
		public int  AGENT_TYPE_CD
		{
			 get { return _agent_type_cd; }
			 set {_agent_type_cd = value;}
		}


		[DataMember]
		public int  Identity
		{
		  get { return _identity; }
		  set	{ _identity = value;}
		}

		[DataMember]
		public char  State
		{
		  get { return _state; }
		  set	{ _state = value;}
		}

		[DataMember]
		public string  Connection
		{
		  get { return _connection; }
		  set	{ _connection = value;}
		}

		#endregion

	}
}
