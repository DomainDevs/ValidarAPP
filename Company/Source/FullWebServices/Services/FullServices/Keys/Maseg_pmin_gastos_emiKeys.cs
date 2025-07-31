using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_pmin_gastos_emiKeys
	{

		#region Data Members

		int _cod_aseg;
		decimal _cod_ramo;
		decimal _cod_moneda;

		#endregion

		#region Constructor

		public Maseg_pmin_gastos_emiKeys()
		{
				
		}

		public Maseg_pmin_gastos_emiKeys(int cod_aseg, decimal cod_ramo, decimal cod_moneda)
		{
			 _cod_aseg = cod_aseg; 
			 _cod_ramo = cod_ramo; 
			 _cod_moneda = cod_moneda; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}
		public decimal  cod_ramo
		{
			 get { return _cod_ramo; }
		}
		public decimal  cod_moneda
		{
			 get { return _cod_moneda; }
		}

		#endregion

	}
}
