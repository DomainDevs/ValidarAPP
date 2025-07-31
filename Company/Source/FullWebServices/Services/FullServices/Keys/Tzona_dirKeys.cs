using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tzona_dirKeys
	{

		#region Data Members

		decimal _cod_pais;
		decimal _cod_dpto;
		decimal _cod_municipio;
		decimal _cod_zona_dir;

		#endregion

		#region Constructor

		public Tzona_dirKeys()
		{
				
		}

		public Tzona_dirKeys(decimal cod_pais, decimal cod_dpto, decimal cod_municipio, decimal cod_zona_dir)
		{
			 _cod_pais = cod_pais; 
			 _cod_dpto = cod_dpto; 
			 _cod_municipio = cod_municipio; 
			 _cod_zona_dir = cod_zona_dir; 
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
		public decimal  cod_zona_dir
		{
			 get { return _cod_zona_dir; }
		}

		#endregion

	}
}
