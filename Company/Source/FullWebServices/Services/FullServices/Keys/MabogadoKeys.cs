using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class MabogadoKeys
	{

		#region Data Members

		int _cod_abogado;

		#endregion

		#region Constructor

		public MabogadoKeys()
		{
				
		}

		public MabogadoKeys(int cod_abogado)
		{
			 _cod_abogado = cod_abogado; 
		}

		#endregion

		#region Properties

		public int  cod_abogado
		{
			 get { return _cod_abogado; }
		}

		#endregion

	}
}
