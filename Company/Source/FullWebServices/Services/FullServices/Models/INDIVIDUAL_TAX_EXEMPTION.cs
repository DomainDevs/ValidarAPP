using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INDIVIDUAL_TAX_EXEMPTION
	{

		#region InnerClass
		public enum INDIVIDUAL_TAX_EXEMPTIONFields
		{
			IND_TAX_EXEMPTION_ID,
			INDIVIDUAL_ID,
			TAX_CD,
			EXEMPTION_PCT,
			CURRENT_FROM,
			CURRENT_TO,
			TAX_CATEGORY_CD,
			STATE_CD,
			COUNTRY_CD,
			BULLETIN_DATE,
			RESOLUTION_NUMBER,
			HAS_FULL_RETENTION
		}
		#endregion

		#region Data Members

			int _ind_tax_exemption_id;
			int _individual_id;
			int _tax_cd;
			double _exemption_pct;
			string _current_from;
			string _current_to;
			string _tax_category_cd;
			string _state_cd;
			string _country_cd;
			string _bulletin_date;
			string _resolution_number;
			bool _has_full_retention;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  IND_TAX_EXEMPTION_ID
		{
			 get { return _ind_tax_exemption_id; }
			 set {_ind_tax_exemption_id = value;}
		}

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
			 set {_individual_id = value;}
		}

		[DataMember]
		public int  TAX_CD
		{
			 get { return _tax_cd; }
			 set {_tax_cd = value;}
		}

		[DataMember]
		public double  EXEMPTION_PCT
		{
			 get { return _exemption_pct; }
			 set {_exemption_pct = value;}
		}

		[DataMember]
		public string  CURRENT_FROM
		{
			 get { return _current_from; }
			 set {_current_from = value;}
		}

		[DataMember]
		public string  CURRENT_TO
		{
			 get { return _current_to; }
			 set {_current_to = value;}
		}

		[DataMember]
		public string  TAX_CATEGORY_CD
		{
			 get { return _tax_category_cd; }
			 set {_tax_category_cd = value;}
		}

		[DataMember]
		public string  STATE_CD
		{
			 get { return _state_cd; }
			 set {_state_cd = value;}
		}

		[DataMember]
		public string  COUNTRY_CD
		{
			 get { return _country_cd; }
			 set {_country_cd = value;}
		}

		[DataMember]
		public string  BULLETIN_DATE
		{
			 get { return _bulletin_date; }
			 set {_bulletin_date = value;}
		}

		[DataMember]
		public string  RESOLUTION_NUMBER
		{
			 get { return _resolution_number; }
			 set {_resolution_number = value;}
		}

		[DataMember]
		public bool  HAS_FULL_RETENTION
		{
			 get { return _has_full_retention; }
			 set {_has_full_retention = value;}
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
