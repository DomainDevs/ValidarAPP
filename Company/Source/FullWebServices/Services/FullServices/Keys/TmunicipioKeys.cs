using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TmunicipioKeys
	{

		#region Data Members

		decimal _cod_pais;
		decimal _cod_dpto;
		decimal _cod_municipio;

		#endregion

		#region Constructor

		public TmunicipioKeys()
		{
				
		}

		public TmunicipioKeys(decimal cod_pais, decimal cod_dpto, decimal cod_municipio)
		{
			 _cod_pais = cod_pais; 
			 _cod_dpto = cod_dpto; 
			 _cod_municipio = cod_municipio; 
		}

		#endregion

		#region Properties

		public decimal  cod_pais
		{
			 get { return _cod_pais; }
		}
		public decimal  cod_dpto
		{
			 get { return _cod_dpto; }
		}
		public decimal  cod_municipio
		{
			 get { return _cod_municipio; }
		}

		#endregion

	}
}
