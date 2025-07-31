using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INDIVIDUAL_DIRECTORY
	{

		#region InnerClass
		public enum INDIVIDUAL_DIRECTORYFields
		{
			INDIVIDUAL_ID,
			DATA_ID,
			DIRECTORY_TYPE_CD,
			TYPE_NUMBER
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			int _dATA_ID;
			int _dIRECTORY_TYPE_CD;
			string _tYPE_NUMBER;
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
		public int  DIRECTORY_TYPE_CD
		{
			 get { return _dIRECTORY_TYPE_CD; }
			 set {_dIRECTORY_TYPE_CD = value;}
		}

		[DataMember]
		public string  TYPE_NUMBER
		{
			 get { return _tYPE_NUMBER; }
			 set {_tYPE_NUMBER = value;}
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
