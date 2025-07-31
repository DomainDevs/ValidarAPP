using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class FINANCIAL_SARLAFT
	{

		#region InnerClass
		public enum FINANCIAL_SARLAFTFields
		{
			SARLAFT_ID,
			INCOME_AMT,
			EXPENSE_AMT,
			EXTRA_INCOME_AMT,
			ASSETS_AMT,
			LIABILITIES_AMT,
			DESCRIPTION
		}
		#endregion

		#region Data Members

			int _sarlaft_id;
			double _income_amt;
			double _expense_amt;
			double _extra_income_amt;
			double _assets_amt;
			double _liabilities_amt;
			string _description;
			int _identity; 
			char _state; 
			string _connection;
            int _iNDIVIDUAL_ID;
		#endregion

		#region Properties

		[DataMember]
		public int  SARLAFT_ID
		{
			 get { return _sarlaft_id; }
			 set {_sarlaft_id = value;}
		}

		[DataMember]
		public double  INCOME_AMT
		{
			 get { return _income_amt; }
			 set {_income_amt = value;}
		}

		[DataMember]
		public double  EXPENSE_AMT
		{
			 get { return _expense_amt; }
			 set {_expense_amt = value;}
		}

		[DataMember]
		public double  EXTRA_INCOME_AMT
		{
			 get { return _extra_income_amt; }
			 set {_extra_income_amt = value;}
		}

		[DataMember]
		public double  ASSETS_AMT
		{
			 get { return _assets_amt; }
			 set {_assets_amt = value;}
		}

		[DataMember]
		public double  LIABILITIES_AMT
		{
			 get { return _liabilities_amt; }
			 set {_liabilities_amt = value;}
		}

		[DataMember]
		public string  DESCRIPTION
		{
			 get { return _description; }
			 set {_description = value;}
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
        public int INDIVIDUAL_ID
        {
            get { return _iNDIVIDUAL_ID; }
            set { _iNDIVIDUAL_ID = value; }
        }
		#endregion

	}
}
