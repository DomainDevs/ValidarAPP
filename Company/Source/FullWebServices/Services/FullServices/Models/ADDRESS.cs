using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class ADDRESS
	{

		#region InnerClass
		public enum ADDRESSFields
		{
			INDIVIDUAL_ID,
			DATA_ID,
			ADDRESS_TYPE_CD,
			IS_MAILING_ADDRESS,
			STREET_TYPE_CD,
			STREET,
			CITY_CD,
			HOUSE_NUMBER,
			FLOOR,
			APARTMENT,
			ZIP_CODE,
			URBANIZATION,
			STATE_CD,
            COUNTRY_CD, //SUPDB
            IS_HOME //SUPDB
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			int _dATA_ID;
			int _aDDRESS_TYPE_CD;
			bool _iS_MAILING_ADDRESS;
			int _sTREET_TYPE_CD;
			string _sTREET;
			string _cITY_CD;
			string _hOUSE_NUMBER;
			string _fLOOR;
			string _aPARTMENT;
			string _zIP_CODE;
			string _uRBANIZATION;
			string _sTATE_CD;
			string _cOUNTRY_CD;
            bool _is_home; //SUPDB
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
		public int  DATA_ID
		{
			 get { return _dATA_ID; }
			 set {_dATA_ID = value;}
		}

		[DataMember]
		public int  ADDRESS_TYPE_CD
		{
			 get { return _aDDRESS_TYPE_CD; }
			 set {_aDDRESS_TYPE_CD = value;}
		}

		[DataMember]
		public bool  IS_MAILING_ADDRESS
		{
			 get { return _iS_MAILING_ADDRESS; }
			 set {_iS_MAILING_ADDRESS = value;}
		}

		[DataMember]
		public int  STREET_TYPE_CD
		{
			 get { return _sTREET_TYPE_CD; }
			 set {_sTREET_TYPE_CD = value;}
		}

		[DataMember]
		public string  STREET
		{
			 get { return _sTREET; }
			 set {_sTREET = value;}
		}

		[DataMember]
		public string  CITY_CD
		{
			 get { return _cITY_CD; }
			 set {_cITY_CD = value;}
		}

		[DataMember]
		public string  HOUSE_NUMBER
		{
			 get { return _hOUSE_NUMBER; }
			 set {_hOUSE_NUMBER = value;}
		}

		[DataMember]
		public string  FLOOR
		{
			 get { return _fLOOR; }
			 set {_fLOOR = value;}
		}

		[DataMember]
		public string  APARTMENT
		{
			 get { return _aPARTMENT; }
			 set {_aPARTMENT = value;}
		}

		[DataMember]
		public string  ZIP_CODE
		{
			 get { return _zIP_CODE; }
			 set {_zIP_CODE = value;}
		}

		[DataMember]
		public string  URBANIZATION
		{
			 get { return _uRBANIZATION; }
			 set {_uRBANIZATION = value;}
		}

		[DataMember]
		public string  STATE_CD
		{
			 get { return _sTATE_CD; }
			 set {_sTATE_CD = value;}
		}

		[DataMember]
		public string  COUNTRY_CD
		{
			 get { return _cOUNTRY_CD; }
			 set {_cOUNTRY_CD = value;}
		}

		//SUPDB - INICIO
        [DataMember]
        public bool IS_HOME
        {
            get { return _is_home; }
            set { _is_home = value; }
        }
		//SUPDB - FIN

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

        [DataMember]
        public string COMPANY_TYPE_CD
        {
            get;
            set;
        }

		#endregion

	}
}
