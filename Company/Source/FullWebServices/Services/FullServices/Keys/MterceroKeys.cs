using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class MterceroKeys
	{

		#region Data Members

		int _cod_tercero;

		#endregion

		#region Constructor

		public MterceroKeys()
		{
				
		}

		public MterceroKeys(int cod_tercero)
		{
			 _cod_tercero = cod_tercero; 
		}

		#endregion

		#region Properties

		public int  cod_tercero
		{
			 get { return _cod_tercero; }
		}

		#endregion

	}
}
