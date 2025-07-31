using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TdeporteKeys
	{

		#region Data Members

		decimal _cod_deporte;

		#endregion

		#region Constructor

		public TdeporteKeys()
		{
				
		}

		public TdeporteKeys(decimal cod_deporte)
		{
			 _cod_deporte = cod_deporte; 
		}

		#endregion

		#region Properties

		public decimal  cod_deporte
		{
			 get { return _cod_deporte; }
		}

		#endregion

	}
}
