using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Test_civilKeys
	{

		#region Data Members

		decimal _cod_est_civil;

		#endregion

		#region Constructor

		public Test_civilKeys()
		{
				
		}

		public Test_civilKeys(decimal cod_est_civil)
		{
			 _cod_est_civil = cod_est_civil; 
		}

		#endregion

		#region Properties

		public decimal  cod_est_civil
		{
			 get { return _cod_est_civil; }
		}

		#endregion

	}
}
