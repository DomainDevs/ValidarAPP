using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Ttipo_asociacionKeys
	{

		#region Data Members

		decimal _cod_tasociacion;

		#endregion

		#region Constructor

		public Ttipo_asociacionKeys()
		{
				
		}

		public Ttipo_asociacionKeys(decimal cod_tasociacion)
		{
			 _cod_tasociacion = cod_tasociacion; 
		}

		#endregion

		#region Properties

		public decimal  cod_tasociacion
		{
			 get { return _cod_tasociacion; }
		}

		#endregion

	}
}
