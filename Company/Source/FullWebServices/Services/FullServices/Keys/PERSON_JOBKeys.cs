using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class PERSON_JOBKeys
	{

		#region Data Members

		int _individual_id;

		#endregion

		#region Constructor

		public PERSON_JOBKeys()
		{
				
		}

		public PERSON_JOBKeys(int iNDIVIDUAL_ID)
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
