using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TdptoKeys
	{

		#region Data Members

		decimal _cod_pais;
		decimal _cod_dpto;

		#endregion

		#region Constructor

		public TdptoKeys()
		{
				
		}

		public TdptoKeys(decimal cod_pais, decimal cod_dpto)
		{
			 _cod_pais = cod_pais; 
			 _cod_dpto = cod_dpto; 
		}

		#endregion

		#region Properties

		public decimal  cod_pais
		{
			 get { return _cod_pais; }
		}
		public decimal  cod_dpto
		{
			 get { return _cod_dpto; }
		}

		#endregion

	}
}
