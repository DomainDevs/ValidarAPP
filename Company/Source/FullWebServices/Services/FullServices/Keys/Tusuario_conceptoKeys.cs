using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tusuario_conceptoKeys
	{

		#region Data Members

		string _cod_usuario;
		double _cod_concepto;

		#endregion

		#region Constructor

		public Tusuario_conceptoKeys()
		{
				
		}

		public Tusuario_conceptoKeys(string cod_usuario, double cod_concepto)
		{
			 _cod_usuario = cod_usuario; 
			 _cod_concepto = cod_concepto; 
		}

		#endregion

		#region Properties

		public string  cod_usuario
		{
			 get { return _cod_usuario; }
		}
		public double  cod_concepto
		{
			 get { return _cod_concepto; }
		}

		#endregion

	}
}
