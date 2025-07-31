using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class PAYMENT_METHOD_CARDKeys
	{

		#region Data Members

		int _iNDIVIDUAL_ID;
		int _pAYMENT_ID;

		#endregion

		#region Constructor

		public PAYMENT_METHOD_CARDKeys()
		{
				
		}

		public PAYMENT_METHOD_CARDKeys(int iNDIVIDUAL_ID, int pAYMENT_ID)
		{
			 _iNDIVIDUAL_ID = iNDIVIDUAL_ID; 
			 _pAYMENT_ID = pAYMENT_ID; 
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

		#endregion

	}
}
