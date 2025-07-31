using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Referencias_terceroKeys
	{

		#region Data Members

		int _cod_tercero;
		decimal _tipo_ref_tercero;
		decimal _sn_ref1;
		decimal _sn_ref2;

		#endregion

		#region Constructor

		public Referencias_terceroKeys()
		{
				
		}

		public Referencias_terceroKeys(int cod_tercero, decimal tipo_ref_tercero, decimal sn_ref1, decimal sn_ref2)
		{
			 _cod_tercero = cod_tercero; 
			 _tipo_ref_tercero = tipo_ref_tercero; 
			 _sn_ref1 = sn_ref1; 
			 _sn_ref2 = sn_ref2; 
		}

		#endregion

		#region Properties

		public int  cod_tercero
		{
			 get { return _cod_tercero; }
		}
		public decimal  tipo_ref_tercero
		{
			 get { return _tipo_ref_tercero; }
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
