using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class COMPANY
	{

		#region InnerClass
		public enum COMPANYFields
		{
			INDIVIDUAL_ID,
			TRADE_NAME,
			TRIBUTARY_ID_TYPE_CD,
			TRIBUTARY_ID_NO,
			COUNTRY_CD,
			COMPANY_TYPE_CD,
			BUSINESS_LEGAL_STATUS_CD,
			MARKET_SECTOR_CD,
			MANAGER_NAME,
			GENERAL_MANAGER_NAME,
			LEGAL_REPRESENTATIVE_NAME,
			LR_ID_CARD_TYPE_CD,
			LR_ID_CARD_NO,
			CONTACT_NAME,
			CONTACT_ADDITIONAL_INFO
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			string _tRADE_NAME;
			int _tRIBUTARY_ID_TYPE_CD;
			string _tRIBUTARY_ID_NO;
			int _cOUNTRY_CD;
			int _cOMPANY_TYPE_CD;
			string _bUSINESS_LEGAL_STATUS_CD;
			string _mARKET_SECTOR_CD;
			string _mANAGER_NAME;
			string _gENERAL_MANAGER_NAME;
			string _lEGAL_REPRESENTATIVE_NAME;
			string _lR_ID_CARD_TYPE_CD;
			string _lR_ID_CARD_NO;
			string _cONTACT_NAME;
			string _cONTACT_ADDITIONAL_INFO;
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
		public string  TRADE_NAME
		{
			 get { return _tRADE_NAME; }
			 set {_tRADE_NAME = value;}
		}

		[DataMember]
		public int  TRIBUTARY_ID_TYPE_CD
		{
			 get { return _tRIBUTARY_ID_TYPE_CD; }
			 set {_tRIBUTARY_ID_TYPE_CD = value;}
		}

		[DataMember]
		public string  TRIBUTARY_ID_NO
		{
			 get { return _tRIBUTARY_ID_NO; }
			 set {_tRIBUTARY_ID_NO = value;}
		}

		[DataMember]
		public int  COUNTRY_CD
		{
			 get { return _cOUNTRY_CD; }
			 set {_cOUNTRY_CD = value;}
		}

		[DataMember]
		public int  COMPANY_TYPE_CD
		{
			 get { return _cOMPANY_TYPE_CD; }
			 set {_cOMPANY_TYPE_CD = value;}
		}

		[DataMember]
		public string  BUSINESS_LEGAL_STATUS_CD
		{
			 get { return _bUSINESS_LEGAL_STATUS_CD; }
			 set {_bUSINESS_LEGAL_STATUS_CD = value;}
		}

		[DataMember]
		public string  MARKET_SECTOR_CD
		{
			 get { return _mARKET_SECTOR_CD; }
			 set {_mARKET_SECTOR_CD = value;}
		}

		[DataMember]
		public string  MANAGER_NAME
		{
			 get { return _mANAGER_NAME; }
			 set {_mANAGER_NAME = value;}
		}

		[DataMember]
		public string  GENERAL_MANAGER_NAME
		{
			 get { return _gENERAL_MANAGER_NAME; }
			 set {_gENERAL_MANAGER_NAME = value;}
		}

		[DataMember]
		public string  LEGAL_REPRESENTATIVE_NAME
		{
			 get { return _lEGAL_REPRESENTATIVE_NAME; }
			 set {_lEGAL_REPRESENTATIVE_NAME = value;}
		}

		[DataMember]
		public string  LR_ID_CARD_TYPE_CD
		{
			 get { return _lR_ID_CARD_TYPE_CD; }
			 set {_lR_ID_CARD_TYPE_CD = value;}
		}

		[DataMember]
		public string  LR_ID_CARD_NO
		{
			 get { return _lR_ID_CARD_NO; }
			 set {_lR_ID_CARD_NO = value;}
		}

		[DataMember]
		public string  CONTACT_NAME
		{
			 get { return _cONTACT_NAME; }
			 set {_cONTACT_NAME = value;}
		}

		[DataMember]
		public string  CONTACT_ADDITIONAL_INFO
		{
			 get { return _cONTACT_ADDITIONAL_INFO; }
			 set {_cONTACT_ADDITIONAL_INFO = value;}
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
