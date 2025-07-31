using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class PERSON_INTEREST_GROUP
	{

		#region InnerClass
		public enum PERSON_INTEREST_GROUPFields
		{
			INDIVIDUAL_ID,
			INTEREST_GROUP_TYPE_CD
		}
		#endregion

		#region Data Members

			int _iNDIVIDUAL_ID;
			int _iNTEREST_GROUP_TYPE_CD;
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
		public int  INTEREST_GROUP_TYPE_CD
		{
			 get { return _iNTEREST_GROUP_TYPE_CD; }
			 set {_iNTEREST_GROUP_TYPE_CD = value;}
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
