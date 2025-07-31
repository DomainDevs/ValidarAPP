using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class AGENCY_COMMISS_CURRENCYKeys
	{

		#region Data Members

		int _currency_cd;
		int _agency_commiss_rate_id;

		#endregion

		#region Constructor

		public AGENCY_COMMISS_CURRENCYKeys()
		{
				
		}

		public AGENCY_COMMISS_CURRENCYKeys(int cURRENCY_CD, int aGENCY_COMMISS_RATE_ID)
		{
			 _currency_cd = cURRENCY_CD; 
			 _agency_commiss_rate_id = aGENCY_COMMISS_RATE_ID; 
		}

		#endregion

		#region Properties

		public int  CURRENCY_CD
		{
			 get { return _currency_cd; }
		}
		public int  AGENCY_COMMISS_RATE_ID
		{
			 get { return _agency_commiss_rate_id; }
		}

		#endregion

	}
}
