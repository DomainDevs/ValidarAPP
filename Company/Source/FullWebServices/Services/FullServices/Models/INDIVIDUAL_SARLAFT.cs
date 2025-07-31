using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INDIVIDUAL_SARLAFT
	{

		#region InnerClass
		public enum INDIVIDUAL_SARLAFTFields
		{
			SARLAFT_ID,
			FORM_NUM,
			YEAR,
			REGISTRATION_DATE,
			AUTHORIZED_BY,
			FILLING_DATE,
			CHECK_DATE,
			VERIFYING_EMPLOYEE,
			INTERVIEW_DATE,
			INTERVIEWER_NAME,
			INTERNATIONAL_OPERATIONS,
			INTERVIEW_PLACE,
			INDIVIDUAL_ID,
			USER_ID,
			BRANCH_CD,
			ECONOMIC_ACTIVITY_CD,
			SECOND_ECONOMIC_ACTIVITY_CD,
			INTERVIEW_RESULT_CD,
			PENDING_EVENT
		}
		#endregion

		#region Data Members

			int _sarlaft_id;
			string _form_num;
			int _year;
			string _registration_date;
			string _authorized_by;
			string _filling_date;
			string _check_date;
			string _verifying_employee;
			string _interview_date;
			string _interviewer_name;
			bool _international_operations;
			string _interview_place;
			int _individual_id;
			string _user_id;
			string _branch_cd;
			int _economic_activity_cd;
			int _second_economic_activity_cd;
			int _interview_result_cd;
			bool _pending_event;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  SARLAFT_ID
		{
			 get { return _sarlaft_id; }
			 set {_sarlaft_id = value;}
		}

		[DataMember]
		public string  FORM_NUM
		{
			 get { return _form_num; }
			 set {_form_num = value;}
		}

		[DataMember]
		public int  YEAR
		{
			 get { return _year; }
			 set {_year = value;}
		}

		[DataMember]
		public string  REGISTRATION_DATE
		{
			 get { return _registration_date; }
			 set {_registration_date = value;}
		}

		[DataMember]
		public string  AUTHORIZED_BY
		{
			 get { return _authorized_by; }
			 set {_authorized_by = value;}
		}

		[DataMember]
		public string  FILLING_DATE
		{
			 get { return _filling_date; }
			 set {_filling_date = value;}
		}

		[DataMember]
		public string  CHECK_DATE
		{
			 get { return _check_date; }
			 set {_check_date = value;}
		}

		[DataMember]
		public string  VERIFYING_EMPLOYEE
		{
			 get { return _verifying_employee; }
			 set {_verifying_employee = value;}
		}

		[DataMember]
		public string  INTERVIEW_DATE
		{
			 get { return _interview_date; }
			 set {_interview_date = value;}
		}

		[DataMember]
		public string  INTERVIEWER_NAME
		{
			 get { return _interviewer_name; }
			 set {_interviewer_name = value;}
		}

		[DataMember]
		public bool  INTERNATIONAL_OPERATIONS
		{
			 get { return _international_operations; }
			 set {_international_operations = value;}
		}

		[DataMember]
		public string  INTERVIEW_PLACE
		{
			 get { return _interview_place; }
			 set {_interview_place = value;}
		}

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
			 set {_individual_id = value;}
		}

		[DataMember]
		public string  USER_ID
		{
			 get { return _user_id; }
			 set {_user_id = value;}
		}

		[DataMember]
		public string  BRANCH_CD
		{
			 get { return _branch_cd; }
			 set {_branch_cd = value;}
		}

		[DataMember]
		public int  ECONOMIC_ACTIVITY_CD
		{
			 get { return _economic_activity_cd; }
			 set {_economic_activity_cd = value;}
		}

		[DataMember]
		public int  SECOND_ECONOMIC_ACTIVITY_CD
		{
			 get { return _second_economic_activity_cd; }
			 set {_second_economic_activity_cd = value;}
		}

		[DataMember]
		public int  INTERVIEW_RESULT_CD
		{
			 get { return _interview_result_cd; }
			 set {_interview_result_cd = value;}
		}

		[DataMember]
		public bool  PENDING_EVENT
		{
			 get { return _pending_event; }
			 set {_pending_event = value;}
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
