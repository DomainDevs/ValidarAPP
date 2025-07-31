using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class INDIVIDUAL_PAYMENT_METHODKeys
	{

		#region Data Members

		int _iNDIVIDUAL_ID;
		int _pAYMENT_ID;
		int _rOLE_CD;

		#endregion

		#region Constructor

		public INDIVIDUAL_PAYMENT_METHODKeys()
		{
				
		}

		public INDIVIDUAL_PAYMENT_METHODKeys(int iNDIVIDUAL_ID, int pAYMENT_ID, int rOLE_CD)
		{
			 _iNDIVIDUAL_ID = iNDIVIDUAL_ID; 
			 _pAYMENT_ID = pAYMENT_ID; 
			 _rOLE_CD = rOLE_CD; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _iNDIVIDUAL_ID; }
		}
		public int  PAYMENT_ID
		{
			 get { return _pAYMENT_ID; }
		}
		public int  ROLE_CD
		{
			 get { return _rOLE_CD; }
		}

		#endregion

	}
}
