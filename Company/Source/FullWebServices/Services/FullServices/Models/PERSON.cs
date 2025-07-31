using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class PERSON
	{

		#region InnerClass
		public enum PERSONFields
		{
			INDIVIDUAL_ID,
			SURNAME,
			NAME,
			GENDER,
			ID_CARD_TYPE_CD,
			ID_CARD_NO,
			MARITAL_STATUS_CD,
			BIRTH_DATE,
			CHILDREN,
			EDUCATIVE_LEVEL_CD,
			MOTHER_LAST_NAME,
			TRIBUTARY_ID_TYPE_CD,
			TRIBUTARY_ID_NO,
			BIRTH_COUNTRY_CD,
			BIRTH_PLACE,
			SPOUSE_NAME,
			HOUSE_TYPE_CD,
			SOCIAL_LAYER_CD
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			string _sURNAME;
			string _nAME;
			string _gENDER;
			int _iD_CARD_TYPE_CD;
			string _iD_CARD_NO;
			int _mARITAL_STATUS_CD;
			string _bIRTH_DATE;
			string _cHILDREN;
			string _eDUCATIVE_LEVEL_CD;
			string _mOTHER_LAST_NAME;
			string _tRIBUTARY_ID_TYPE_CD;
			string _tRIBUTARY_ID_NO;
			string _bIRTH_COUNTRY_CD;
			string _bIRTH_PLACE;
			string _sPOUSE_NAME;
			string _hOUSE_TYPE_CD;
			string _sOCIAL_LAYER_CD;
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
		public string  SURNAME
		{
			 get { return _sURNAME; }
			 set {_sURNAME = value;}
		}

		[DataMember]
		public string  NAME
		{
			 get { return _nAME; }
			 set {_nAME = value;}
		}

		[DataMember]
		public string  GENDER
		{
			 get { return _gENDER; }
			 set {_gENDER = value;}
		}

		[DataMember]
		public int  ID_CARD_TYPE_CD
		{
			 get { return _iD_CARD_TYPE_CD; }
			 set {_iD_CARD_TYPE_CD = value;}
		}

		[DataMember]
		public string  ID_CARD_NO
		{
			 get { return _iD_CARD_NO; }
			 set {_iD_CARD_NO = value;}
		}

		[DataMember]
		public int  MARITAL_STATUS_CD
		{
			 get { return _mARITAL_STATUS_CD; }
			 set {_mARITAL_STATUS_CD = value;}
		}

		[DataMember]
		public string  BIRTH_DATE
		{
			 get { return _bIRTH_DATE; }
			 set {_bIRTH_DATE = value;}
		}

		[DataMember]
		public string  CHILDREN
		{
			 get { return _cHILDREN; }
			 set {_cHILDREN = value;}
		}

		[DataMember]
		public string  EDUCATIVE_LEVEL_CD
		{
			 get { return _eDUCATIVE_LEVEL_CD; }
			 set {_eDUCATIVE_LEVEL_CD = value;}
		}

		[DataMember]
		public string  MOTHER_LAST_NAME
		{
			 get { return _mOTHER_LAST_NAME; }
			 set {_mOTHER_LAST_NAME = value;}
		}

		[DataMember]
		public string  TRIBUTARY_ID_TYPE_CD
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
		public string  BIRTH_COUNTRY_CD
		{
			 get { return _bIRTH_COUNTRY_CD; }
			 set {_bIRTH_COUNTRY_CD = value;}
		}

		[DataMember]
		public string  BIRTH_PLACE
		{
			 get { return _bIRTH_PLACE; }
			 set {_bIRTH_PLACE = value;}
		}

		[DataMember]
		public string  SPOUSE_NAME
		{
			 get { return _sPOUSE_NAME; }
			 set {_sPOUSE_NAME = value;}
		}

		[DataMember]
		public string  HOUSE_TYPE_CD
		{
			 get { return _hOUSE_TYPE_CD; }
			 set {_hOUSE_TYPE_CD = value;}
		}

		[DataMember]
		public string  SOCIAL_LAYER_CD
		{
			 get { return _sOCIAL_LAYER_CD; }
			 set {_sOCIAL_LAYER_CD = value;}
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
