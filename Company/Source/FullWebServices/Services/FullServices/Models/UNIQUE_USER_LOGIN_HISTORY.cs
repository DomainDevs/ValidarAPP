using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class UNIQUE_USER_LOGIN_HISTORY
	{

		#region InnerClass
		public enum UNIQUE_USER_LOGIN_HISTORYFields
		{
			USER_ID,
			CHANGE_DATE,
			PASSWORD,
			PASSWORD_INITIALIZED
		}
		#endregion

		#region Data Members

			int _user_id;
			string _change_date;
			string _password;
			bool _password_initialized;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  USER_ID
		{
			 get { return _user_id; }
			 set {_user_id = value;}
		}

		[DataMember]
		public string  CHANGE_DATE
		{
			 get { return _change_date; }
			 set {_change_date = value;}
		}

		[DataMember]
		public string  PASSWORD
		{
			 get { return _password; }
			 set {_password = value;}
		}

		[DataMember]
		public bool  PASSWORD_INITIALIZED
		{
			 get { return _password_initialized; }
			 set {_password_initialized = value;}
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
