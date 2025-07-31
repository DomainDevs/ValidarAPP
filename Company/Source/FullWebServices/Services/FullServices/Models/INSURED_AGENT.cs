using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INSURED_AGENT
	{

		#region InnerClass
		public enum INSURED_AGENTFields
		{
			INSURED_IND_ID,
			AGENT_IND_ID,
			AGENT_AGENCY_ID,
			IS_MAIN
		}
		#endregion

		#region Data Members

			int _iNSURED_IND_ID;
			int _aGENT_IND_ID;
			int _aGENT_AGENCY_ID;
			bool _iS_MAIN;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  INSURED_IND_ID
		{
			 get { return _iNSURED_IND_ID; }
			 set {_iNSURED_IND_ID = value;}
		}

		[DataMember]
		public int  AGENT_IND_ID
		{
			 get { return _aGENT_IND_ID; }
			 set {_aGENT_IND_ID = value;}
		}

		[DataMember]
		public int  AGENT_AGENCY_ID
		{
			 get { return _aGENT_AGENCY_ID; }
			 set {_aGENT_AGENCY_ID = value;}
		}

		[DataMember]
		public bool  IS_MAIN
		{
			 get { return _iS_MAIN; }
			 set {_iS_MAIN = value;}
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
