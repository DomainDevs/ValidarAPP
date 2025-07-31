using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Ttipo_empresaKeys
	{

		#region Data Members

		string _cod_ttipo_empresa;

		#endregion

		#region Constructor

		public Ttipo_empresaKeys()
		{
				
		}

		public Ttipo_empresaKeys(string cod_ttipo_empresa)
		{
			 _cod_ttipo_empresa = cod_ttipo_empresa; 
		}

		#endregion

		#region Properties

		public string  cod_ttipo_empresa
		{
			 get { return _cod_ttipo_empresa; }
		}

		#endregion

	}
}
