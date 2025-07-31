using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Mpersona_usuarioKeys
	{

		#region Data Members

		string _cod_usuario;

		#endregion

		#region Constructor

		public Mpersona_usuarioKeys()
		{
				
		}

		public Mpersona_usuarioKeys(string cod_usuario)
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
