using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tpj_usuarios_emailKeys
	{

		#region Data Members

		string _cod_usuario;

		#endregion

		#region Constructor

		public Tpj_usuarios_emailKeys()
		{
				
		}

		public Tpj_usuarios_emailKeys(string cod_usuario)
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
