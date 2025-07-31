using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class UNIQUE_USER_LOGINKeys
	{

		#region Data Members

		int _user_id;

		#endregion

		#region Constructor

		public UNIQUE_USER_LOGINKeys()
		{
				
		}

		public UNIQUE_USER_LOGINKeys(int uSER_ID)
		{
			 _user_id = uSER_ID; 
		}

		#endregion

		#region Properties

		public int  USER_ID
		{
			 get { return _user_id; }
		}

		#endregion

	}
}
