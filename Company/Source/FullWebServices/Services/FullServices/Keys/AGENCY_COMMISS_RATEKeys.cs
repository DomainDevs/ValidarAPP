using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class AGENCY_COMMISS_RATEKeys
	{

		#region Data Members

		int _agency_commiss_rate_id;

		#endregion

		#region Constructor

		public AGENCY_COMMISS_RATEKeys()
		{
				
		}

		public AGENCY_COMMISS_RATEKeys(int aGENCY_COMMISS_RATE_ID)
		{
			 _agency_commiss_rate_id = aGENCY_COMMISS_RATE_ID; 
		}

		#endregion

		#region Properties

		public int  AGENCY_COMMISS_RATE_ID
		{
			 get { return _agency_commiss_rate_id; }
		}

		#endregion

	}
}
