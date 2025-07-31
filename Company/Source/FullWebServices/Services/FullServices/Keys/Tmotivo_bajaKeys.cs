using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tmotivo_bajaKeys
	{

		#region Data Members

		decimal _cod_baja;

		#endregion

		#region Constructor

		public Tmotivo_bajaKeys()
		{
				
		}

		public Tmotivo_bajaKeys(decimal cod_baja)
		{
			 _cod_baja = cod_baja; 
		}

		#endregion

		#region Properties

		public decimal  cod_baja
		{
			 get { return _cod_baja; }
		}

		#endregion

	}
}
