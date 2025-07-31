using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tcondicion_impuestoKeys
	{

		#region Data Members

		double _cod_impuesto;
		double _cod_condicion;

		#endregion

		#region Constructor

		public Tcondicion_impuestoKeys()
		{
				
		}

		public Tcondicion_impuestoKeys(double cod_impuesto, double cod_condicion)
		{
			 _cod_impuesto = cod_impuesto; 
			 _cod_condicion = cod_condicion; 
		}

		#endregion

		#region Properties

		public double  cod_impuesto
		{
			 get { return _cod_impuesto; }
		}
		public double  cod_condicion
		{
			 get { return _cod_condicion; }
		}

		#endregion

	}
}
