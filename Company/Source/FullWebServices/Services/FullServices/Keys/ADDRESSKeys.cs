using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class ADDRESSKeys
	{

		#region Data Members

		int _iNDIVIDUAL_ID;
		int _dATA_ID;

		#endregion

		#region Constructor

		public ADDRESSKeys()
		{
				
		}

		public ADDRESSKeys(int iNDIVIDUAL_ID, int dATA_ID)
		{
			 _iNDIVIDUAL_ID = iNDIVIDUAL_ID; 
			 _dATA_ID = dATA_ID; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _iNDIVIDUAL_ID; }
		}
		public int  DATA_ID
		{
			 get { return _dATA_ID; }
		}

		#endregion

	}
}
