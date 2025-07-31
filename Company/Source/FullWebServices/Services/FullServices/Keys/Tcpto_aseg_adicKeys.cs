using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tcpto_aseg_adicKeys
	{

		#region Data Members

		int _cod_aseg;

		#endregion

		#region Constructor

		public Tcpto_aseg_adicKeys()
		{
				
		}

		public Tcpto_aseg_adicKeys(int cod_aseg)
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
