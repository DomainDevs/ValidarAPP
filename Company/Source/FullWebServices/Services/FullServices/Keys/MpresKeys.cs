using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class MpresKeys
	{

		#region Data Members

		decimal _cod_pres;

		#endregion

		#region Constructor

		public MpresKeys()
		{
				
		}

		public MpresKeys(decimal cod_pres)
		{
			 _cod_pres = cod_pres; 
		}

		#endregion

		#region Properties

		public decimal  cod_pres
		{
			 get { return _cod_pres; }
		}

		#endregion

	}
}
