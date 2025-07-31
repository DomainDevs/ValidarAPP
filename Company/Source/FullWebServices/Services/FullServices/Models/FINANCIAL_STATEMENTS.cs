using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class FINANCIAL_STATEMENTS
	{

		#region InnerClass
		public enum FINANCIAL_STATEMENTSFields
		{
			TECHNICAL_CARD_ID,
			BALANCE_DATE,
			INVENTORY_AMT,
			ACCOUNTS_RECEIVABLE_AMT,
			CASH_INVESTMENT_TEMPORARY_AMT,
			CURRENT_ASSETS_AMT,
			FIXED_GROSS_ASSETS_AMT,
			FIXED_NET_ASSETS_AMT,
			VALUATION_AMT,
			ASSETS_AMT,
			CURRENT_LIABILITIES_AMT,
			LONG_TERM_LIABILITIES_AMT,
			LIABILITIES_AMT,
			CAPITAL_AMT,
			REVALUATION_AMT,
			SURPLUS_VALUE_AMT,
			OTHERS_SURPLUS_AMT,
			RESERVES_AMT,
			PREMIUM_AMT,
			ACCUMULATED_PROFIT_AMT,
			PATRIMONY_AMT,
			NET_SALES_AMT,
			SALES_COST_AMT,
			GROSS_PROFIT_AMT,
			OPERATING_PROFIT_AMT,
			NET_PROFIT_AMT,
			OTHERS_INCOME_AMT,
			INFLATION_ADJUSTMENTS_AMT,
			INTERESTS_AMT,
			OTHERS_EXPENSE_AMT,
			REGISTRATION_BALANCE_DATE,
			BALANCE_USER_ID
		}
		#endregion

		#region Data Members

			int _technical_card_id;
			string _balance_date;
			double _inventory_amt;
			double _accounts_receivable_amt;
			double _cash_investment_temporary_amt;
			double _current_assets_amt;
			double _fixed_gross_assets_amt;
			double _fixed_net_assets_amt;
			double _valuation_amt;
			double _assets_amt;
			double _current_liabilities_amt;
			double _long_term_liabilities_amt;
			double _liabilities_amt;
			double _capital_amt;
			double _revaluation_amt;
			double _surplus_value_amt;
			double _others_surplus_amt;
			double _reserves_amt;
			double _premium_amt;
			double _accumulated_profit_amt;
			double _patrimony_amt;
			double _net_sales_amt;
			double _sales_cost_amt;
			double _gross_profit_amt;
			double _operating_profit_amt;
			double _net_profit_amt;
			double _others_income_amt;
			double _inflation_adjustments_amt;
			double _interests_amt;
			double _others_expense_amt;
			string _registration_balance_date;
			int _balance_user_id;
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
		public string  BALANCE_DATE
		{
			 get { return _balance_date; }
			 set {_balance_date = value;}
		}

		[DataMember]
		public double  INVENTORY_AMT
		{
			 get { return _inventory_amt; }
			 set {_inventory_amt = value;}
		}

		[DataMember]
		public double  ACCOUNTS_RECEIVABLE_AMT
		{
			 get { return _accounts_receivable_amt; }
			 set {_accounts_receivable_amt = value;}
		}

		[DataMember]
		public double  CASH_INVESTMENT_TEMPORARY_AMT
		{
			 get { return _cash_investment_temporary_amt; }
			 set {_cash_investment_temporary_amt = value;}
		}

		[DataMember]
		public double  CURRENT_ASSETS_AMT
		{
			 get { return _current_assets_amt; }
			 set {_current_assets_amt = value;}
		}

		[DataMember]
		public double  FIXED_GROSS_ASSETS_AMT
		{
			 get { return _fixed_gross_assets_amt; }
			 set {_fixed_gross_assets_amt = value;}
		}

		[DataMember]
		public double  FIXED_NET_ASSETS_AMT
		{
			 get { return _fixed_net_assets_amt; }
			 set {_fixed_net_assets_amt = value;}
		}

		[DataMember]
		public double  VALUATION_AMT
		{
			 get { return _valuation_amt; }
			 set {_valuation_amt = value;}
		}

		[DataMember]
		public double  ASSETS_AMT
		{
			 get { return _assets_amt; }
			 set {_assets_amt = value;}
		}

		[DataMember]
		public double  CURRENT_LIABILITIES_AMT
		{
			 get { return _current_liabilities_amt; }
			 set {_current_liabilities_amt = value;}
		}

		[DataMember]
		public double  LONG_TERM_LIABILITIES_AMT
		{
			 get { return _long_term_liabilities_amt; }
			 set {_long_term_liabilities_amt = value;}
		}

		[DataMember]
		public double  LIABILITIES_AMT
		{
			 get { return _liabilities_amt; }
			 set {_liabilities_amt = value;}
		}

		[DataMember]
		public double  CAPITAL_AMT
		{
			 get { return _capital_amt; }
			 set {_capital_amt = value;}
		}

		[DataMember]
		public double  REVALUATION_AMT
		{
			 get { return _revaluation_amt; }
			 set {_revaluation_amt = value;}
		}

		[DataMember]
		public double  SURPLUS_VALUE_AMT
		{
			 get { return _surplus_value_amt; }
			 set {_surplus_value_amt = value;}
		}

		[DataMember]
		public double  OTHERS_SURPLUS_AMT
		{
			 get { return _others_surplus_amt; }
			 set {_others_surplus_amt = value;}
		}

		[DataMember]
		public double  RESERVES_AMT
		{
			 get { return _reserves_amt; }
			 set {_reserves_amt = value;}
		}

		[DataMember]
		public double  PREMIUM_AMT
		{
			 get { return _premium_amt; }
			 set {_premium_amt = value;}
		}

		[DataMember]
		public double  ACCUMULATED_PROFIT_AMT
		{
			 get { return _accumulated_profit_amt; }
			 set {_accumulated_profit_amt = value;}
		}

		[DataMember]
		public double  PATRIMONY_AMT
		{
			 get { return _patrimony_amt; }
			 set {_patrimony_amt = value;}
		}

		[DataMember]
		public double  NET_SALES_AMT
		{
			 get { return _net_sales_amt; }
			 set {_net_sales_amt = value;}
		}

		[DataMember]
		public double  SALES_COST_AMT
		{
			 get { return _sales_cost_amt; }
			 set {_sales_cost_amt = value;}
		}

		[DataMember]
		public double  GROSS_PROFIT_AMT
		{
			 get { return _gross_profit_amt; }
			 set {_gross_profit_amt = value;}
		}

		[DataMember]
		public double  OPERATING_PROFIT_AMT
		{
			 get { return _operating_profit_amt; }
			 set {_operating_profit_amt = value;}
		}

		[DataMember]
		public double  NET_PROFIT_AMT
		{
			 get { return _net_profit_amt; }
			 set {_net_profit_amt = value;}
		}

		[DataMember]
		public double  OTHERS_INCOME_AMT
		{
			 get { return _others_income_amt; }
			 set {_others_income_amt = value;}
		}

		[DataMember]
		public double  INFLATION_ADJUSTMENTS_AMT
		{
			 get { return _inflation_adjustments_amt; }
			 set {_inflation_adjustments_amt = value;}
		}

		[DataMember]
		public double  INTERESTS_AMT
		{
			 get { return _interests_amt; }
			 set {_interests_amt = value;}
		}

		[DataMember]
		public double  OTHERS_EXPENSE_AMT
		{
			 get { return _others_expense_amt; }
			 set {_others_expense_amt = value;}
		}

		[DataMember]
		public string  REGISTRATION_BALANCE_DATE
		{
			 get { return _registration_balance_date; }
			 set {_registration_balance_date = value;}
		}

		[DataMember]
		public int  BALANCE_USER_ID
		{
			 get { return _balance_user_id; }
			 set {_balance_user_id = value;}
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
