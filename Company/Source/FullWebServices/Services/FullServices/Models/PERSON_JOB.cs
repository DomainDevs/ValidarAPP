using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class PERSON_JOB
	{

		#region InnerClass
		public enum PERSON_JOBFields
		{
			INDIVIDUAL_ID,
			OCCUPATION_CD,
			INCOME_LEVEL_CD,
			COMPANY_NAME,
			JOB_SECTOR,
			POSITION,
			CONTACT,
			COMPANY_PHONE,
			SPECIALITY_CD,
			OTHER_OCCUPATION_CD
		}
		#endregion

		#region Data Members

			int _individual_id;
			int _occupation_cd;
			string _income_level_cd;
			string _company_name;
			string _job_sector;
			string _position;
			string _contact;
			string _company_phone;
			string _speciality_cd;
			string _other_occupation_cd;
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
		public int  OCCUPATION_CD
		{
			 get { return _occupation_cd; }
			 set {_occupation_cd = value;}
		}

		[DataMember]
		public string  INCOME_LEVEL_CD
		{
			 get { return _income_level_cd; }
			 set {_income_level_cd = value;}
		}

		[DataMember]
		public string  COMPANY_NAME
		{
			 get { return _company_name; }
			 set {_company_name = value;}
		}

		[DataMember]
		public string  JOB_SECTOR
		{
			 get { return _job_sector; }
			 set {_job_sector = value;}
		}

		[DataMember]
		public string  POSITION
		{
			 get { return _position; }
			 set {_position = value;}
		}

		[DataMember]
		public string  CONTACT
		{
			 get { return _contact; }
			 set {_contact = value;}
		}

		[DataMember]
		public string  COMPANY_PHONE
		{
			 get { return _company_phone; }
			 set {_company_phone = value;}
		}

		[DataMember]
		public string  SPECIALITY_CD
		{
			 get { return _speciality_cd; }
			 set {_speciality_cd = value;}
		}

		[DataMember]
		public string  OTHER_OCCUPATION_CD
		{
			 get { return _other_occupation_cd; }
			 set {_other_occupation_cd = value;}
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
