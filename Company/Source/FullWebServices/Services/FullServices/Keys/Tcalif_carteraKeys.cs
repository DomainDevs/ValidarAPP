using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tcalif_carteraKeys
	{

		#region Data Members

		string _cod_calif_cart;

		#endregion

		#region Constructor

		public Tcalif_carteraKeys()
		{
				
		}

		public Tcalif_carteraKeys(string cod_calif_cart)
		{
			 _cod_calif_cart = cod_calif_cart; 
		}

		#endregion

		#region Properties

		public string  cod_calif_cart
		{
			 get { return _cod_calif_cart; }
		}

		#endregion

	}
}
