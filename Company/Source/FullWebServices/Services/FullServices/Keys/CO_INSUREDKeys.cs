using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class CO_INSUREDKeys
	{

		#region Data Members

		int _iNDIVIDUAL_ID;

		#endregion

		#region Constructor

		public CO_INSUREDKeys()
		{
				
		}

		public CO_INSUREDKeys(int iNDIVIDUAL_ID)
		{
			 _iNDIVIDUAL_ID = iNDIVIDUAL_ID; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _iNDIVIDUAL_ID; }
		}

		#endregion

	}
}
