using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class PAYMENT_METHOD_CARD
	{

		#region InnerClass
		public enum PAYMENT_METHOD_CARDFields
		{
			INDIVIDUAL_ID,
			PAYMENT_ID,
			CARD_NUMBER,
			BANK_CD,
			SECURITY_NUMBER,
			SINCE,
			THRU
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			int _pAYMENT_ID;
			double _cARD_NUMBER;
			string _bANK_CD;
			string _sECURITY_NUMBER;
			string _sINCE;
			string _tHRU;
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
		public double  CARD_NUMBER
		{
			 get { return _cARD_NUMBER; }
			 set {_cARD_NUMBER = value;}
		}

		[DataMember]
		public string  BANK_CD
		{
			 get { return _bANK_CD; }
			 set {_bANK_CD = value;}
		}

		[DataMember]
		public string  SECURITY_NUMBER
		{
			 get { return _sECURITY_NUMBER; }
			 set {_sECURITY_NUMBER = value;}
		}

		[DataMember]
		public string  SINCE
		{
			 get { return _sINCE; }
			 set {_sINCE = value;}
		}

		[DataMember]
		public string  THRU
		{
			 get { return _tHRU; }
			 set {_tHRU = value;}
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
