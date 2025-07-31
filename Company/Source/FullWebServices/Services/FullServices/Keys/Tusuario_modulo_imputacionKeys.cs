using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tusuario_modulo_imputacionKeys
	{

		#region Data Members

		string _cod_usuario;
		double _cod_imputacion;

		#endregion

		#region Constructor

		public Tusuario_modulo_imputacionKeys()
		{
				
		}

		public Tusuario_modulo_imputacionKeys(string cod_usuario, double cod_imputacion)
		{
			 _cod_usuario = cod_usuario; 
			 _cod_imputacion = cod_imputacion; 
		}

		#endregion

		#region Properties

		public string  cod_usuario
		{
			 get { return _cod_usuario; }
		}
		public double  cod_imputacion
		{
			 get { return _cod_imputacion; }
		}

		#endregion

	}
}
