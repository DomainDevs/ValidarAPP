using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class CO_PRV_INDIVIDUALKeys
	{

		#region Data Members

		int _individual_id;

		#endregion

		#region Constructor

		public CO_PRV_INDIVIDUALKeys()
		{
				
		}

		public CO_PRV_INDIVIDUALKeys(int iNDIVIDUAL_ID)
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
