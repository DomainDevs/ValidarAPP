using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_tasa_tarifaKeys
	{

		#region Data Members

		int _cod_aseg;
		double _cod_ramo;
		double _cod_subramo;
		double _cod_tarifa;

		#endregion

		#region Constructor

		public Maseg_tasa_tarifaKeys()
		{
				
		}

		public Maseg_tasa_tarifaKeys(int cod_aseg, double cod_ramo, double cod_subramo, double cod_tarifa)
		{
			 _cod_aseg = cod_aseg; 
			 _cod_ramo = cod_ramo; 
			 _cod_subramo = cod_subramo; 
			 _cod_tarifa = cod_tarifa; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}
		public double  cod_ramo
		{
			 get { return _cod_ramo; }
		}
		public double  cod_subramo
		{
			 get { return _cod_subramo; }
		}
		public double  cod_tarifa
		{
			 get { return _cod_tarifa; }
		}

		#endregion

	}
}
