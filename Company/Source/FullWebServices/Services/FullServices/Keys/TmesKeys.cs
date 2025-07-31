using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TmesKeys
	{

		#region Data Members

		decimal _cod_mes;

		#endregion

		#region Constructor

		public TmesKeys()
		{
				
		}

		public TmesKeys(decimal cod_mes)
		{
			 _cod_mes = cod_mes; 
		}

		#endregion

		#region Properties

		public decimal  cod_mes
		{
			 get { return _cod_mes; }
		}

		#endregion

	}
}
