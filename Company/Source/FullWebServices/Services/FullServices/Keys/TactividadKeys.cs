using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TactividadKeys
	{

		#region Data Members

		double _cod_actividad;
		double _cod_dpto;
		double _cod_munic;

		#endregion

		#region Constructor

		public TactividadKeys()
		{
				
		}

		public TactividadKeys(double cod_actividad, double cod_dpto, double cod_munic)
		{
			 _cod_actividad = cod_actividad; 
			 _cod_dpto = cod_dpto; 
			 _cod_munic = cod_munic; 
		}

		#endregion

		#region Properties

		public double  cod_actividad
		{
			 get { return _cod_actividad; }
		}
		public double  cod_dpto
		{
			 get { return _cod_dpto; }
		}
		public double  cod_munic
		{
			 get { return _cod_munic; }
		}

		#endregion

	}
}
