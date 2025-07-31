using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tusuario_centro_costoKeys
	{

		#region Data Members

		string _cod_usuario;
		double _cod_cencosto;

		#endregion

		#region Constructor

		public Tusuario_centro_costoKeys()
		{
				
		}

		public Tusuario_centro_costoKeys(string cod_usuario, double cod_cencosto)
		{
			 _cod_usuario = cod_usuario; 
			 _cod_cencosto = cod_cencosto; 
		}

		#endregion

		#region Properties

		public string  cod_usuario
		{
			 get { return _cod_usuario; }
		}
		public double  cod_cencosto
		{
			 get { return _cod_cencosto; }
		}

		#endregion

	}
}
