using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class INTEGRATION_SARLAFTKeys
	{

		#region Data Members

		int _id_form;

		#endregion

		#region Constructor

		public INTEGRATION_SARLAFTKeys()
		{
				
		}

		public INTEGRATION_SARLAFTKeys(int iD_FORM)
		{
			 _id_form = iD_FORM; 
		}

		#endregion

		#region Properties

		public int  ID_FORM
		{
			 get { return _id_form; }
		}

		#endregion

	}
}
