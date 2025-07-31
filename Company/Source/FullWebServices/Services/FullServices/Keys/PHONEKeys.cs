using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class PHONEKeys
	{

		#region Data Members

		int _individual_id;
		int _data_id;

		#endregion

		#region Constructor

		public PHONEKeys()
		{
				
		}

		public PHONEKeys(int iNDIVIDUAL_ID, int dATA_ID)
		{
			 _individual_id = iNDIVIDUAL_ID; 
			 _data_id = dATA_ID; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
		}
		public int  DATA_ID
		{
			 get { return _data_id; }
		}

		#endregion

	}
}
