using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_ficha_tec_obsKeys
	{

		#region Data Members

		int _cod_aseg;
		double _consec_obs;

		#endregion

		#region Constructor

		public Maseg_ficha_tec_obsKeys()
		{
				
		}

		public Maseg_ficha_tec_obsKeys(int cod_aseg, double consec_obs)
		{
			 _cod_aseg = cod_aseg; 
			 _consec_obs = consec_obs; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}
		public double  consec_obs
		{
			 get { return _consec_obs; }
		}

		#endregion

	}
}
