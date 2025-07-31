using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TtarifaKeys
	{

		#region Data Members

		decimal _cod_ramo;
		decimal _cod_subramo;
		decimal _cod_tarifa;

		#endregion

		#region Constructor

		public TtarifaKeys()
		{
				
		}

		public TtarifaKeys(decimal cod_ramo, decimal cod_subramo, decimal cod_tarifa)
		{
			 _cod_ramo = cod_ramo; 
			 _cod_subramo = cod_subramo; 
			 _cod_tarifa = cod_tarifa; 
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
		public decimal  cod_tarifa
		{
			 get { return _cod_tarifa; }
		}

		#endregion

	}
}
