using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Ttipo_dirKeys
	{

		#region Data Members

		decimal _cod_tipo_dir;

		#endregion

		#region Constructor

		public Ttipo_dirKeys()
		{
				
		}

		public Ttipo_dirKeys(decimal cod_tipo_dir)
		{
			 _cod_tipo_dir = cod_tipo_dir; 
		}

		#endregion

		#region Properties

		public decimal  cod_tipo_dir
		{
			 get { return _cod_tipo_dir; }
		}

		#endregion

	}
}
