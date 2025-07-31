using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class AGENCY_COMMISS_RATE
	{

		#region InnerClass
		public enum AGENCY_COMMISS_RATEFields
		{
			AGENCY_COMMISS_RATE_ID,
			INDIVIDUAL_ID,
			AGENT_AGENCY_ID,
			PREFIX_CD,
			ST_COMMISS_PCT,
			LINE_BUSINESS_CD,
			SUB_LINE_BUSINESS_CD,
			ADDIT_COMMISS_PCT,
			SCH_COMMISS_PCT,
			ST_DIS_COMMISS_PCT,
			ADDIT_DIS_COMMISS_PCT,
			INC_COMMISS_AD_FAC_PCT,
			DIM_COMMISS_AD_FAC_PCT
		}
		#endregion

		#region Data Members

			int _agency_commiss_rate_id;
			int _individual_id;
			int _agent_agency_id;
			int _prefix_cd;
			double _st_commiss_pct;
			string _line_business_cd;
			string _sub_line_business_cd;
			string _addit_commiss_pct;
			string _sch_commiss_pct;
			string _st_dis_commiss_pct;
			string _addit_dis_commiss_pct;
			string _inc_commiss_ad_fac_pct;
			string _dim_commiss_ad_fac_pct;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  AGENCY_COMMISS_RATE_ID
		{
			 get { return _agency_commiss_rate_id; }
			 set {_agency_commiss_rate_id = value;}
		}

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
			 set {_individual_id = value;}
		}

		[DataMember]
		public int  AGENT_AGENCY_ID
		{
			 get { return _agent_agency_id; }
			 set {_agent_agency_id = value;}
		}

		[DataMember]
		public int  PREFIX_CD
		{
			 get { return _prefix_cd; }
			 set {_prefix_cd = value;}
		}

		[DataMember]
		public double  ST_COMMISS_PCT
		{
			 get { return _st_commiss_pct; }
			 set {_st_commiss_pct = value;}
		}

		[DataMember]
		public string  LINE_BUSINESS_CD
		{
			 get { return _line_business_cd; }
			 set {_line_business_cd = value;}
		}

		[DataMember]
		public string  SUB_LINE_BUSINESS_CD
		{
			 get { return _sub_line_business_cd; }
			 set {_sub_line_business_cd = value;}
		}

		[DataMember]
		public string  ADDIT_COMMISS_PCT
		{
			 get { return _addit_commiss_pct; }
			 set {_addit_commiss_pct = value;}
		}

		[DataMember]
		public string  SCH_COMMISS_PCT
		{
			 get { return _sch_commiss_pct; }
			 set {_sch_commiss_pct = value;}
		}

		[DataMember]
		public string  ST_DIS_COMMISS_PCT
		{
			 get { return _st_dis_commiss_pct; }
			 set {_st_dis_commiss_pct = value;}
		}

		[DataMember]
		public string  ADDIT_DIS_COMMISS_PCT
		{
			 get { return _addit_dis_commiss_pct; }
			 set {_addit_dis_commiss_pct = value;}
		}

		[DataMember]
		public string  INC_COMMISS_AD_FAC_PCT
		{
			 get { return _inc_commiss_ad_fac_pct; }
			 set {_inc_commiss_ad_fac_pct = value;}
		}

		[DataMember]
		public string  DIM_COMMISS_AD_FAC_PCT
		{
			 get { return _dim_commiss_ad_fac_pct; }
			 set {_dim_commiss_ad_fac_pct = value;}
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
