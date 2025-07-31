using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class MagenteKeys
	{

		#region Data Members

		decimal _cod_tipo_agente;
		int _cod_agente;

		#endregion

		#region Constructor

		public MagenteKeys()
		{
				
		}

		public MagenteKeys(decimal cod_tipo_agente, int cod_agente)
		{
			 _cod_tipo_agente = cod_tipo_agente; 
			 _cod_agente = cod_agente; 
		}

		#endregion

		#region Properties

		public decimal  cod_tipo_agente
		{
			 get { return _cod_tipo_agente; }
		}
		public int  cod_agente
		{
			 get { return _cod_agente; }
		}

		#endregion

	}
}
