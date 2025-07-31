using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class PERSON_INTEREST_GROUPKeys
	{

		#region Data Members

		int _iNDIVIDUAL_ID;
		int _iNTEREST_GROUP_TYPE_CD;

		#endregion

		#region Constructor

		public PERSON_INTEREST_GROUPKeys()
		{
				
		}

		public PERSON_INTEREST_GROUPKeys(int iNDIVIDUAL_ID, int iNTEREST_GROUP_TYPE_CD)
		{
			 _iNDIVIDUAL_ID = iNDIVIDUAL_ID; 
			 _iNTEREST_GROUP_TYPE_CD = iNTEREST_GROUP_TYPE_CD; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _iNDIVIDUAL_ID; }
		}
		public int  INTEREST_GROUP_TYPE_CD
		{
			 get { return _iNTEREST_GROUP_TYPE_CD; }
		}

		#endregion

	}
}
