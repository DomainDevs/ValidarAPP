using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Referencias_cesionarioKeys
	{

		#region Data Members

		int _cod_cesionario;
		decimal _tipo_ref_cesionario;
		decimal _sn_ref1;
		decimal _sn_ref2;

		#endregion

		#region Constructor

		public Referencias_cesionarioKeys()
		{
				
		}

		public Referencias_cesionarioKeys(int cod_cesionario, decimal tipo_ref_cesionario, decimal sn_ref1, decimal sn_ref2)
		{
			 _cod_cesionario = cod_cesionario; 
			 _tipo_ref_cesionario = tipo_ref_cesionario; 
			 _sn_ref1 = sn_ref1; 
			 _sn_ref2 = sn_ref2; 
		}

		#endregion

		#region Properties

		public int  cod_cesionario
		{
			 get { return _cod_cesionario; }
		}
		public decimal  tipo_ref_cesionario
		{
			 get { return _tipo_ref_cesionario; }
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
