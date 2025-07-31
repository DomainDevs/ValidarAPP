using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class TECHNICAL_CARD
	{

		#region InnerClass
		public enum TECHNICAL_CARDFields
		{
			TECHNICAL_CARD_ID,
			INDIVIDUAL_ID,
			EXPERIENCE_TYPE_CD,
			AUTHORIZED_CAPITAL_AMT,
			CURRENT_PILE_AMT,
			ENROLLMENT_NUM,
			ENROLLMENT_FROM,
			ENROLLMENT_TO,
			TECHNICAL_CARD_LOCATION,
			TAX_INSPECTOR,
			CORPORATE_PURPOSE,
			REFERENCES,
			FINANCIAL_CONCEPT,
			PILE_DESCRIPTION,
			EXPERIENCE,
			REGISTRATION_DATE,
			REGISTERED_USER_ID
		}
		#endregion

		#region Data Members

			int _technical_card_id;
			int _individual_id;
			int _experience_type_cd;
			double _authorized_capital_amt;
			string _current_pile_amt;
			string _enrollment_num;
			string _enrollment_from;
			string _enrollment_to;
			string _technical_card_location;
			string _tax_inspector;
			string _corporate_purpose;
			string _references;
			string _financial_concept;
			string _pile_description;
			string _experience;
			string _registration_date;
			string _registered_user_id;
			int _identity; 
			char _state;
            char _state_3G;
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  TECHNICAL_CARD_ID
		{
			 get { return _technical_card_id; }
			 set {_technical_card_id = value;}
		}

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
			 set {_individual_id = value;}
		}

		[DataMember]
		public int  EXPERIENCE_TYPE_CD
		{
			 get { return _experience_type_cd; }
			 set {_experience_type_cd = value;}
		}

		[DataMember]
		public double  AUTHORIZED_CAPITAL_AMT
		{
			 get { return _authorized_capital_amt; }
			 set {_authorized_capital_amt = value;}
		}

		[DataMember]
		public string  CURRENT_PILE_AMT
		{
			 get { return _current_pile_amt; }
			 set {_current_pile_amt = value;}
		}

		[DataMember]
		public string  ENROLLMENT_NUM
		{
			 get { return _enrollment_num; }
			 set {_enrollment_num = value;}
		}

		[DataMember]
		public string  ENROLLMENT_FROM
		{
			 get { return _enrollment_from; }
			 set {_enrollment_from = value;}
		}

		[DataMember]
		public string  ENROLLMENT_TO
		{
			 get { return _enrollment_to; }
			 set {_enrollment_to = value;}
		}

		[DataMember]
		public string  TECHNICAL_CARD_LOCATION
		{
			 get { return _technical_card_location; }
			 set {_technical_card_location = value;}
		}

		[DataMember]
		public string  TAX_INSPECTOR
		{
			 get { return _tax_inspector; }
			 set {_tax_inspector = value;}
		}

		[DataMember]
		public string  CORPORATE_PURPOSE
		{
			 get { return _corporate_purpose; }
			 set {_corporate_purpose = value;}
		}

		[DataMember]
		public string  REFERENCES
		{
			 get { return _references; }
			 set {_references = value;}
		}

		[DataMember]
		public string  FINANCIAL_CONCEPT
		{
			 get { return _financial_concept; }
			 set {_financial_concept = value;}
		}

		[DataMember]
		public string  PILE_DESCRIPTION
		{
			 get { return _pile_description; }
			 set {_pile_description = value;}
		}

		[DataMember]
		public string  EXPERIENCE
		{
			 get { return _experience; }
			 set {_experience = value;}
		}

		[DataMember]
		public string  REGISTRATION_DATE
		{
			 get { return _registration_date; }
			 set {_registration_date = value;}
		}

		[DataMember]
		public string  REGISTERED_USER_ID
		{
			 get { return _registered_user_id; }
			 set {_registered_user_id = value;}
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

        [DataMember]
        public char State_3G
        {
            get { return _state_3G; }
            set { _state_3G = value; }
        }
		#endregion

	}
}
