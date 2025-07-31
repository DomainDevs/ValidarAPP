using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class UNIQUE_USERSKeys
	{

		#region Data Members

		int _user_id;

		#endregion

		#region Constructor

		public UNIQUE_USERSKeys()
		{
				
		}

		public UNIQUE_USERSKeys(int uSER_ID)
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
