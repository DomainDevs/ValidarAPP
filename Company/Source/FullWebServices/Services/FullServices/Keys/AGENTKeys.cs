using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class AGENTKeys
	{

		#region Data Members

		int _individual_id;

		#endregion

		#region Constructor

		public AGENTKeys()
		{
				
		}

		public AGENTKeys(int iNDIVIDUAL_ID)
		{
			 _individual_id = iNDIVIDUAL_ID; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
		}

		#endregion

	}
}
