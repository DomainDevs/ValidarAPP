using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class FINANCIAL_STATEMENTSKeys
	{

		#region Data Members

		int _technical_card_id;
		string _balance_date;

		#endregion

		#region Constructor

		public FINANCIAL_STATEMENTSKeys()
		{
				
		}

		public FINANCIAL_STATEMENTSKeys(int tECHNICAL_CARD_ID, string bALANCE_DATE)
		{
			 _technical_card_id = tECHNICAL_CARD_ID; 
			 _balance_date = bALANCE_DATE; 
		}

		#endregion

		#region Properties

		public int  TECHNICAL_CARD_ID
		{
			 get { return _technical_card_id; }
		}
		public string  BALANCE_DATE
		{
			 get { return _balance_date; }
		}

		#endregion

	}
}
