using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TunidadKeys
	{

		#region Data Members

		decimal _cod_unidad;

		#endregion

		#region Constructor

		public TunidadKeys()
		{
				
		}

		public TunidadKeys(decimal cod_unidad)
		{
			 _cod_unidad = cod_unidad; 
		}

		#endregion

		#region Properties

		public decimal  cod_unidad
		{
			 get { return _cod_unidad; }
		}

		#endregion

	}
}
