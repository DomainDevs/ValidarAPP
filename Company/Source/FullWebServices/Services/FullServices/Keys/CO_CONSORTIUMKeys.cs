using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class CO_CONSORTIUMKeys
	{

		#region Data Members

		int _iNSURED_CD;
		int _iNDIVIDUAL_ID;

		#endregion

		#region Constructor

		public CO_CONSORTIUMKeys()
		{
				
		}

		public CO_CONSORTIUMKeys(int iNSURED_CD, int iNDIVIDUAL_ID)
		{
			 _iNSURED_CD = iNSURED_CD; 
			 _iNDIVIDUAL_ID = iNDIVIDUAL_ID; 
		}

		#endregion

		#region Properties

		public int  INSURED_CD
		{
			 get { return _iNSURED_CD; }
		}
		public int  INDIVIDUAL_ID
		{
			 get { return _iNDIVIDUAL_ID; }
		}

		#endregion

	}
}
