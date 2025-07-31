using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TocupacionKeys
	{

		#region Data Members

		decimal _cod_ocupacion;

		#endregion

		#region Constructor

		public TocupacionKeys()
		{
				
		}

		public TocupacionKeys(decimal cod_ocupacion)
		{
			 _cod_ocupacion = cod_ocupacion; 
		}

		#endregion

		#region Properties

		public decimal  cod_ocupacion
		{
			 get { return _cod_ocupacion; }
		}

		#endregion

	}
}
