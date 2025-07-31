using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class EMAIL
	{

		#region InnerClass
		public enum EMAILFields
		{
			INDIVIDUAL_ID,
			DATA_ID,
			ADDRESS,
			EMAIL_TYPE_CD,
			IS_MAILING_ADDRESS
		}
		#endregion

		#region Data Members

			int _individual_id;
			int _data_id;
			string _address;
			int _email_type_cd;
			bool _is_mailing_address;
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
		public int  DATA_ID
		{
			 get { return _data_id; }
			 set {_data_id = value;}
		}

		[DataMember]
		public string  ADDRESS
		{
			 get { return _address; }
			 set {_address = value;}
		}

		[DataMember]
		public int  EMAIL_TYPE_CD
		{
			 get { return _email_type_cd; }
			 set {_email_type_cd = value;}
		}

		[DataMember]
		public bool  IS_MAILING_ADDRESS
		{
			 get { return _is_mailing_address; }
			 set {_is_mailing_address = value;}
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
