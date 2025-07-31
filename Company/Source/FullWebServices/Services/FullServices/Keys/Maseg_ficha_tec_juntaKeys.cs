using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_ficha_tec_juntaKeys
	{

		#region Data Members

		int _cod_aseg;
		double _consec_miembro;

		#endregion

		#region Constructor

		public Maseg_ficha_tec_juntaKeys()
		{
				
		}

		public Maseg_ficha_tec_juntaKeys(int cod_aseg, double consec_miembro)
		{
			 _cod_aseg = cod_aseg; 
			 _consec_miembro = consec_miembro; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}
		public double  consec_miembro
		{
			 get { return _consec_miembro; }
		}

		#endregion

	}
}
