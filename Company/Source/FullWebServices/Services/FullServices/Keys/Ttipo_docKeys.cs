using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Ttipo_docKeys
	{

		#region Data Members

		decimal _cod_tipo_doc;

		#endregion

		#region Constructor

		public Ttipo_docKeys()
		{
				
		}

		public Ttipo_docKeys(decimal cod_tipo_doc)
		{
			 _cod_tipo_doc = cod_tipo_doc; 
		}

		#endregion

		#region Properties

		public decimal  cod_tipo_doc
		{
			 get { return _cod_tipo_doc; }
		}

		#endregion

	}
}
