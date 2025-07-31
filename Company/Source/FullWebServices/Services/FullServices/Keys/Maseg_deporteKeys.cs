using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_deporteKeys
	{

		#region Data Members

		int _cod_aseg;
		double _cod_deporte;

		#endregion

		#region Constructor

		public Maseg_deporteKeys()
		{
				
		}

		public Maseg_deporteKeys(int cod_aseg, double cod_deporte)
		{
			 _cod_aseg = cod_aseg; 
			 _cod_deporte = cod_deporte; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}
		public double  cod_deporte
		{
			 get { return _cod_deporte; }
		}

		#endregion

	}
}
