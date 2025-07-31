using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class UNIQUE_USER_LOGIN
	{

		#region InnerClass
		public enum UNIQUE_USER_LOGINFields
		{
			USER_ID,
			PASSWORD,
			PASSWORD_EXPIRATION_DATE,
			PASSWORD_EXPIRATION_DAYS,
			PASSWORD_NEVER_EXPIRE,
			MUST_CHANGE_PASSWORD,
			CAN_CHANGE_PASSWORD
		}
		#endregion

		#region Data Members

			int _user_id;
			String _password;
			string _password_expiration_date;
			string _password_expiration_days;
			bool _password_never_expire;
			bool _must_change_password;
			bool _can_change_password;
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
		public String  PASSWORD
		{
			 get { return _password; }
			 set {_password = value;}
		}

		[DataMember]
		public string  PASSWORD_EXPIRATION_DATE
		{
			 get { return _password_expiration_date; }
			 set {_password_expiration_date = value;}
		}

		[DataMember]
		public string  PASSWORD_EXPIRATION_DAYS
		{
			 get { return _password_expiration_days; }
			 set {_password_expiration_days = value;}
		}

		[DataMember]
		public bool  PASSWORD_NEVER_EXPIRE
		{
			 get { return _password_never_expire; }
			 set {_password_never_expire = value;}
		}

		[DataMember]
		public bool  MUST_CHANGE_PASSWORD
		{
			 get { return _must_change_password; }
			 set {_must_change_password = value;}
		}

		[DataMember]
		public bool  CAN_CHANGE_PASSWORD
		{
			 get { return _can_change_password; }
			 set {_can_change_password = value;}
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
