using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Referencias_presKeys
	{

		#region Data Members

		decimal _cod_pres;
		decimal _tipo_ref_pres;
		decimal _sn_ref1;
		decimal _sn_ref2;

		#endregion

		#region Constructor

		public Referencias_presKeys()
		{
				
		}

		public Referencias_presKeys(decimal cod_pres, decimal tipo_ref_pres, decimal sn_ref1, decimal sn_ref2)
		{
			 _cod_pres = cod_pres; 
			 _tipo_ref_pres = tipo_ref_pres; 
			 _sn_ref1 = sn_ref1; 
			 _sn_ref2 = sn_ref2; 
		}

		#endregion

		#region Properties

		public decimal  cod_pres
		{
			 get { return _cod_pres; }
		}
		public decimal  tipo_ref_pres
		{
			 get { return _tipo_ref_pres; }
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
