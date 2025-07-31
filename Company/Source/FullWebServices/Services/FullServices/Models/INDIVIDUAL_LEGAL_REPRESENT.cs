using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INDIVIDUAL_LEGAL_REPRESENT
	{

		#region InnerClass
		public enum INDIVIDUAL_LEGAL_REPRESENTFields
		{
			INDIVIDUAL_ID,
			LEGAL_REPRESENTATIVE_NAME,
			EXPEDITION_DATE,
			EXPEDITION_PLACE,
			BIRTH_DATE,
			BIRTH_PLACE,
			NATIONALITY,
			CITY,
			PHONE,
			JOB_TITLE,
			CELL_PHONE,
			EMAIL,
			ADDRESS,
			ID_CARD_NO,
			AUTHORIZATION_AMT,
			DESCRIPTION,
			CURRENCY_CD,
			ID_CARD_TYPE_CD,
			COUNTRY_CD,
			STATE_CD,
			CITY_CD
		}
		#endregion

		#region Data Members

			int _individual_id;
			string _legal_representative_name;
			string _expedition_date;
			string _expedition_place;
			string _birth_date;
			string _birth_place;
			string _nationality;
			string _city;
			string _phone;
			string _job_title;
			string _cell_phone;
			string _email;
			string _address;
			string _id_card_no;
			string _authorization_amt;
			string _description;
			string _currency_cd;
			int _id_card_type_cd;
			int _country_cd;
			int _state_cd;
			int _city_cd;
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
		public string  LEGAL_REPRESENTATIVE_NAME
		{
			 get { return _legal_representative_name; }
			 set {_legal_representative_name = value;}
		}

		[DataMember]
		public string  EXPEDITION_DATE
		{
			 get { return _expedition_date; }
			 set {_expedition_date = value;}
		}

		[DataMember]
		public string  EXPEDITION_PLACE
		{
			 get { return _expedition_place; }
			 set {_expedition_place = value;}
		}

		[DataMember]
		public string  BIRTH_DATE
		{
			 get { return _birth_date; }
			 set {_birth_date = value;}
		}

		[DataMember]
		public string  BIRTH_PLACE
		{
			 get { return _birth_place; }
			 set {_birth_place = value;}
		}

		[DataMember]
		public string  NATIONALITY
		{
			 get { return _nationality; }
			 set {_nationality = value;}
		}

		[DataMember]
		public string  CITY
		{
			 get { return _city; }
			 set {_city = value;}
		}

		[DataMember]
		public string  PHONE
		{
			 get { return _phone; }
			 set {_phone = value;}
		}

		[DataMember]
		public string  JOB_TITLE
		{
			 get { return _job_title; }
			 set {_job_title = value;}
		}

		[DataMember]
		public string  CELL_PHONE
		{
			 get { return _cell_phone; }
			 set {_cell_phone = value;}
		}

		[DataMember]
		public string  EMAIL
		{
			 get { return _email; }
			 set {_email = value;}
		}

		[DataMember]
		public string  ADDRESS
		{
			 get { return _address; }
			 set {_address = value;}
		}

		[DataMember]
		public string  ID_CARD_NO
		{
			 get { return _id_card_no; }
			 set {_id_card_no = value;}
		}

		[DataMember]
		public string  AUTHORIZATION_AMT
		{
			 get { return _authorization_amt; }
			 set {_authorization_amt = value;}
		}

		[DataMember]
		public string  DESCRIPTION
		{
			 get { return _description; }
			 set {_description = value;}
		}

		[DataMember]
		public string  CURRENCY_CD
		{
			 get { return _currency_cd; }
			 set {_currency_cd = value;}
		}

		[DataMember]
		public int  ID_CARD_TYPE_CD
		{
			 get { return _id_card_type_cd; }
			 set {_id_card_type_cd = value;}
		}

		[DataMember]
		public int  COUNTRY_CD
		{
			 get { return _country_cd; }
			 set {_country_cd = value;}
		}

		[DataMember]
		public int  STATE_CD
		{
			 get { return _state_cd; }
			 set {_state_cd = value;}
		}

		[DataMember]
		public int  CITY_CD
		{
			 get { return _city_cd; }
			 set {_city_cd = value;}
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
