using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TmonedaKeys
	{

		#region Data Members

		decimal _cod_moneda;

		#endregion

		#region Constructor

		public TmonedaKeys()
		{
				
		}

		public TmonedaKeys(decimal cod_moneda)
		{
			 _cod_moneda = cod_moneda; 
		}

		#endregion

		#region Properties

		public decimal  cod_moneda
		{
			 get { return _cod_moneda; }
		}

		#endregion

	}
}
