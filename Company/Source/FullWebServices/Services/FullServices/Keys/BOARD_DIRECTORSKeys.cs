using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class BOARD_DIRECTORSKeys
	{

		#region Data Members

		int _board_directors_cd;
		int _technical_card_id;

		#endregion

		#region Constructor

		public BOARD_DIRECTORSKeys()
		{
				
		}

		public BOARD_DIRECTORSKeys(int bOARD_DIRECTORS_CD, int tECHNICAL_CARD_ID)
		{
			 _board_directors_cd = bOARD_DIRECTORS_CD; 
			 _technical_card_id = tECHNICAL_CARD_ID; 
		}

		#endregion

		#region Properties

		public int  BOARD_DIRECTORS_CD
		{
			 get { return _board_directors_cd; }
		}
		public int  TECHNICAL_CARD_ID
		{
			 get { return _technical_card_id; }
		}

		#endregion

	}
}
