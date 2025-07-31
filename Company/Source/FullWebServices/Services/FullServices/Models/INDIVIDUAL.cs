using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INDIVIDUAL
	{

		#region InnerClass
		public enum INDIVIDUALFields
		{
			INDIVIDUAL_ID,
			INDIVIDUAL_TYPE_CD,
			AT_DATA_ID,
			AT_PAYMENT_ID,
			AT_AGENT_AGENCY_ID,
			OWNER_ROLE_CD,
			ECONOMIC_ACTIVITY_CD
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			int _iNDIVIDUAL_TYPE_CD;
			int _aT_DATA_ID;
			int _aT_PAYMENT_ID;
			int _aT_AGENT_AGENCY_ID;
			string _oWNER_ROLE_CD;
			string _eCONOMIC_ACTIVITY_CD;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _iNDIVIDUAL_ID; }
			 set {_iNDIVIDUAL_ID = value;}
		}

		[DataMember]
		public int  INDIVIDUAL_TYPE_CD
		{
			 get { return _iNDIVIDUAL_TYPE_CD; }
			 set {_iNDIVIDUAL_TYPE_CD = value;}
		}

		[DataMember]
		public int  AT_DATA_ID
		{
			 get { return _aT_DATA_ID; }
			 set {_aT_DATA_ID = value;}
		}

		[DataMember]
		public int  AT_PAYMENT_ID
		{
			 get { return _aT_PAYMENT_ID; }
			 set {_aT_PAYMENT_ID = value;}
		}

		[DataMember]
		public int  AT_AGENT_AGENCY_ID
		{
			 get { return _aT_AGENT_AGENCY_ID; }
			 set {_aT_AGENT_AGENCY_ID = value;}
		}

		[DataMember]
		public string  OWNER_ROLE_CD
		{
			 get { return _oWNER_ROLE_CD; }
			 set {_oWNER_ROLE_CD = value;}
		}

		[DataMember]
		public string  ECONOMIC_ACTIVITY_CD
		{
			 get { return _eCONOMIC_ACTIVITY_CD; }
			 set {_eCONOMIC_ACTIVITY_CD = value;}
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
