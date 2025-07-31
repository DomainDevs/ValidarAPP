using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TconductoKeys
	{

		#region Data Members

		decimal _cod_conducto;

		#endregion

		#region Constructor

		public TconductoKeys()
		{
				
		}

		public TconductoKeys(decimal cod_conducto)
		{
			 _cod_conducto = cod_conducto; 
		}

		#endregion

		#region Properties

		public decimal  cod_conducto
		{
			 get { return _cod_conducto; }
		}

		#endregion

	}
}
