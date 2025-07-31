using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INSURED
	{

		#region InnerClass
		public enum INSUREDFields
		{
			INDIVIDUAL_ID,
			INSURED_CD,
			BRANCH_CD,
			INS_PROFILE_CD,
			INS_SEGMENT_CD,
			ENTERED_DATE,
			DECLINED_DATE,
			INS_DECLINED_TYPE_CD,
			MAIN_INSURED_IND_ID,
			OPERATIVE_ENDORSEMENT,
			CHECK_PAYABLE_TO,
			ANNOTATIONS,
			REFERRED_BY,
			EXONERATION_TYPE_CD,
			REQUIRED
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			int _iNSURED_CD;
			int _bRANCH_CD;
			int _iNS_PROFILE_CD;
			int _iNS_SEGMENT_CD;
			string _eNTERED_DATE;
			string _dECLINED_DATE;
			string _iNS_DECLINED_TYPE_CD;
			string _mAIN_INSURED_IND_ID;
			string _oPERATIVE_ENDORSEMENT;
			string _cHECK_PAYABLE_TO;
			string _aNNOTATIONS;
			string _rEFERRED_BY;
			string _eXONERATION_TYPE_CD;
			bool _rEQUIRED;
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
		public int  INSURED_CD
		{
			 get { return _iNSURED_CD; }
			 set {_iNSURED_CD = value;}
		}

		[DataMember]
		public int  BRANCH_CD
		{
			 get { return _bRANCH_CD; }
			 set {_bRANCH_CD = value;}
		}

		[DataMember]
		public int  INS_PROFILE_CD
		{
			 get { return _iNS_PROFILE_CD; }
			 set {_iNS_PROFILE_CD = value;}
		}

		[DataMember]
		public int  INS_SEGMENT_CD
		{
			 get { return _iNS_SEGMENT_CD; }
			 set {_iNS_SEGMENT_CD = value;}
		}

		[DataMember]
		public string  ENTERED_DATE
		{
			 get { return _eNTERED_DATE; }
			 set {_eNTERED_DATE = value;}
		}

		[DataMember]
		public string  DECLINED_DATE
		{
			 get { return _dECLINED_DATE; }
			 set {_dECLINED_DATE = value;}
		}

		[DataMember]
		public string  INS_DECLINED_TYPE_CD
		{
			 get { return _iNS_DECLINED_TYPE_CD; }
			 set {_iNS_DECLINED_TYPE_CD = value;}
		}

		[DataMember]
		public string  MAIN_INSURED_IND_ID
		{
			 get { return _mAIN_INSURED_IND_ID; }
			 set {_mAIN_INSURED_IND_ID = value;}
		}

		[DataMember]
		public string  OPERATIVE_ENDORSEMENT
		{
			 get { return _oPERATIVE_ENDORSEMENT; }
			 set {_oPERATIVE_ENDORSEMENT = value;}
		}

		[DataMember]
		public string  CHECK_PAYABLE_TO
		{
			 get { return _cHECK_PAYABLE_TO; }
			 set {_cHECK_PAYABLE_TO = value;}
		}

		[DataMember]
		public string  ANNOTATIONS
		{
			 get { return _aNNOTATIONS; }
			 set {_aNNOTATIONS = value;}
		}

		[DataMember]
		public string  REFERRED_BY
		{
			 get { return _rEFERRED_BY; }
			 set {_rEFERRED_BY = value;}
		}

		[DataMember]
		public string  EXONERATION_TYPE_CD
		{
			 get { return _eXONERATION_TYPE_CD; }
			 set {_eXONERATION_TYPE_CD = value;}
		}

		[DataMember]
		public bool  REQUIRED
		{
			 get { return _rEQUIRED; }
			 set {_rEQUIRED = value;}
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
