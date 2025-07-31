using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Ttipo_telefKeys
	{

		#region Data Members

		decimal _cod_tipo_telef;

		#endregion

		#region Constructor

		public Ttipo_telefKeys()
		{
				
		}

		public Ttipo_telefKeys(decimal cod_tipo_telef)
		{
			 _cod_tipo_telef = cod_tipo_telef; 
		}

		#endregion

		#region Properties

		public decimal  cod_tipo_telef
		{
			 get { return _cod_tipo_telef; }
		}

		#endregion

	}
}
