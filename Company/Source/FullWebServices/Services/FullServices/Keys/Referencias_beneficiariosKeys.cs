using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Referencias_beneficiariosKeys
	{

		#region Data Members

		int _cod_beneficiario;
		decimal _tipo_ref_benef;
		decimal _sn_ref1;
		decimal _sn_ref2;

		#endregion

		#region Constructor

		public Referencias_beneficiariosKeys()
		{
				
		}

		public Referencias_beneficiariosKeys(int cod_beneficiario, decimal tipo_ref_benef, decimal sn_ref1, decimal sn_ref2)
		{
			 _cod_beneficiario = cod_beneficiario; 
			 _tipo_ref_benef = tipo_ref_benef; 
			 _sn_ref1 = sn_ref1; 
			 _sn_ref2 = sn_ref2; 
		}

		#endregion

		#region Properties

		public int  cod_beneficiario
		{
			 get { return _cod_beneficiario; }
		}
		public decimal  tipo_ref_benef
		{
			 get { return _tipo_ref_benef; }
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
