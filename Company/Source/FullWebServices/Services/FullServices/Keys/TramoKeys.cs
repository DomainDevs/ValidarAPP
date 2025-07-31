using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TramoKeys
	{

		#region Data Members

		decimal _cod_ramo;

		#endregion

		#region Constructor

		public TramoKeys()
		{
				
		}

		public TramoKeys(decimal cod_ramo)
		{
			 _cod_ramo = cod_ramo; 
		}

		#endregion

		#region Properties

		public decimal  cod_ramo
		{
			 get { return _cod_ramo; }
		}

		#endregion

	}
}
