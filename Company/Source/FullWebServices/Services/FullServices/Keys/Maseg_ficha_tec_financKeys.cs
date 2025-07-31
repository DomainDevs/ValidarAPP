using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_ficha_tec_financKeys
	{

		#region Data Members

		int _cod_aseg;
		string _fec_informacion;

		#endregion

		#region Constructor

		public Maseg_ficha_tec_financKeys()
		{
				
		}

		public Maseg_ficha_tec_financKeys(int cod_aseg, string fec_informacion)
		{
			 _cod_aseg = cod_aseg; 
			 _fec_informacion = fec_informacion; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}
		public string  fec_informacion
		{
			 get { return _fec_informacion; }
		}

		#endregion

	}
}
