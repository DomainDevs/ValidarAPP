using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class UNIQUE_USERS
	{

		#region InnerClass
		public enum UNIQUE_USERSFields
		{
			USER_ID,
			ACCOUNT_NAME,
			PERSON_ID,
			AUTHENTICATION_TYPE_CD,
			USER_DOMAIN,
			DISABLED_DATE,
			LOCK_DATE,
			EXPIRATION_DATE,
			LOCK_PASSWORD,
			ACTIVATION_DATE,
			CREATED_DATE,
			CREATED_USER_ID,
			MODIFIED_DATE,
			MODIFIED_USER_ID
		}
		#endregion

		#region Data Members

			int _user_id;
			string _account_name;
			int _person_id;
			int _authentication_type_cd;
			string _user_domain;
			string _disabled_date;
			string _lock_date;
			string _expiration_date;
			bool _lock_password;
			bool _activation_date;
			string _created_date;
			string _created_user_id;
			string _modified_date;
			string _modified_user_id;
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
		public string  ACCOUNT_NAME
		{
			 get { return _account_name; }
			 set {_account_name = value;}
		}

		[DataMember]
		public int  PERSON_ID
		{
			 get { return _person_id; }
			 set {_person_id = value;}
		}

		[DataMember]
		public int  AUTHENTICATION_TYPE_CD
		{
			 get { return _authentication_type_cd; }
			 set {_authentication_type_cd = value;}
		}

		[DataMember]
		public string  USER_DOMAIN
		{
			 get { return _user_domain; }
			 set {_user_domain = value;}
		}

		[DataMember]
		public string  DISABLED_DATE
		{
			 get { return _disabled_date; }
			 set {_disabled_date = value;}
		}

		[DataMember]
		public string  LOCK_DATE
		{
			 get { return _lock_date; }
			 set {_lock_date = value;}
		}

		[DataMember]
		public string  EXPIRATION_DATE
		{
			 get { return _expiration_date; }
			 set {_expiration_date = value;}
		}

		[DataMember]
		public bool  LOCK_PASSWORD
		{
			 get { return _lock_password; }
			 set {_lock_password = value;}
		}

		[DataMember]
		public bool  ACTIVATION_DATE
		{
			 get { return _activation_date; }
			 set {_activation_date = value;}
		}

		[DataMember]
		public string  CREATED_DATE
		{
			 get { return _created_date; }
			 set {_created_date = value;}
		}

		[DataMember]
		public string  CREATED_USER_ID
		{
			 get { return _created_user_id; }
			 set {_created_user_id = value;}
		}

		[DataMember]
		public string  MODIFIED_DATE
		{
			 get { return _modified_date; }
			 set {_modified_date = value;}
		}

		[DataMember]
		public string  MODIFIED_USER_ID
		{
			 get { return _modified_user_id; }
			 set {_modified_user_id = value;}
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
