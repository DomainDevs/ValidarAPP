using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class INDIVIDUAL_LINKKeys
	{

		#region Data Members

		int _individual_id;
		int _link_type_cd;
		int _relationship_sarlaft_cd;

		#endregion

		#region Constructor

		public INDIVIDUAL_LINKKeys()
		{
				
		}

		public INDIVIDUAL_LINKKeys(int iNDIVIDUAL_ID, int lINK_TYPE_CD, int rELATIONSHIP_SARLAFT_CD)
		{
			 _individual_id = iNDIVIDUAL_ID; 
			 _link_type_cd = lINK_TYPE_CD; 
			 _relationship_sarlaft_cd = rELATIONSHIP_SARLAFT_CD; 
		}

		#endregion

		#region Properties

		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
		}
		public int  LINK_TYPE_CD
		{
			 get { return _link_type_cd; }
		}
		public int  RELATIONSHIP_SARLAFT_CD
		{
			 get { return _relationship_sarlaft_cd; }
		}

		#endregion

	}
}
