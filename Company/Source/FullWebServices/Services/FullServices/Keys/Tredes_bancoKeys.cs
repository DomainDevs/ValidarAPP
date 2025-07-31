using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tredes_bancoKeys
	{

		#region Data Members

		decimal _cod_red;

		#endregion

		#region Constructor

		public Tredes_bancoKeys()
		{
				
		}

		public Tredes_bancoKeys(decimal cod_red)
		{
			 _cod_red = cod_red; 
		}

		#endregion

		#region Properties

		public decimal  cod_red
		{
			 get { return _cod_red; }
		}

		#endregion

	}
}
