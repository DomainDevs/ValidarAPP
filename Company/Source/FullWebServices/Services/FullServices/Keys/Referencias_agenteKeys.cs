using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Referencias_agenteKeys
	{

		#region Data Members

		int _cod_agente;
		decimal _tipo_ref_agente;
		decimal _sn_ref1;
		decimal _sn_ref2;

		#endregion

		#region Constructor

		public Referencias_agenteKeys()
		{
				
		}

		public Referencias_agenteKeys(int cod_agente, decimal tipo_ref_agente, decimal sn_ref1, decimal sn_ref2)
		{
			 _cod_agente = cod_agente; 
			 _tipo_ref_agente = tipo_ref_agente; 
			 _sn_ref1 = sn_ref1; 
			 _sn_ref2 = sn_ref2; 
		}

		#endregion

		#region Properties

		public int  cod_agente
		{
			 get { return _cod_agente; }
		}
		public decimal  tipo_ref_agente
		{
			 get { return _tipo_ref_agente; }
		}
		public decimal  sn_ref1
		{
			 get { return _sn_ref1; }
		}
		public decimal  sn_ref2
		{
			 get { return _sn_ref2; }
		}

		#endregion

	}
}
