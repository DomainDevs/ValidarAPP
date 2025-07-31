using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tdirector_comercialKeys
	{

		#region Data Members

		int _cod_director_comercial;

		#endregion

		#region Constructor

		public Tdirector_comercialKeys()
		{
				
		}

		public Tdirector_comercialKeys(int cod_director_comercial)
		{
			 _cod_director_comercial = cod_director_comercial; 
		}

		#endregion

		#region Properties

		public int  cod_director_comercial
		{
			 get { return _cod_director_comercial; }
		}

		#endregion

	}
}
