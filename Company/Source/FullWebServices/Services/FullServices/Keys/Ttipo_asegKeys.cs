using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Ttipo_asegKeys
	{

		#region Data Members

		decimal _cod_tipo_aseg;

		#endregion

		#region Constructor

		public Ttipo_asegKeys()
		{
				
		}

		public Ttipo_asegKeys(decimal cod_tipo_aseg)
		{
			 _cod_tipo_aseg = cod_tipo_aseg; 
		}

		#endregion

		#region Properties

		public decimal  cod_tipo_aseg
		{
			 get { return _cod_tipo_aseg; }
		}

		#endregion

	}
}
