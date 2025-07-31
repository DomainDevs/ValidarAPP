using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class AGENT_AGENCYKeys
	{

		#region Data Members

		int _individual_id;
		int _agent_agency_id;

		#endregion

		#region Constructor

		public AGENT_AGENCYKeys()
		{
				
		}

		public AGENT_AGENCYKeys(int iNDIVIDUAL_ID, int aGENT_AGENCY_ID)
		{
			 _individual_id = iNDIVIDUAL_ID; 
			 _agent_agency_id = aGENT_AGENCY_ID; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
		}
		public int  AGENT_AGENCY_ID
		{
			 get { return _agent_agency_id; }
		}

		#endregion

	}
}
