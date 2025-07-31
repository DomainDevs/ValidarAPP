using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Mpersona_cuentas_bancariasKeys
	{

		#region Data Members

		int _cod_tipo_red;
		string _cod_plaza;
		decimal _cod_banco;
		int _id_persona;
		string _txt_nro_cta;

		#endregion

		#region Constructor

		public Mpersona_cuentas_bancariasKeys()
		{
				
		}

		public Mpersona_cuentas_bancariasKeys(int cod_tipo_red, string cod_plaza, decimal cod_banco, int id_persona, string txt_nro_cta)
		{
			 _cod_tipo_red = cod_tipo_red; 
			 _cod_plaza = cod_plaza; 
			 _cod_banco = cod_banco; 
			 _id_persona = id_persona; 
			 _txt_nro_cta = txt_nro_cta; 
		}

		#endregion

		#region Properties

		public int  cod_tipo_red
		{
			 get { return _cod_tipo_red; }
		}
		public string  cod_plaza
		{
			 get { return _cod_plaza; }
		}
		public decimal  cod_banco
		{
			 get { return _cod_banco; }
		}
		public int  id_persona
		{
			 get { return _id_persona; }
		}
		public string  txt_nro_cta
		{
			 get { return _txt_nro_cta; }
		}

		#endregion

	}
}
