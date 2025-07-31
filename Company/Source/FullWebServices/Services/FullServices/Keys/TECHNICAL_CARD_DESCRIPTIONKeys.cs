using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TECHNICAL_CARD_DESCRIPTIONKeys
	{

		#region Data Members

		int _technical_card_description_cd;
		int _technical_card_id;

		#endregion

		#region Constructor

		public TECHNICAL_CARD_DESCRIPTIONKeys()
		{
				
		}

		public TECHNICAL_CARD_DESCRIPTIONKeys(int tECHNICAL_CARD_DESCRIPTION_CD, int tECHNICAL_CARD_ID)
		{
			 _technical_card_description_cd = tECHNICAL_CARD_DESCRIPTION_CD; 
			 _technical_card_id = tECHNICAL_CARD_ID; 
		}

		#endregion

		#region Properties

		public int  TECHNICAL_CARD_DESCRIPTION_CD
		{
			 get { return _technical_card_description_cd; }
		}
		public int  TECHNICAL_CARD_ID
		{
			 get { return _technical_card_id; }
		}

		#endregion

	}
}
