using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tciiu_nvaKeys
	{

		#region Data Members

		decimal _cod_ciiu;

		#endregion

		#region Constructor

		public Tciiu_nvaKeys()
		{
				
		}

		public Tciiu_nvaKeys(decimal cod_ciiu)
		{
			 _cod_ciiu = cod_ciiu; 
		}

		#endregion

		#region Properties

		public decimal  cod_ciiu
		{
			 get { return _cod_ciiu; }
		}

		#endregion

	}
}
