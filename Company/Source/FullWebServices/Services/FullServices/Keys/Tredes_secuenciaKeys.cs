using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tredes_secuenciaKeys
	{

		#region Data Members

		decimal _cod_red;
		decimal _nro_secuencia;

		#endregion

		#region Constructor

		public Tredes_secuenciaKeys()
		{
				
		}

		public Tredes_secuenciaKeys(decimal cod_red, decimal nro_secuencia)
		{
			 _cod_red = cod_red; 
			 _nro_secuencia = nro_secuencia; 
		}

		#endregion

		#region Properties

		public decimal  cod_red
		{
			 get { return _cod_red; }
		}
		public decimal  nro_secuencia
		{
			 get { return _nro_secuencia; }
		}

		#endregion

	}
}
