using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class LOGBOOKKeys
	{

		#region Data Members

		decimal _id_logbook;

		#endregion

		#region Constructor

		public LOGBOOKKeys()
		{
				
		}

		public LOGBOOKKeys(decimal ID_LOGBOOK)
		{
            _id_logbook = ID_LOGBOOK; 
		}

		#endregion

		#region Properties

		public decimal  ID_LOGBOOK
		{
            get { return _id_logbook; }
		}

		#endregion

	}
}
