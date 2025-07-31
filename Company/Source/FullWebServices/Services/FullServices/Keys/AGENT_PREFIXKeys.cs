using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class AGENT_PREFIXKeys
	{

		#region Data Members

		int _individual_id;
		int _prefix_cd;

		#endregion

		#region Constructor

		public AGENT_PREFIXKeys()
		{
				
		}

		public AGENT_PREFIXKeys(int iNDIVIDUAL_ID, int pREFIX_CD)
		{
			 _individual_id = iNDIVIDUAL_ID; 
			 _prefix_cd = pREFIX_CD; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
		}
		public int  PREFIX_CD
		{
			 get { return _prefix_cd; }
		}

		#endregion

	}
}
