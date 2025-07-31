using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class SARLAFT_OPERATIONKeys
	{

		#region Data Members

		int _sarlaft_operation_id;
		int _sarlaft_id;

		#endregion

		#region Constructor

		public SARLAFT_OPERATIONKeys()
		{
				
		}

		public SARLAFT_OPERATIONKeys(int sARLAFT_OPERATION_ID, int sARLAFT_ID)
		{
			 _sarlaft_operation_id = sARLAFT_OPERATION_ID; 
			 _sarlaft_id = sARLAFT_ID; 
		}

		#endregion

		#region Properties

		public int  SARLAFT_OPERATION_ID
		{
			 get { return _sarlaft_operation_id; }
		}
		public int  SARLAFT_ID
		{
			 get { return _sarlaft_id; }
		}

		#endregion

	}
}
