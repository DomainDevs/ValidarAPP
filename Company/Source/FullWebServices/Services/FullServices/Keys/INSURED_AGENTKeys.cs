using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class INSURED_AGENTKeys
	{

		#region Data Members

		int _iNSURED_IND_ID;
		int _aGENT_IND_ID;
		int _aGENT_AGENCY_ID;

		#endregion

		#region Constructor

		public INSURED_AGENTKeys()
		{
				
		}

		public INSURED_AGENTKeys(int iNSURED_IND_ID, int aGENT_IND_ID, int aGENT_AGENCY_ID)
		{
			 _iNSURED_IND_ID = iNSURED_IND_ID; 
			 _aGENT_IND_ID = aGENT_IND_ID; 
			 _aGENT_AGENCY_ID = aGENT_AGENCY_ID; 
		}

		#endregion

		#region Properties

		public int  INSURED_IND_ID
		{
			 get { return _iNSURED_IND_ID; }
		}
		public int  AGENT_IND_ID
		{
			 get { return _aGENT_IND_ID; }
		}
		public int  AGENT_AGENCY_ID
		{
			 get { return _aGENT_AGENCY_ID; }
		}

		#endregion

	}
}
