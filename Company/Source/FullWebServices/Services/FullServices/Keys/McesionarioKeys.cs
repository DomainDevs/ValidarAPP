using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class McesionarioKeys
	{

		#region Data Members

		int _cod_cesionario;

		#endregion

		#region Constructor

		public McesionarioKeys()
		{
				
		}

		public McesionarioKeys(int cod_cesionario)
		{
			 _cod_cesionario = cod_cesionario; 
		}

		#endregion

		#region Properties

		public int  cod_cesionario
		{
			 get { return _cod_cesionario; }
		}

		#endregion

	}
}
