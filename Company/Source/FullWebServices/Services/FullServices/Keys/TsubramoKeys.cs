using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TsubramoKeys
	{

		#region Data Members

		decimal _cod_ramo;
		decimal _cod_subramo;

		#endregion

		#region Constructor

		public TsubramoKeys()
		{
				
		}

		public TsubramoKeys(decimal cod_ramo, decimal cod_subramo)
		{
			 _cod_ramo = cod_ramo; 
			 _cod_subramo = cod_subramo; 
		}

		#endregion

		#region Properties

		public decimal  cod_ramo
		{
			 get { return _cod_ramo; }
		}
		public decimal  cod_subramo
		{
			 get { return _cod_subramo; }
		}

		#endregion

	}
}
