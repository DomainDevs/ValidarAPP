using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class AGENCY_COMMISS_CURRENCY
	{

		#region InnerClass
		public enum AGENCY_COMMISS_CURRENCYFields
		{
			CURRENCY_CD,
			AGENCY_COMMISS_RATE_ID
		}
		#endregion

		#region Data Members

			int _currency_cd;
			int _agency_commiss_rate_id;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  CURRENCY_CD
		{
			 get { return _currency_cd; }
			 set {_currency_cd = value;}
		}

		[DataMember]
		public int  AGENCY_COMMISS_RATE_ID
		{
			 get { return _agency_commiss_rate_id; }
			 set {_agency_commiss_rate_id = value;}
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
