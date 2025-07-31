using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class UNIQUE_USER_LOGIN_HISTORYKeys
	{

		#region Data Members

		int _user_id;
		string _change_date;

		#endregion

		#region Constructor

		public UNIQUE_USER_LOGIN_HISTORYKeys()
		{
				
		}

		public UNIQUE_USER_LOGIN_HISTORYKeys(int uSER_ID, string cHANGE_DATE)
		{
			 _user_id = uSER_ID; 
			 _change_date = cHANGE_DATE; 
		}

		#endregion

		#region Properties

		public int  USER_ID
		{
			 get { return _user_id; }
		}
		public string  CHANGE_DATE
		{
			 get { return _change_date; }
		}

		#endregion

	}
}
