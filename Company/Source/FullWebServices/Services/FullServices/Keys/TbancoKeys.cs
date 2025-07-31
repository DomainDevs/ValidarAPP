using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TbancoKeys
	{

		#region Data Members

		decimal _cod_banco;

		#endregion

		#region Constructor

		public TbancoKeys()
		{
				
		}

		public TbancoKeys(decimal cod_banco)
		{
			 _cod_banco = cod_banco; 
		}

		#endregion

		#region Properties

		public decimal  cod_banco
		{
			 get { return _cod_banco; }
		}

		#endregion

	}
}
