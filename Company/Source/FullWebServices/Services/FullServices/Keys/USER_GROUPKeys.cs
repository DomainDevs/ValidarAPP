using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class USER_GROUPKeys
	{

		#region Data Members

		int _user_id;
		int _group_cd;

		#endregion

		#region Constructor

		public USER_GROUPKeys()
		{
				
		}

		public USER_GROUPKeys(int uSER_ID, int gROUP_CD)
		{
			 _user_id = uSER_ID; 
			 _group_cd = gROUP_CD; 
		}

		#endregion

		#region Properties

		public int  USER_ID
		{
			 get { return _user_id; }
		}
		public int  GROUP_CD
		{
			 get { return _group_cd; }
		}

		#endregion

	}
}
