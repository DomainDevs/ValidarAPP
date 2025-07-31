using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class MbeneficiarioKeys
	{

		#region Data Members

		int _cod_beneficiario;

		#endregion

		#region Constructor

		public MbeneficiarioKeys()
		{
				
		}

		public MbeneficiarioKeys(int cod_beneficiario)
		{
			 _cod_beneficiario = cod_beneficiario; 
		}

		#endregion

		#region Properties

		public int  cod_beneficiario
		{
			 get { return _cod_beneficiario; }
		}

		#endregion

	}
}
