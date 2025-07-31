using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class CO_INSURED
	{

		#region InnerClass
		public enum CO_INSUREDFields
		{
			INDIVIDUAL_ID,
			SIGNING_DATE,
			AUTHORIZED_BY,
			ID_CARD_NO,
			ID_CARD_TYPE_CD,
			USER_CREATE,
			CREATE_DATE,
			LAST_UPDATE_USER,
			LAST_UPDATE_DATE
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			string _sIGNING_DATE;
			string _aUTHORIZED_BY;
			string _iD_CARD_NO;
			string _iD_CARD_TYPE_CD;
			string _uSER_CREATE;
            string _cREATE_DATE;
			string _lAST_UPDATE_USER;
            string _lAST_UPDATE_DATE;
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
		public string  SIGNING_DATE
		{
			 get { return _sIGNING_DATE; }
			 set {_sIGNING_DATE = value;}
		}

		[DataMember]
		public string  AUTHORIZED_BY
		{
			 get { return _aUTHORIZED_BY; }
			 set {_aUTHORIZED_BY = value;}
		}

		[DataMember]
		public string  ID_CARD_NO
		{
			 get { return _iD_CARD_NO; }
			 set {_iD_CARD_NO = value;}
		}

		[DataMember]
		public string  ID_CARD_TYPE_CD
		{
			 get { return _iD_CARD_TYPE_CD; }
			 set {_iD_CARD_TYPE_CD = value;}
		}

		[DataMember]
		public string  USER_CREATE
		{
			 get { return _uSER_CREATE; }
			 set {_uSER_CREATE = value;}
		}

		[DataMember]
		public string  CREATE_DATE
		{
			 get { return _cREATE_DATE; }
			 set {_cREATE_DATE = value;}
		}

		[DataMember]
		public string  LAST_UPDATE_USER
		{
			 get { return _lAST_UPDATE_USER; }
			 set {_lAST_UPDATE_USER = value;}
		}

		[DataMember]
		public string  LAST_UPDATE_DATE
		{
			 get { return _lAST_UPDATE_DATE; }
			 set {_lAST_UPDATE_DATE = value;}
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
