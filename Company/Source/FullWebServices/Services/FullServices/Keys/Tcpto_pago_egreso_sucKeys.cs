using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tcpto_pago_egreso_sucKeys
	{

		#region Data Members

		decimal _cod_cpto;
		decimal _cod_suc;

		#endregion

		#region Constructor

		public Tcpto_pago_egreso_sucKeys()
		{
				
		}

		public Tcpto_pago_egreso_sucKeys(decimal cod_cpto, decimal cod_suc)
		{
			 _cod_cpto = cod_cpto; 
			 _cod_suc = cod_suc; 
		}

		#endregion

		#region Properties

		public decimal  cod_cpto
		{
			 get { return _cod_cpto; }
		}
		public decimal  cod_suc
		{
			 get { return _cod_suc; }
		}

		#endregion

	}
}
