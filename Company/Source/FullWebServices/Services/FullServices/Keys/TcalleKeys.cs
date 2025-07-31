using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TcalleKeys
	{

		#region Data Members

		decimal _cod_calle;

		#endregion

		#region Constructor

		public TcalleKeys()
		{
				
		}

		public TcalleKeys(decimal cod_calle)
		{
			 _cod_calle = cod_calle; 
		}

		#endregion

		#region Properties

		public decimal  cod_calle
		{
			 get { return _cod_calle; }
		}

		#endregion

	}
}
