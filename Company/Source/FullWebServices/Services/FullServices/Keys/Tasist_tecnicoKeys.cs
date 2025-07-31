using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tasist_tecnicoKeys
	{

		#region Data Members

		int _cod_asist_tecnico;

		#endregion

		#region Constructor

		public Tasist_tecnicoKeys()
		{
				
		}

		public Tasist_tecnicoKeys(int cod_asist_tecnico)
		{
			 _cod_asist_tecnico = cod_asist_tecnico; 
		}

		#endregion

		#region Properties

		public int  cod_asist_tecnico
		{
			 get { return _cod_asist_tecnico; }
		}

		#endregion

	}
}
