using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tplaza_trans_bancKeys
	{

		#region Data Members

		string _cod_plaza;
		int _cod_tipo_red;

		#endregion

		#region Constructor

		public Tplaza_trans_bancKeys()
		{
				
		}

		public Tplaza_trans_bancKeys(string cod_plaza, int cod_tipo_red)
		{
			 _cod_plaza = cod_plaza; 
			 _cod_tipo_red = cod_tipo_red; 
		}

		#endregion

		#region Properties

		public string  cod_plaza
		{
			 get { return _cod_plaza; }
		}
		public int  cod_tipo_red
		{
			 get { return _cod_tipo_red; }
		}

		#endregion

	}
}
