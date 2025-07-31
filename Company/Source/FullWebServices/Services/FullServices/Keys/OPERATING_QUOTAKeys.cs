using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class OPERATING_QUOTAKeys
	{

		#region Data Members

		int _individual_id;
		int _line_business_cd;
		int _currency_cd;

		#endregion

		#region Constructor

		public OPERATING_QUOTAKeys()
		{
				
		}

		public OPERATING_QUOTAKeys(int iNDIVIDUAL_ID, int lINE_BUSINESS_CD, int cURRENCY_CD)
		{
			 _individual_id = iNDIVIDUAL_ID; 
			 _line_business_cd = lINE_BUSINESS_CD; 
			 _currency_cd = cURRENCY_CD; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
		}
		public int  LINE_BUSINESS_CD
		{
			 get { return _line_business_cd; }
		}
		public int  CURRENCY_CD
		{
			 get { return _currency_cd; }
		}

		#endregion

	}
}
