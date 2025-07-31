using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class INDIVIDUAL_SARLAFTKeys
	{

		#region Data Members

		int _sarlaft_id;

		#endregion

		#region Constructor

		public INDIVIDUAL_SARLAFTKeys()
		{
				
		}

		public INDIVIDUAL_SARLAFTKeys(int sARLAFT_ID)
		{
			 _sarlaft_id = sARLAFT_ID; 
		}

		#endregion

		#region Properties

		public int  SARLAFT_ID
		{
			 get { return _sarlaft_id; }
		}

		#endregion

	}
}
