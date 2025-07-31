using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TusuarioKeys
	{

		#region Data Members

		string _cod_usuario;

		#endregion

		#region Constructor

		public TusuarioKeys()
		{
				
		}

		public TusuarioKeys(string cod_usuario)
		{
			 _cod_usuario = cod_usuario; 
		}

		#endregion

		#region Properties

		public string  cod_usuario
		{
			 get { return _cod_usuario; }
		}

		#endregion

	}
}
