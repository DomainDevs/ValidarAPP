using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Ttipo_cta_bcoKeys
	{

		#region Data Members

		decimal _cod_tipo_cta_bco;

		#endregion

		#region Constructor

		public Ttipo_cta_bcoKeys()
		{
				
		}

		public Ttipo_cta_bcoKeys(decimal cod_tipo_cta_bco)
		{
			 _cod_tipo_cta_bco = cod_tipo_cta_bco; 
		}

		#endregion

		#region Properties

		public decimal  cod_tipo_cta_bco
		{
			 get { return _cod_tipo_cta_bco; }
		}

		#endregion

	}
}
