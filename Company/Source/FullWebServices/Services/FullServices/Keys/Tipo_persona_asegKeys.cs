using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tipo_persona_asegKeys
	{

		#region Data Members

		int _cod_aseg;

		#endregion

		#region Constructor

		public Tipo_persona_asegKeys()
		{
				
		}

		public Tipo_persona_asegKeys(int cod_aseg)
		{
			 _cod_aseg = cod_aseg; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}

		#endregion

	}
}
