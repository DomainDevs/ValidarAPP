using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class CO_EQUIVALENCE_INSURED_3GKeys
	{

		#region Data Members

		int _individual_2g_id;

		#endregion

		#region Constructor

		public CO_EQUIVALENCE_INSURED_3GKeys()
		{
				
		}

		public CO_EQUIVALENCE_INSURED_3GKeys(int iNDIVIDUAL_2G_ID)
		{
			 _individual_2g_id = iNDIVIDUAL_2G_ID; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_2G_ID
		{
			 get { return _individual_2g_id; }
		}

		#endregion

	}
}
