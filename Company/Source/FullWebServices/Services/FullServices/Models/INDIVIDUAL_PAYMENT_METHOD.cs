using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INDIVIDUAL_PAYMENT_METHOD
	{

		#region InnerClass
		public enum INDIVIDUAL_PAYMENT_METHODFields
		{
			INDIVIDUAL_ID,
			PAYMENT_ID,
			ROLE_CD,
			PAYMENT_METHOD_CD,
			ENABLED
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			int _pAYMENT_ID;
			int _rOLE_CD;
			int _pAYMENT_METHOD_CD;
			bool _eNABLED;
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
		public int  PAYMENT_ID
		{
			 get { return _pAYMENT_ID; }
			 set {_pAYMENT_ID = value;}
		}

		[DataMember]
		public int  ROLE_CD
		{
			 get { return _rOLE_CD; }
			 set {_rOLE_CD = value;}
		}

		[DataMember]
		public int  PAYMENT_METHOD_CD
		{
			 get { return _pAYMENT_METHOD_CD; }
			 set {_pAYMENT_METHOD_CD = value;}
		}

		[DataMember]
		public bool  ENABLED
		{
			 get { return _eNABLED; }
			 set {_eNABLED = value;}
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
