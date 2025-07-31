using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class OPERATING_QUOTA
	{

		#region InnerClass
		public enum OPERATING_QUOTAFields
		{
			INDIVIDUAL_ID,
			LINE_BUSINESS_CD,
			CURRENCY_CD,
			OPERATING_QUOTA_AMT,
			CURRENT_TO
		}
		#endregion

		#region Data Members

			int _individual_id;
			int _line_business_cd;
			int _currency_cd;
			double _operating_quota_amt;
			string _current_to;
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
		public int  LINE_BUSINESS_CD
		{
			 get { return _line_business_cd; }
			 set {_line_business_cd = value;}
		}

		[DataMember]
		public int  CURRENCY_CD
		{
			 get { return _currency_cd; }
			 set {_currency_cd = value;}
		}

		[DataMember]
		public double  OPERATING_QUOTA_AMT
		{
			 get { return _operating_quota_amt; }
			 set {_operating_quota_amt = value;}
		}

		[DataMember]
		public string  CURRENT_TO
		{
			 get { return _current_to; }
			 set {_current_to = value;}
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
