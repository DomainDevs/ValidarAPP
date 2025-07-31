using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Referencias_abogadoKeys
	{

		#region Data Members

		int _cod_abogado;
		decimal _tipo_ref_abogado;
		decimal _sn_ref1;
		decimal _sn_ref2;

		#endregion

		#region Constructor

		public Referencias_abogadoKeys()
		{
				
		}

		public Referencias_abogadoKeys(int cod_abogado, decimal tipo_ref_abogado, decimal sn_ref1, decimal sn_ref2)
		{
			 _cod_abogado = cod_abogado; 
			 _tipo_ref_abogado = tipo_ref_abogado; 
			 _sn_ref1 = sn_ref1; 
			 _sn_ref2 = sn_ref2; 
		}

		#endregion

		#region Properties

		public int  cod_abogado
		{
			 get { return _cod_abogado; }
		}
		public decimal  tipo_ref_abogado
		{
			 get { return _tipo_ref_abogado; }
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
