using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TpaisKeys
	{

		#region Data Members

		decimal _cod_pais;

		#endregion

		#region Constructor

		public TpaisKeys()
		{
				
		}

		public TpaisKeys(decimal cod_pais)
		{
			 _cod_pais = cod_pais; 
		}

		#endregion

		#region Properties

		public decimal  cod_pais
		{
			 get { return _cod_pais; }
		}

		#endregion

	}
}
