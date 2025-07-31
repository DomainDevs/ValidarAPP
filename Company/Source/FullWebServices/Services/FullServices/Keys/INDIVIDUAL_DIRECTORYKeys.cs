using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class INDIVIDUAL_DIRECTORYKeys
	{

		#region Data Members

		int _iNDIVIDUAL_ID;
		int _dATA_ID;
		int _dIRECTORY_TYPE_CD;

		#endregion

		#region Constructor

		public INDIVIDUAL_DIRECTORYKeys()
		{
				
		}

		public INDIVIDUAL_DIRECTORYKeys(int iNDIVIDUAL_ID, int dATA_ID, int dIRECTORY_TYPE_CD)
		{
			 _iNDIVIDUAL_ID = iNDIVIDUAL_ID; 
			 _dATA_ID = dATA_ID; 
			 _dIRECTORY_TYPE_CD = dIRECTORY_TYPE_CD; 
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
		public int  DIRECTORY_TYPE_CD
		{
			 get { return _dIRECTORY_TYPE_CD; }
		}

		#endregion

	}
}
