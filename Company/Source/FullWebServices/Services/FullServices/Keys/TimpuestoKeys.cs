using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class TimpuestoKeys
	{

		#region Data Members

		double _cod_impuesto;

		#endregion

		#region Constructor

		public TimpuestoKeys()
		{
				
		}

		public TimpuestoKeys(double cod_impuesto)
		{
			 _cod_impuesto = cod_impuesto; 
		}

		#endregion

		#region Properties

		public double  cod_impuesto
		{
			 get { return _cod_impuesto; }
		}

		#endregion

	}
}
