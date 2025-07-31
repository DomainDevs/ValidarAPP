using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class CO_COMPANY
	{

		#region InnerClass
		public enum CO_COMPANYFields
		{
			INDIVIDUAL_ID,
			VERIFY_DIGIT,
			ASSOCIATION_TYPE_CD,
			AUTHORIZATION_AMOUNT,
			CURRENCY_CODE,
			CATEGORY_CD,
			ENTITY_OFFICIAL_CD,
			INSURED_GROUP_ID
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			string _vERIFY_DIGIT;
			int _aSSOCIATION_TYPE_CD;
			string _aUTHORIZATION_AMOUNT;
			string _cURRENCY_CODE;
			string _cATEGORY_CD;
			string _eNTITY_OFFICIAL_CD;
			string _iNSURED_GROUP_ID;
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
		public string  VERIFY_DIGIT
		{
			 get { return _vERIFY_DIGIT; }
			 set {_vERIFY_DIGIT = value;}
		}

		[DataMember]
		public int  ASSOCIATION_TYPE_CD
		{
			 get { return _aSSOCIATION_TYPE_CD; }
			 set {_aSSOCIATION_TYPE_CD = value;}
		}

		[DataMember]
		public string  AUTHORIZATION_AMOUNT
		{
			 get { return _aUTHORIZATION_AMOUNT; }
			 set {_aUTHORIZATION_AMOUNT = value;}
		}

		[DataMember]
		public string  CURRENCY_CODE
		{
			 get { return _cURRENCY_CODE; }
			 set {_cURRENCY_CODE = value;}
		}

		[DataMember]
		public string  CATEGORY_CD
		{
			 get { return _cATEGORY_CD; }
			 set {_cATEGORY_CD = value;}
		}

		[DataMember]
		public string  ENTITY_OFFICIAL_CD
		{
			 get { return _eNTITY_OFFICIAL_CD; }
			 set {_eNTITY_OFFICIAL_CD = value;}
		}

		[DataMember]
		public string  INSURED_GROUP_ID
		{
			 get { return _iNSURED_GROUP_ID; }
			 set {_iNSURED_GROUP_ID = value;}
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
