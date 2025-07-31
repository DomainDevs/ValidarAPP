using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TECHNICAL_CARDKeys
	{

		#region Data Members

		int _technical_card_id;

		#endregion

		#region Constructor

		public TECHNICAL_CARDKeys()
		{
				
		}

		public TECHNICAL_CARDKeys(int tECHNICAL_CARD_ID)
		{
			 _technical_card_id = tECHNICAL_CARD_ID; 
		}

		#endregion

		#region Properties

		public int  TECHNICAL_CARD_ID
		{
			 get { return _technical_card_id; }
		}

		#endregion

	}
}
