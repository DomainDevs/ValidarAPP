using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INDIVIDUAL_SARLAFT_LOG
	{

		#region InnerClass
		public enum INDIVIDUAL_SARLAFT_LOGFields
		{
			INDIVIDUAL_ID,
			USER_ID,
			EXONERATION_TYPE_CD,
			IS_EXONERATED,
			CHANGE_DATE,
			ROLE_CD
		}
		#endregion

		#region Data Members

			int _individual_id;
			int _user_id;
			string _exoneration_type_cd;
			bool _is_exonerated;
			string _change_date;
			int _role_cd;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
			 set {_individual_id = value;}
		}

		[DataMember]
		public int  USER_ID
		{
			 get { return _user_id; }
			 set {_user_id = value;}
		}

		[DataMember]
		public string  EXONERATION_TYPE_CD
		{
			 get { return _exoneration_type_cd; }
			 set {_exoneration_type_cd = value;}
		}

		[DataMember]
		public bool  IS_EXONERATED
		{
			 get { return _is_exonerated; }
			 set {_is_exonerated = value;}
		}

		[DataMember]
		public string  CHANGE_DATE
		{
			 get { return _change_date; }
			 set {_change_date = value;}
		}

		[DataMember]
		public int  ROLE_CD
		{
			 get { return _role_cd; }
			 set {_role_cd = value;}
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
