using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_ficha_tecKeys
	{

		#region Data Members

		int _cod_aseg;

		#endregion

		#region Constructor

		public Maseg_ficha_tecKeys()
		{
				
		}

		public Maseg_ficha_tecKeys(int cod_aseg)
		{
			 _cod_aseg = cod_aseg; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}

		#endregion

	}
}
